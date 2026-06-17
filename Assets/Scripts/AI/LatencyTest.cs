using UnityEngine;
using Unity.Barracuda;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

/// <summary>
/// LatencyTest: Comprehensive benchmark component for Yazh 30K ONNX models.
/// Attach to any GameObject in the scene to run latency tests on startup.
///
/// SUPPORTED MODELS (StreamingAssets/MLModels/):
///   yazh-30k.onnx      — FP32, 10.0 MB — Full precision, highest quality
///   yazh-30k-int8.onnx  — INT8,  5.0 MB — Quantized, recommended default
///   yazh-30k-int4.onnx  — INT4,  2.5 MB — Aggressive quant, smallest size
///
/// MODEL SPECIFICATION:
///   Format:        ONNX (Open Neural Network Exchange)
///   Architecture:  Transformer decoder, ~30M parameters
///   Vocabulary:    30,000 Tamil BPE tokens (Unicode U+0B80–U+0BFF + subwords)
///   Context:       256 tokens max
///   Input shape:   [1, 256] int32 token IDs (left-padded)
///   Output shape:  [1, 256, 30000] float32 logits
///   Special tokens: <PAD>=0, <EOS>=2, <UNK>=1
///
/// TARGET LATENCY: < 150ms per inference on mobile CPU (ARM Cortex-A76+)
///
/// ARIVU — Rotation 25 — Jun 17, 2026
/// </summary>
public class LatencyTest : MonoBehaviour
{
    [Header("Model Configuration")]
    [Tooltip("Model file name in StreamingAssets/MLModels/")]
    [SerializeField] private string modelPath = "MLModels/yazh-30k-int8.onnx";

    [Tooltip("Model variant for reporting")]
    [SerializeField] private ModelVariant modelVariant = ModelVariant.INT8;

    [Header("Benchmark Settings")]
    [SerializeField] private int warmupIterations = 5;
    [SerializeField] private int benchmarkIterations = 30;
    [SerializeField] private int maxResponseTokens = 1;

    [Header("Test Prompts (Tamil — child-appropriate)")]
    [SerializeField] private string[] testPrompts = new string[]
    {
        "வணக்கம்",                    // Hello (short)
        "நான் உன்னை நேசிக்கிறேன்",      // I love you (medium)
        "என் பெயர் குருவி",            // My name is Kuruvi (medium)
        "மழை பெய்யுது",               // It's raining (short)
        "கதை சொல்லு",                 // Tell a story (short)
        "நாளை என்ன செய்வோம்",         // What shall we do tomorrow? (long)
        "இனிய நாள்",                   // Have a nice day (short)
        "பறவை பறக்கிறது",             // The bird flies (medium)
        "மரம் உயரமாக இருக்கிறது",      // The tree is tall (long)
        "நீ எங்கே போகிறாய்"            // Where are you going? (medium)
    };

    [Header("Reporting")]
    [SerializeField] private bool logToConsole = true;
    [SerializeField] private bool saveResultsToFile = true;
    [SerializeField] private string outputPath = "latency_results.json";

    [Header("Target Thresholds")]
    [SerializeField] private float targetAvgMs = 150f;
    [SerializeField] private float targetP95Ms = 300f;

    // Results
    private float[] latencies;
    private float minLatency;
    private float maxLatency;
    private float avgLatency;
    private float p50Latency;
    private float p90Latency;
    private float p95Latency;
    private float stdDevLatency;
    private int successCount;
    private string modelInfo;
    private long modelFileSize;

    private Model model;
    private IWorker worker;
    private bool isReady = false;

    public enum ModelVariant { FP32, INT8, INT4 }

    private void Start()
    {
        RunFullBenchmark();
    }

    public void RunFullBenchmark()
    {
        Stopwatch sw = Stopwatch.StartNew();

        Log("===========================================");
        Log("  YAZH 30K MODEL — LATENCY BENCHMARK");
        Log("  ARIVU Rotation 25 | ML Integration");
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

            // Standard deviation
            float sumSqDiff = 0f;
            foreach (float l in validLatencies)
            {
                float diff = l - avgLatency;
                sumSqDiff += diff * diff;
            }
            stdDevLatency = (float)Math.Sqrt(sumSqDiff / successCount);
        }

        sw.Stop();

        // Step 5: Report
        bool passedAvg = avgLatency < targetAvgMs;
        bool passedP95 = p95Latency < targetP95Ms;
        bool passed = passedAvg && passedP95;
        string verdict = passed ? "PASS ✓" : "FAIL ✗";

        Log("\n===========================================");
        Log("  BENCHMARK RESULTS");
        Log("===========================================");
        Log($"  Model File:     {modelPath}");
        Log($"  Model Variant:  {modelVariant}");
        Log($"  Model Size:     {modelFileSize / 1024.0f:F0} KB ({modelFileSize / 1024 / 1024.0f:F1} MB)");
        Log($"  Model Info:     {modelInfo}");
        Log($"  Total Iters:    {benchmarkIterations}");
        Log($"  Warm-up Iters:  {warmupIterations}");
        Log($"  Success:        {successCount}/{benchmarkIterations} ({(float)successCount / benchmarkIterations * 100:F0}%)");
        Log($"  ──────────────────────────────────────");
        Log($"  Min Latency:    {minLatency,8:F2} ms");
        Log($"  Avg Latency:    {avgLatency,8:F2} ms   (target: <{targetAvgMs}ms)  [{(passedAvg ? "PASS" : "FAIL")}]");
        Log($"  P50 Latency:    {p50Latency,8:F2} ms");
        Log($"  P90 Latency:    {p90Latency,8:F2} ms");
        Log($"  P95 Latency:    {p95Latency,8:F2} ms   (target: <{targetP95Ms}ms)  [{(passedP95 ? "PASS" : "FAIL")}]");
        Log($"  Max Latency:    {maxLatency,8:F2} ms");
        Log($"  Std Dev:        {stdDevLatency,8:F2} ms");
        Log($"  ──────────────────────────────────────");
        Log($"  Overall:        {verdict}");
        Log($"  Total Time:     {sw.ElapsedMilliseconds} ms");
        Log("===========================================");

        // Step 6: Save results
        if (saveResultsToFile)
        {
            SaveResults(sw.ElapsedMilliseconds, passed, passedAvg, passedP95);
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
            modelFileSize = modelData.Length;
            Log($"[MODEL] Size: {modelFileSize / 1024.0f:F0} KB ({modelFileSize / 1024 / 1024.0f:F1} MB)");

            model = ModelLoader.Load(modelData);
            if (model == null)
            {
                Log("[MODEL] ERROR: ModelLoader.Load returned null");
                return false;
            }

            // Use ComputePrecompiled for best CPU performance on mobile
            worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
            modelInfo = $"Inputs: {model.inputs.Count}, Outputs: {model.outputs.Count}, " +
                        $"Layers: {model.layers.Count}";
            Log($"[MODEL] Loaded successfully. {modelInfo}");
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
        // Matches YazhTokenizer.Encode() behavior in YazhInferenceManager.cs
        int[] tokens = new int[text.Length + 1];
        for (int i = 0; i < text.Length; i++)
        {
            tokens[i] = (int)text[i] % 30000;
        }
        tokens[text.Length] = 2; // EOS token
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

    private void SaveResults(long totalTimeMs, bool passed, bool passedAvg, bool passedP95)
    {
        try
        {
            string results = $@"{{
  ""model"": ""{modelPath}"",
  ""model_variant"": ""{modelVariant}"",
  ""model_size_bytes"": {modelFileSize},
  ""model_info"": ""{modelInfo}"",
  ""timestamp"": ""{DateTime.Now:yyyy-MM-dd HH:mm:ss}"",
  ""warmup_iterations"": {warmupIterations},
  ""benchmark_iterations"": {benchmarkIterations},
  ""success_count"": {successCount},
  ""success_rate"": {(float)successCount / benchmarkIterations * 100:F1},
  ""latency_ms"": {{
    ""min"": {minLatency:F2},
    ""avg"": {avgLatency:F2},
    ""p50"": {p50Latency:F2},
    ""p90"": {p90Latency:F2},
    ""p95"": {p95Latency:F2},
    ""max"": {maxLatency:F2},
    ""stddev"": {stdDevLatency:F2}
  }},
  ""targets"": {{
    ""avg_target_ms"": {targetAvgMs},
    ""p95_target_ms"": {targetP95Ms},
    ""avg_passed"": {(passedAvg ? "true" : "false")},
    ""p95_passed"": {(passedP95 ? "true" : "false")},
    ""overall_passed"": {(passed ? "true" : "false")}
  }},
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
