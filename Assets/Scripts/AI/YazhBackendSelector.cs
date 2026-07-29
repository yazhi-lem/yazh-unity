using UnityEngine;
using Unity.InferenceEngine;

/// <summary>
/// FIX (Issue #2): both YazhInferenceEngine and YazhInferenceManager
/// previously hardcoded `new Worker(model, BackendType.GPUCompute)`, with a
/// comment claiming "use GPU if available; fall back to CPU" that no code
/// actually implemented. BackendType.GPUCompute requires compute-shader
/// support (Vulkan/Metal); on devices at the low end of the app's stated
/// min-spec (Android 7+ / iOS 12+) that support isn't guaranteed, so Worker
/// construction could throw or inference could silently fail with no
/// recovery path.
///
/// This centralizes the "GPU if supported, else CPU" decision so both
/// inference classes share one tested implementation instead of two copies
/// that can drift (see Issue #3).
/// </summary>
public static class YazhBackendSelector
{
    /// <summary>
    /// Picks BackendType.GPUCompute when the device reports compute-shader
    /// support, otherwise falls back to BackendType.CPU. Never throws.
    /// </summary>
    public static BackendType SelectBackend(string logTag = "[Yazh AI]")
    {
        bool gpuSupported = SystemInfo.supportsComputeShaders;

        BackendType backend = gpuSupported ? BackendType.GPUCompute : BackendType.CPU;
        Debug.Log($"{logTag} Backend selected: {backend} " +
                   $"(supportsComputeShaders={gpuSupported}, device={SystemInfo.deviceModel}, gfx={SystemInfo.graphicsDeviceType})");

        return backend;
    }

    /// <summary>
    /// Creates a Worker for the given model, trying the preferred backend
    /// first and retrying on CPU once if construction throws (covers
    /// devices that misreport compute-shader support).
    /// </summary>
    public static Worker CreateWorkerSafe(Model model, string logTag = "[Yazh AI]")
    {
        BackendType backend = SelectBackend(logTag);

        try
        {
            return new Worker(model, backend);
        }
        catch (System.Exception ex) when (backend != BackendType.CPU)
        {
            Debug.LogWarning($"{logTag} Worker construction failed on {backend} ({ex.Message}); retrying on CPU.");
            return new Worker(model, BackendType.CPU);
        }
    }
}
