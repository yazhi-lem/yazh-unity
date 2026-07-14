using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
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
    /// Headless CI entry point (GitHub Actions / game-ci):
    ///   unity -batchmode -executeMethod BuildScript.BuildAndroidCI
    /// Reads the output path from -customBuildPath (passed by game-ci) or
    /// -buildPath, builds an APK, and exits 0/1 so the CI job reflects the
    /// real build result. Works without the ML models — they are optional
    /// StreamingAssets; the app falls back to scripted Tamil responses.
    /// </summary>
    public static void BuildAndroidCI()
    {
        string buildPath = GetCommandLineArg("-customBuildPath")
                        ?? GetCommandLineArg("-buildPath")
                        ?? Path.Combine("Build", "Android", "Yazh-Android.apk");
        if (string.IsNullOrEmpty(Path.GetExtension(buildPath)))
            buildPath += ".apk";

        Debug.Log($"[BuildScript] CI Android build → {buildPath}");

        // Models are optional; only a present-but-tampered model aborts.
        if (!VerifyAllModelHashes())
        {
            Debug.LogError("[BuildScript] [SEC-001] Model hash mismatch. Aborting CI build.");
            EditorApplication.Exit(1);
            return;
        }

        EnsureAndroidIdentity();
        EditorUserBuildSettings.buildAppBundle = false;   // APK, not AAB
        PlayerSettings.Android.useCustomKeystore = false; // debug-signed CI artifact
        PlayerSettings.Android.bundleVersionCode = 1;
        PlayerSettings.bundleVersion = "0.1.0-prototype";

        string dir = Path.GetDirectoryName(buildPath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        BuildReport report = BuildPipeline.BuildPlayer(
            GetExistingScenes(), buildPath, BuildTarget.Android, BuildOptions.None);

        BuildSummary summary = report.summary;
        Debug.Log($"[BuildScript] CI build result: {summary.result} | " +
                  $"size: {summary.totalSize / 1024 / 1024}MB | errors: {summary.totalErrors}");

        EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
    }

    /// <summary>
    /// Guarantee a Play-Store-valid application identifier and product name.
    /// The hand-authored ProjectSettings has neither, and Unity's fallback
    /// (com.DefaultCompany.yazh-unity) is not a valid Android package name.
    /// </summary>
    private static void EnsureAndroidIdentity()
    {
        if (string.IsNullOrWhiteSpace(PlayerSettings.productName)
            || PlayerSettings.productName.Contains("-"))
            PlayerSettings.productName = "Yazh";
        if (string.IsNullOrWhiteSpace(PlayerSettings.companyName)
            || PlayerSettings.companyName == "DefaultCompany")
            PlayerSettings.companyName = "Yazhi";

        string id = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);
        if (!IsValidAndroidPackage(id))
        {
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "org.yazhi.yazh");
            Debug.Log($"[BuildScript] Application identifier '{id}' invalid → org.yazhi.yazh");
        }
    }

    private static bool IsValidAndroidPackage(string id)
    {
        return !string.IsNullOrEmpty(id) &&
               System.Text.RegularExpressions.Regex.IsMatch(
                   id, @"^[a-zA-Z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)+$");
    }

    private static string GetCommandLineArg(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == name && !args[i + 1].StartsWith("-"))
                return args[i + 1];
        }
        return null;
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
    /// SEC-001: Verify ONNX model hashes before build.
    /// Models are OPTIONAL — a missing model file logs a warning and the
    /// build proceeds (the app runs its scripted-dialogue fallback without
    /// them). A model that is present but hash-mismatched always fails: that
    /// means corruption or tampering, and we never ship it.
    /// Set env YAZH_REQUIRE_MODELS=1 to make missing models fatal too
    /// (for founder/release builds that must bundle the AI).
    /// </summary>
    private static bool VerifyAllModelHashes()
    {
        Debug.Log("[BuildScript] [SEC-001] Verifying ONNX model integrity...");

        bool requireModels = System.Environment.GetEnvironmentVariable("YAZH_REQUIRE_MODELS") == "1";
        bool allValid = true;
        foreach (var kvp in ONNX_MODEL_HASHES)
        {
            string modelPath = kvp.Key;
            string trustedHash = kvp.Value;

            if (!File.Exists(modelPath))
            {
                if (requireModels)
                {
                    Debug.LogError($"[BuildScript] [SEC-001] Model file REQUIRED but not found: {modelPath}");
                    allValid = false;
                }
                else
                {
                    Debug.LogWarning($"[BuildScript] [SEC-001] Model absent (optional): {modelPath} — building without it.");
                }
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
