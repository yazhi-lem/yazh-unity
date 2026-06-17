using UnityEditor;
using UnityEngine;
using System.IO;

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
        "Assets/Scenes/BiomeArena.unity"
    };

    [MenuItem("Build/iOS")]
    public static void BuildiOS()
    {
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
}
