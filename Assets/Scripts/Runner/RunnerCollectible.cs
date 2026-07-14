// RunnerCollectible.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// A tinai collectible: தேன் (honey), முல்லை மலர் (jasmine), நெல் (paddy),
// முத்து (pearl), நீர் துளி (water drop). Pulliruvi's song pulls them closer.

using UnityEngine;

public class RunnerCollectible : MonoBehaviour
{
    [HideInInspector] public string tamilName;
    [HideInInspector] public int value = 1;

    private Transform magnetTarget;
    private float magnetRadius;
    private bool collected;

    private const float SpinSpeed = 120f;
    private const float MagnetPullSpeed = 14f;

    /// <summary>Enable magnet attraction toward the runner (Pulliruvi ability).</summary>
    public void SetMagnet(Transform target, float radius)
    {
        magnetTarget = target;
        magnetRadius = radius;
    }

    private void Update()
    {
        transform.Rotate(0f, SpinSpeed * Time.deltaTime, 0f, Space.World);

        if (magnetTarget == null || collected) return;

        // Pull toward the runner when inside the magnet radius.
        Vector3 toTarget = magnetTarget.position - transform.position;
        if (toTarget.sqrMagnitude < magnetRadius * magnetRadius)
        {
            transform.position += toTarget.normalized * (MagnetPullSpeed * Time.deltaTime);
        }
    }

    /// <summary>Consume the collectible. Returns false if already taken.</summary>
    public bool Collect()
    {
        if (collected) return false;
        collected = true;
        gameObject.SetActive(false);
        return true;
    }

    /// <summary>Reset for pool reuse when the segment recycles.</summary>
    public void ResetForReuse()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}
