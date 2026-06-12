using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// ARSetup: Initializes AR Foundation for mobile (iOS ARKit / Android ARCore)
/// Configures camera, plane detection, light estimation, and session lifecycle
/// </summary>
public class ARSetup : MonoBehaviour
{
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARCameraManager arCameraManager;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private AROcclusionManager arOcclusionManager;
    [SerializeField] private ARLightEstimationManager arLightEstimationManager;

    [SerializeField] private bool enablePlaneDetection = true;
    [SerializeField] private bool enableLightEstimation = true;
    [SerializeField] private DetectionMode planeDetectionMode = DetectionMode.Horizontal;

    private bool isARSupported = false;

    private void Awake()
    {
        CheckARSupport();
        ConfigureARSession();
    }

    private void CheckARSupport()
    {
        // Check device AR capability
        #if UNITY_IOS
        isARSupported = UnityEngine.iOS.Device.systemVersion.Contains("ARKit");
        Debug.Log($"[AR] iOS ARKit supported: {isARSupported}");
        #elif UNITY_ANDROID
        isARSupported = UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.CAMERA");
        Debug.Log($"[AR] Android camera permission: {isARSupported}");
        #else
        isARSupported = false;
        Debug.LogWarning("[AR] AR not supported on this platform");
        #endif
    }

    private void ConfigureARSession()
    {
        if (!isARSupported)
        {
            Debug.LogError("[AR] AR not supported. Disabling AR managers.");
            return;
        }

        // Configure AR components
        if (arPlaneManager != null)
        {
            arPlaneManager.detectionMode = enablePlaneDetection ? planeDetectionMode : DetectionMode.None;
            Debug.Log($"[AR] Plane detection: {planeDetectionMode}");
        }

        if (arLightEstimationManager != null)
        {
            arLightEstimationManager.lightEstimationMode = enableLightEstimation ?
                LightEstimationMode.AmbientIntensity : LightEstimationMode.Disabled;
            Debug.Log("[AR] Light estimation enabled");
        }

        if (arOcclusionManager != null)
        {
            arOcclusionManager.requestedOcclusionPreferenceMode = OcclusionPreferenceMode.PreferUsingHumanSegmentationCPU;
            Debug.Log("[AR] Occlusion (human segmentation) enabled");
        }

        // Start AR session
        if (arSession != null && !arSession.enabled)
        {
            arSession.enabled = true;
            Debug.Log("[AR] Session started");
        }
    }

    private void Update()
    {
        // Monitor AR session state
        if (arSession != null && arSession.state == ARSessionState.SessionInitializing)
        {
            Debug.Log("[AR] Session initializing...");
        }
        else if (arSession != null && arSession.state == ARSessionState.SessionTracking)
        {
            // AR is running smoothly
        }
    }

    public bool IsARReady()
    {
        return isARSupported && arSession != null && arSession.state == ARSessionState.SessionTracking;
    }

    private void OnDestroy()
    {
        if (arSession != null)
        {
            arSession.enabled = false;
            Debug.Log("[AR] Session stopped");
        }
    }
}

public enum DetectionMode
{
    None = 0,
    Horizontal = 1,
    Vertical = 2,
    HorizontalAndVertical = 3
}

public enum LightEstimationMode
{
    Disabled = 0,
    AmbientIntensity = 1,
    AmbientColor = 2,
    AmbientColorAndDirectionalLight = 3
}

public enum OcclusionPreferenceMode
{
    PreferUsingGPU = 0,
    PreferUsingCPU = 1,
    PreferUsingHumanSegmentationCPU = 2
}
