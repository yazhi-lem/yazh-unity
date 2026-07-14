// TinaiSegment.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// One pooled stretch of track. Built entirely from primitives (code-first
// project — no art assets yet), re-themed and re-populated each recycle.

using UnityEngine;
using System.Collections.Generic;

public class TinaiSegment : MonoBehaviour
{
    public const float Length = 30f;
    public const float LaneWidth = 2f;      // lanes at x = -2, 0, +2
    public const int LaneCount = 3;

    private Renderer groundRenderer;
    private readonly List<Renderer> decorRenderers = new();
    private readonly List<GameObject> obstacles = new();
    private readonly List<GameObject> collectibles = new();

    private Transform contentRoot; // obstacles + collectibles, cleared on recycle

    /// <summary>Build the static geometry once (ground + side decor slots).</summary>
    public void Build()
    {
        // Ground slab spanning all three lanes.
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.SetParent(transform, false);
        ground.transform.localPosition = new Vector3(0f, -0.25f, 0f);
        ground.transform.localScale = new Vector3(LaneWidth * LaneCount + 2f, 0.5f, Length);
        groundRenderer = ground.GetComponent<Renderer>();
        Destroy(ground.GetComponent<Collider>()); // runner never physically lands on it

        // Side decor: simple pillars each side (trees / crags / palms by color).
        for (int side = -1; side <= 1; side += 2)
        {
            for (int i = 0; i < 4; i++)
            {
                var decor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                decor.name = "Decor";
                decor.transform.SetParent(transform, false);
                float z = -Length / 2f + (i + 0.5f) * (Length / 4f);
                decor.transform.localPosition = new Vector3(side * (LaneWidth * 2f + 1.5f), 1.5f, z);
                decor.transform.localScale = new Vector3(0.6f, 1.5f, 0.6f);
                Destroy(decor.GetComponent<Collider>());
                decorRenderers.Add(decor.GetComponent<Renderer>());
            }
        }

        contentRoot = new GameObject("Content").transform;
        contentRoot.SetParent(transform, false);
    }

    /// <summary>
    /// Theme + populate this segment for a given track distance.
    /// Called on first spawn and on every recycle to the front.
    /// </summary>
    public void Populate(TinaiTheme theme, float difficulty01, Transform magnetTarget, float magnetRadius)
    {
        name = $"Segment_{theme.englishName}";

        SetColor(groundRenderer, theme.groundColor);
        foreach (var r in decorRenderers) SetColor(r, theme.decorColor);

        ClearContent();

        // Obstacle rows: a few z-positions along the segment, each row places
        // obstacles on some lanes but always leaves at least one lane open.
        int rows = 2 + Mathf.FloorToInt(difficulty01 * 2f); // 2–4 rows
        float density = Mathf.Clamp01(theme.obstacleDensity + difficulty01 * 0.15f);

        for (int row = 0; row < rows; row++)
        {
            float z = -Length / 2f + (row + 1) * (Length / (rows + 1));
            int openLane = Random.Range(0, LaneCount); // guaranteed escape route

            for (int lane = 0; lane < LaneCount; lane++)
            {
                if (lane == openLane) continue;
                if (Random.value > density) continue;
                SpawnObstacle(lane, z, theme);
            }
        }

        // Collectible run: a short line of pickups along one lane.
        if (Random.value < theme.collectibleDensity)
        {
            int lane = Random.Range(0, LaneCount);
            float startZ = -Length / 2f + Random.Range(4f, 10f);
            int count = Random.Range(3, 6);
            for (int i = 0; i < count; i++)
            {
                SpawnCollectible(lane, startZ + i * 2f, theme, magnetTarget, magnetRadius);
            }
        }
    }

    private void SpawnObstacle(int lane, float z, TinaiTheme theme)
    {
        var kind = (RunnerObstacle.ObstacleKind)Random.Range(0, 3);
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(contentRoot, false);
        float x = (lane - 1) * LaneWidth;

        switch (kind)
        {
            case RunnerObstacle.ObstacleKind.LowBarrier:
                go.transform.localPosition = new Vector3(x, 0.4f, z);
                go.transform.localScale = new Vector3(LaneWidth * 0.9f, 0.8f, 0.5f);
                break;
            case RunnerObstacle.ObstacleKind.HighBarrier:
                go.transform.localPosition = new Vector3(x, 1.6f, z);
                go.transform.localScale = new Vector3(LaneWidth * 0.9f, 0.8f, 0.5f);
                break;
            case RunnerObstacle.ObstacleKind.FullBlock:
                go.transform.localPosition = new Vector3(x, 1.1f, z);
                go.transform.localScale = new Vector3(LaneWidth * 0.9f, 2.2f, 0.8f);
                break;
        }

        go.name = $"Obstacle_{kind}";
        SetColor(go.GetComponent<Renderer>(), theme.obstacleColor);
        go.GetComponent<Collider>().isTrigger = true;
        var obstacle = go.AddComponent<RunnerObstacle>();
        obstacle.kind = kind;
        obstacles.Add(go);
    }

    private void SpawnCollectible(int lane, float z, TinaiTheme theme, Transform magnetTarget, float magnetRadius)
    {
        if (z > Length / 2f - 1f) return;

        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = $"Collectible_{theme.collectibleName}";
        go.transform.SetParent(contentRoot, false);
        go.transform.localPosition = new Vector3((lane - 1) * LaneWidth, 1f, z);
        go.transform.localScale = Vector3.one * 0.5f;
        SetColor(go.GetComponent<Renderer>(), theme.collectibleColor);
        go.GetComponent<Collider>().isTrigger = true;

        var collectible = go.AddComponent<RunnerCollectible>();
        collectible.tamilName = theme.collectibleName;
        collectible.SetMagnet(magnetTarget, magnetRadius);
        collectibles.Add(go);
    }

    private void ClearContent()
    {
        foreach (var go in obstacles) if (go != null) Destroy(go);
        foreach (var go in collectibles) if (go != null) Destroy(go);
        obstacles.Clear();
        collectibles.Clear();
    }

    private static void SetColor(Renderer r, Color c)
    {
        if (r == null) return;
        // Per-renderer material instance; fine at this scale (few dozen renderers).
        r.material.color = c;
    }
}
