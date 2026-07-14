// RunnerObstacle.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// An obstacle on the track. Jump it, slide under it, dodge it —
// or, if you are Yanai, smash straight through the breakable ones.

using UnityEngine;

public class RunnerObstacle : MonoBehaviour
{
    public enum ObstacleKind
    {
        LowBarrier,   // jump over (rocks, logs, nets)
        HighBarrier,  // slide under (branches, cart yokes)
        FullBlock     // change lane (boulders, boats)
    }

    public ObstacleKind kind;

    /// <summary>Low barriers are breakable — Yanai (elephant) smashes through.</summary>
    public bool IsBreakable => kind == ObstacleKind.LowBarrier;

    /// <summary>Collapse the obstacle after a Yanai smash.</summary>
    public void Smash()
    {
        // Flatten and disable — the segment pool re-creates obstacles on recycle.
        transform.localScale = new Vector3(transform.localScale.x, 0.05f, transform.localScale.z);
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Debug.Log("[RunnerObstacle] Smashed by Yanai! உடைந்தது!");
    }
}
