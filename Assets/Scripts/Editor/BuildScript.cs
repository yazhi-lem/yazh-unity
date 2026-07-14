using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// BuildScript: Static build methods for CI/CD pipeline
/// Called by build-yazh.sh via Unity -executeMethod
/// </summary>
public static class BuildScript
{
    private static readonly string[] Scenes = new[]
    {
        "Assets/Scenes/MainMenu.unity",
        "Assets/Scenes/PetSelection.unity",
        "Assets/Scenes/TinaiRunner.unity",
        "Assets/Scenes/BiomeArena.unity"
    };

    // SEC-001: Embedded ONNX model hashes (SHA-256)
    // These are verified at runtime by YazhInferenceManager.VerifyModelHash()
    // Generated: 2026-06-18 | Hashes must match StreamingAssets/MLModels/*.onnx
    // If any hash changes, update both this dict AND YazhInferenceManager's ONNX_MODEL_HASHES
    // This is the build-time manifest; runtime verification happens in-app
    private static readonly Dictionary<string, string> ONNX_MODEL_HASHES = new()
    {
        { "Assets/StreamingAssets/MLModels/yazh-30k-int8.onnx", "3d9bfaeec2994ce78f3f29c979354a105cc8198aa5018bf4dc0d13a892aa59dc" },
        { "Assets/StreamingAssets/MLModels/yazh-30k-int4.onnx", "ca791d14644203acb35e76413f2b0a914ce6d0a2c81d8957b9654dc23a4765ec" },
        { "Assets/StreamingAssets/MLModels/yazh-30k.onnx", "d6bf01d17df05a0ec51ef814a500d645aa94b367e2907c1179131e69a442f8a6" }
    };

    [MenuItem("Build/iOS")]
    public static void BuildiOS()
    {
        // SEC-001: Verify model hashes before building
        if (!VerifyAllModelHashes())
        {
            Debug.LogError("[BuildScript] [SEC-001] Model hash verification failed. Aborting build.");
            return;
        }

        string outputPath = Path.Combine("Build", "Yazh-iOS");
        BuildOptions options = BuildOptions.None;

        Debug.Log("[BuildScript] Starting iOS build...");
        Debug.Log($"[BuildScript] Output: {outputPath}");

        // Ensure scenes exist (use empty array if scenes not yet created)
        string[] activeScenes = GetExistingScenes();
        if (activeScenes.Length == 0)
        {
            Debug.LogWarning("[BuildScript] No scenes found. Creating minimal build.");
        }

        BuildPipeline.BuildPlayer(
            activeScenes,
            outputPath,
            BuildTarget.iOS,
            options
        );

        Debug.Log("[BuildScript] iOS build complete.");
    }

    [MenuItem("Build/Android")]
    public static void BuildAndroid()
    {
        // SEC-001: Verify model hashes before building
        if (!VerifyAllModelHashes())
        {
            Debug.LogError("[BuildScript] [SEC-001] Model hash verification failed. Aborting build.");
            return;
        }

        string outputPath = Path.Combine("Build", "Yazh-Android.apk");
        BuildOptions options = BuildOptions.None;

        Debug.Log("[BuildScript] Starting Android build...");
        Debug.Log($"[BuildScript] Output: {outputPath}");

        string[] activeScenes = GetExistingScenes();
        if (activeScenes.Length == 0)
        {
            Debug.LogWarning("[BuildScript] No scenes found. Creating minimal build.");
        }

        // Set Android bundle version
        PlayerSettings.Android.bundleVersionCode = 1;
        PlayerSettings.bundleVersion = "0.1.0-prototype";

        BuildPipeline.BuildPlayer(
            activeScenes,
            outputPath,
            BuildTarget.Android,
            options
        );

        Debug.Log("[BuildScript] Android build complete.");
    }

    [MenuItem("Build/Both Platforms")]
    public static void BuildBoth()
    {
        BuildiOS();
        BuildAndroid();
    }

    /// <summary>
    /// Returns only scenes that actually exist on disk
    /// Prevents build failures when scenes haven't been created yet
    /// </summary>
    private static string[] GetExistingScenes()
    {
        var existing = new System.Collections.Generic.List<string>();
        foreach (string scene in Scenes)
        {
            if (File.Exists(scene))
            {
                existing.Add(scene);
            }
            else
            {
                Debug.LogWarning($"[BuildScript] Scene not found: {scene}");
            }
        }
        return existing.ToArray();
    }

    /// <summary>
    /// SEC-001: Verify all ONNX model hashes before build
    /// Computes SHA-256 for each model file in ONNX_MODEL_HASHES
    /// Returns false if any hash mismatches (model may be corrupted or tampered)
    /// This is a build-time security gate to prevent shipping compromised models
    /// </summary>
    private static bool VerifyAllModelHashes()
    {
        Debug.Log("[BuildScript] [SEC-001] Verifying ONNX model integrity...");

        bool allValid = true;
        foreach (var kvp in ONNX_MODEL_HASHES)
        {
            string modelPath = kvp.Key;
            string trustedHash = kvp.Value;

            if (!File.Exists(modelPath))
            {
                Debug.LogError($"[BuildScript] [SEC-001] Model file not found: {modelPath}");
                allValid = false;
                continue;
            }

            try
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    using (FileStream fileStream = new FileStream(modelPath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] hashBytes = sha256.ComputeHash(fileStream);
                        string computedHash = System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                        if (!computedHash.Equals(trustedHash, System.StringComparison.OrdinalIgnoreCase))
                        {
                            Debug.LogError($"[BuildScript] [SEC-001] HASH MISMATCH: {modelPath}");
                            Debug.LogError($"[BuildScript] [SEC-001] Expected: {trustedHash}");
                            Debug.LogError($"[BuildScript] [SEC-001] Computed: {computedHash}");
                            allValid = false;
                        }
                        else
                        {
                            Debug.Log($"[BuildScript] [SEC-001] ✓ Hash verified: {Path.GetFileName(modelPath)}");
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[BuildScript] [SEC-001] Hash verification exception for {modelPath}: {e.Message}");
                allValid = false;
            }
        }

        if (allValid)
        {
            Debug.Log("[BuildScript] [SEC-001] All model hashes verified successfully. Build can proceed.");
        }
        else
        {
            Debug.LogError("[BuildScript] [SEC-001] Model hash verification failed. Do not ship this build.");
        }

        return allValid;
    }
}
