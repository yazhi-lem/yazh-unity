using UnityEngine;
using Unity.Barracuda;
using System;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

/// <summary>
/// LatencyTest: Standalone benchmark component for Yazh 30K ONNX model.
/// Attach to any GameObject in the scene to run latency tests on startup.
/// Measures inference latency across multiple Tamil prompts and reports statistics.
/// </summary>
public class LatencyTest : MonoBehaviour
{
    [Header("Model Configuration")]
    [SerializeField] private string modelPath = "MLModels/yazh-30k-int8.onnx";
    [SerializeField] private int warmupIterations = 3;
    [SerializeField] private int benchmarkIterations = 20;

    [Header("Test Prompts (Tamil)")]
    [SerializeField] private string[] testPrompts = new string[]
    {
        "வணக்கம்",
        "நான் உன்னை நேசிக்கிறேன்",
        "என் பெயர் குருவி",
        "மழை பெய்யுது",
        "கதை சொல்லு",
        "நாளை என்ன செய்வோம்",
        "இனிய நாள்",
        "பறவை பறக்கிறது",
        "மரம் உயரமாக இருக்கிறது",
        "நீ எங்கே போகிறாய்"
    };

    [Header("Reporting")]
    [SerializeField] private bool logToConsole = true;
    [SerializeField] private bool saveResultsToFile = true;
    [SerializeField] private string outputPath = "latency_results.json";

    // Results
    private float[] latencies;
    private float minLatency;
    private float maxLatency;
    private float avgLatency;
    private float p50Latency;
    private float p90Latency;
    private float p95Latency;
    private int successCount;
    private string modelInfo;

    private Model model;
    private IWorker worker;
    private bool isReady = false;

    private void Start()
    {
        RunFullBenchmark();
    }

    public void RunFullBenchmark()
    {
        Stopwatch sw = Stopwatch.StartNew();

        Log("===========================================");
        Log("  YAZH 30K MODEL — LATENCY TEST");
        Log("===========================================");

        // Step 1: Load model
        if (!LoadModel())
        {
            Log("FATAL: Model loading failed. Aborting benchmark.");
            return;
        }

        // Step 2: Warm-up phase
        Log($"\n[WARM-UP] Running {warmupIterations} warm-up iterations...");
        for (int i = 0; i < warmupIterations; i++)
        {
            try
            {
                RunSingleInference("வணக்கம்");
                Log($"  Warm-up {i + 1}/{warmupIterations} — OK");
            }
            catch (Exception e)
            {
                Log($"  Warm-up {i + 1}/{warmupIterations} — FAILED: {e.Message}");
            }
        }

        // Step 3: Benchmark phase
        Log($"\n[BENCHMARK] Running {benchmarkIterations} iterations...");
        latencies = new float[benchmarkIterations];
        successCount = 0;
        minLatency = float.MaxValue;
        maxLatency = 0f;
        float totalLatency = 0f;

        for (int i = 0; i < benchmarkIterations; i++)
        {
            string prompt = testPrompts[i % testPrompts.Length];
            try
            {
                float latency = RunSingleInference(prompt);
                latencies[i] = latency;
                successCount++;
                totalLatency += latency;
                if (latency < minLatency) minLatency = latency;
                if (latency > maxLatency) maxLatency = latency;
                Log($"  [{i + 1:D2}/{benchmarkIterations}] {latency,8:F2}ms — \"{prompt}\"");
            }
            catch (Exception e)
            {
                latencies[i] = -1f;
                Log($"  [{i + 1:D2}/{benchmarkIterations}]    FAILED — \"{prompt}\": {e.Message}");
            }
        }

        // Step 4: Compute statistics
        if (successCount > 0)
        {
            avgLatency = totalLatency / successCount;

            // Sort for percentile calculation
            float[] validLatencies = new float[successCount];
            int idx = 0;
            for (int i = 0; i < benchmarkIterations; i++)
            {
                if (latencies[i] >= 0)
                    validLatencies[idx++] = latencies[i];
            }
            Array.Sort(validLatencies);

            p50Latency = GetPercentile(validLatencies, 50);
            p90Latency = GetPercentile(validLatencies, 90);
            p95Latency = GetPercentile(validLatencies, 95);
        }

        sw.Stop();

        // Step 5: Report
        bool passed = avgLatency < 150f;
        string verdict = passed ? "PASS" : "FAIL";

        Log("\n===========================================");
        Log("  RESULTS");
        Log("===========================================");
        Log($"  Model:          {modelPath}");
        Log($"  Model Info:     {modelInfo}");
        Log($"  Total Iters:    {benchmarkIterations}");
        Log($"  Success:        {successCount}/{benchmarkIterations}");
        Log($"  Min Latency:    {minLatency:F2} ms");
        Log($"  Avg Latency:    {avgLatency:F2} ms");
        Log($"  P50 Latency:    {p50Latency:F2} ms");
        Log($"  P90 Latency:    {p90Latency:F2} ms");
        Log($"  P95 Latency:    {p95Latency:F2} ms");
        Log($"  Max Latency:    {maxLatency:F2} ms");
        Log($"  Target:         < 150 ms");
        Log($"  Verdict:        {verdict}");
        Log($"  Total Time:     {sw.ElapsedMilliseconds} ms");
        Log("===========================================");

        // Step 6: Save results
        if (saveResultsToFile)
        {
            SaveResults(sw.ElapsedMilliseconds);
        }

        // Cleanup
        worker?.Dispose();
    }

    private bool LoadModel()
    {
        try
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, modelPath);
            Log($"[MODEL] Loading from: {fullPath}");

            if (!File.Exists(fullPath))
            {
                Log($"[MODEL] ERROR: File not found at {fullPath}");
                return false;
            }

            byte[] modelData = File.ReadAllBytes(fullPath);
            Log($"[MODEL] Size: {modelData.Length / 1024.0f:F1} KB");

            model = ModelLoader.Load(modelData);
            if (model == null)
            {
                Log("[MODEL] ERROR: ModelLoader.Load returned null");
                return false;
            }

            worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
            modelInfo = $"Inputs: {model.inputs.Count}, Outputs: {model.outputs.Count}, " +
                        $"Layers: {model.layers.Count}";
            Log($"[MODEL] Loaded. {modelInfo}");
            isReady = true;
            return true;
        }
        catch (Exception e)
        {
            Log($"[MODEL] ERROR: {e.Message}");
            return false;
        }
    }

    private float RunSingleInference(string prompt)
    {
        Stopwatch sw = Stopwatch.StartNew();

        // Simple tokenization: map chars to int tokens
        int[] tokens = SimpleTokenize(prompt);

        // Create input tensor [1, 256] — pad to MAX_CONTEXT_TOKENS
        Tensor input = new Tensor(1, 256, tokens);

        // Run inference
        worker.Execute(input);

        // Get output
        Tensor output = worker.PeekOutput();

        // Greedy decode: find argmax
        int bestToken = 0;
        float bestVal = float.MinValue;
        for (int i = 0; i < output.channels; i++)
        {
            float val = output[0, 0, 0, i];
            if (val > bestVal)
            {
                bestVal = val;
                bestToken = i;
            }
        }

        input.Dispose();

        sw.Stop();
        return (float)sw.Elapsed.TotalMilliseconds;
    }

    private int[] SimpleTokenize(string text)
    {
        // Simple char-level tokenization for benchmarking
        int[] tokens = new int[text.Length + 1];
        for (int i = 0; i < text.Length; i++)
        {
            tokens[i] = (int)text[i] % 30000;
        }
        tokens[text.Length] = 2; // EOS
        return tokens;
    }

    private float GetPercentile(float[] sorted, int percentile)
    {
        if (sorted.Length == 0) return 0f;
        float index = (percentile / 100f) * (sorted.Length - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);
        if (lower == upper) return sorted[lower];
        float frac = index - lower;
        return sorted[lower] * (1 - frac) + sorted[upper] * frac;
    }

    private void SaveResults(long totalTimeMs)
    {
        try
        {
            string results = $@"{{
  ""model"": ""{modelPath}"",
  ""model_info"": ""{modelInfo}"",
  ""timestamp"": ""{DateTime.Now:yyyy-MM-dd HH:mm:ss}"",
  ""warmup_iterations"": {warmupIterations},
  ""benchmark_iterations"": {benchmarkIterations},
  ""success_count"": {successCount},
  ""latency_ms"": {{
    ""min"": {minLatency:F2},
    ""avg"": {avgLatency:F2},
    ""p50"": {p50Latency:F2},
    ""p90"": {p90Latency:F2},
    ""p95"": {p95Latency:F2},
    ""max"": {maxLatency:F2}
  }},
  ""target_ms"": 150,
  ""passed"": {(avgLatency < 150f ? "true" : "false")},
  ""total_time_ms"": {totalTimeMs},
  ""raw_latencies"": [{string.Join(", ", Array.ConvertAll(latencies, x => x >= 0 ? x.ToString("F2") : "\"failed\""))}]
}}";

            string filePath = Path.Combine(Application.dataPath, "..", outputPath);
            File.WriteAllText(filePath, results);
            Log($"[SAVE] Results saved to: {filePath}");
        }
        catch (Exception e)
        {
            Log($"[SAVE] ERROR: {e.Message}");
        }
    }

    private void Log(string message)
    {
        if (logToConsole)
        {
            Debug.Log(message);
        }
    }
}
