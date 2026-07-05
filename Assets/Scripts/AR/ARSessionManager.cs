using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Manages AR session, plane detection, and pet placement.
/// Handles device tracking and AR Foundation lifecycle.
/// </summary>
public class ARSessionManager : MonoBehaviour
{
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private ARRaycastManager raycastManager;

    private bool isARReady = false;

    private void OnEnable()
    {
        if (arSession != null)
            arSession.Reset();
    }

    private void Update()
    {
        if (!isARReady && arSession != null)
        {
            isARReady = true;
            Debug.Log("[ARSessionManager] AR session active");
        }

        // Detect planes for pet placement
        if (arPlaneManager != null && arPlaneManager.trackablesCount > 0)
        {
            Debug.Log($"[ARSessionManager] Planes detected: {arPlaneManager.trackablesCount}");
        }
    }

    public bool TryPlacePetAtScreen(Vector2 screenPos, out Vector3 worldPos)
    {
        worldPos = Vector3.zero;

        if (raycastManager == null)
            return false;

        var hits = new UnityEngine.XR.ARSubsystems.ARRaycastHit[1];
        if (raycastManager.Raycast(screenPos, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            var hit = hits[0];
            worldPos = hit.pose.position;
            Debug.Log($"[ARSessionManager] Pet placement: {worldPos}");
            return true;
        }

        return false;
    }

    public void ResetAR()
    {
        if (arSession != null)
            arSession.Reset();

        Debug.Log("[ARSessionManager] AR session reset");
    }
}
