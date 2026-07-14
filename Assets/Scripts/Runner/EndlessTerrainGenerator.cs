// EndlessTerrainGenerator.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// Endless tinai terrain: a pool of track segments slides toward the runner
// (the runner stays near the origin — no floating-point drift on long runs).
// Segments recycling to the front are re-themed for the tinai at that distance.

using UnityEngine;
using System.Collections.Generic;

public class EndlessTerrainGenerator : MonoBehaviour
{
    [Header("Track")]
    [SerializeField] private int segmentCount = 7;      // ~210 m visible
    [SerializeField] private float recycleZ = -25f;     // behind the camera

    [Header("Runner reference (for collectible magnet)")]
    public Transform runner;
    public float magnetRadius = 0f; // > 0 only for Pulliruvi

    private readonly List<TinaiSegment> segments = new();
    private float distanceTraveled;
    private TinaiType currentTinai;

    /// <summary>Fired when the track crosses into a new tinai stretch.</summary>
    public static event System.Action<TinaiTheme> OnTinaiChanged;

    public float DistanceTraveled => distanceTraveled;
    public TinaiTheme CurrentTheme => TinaiSystem.GetTheme(currentTinai);

    private void Start()
    {
        currentTinai = TinaiSystem.GetTinaiForDistance(0f);

        for (int i = 0; i < segmentCount; i++)
        {
            var go = new GameObject("TinaiSegment");
            go.transform.SetParent(transform, false);
            var segment = go.AddComponent<TinaiSegment>();
            segment.Build();

            float z = i * TinaiSegment.Length;
            go.transform.position = new Vector3(0f, 0f, z);
            // Theme by where this segment sits on the endless track.
            segment.Populate(TinaiSystem.GetThemeForDistance(distanceTraveled + z),
                             Difficulty01(), runner, magnetRadius);
            segments.Add(segment);
        }

        ApplyAmbience(CurrentTheme);
        OnTinaiChanged?.Invoke(CurrentTheme);
    }

    /// <summary>Advance the world by the given meters (called by RunnerGameManager).</summary>
    public void Advance(float meters)
    {
        distanceTraveled += meters;

        foreach (var segment in segments)
        {
            segment.transform.position += Vector3.back * meters;
        }

        // Recycle segments that fell behind the camera to the front of the track.
        foreach (var segment in segments)
        {
            if (segment.transform.position.z < recycleZ - TinaiSegment.Length / 2f)
            {
                float frontZ = FrontZ();
                float newZ = frontZ + TinaiSegment.Length;
                segment.transform.position = new Vector3(0f, 0f, newZ);
                segment.Populate(TinaiSystem.GetThemeForDistance(distanceTraveled + newZ),
                                 Difficulty01(), runner, magnetRadius);
            }
        }

        // Detect tinai transitions at the runner's position.
        TinaiType tinaiNow = TinaiSystem.GetTinaiForDistance(distanceTraveled);
        if (tinaiNow != currentTinai)
        {
            currentTinai = tinaiNow;
            var theme = TinaiSystem.GetTheme(currentTinai);
            ApplyAmbience(theme);
            OnTinaiChanged?.Invoke(theme);
            Debug.Log($"[EndlessTerrain] Entered tinai: {theme.tamilName} ({theme.englishName}) at {distanceTraveled:F0}m");
        }
    }

    private float FrontZ()
    {
        float front = float.MinValue;
        foreach (var s in segments)
            if (s.transform.position.z > front) front = s.transform.position.z;
        return front;
    }

    /// <summary>Difficulty ramps 0→1 over the first ~2000 m.</summary>
    private float Difficulty01() => Mathf.Clamp01(distanceTraveled / 2000f);

    private void ApplyAmbience(TinaiTheme theme)
    {
        var cam = Camera.main;
        if (cam != null)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = theme.skyColor;
        }
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = theme.skyColor;
        RenderSettings.fogStartDistance = 40f;
        RenderSettings.fogEndDistance = 150f;
    }
}
