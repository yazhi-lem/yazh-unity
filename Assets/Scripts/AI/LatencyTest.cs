// LatencyTest.cs — YAZH-UNITY
// ARIVU | Rotation 25 | Jun 17, 2026
// Benchmarks Yazh 30K ONNX model inference latency on target hardware

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LatencyTest : MonoBehaviour
{
    [Header("Test Configuration")]
    public int warmupIterations = 5;
    public int benchmarkIterations = 20; // Clamped to [1, 100]
    public string testInput = "வணக்கம்"; // "Hello" in Tamil

    [Header("Output")]
    public string resultsPath = "latency_results.json";

    private YazhInferenceManager inference;
    private List<float> latencies = new List<float>();
    private bool testRunning = false;

    void Start()
    {
        inference = YazhInferenceManager.Instance;
        if (inference == null)
        {
            Debug.LogError("[LatencyTest] YazhInferenceManager not found!");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Run the full latency benchmark. Call from another script or UI button.
    /// </summary>
    public void RunBenchmark()
    {
        if (testRunning)
        {
            Debug.LogWarning("[LatencyTest] Test already running!");
            return;
        }
        StartCoroutine(BenchmarkCoroutine());
    }

    IEnumerator BenchmarkCoroutine()
    {
        testRunning = true;
        latencies.Clear();

        // Validate iteration count
        benchmarkIterations = Mathf.Clamp(benchmarkIterations, 1, 100);

        Debug.Log("[LatencyTest] Starting benchmark — warmup: " + warmupIterations +
                  ", iterations: " + benchmarkIterations);

        // Wait for model to be ready
        while (!inference.IsModelReady())
        {
            Debug.Log("[LatencyTest] Waiting for model...");
            yield return new WaitForSeconds(1f);
        }

        // Warmup phase
        Debug.Log("[LatencyTest] Warmup phase...");
        for (int i = 0; i < warmupIterations; i++)
        {
            bool done = false;
            inference.GenerateResponse(testInput, (result) => { done = true; });
            while (!done) yield return null;
            yield return new WaitForSeconds(0.1f);
        }

        // Benchmark phase
        Debug.Log("[LatencyTest] Benchmark phase...");
        for (int i = 0; i < benchmarkIterations; i++)
        {
            float startTime = Time.realtimeSinceStartup;
            bool done = false;

            inference.GenerateResponse(testInput, (result) => {
                done = true;
            });

            while (!done) yield return null;

            float elapsed = (Time.realtimeSinceStartup - startTime) * 1000f; // ms
            latencies.Add(elapsed);

            Debug.Log("[LatencyTest] Iteration " + (i + 1) + ": " + elapsed.ToString("F1") + "ms");
            yield return new WaitForSeconds(0.05f); // Small gap between iterations
        }

        // Calculate results
        float min = float.MaxValue, max = 0, sum = 0;
        foreach (float l in latencies)
        {
            if (l < min) min = l;
            if (l > max) max = l;
            sum += l;
        }
        float avg = sum / latencies.Count;

        // Calculate median
        List<float> sorted = new List<float>(latencies);
        sorted.Sort();
        float median = sorted[sorted.Count / 2];

        // Calculate p95
        int p95Index = Mathf.FloorToInt(sorted.Count * 0.95f);
        float p95 = sorted[Mathf.Clamp(p95Index, 0, sorted.Count - 1)];

        // Target check
        bool meetsTarget = avg < 150f; // 150ms target

        // Build results JSON
        string json = "{\n";
        json += "  \"timestamp\": \"" + System.DateTime.UtcNow.ToString("o") + "\",\n";
        json += "  \"model\": \"Yazh 30K\",\n";
        json += "  \"test_input\": \"" + testInput + "\",\n";
        json += "  \"iterations\": " + latencies.Count + ",\n";
        json += "  \"results_ms\": {\n";
        json += "    \"min\": " + min.ToString("F2") + ",\n";
        json += "    \"max\": " + max.ToString("F2") + ",\n";
        json += "    \"avg\": " + avg.ToString("F2") + ",\n";
        json += "    \"median\": " + median.ToString("F2") + ",\n";
        json += "    \"p95\": " + p95.ToString("F2") + "\n";
        json += "  },\n";
        json += "  \"target_ms\": 150,\n";
        json += "  \"meets_target\": " + meetsTarget.ToString().ToLower() + ",\n";
        json += "  \"all_latencies\": [";
        for (int i = 0; i < latencies.Count; i++)
        {
            json += latencies[i].ToString("F2");
            if (i < latencies.Count - 1) json += ", ";
        }
        json += "]\n";
        json += "}";

        // Save results
        string path = Path.Combine(Application.persistentDataPath, resultsPath);
        File.WriteAllText(path, json);

        // Log summary
        Debug.Log("=== LATENCY TEST RESULTS ===");
        Debug.Log("Model: Yazh 30K");
        Debug.Log("Iterations: " + latencies.Count);
        Debug.Log("Min: " + min.ToString("F1") + "ms");
        Debug.Log("Max: " + max.ToString("F1") + "ms");
        Debug.Log("Avg: " + avg.ToString("F1") + "ms");
        Debug.Log("Median: " + median.ToString("F1") + "ms");
        Debug.Log("P95: " + p95.ToString("F1") + "ms");
        Debug.Log("Target (<150ms): " + (meetsTarget ? "PASS ✓" : "FAIL ✗"));
        Debug.Log("Results saved to: " + path);
        Debug.Log("===========================");

        testRunning = false;
    }

    /// <summary>
    /// Quick single inference test. Returns latency in ms.
    /// </summary>
    public IEnumerator QuickTest()
    {
        if (inference == null || !inference.IsModelReady())
        {
            Debug.LogError("[LatencyTest] Model not ready!");
            yield break;
        }

        float start = Time.realtimeSinceStartup;
        bool done = false;
        string response = "";

        inference.GenerateResponse(testInput, (result) => {
            response = result;
            done = true;
        });

        while (!done) yield return null;

        float latency = (Time.realtimeSinceStartup - start) * 1000f;
        Debug.Log("[LatencyTest] Quick test: " + latency.ToString("F1") +
                  "ms | Response: " + response);
    }
}
