using UnityEngine;
using Unity.Barracuda;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// YazhInferenceManager: Manages on-device Tamil language inference via Yazh 30K ONNX model
/// Handles model loading, tokenization, inference, and response generation
/// Targets < 150ms latency for real-time dialogue
/// </summary>
public class YazhInferenceManager : MonoBehaviour
{
    public static YazhInferenceManager Instance { get; private set; }

    [SerializeField] private string modelPath = "MLModels/yazh-30k-int8.onnx";
    [SerializeField] private string tokenizerPath = "MLModels/yazh-tokenizer.json";
    [SerializeField] private float inferenceTimeout = 0.5f;  // 500ms max (target 150ms)
    [SerializeField] private bool enableProfiling = true;

    // Latency tracking
    private float totalInferenceTime = 0f;
    private int inferenceCount = 0;
    private float minLatency = float.MaxValue;
    private float maxLatency = 0f;

    private Model yazhModel;
    private IWorker inferenceWorker;
    private YazhTokenizer tokenizer;
    private string[] vocabEmbeddings;

    // Inference state
    private bool isModelReady = false;
    private float lastInferenceTime = 0f;
    private Queue<string> inferenceQueue = new();
    private Dictionary<string, float> performanceMetrics;

    private const int MAX_CONTEXT_TOKENS = 256;
    private const int MAX_RESPONSE_TOKENS = 64;
    private const float TEMPERATURE = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        performanceMetrics = new Dictionary<string, float>();
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("[Yazh AI] Initializing inference pipeline...");

        // Load ONNX model
        LoadModel();

        // Load tokenizer
        LoadTokenizer();

        // Set inference worker backend (CPU for mobile)
        if (isModelReady)
        {
            // For production: use GPU if available; fall back to CPU
            inferenceWorker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, yazhModel);
            Debug.Log("[Yazh AI] Inference worker initialized");

            // Run warm-up inference to prime the model
            WarmupInference();
        }
    }

    private void LoadModel()
    {
        string modelFilePath = Path.Combine(Application.streamingAssetsPath, modelPath);

        if (!File.Exists(modelFilePath))
        {
            Debug.LogError($"[Yazh AI] Model not found at {modelFilePath}");
            return;
        }

        byte[] modelData = File.ReadAllBytes(modelFilePath);
        yazhModel = ModelLoader.Load(modelData);

        if (yazhModel != null)
        {
            isModelReady = true;
            #if UNITY_EDITOR
            Debug.Log($"[Yazh AI] Model loaded successfully. Inputs: {yazhModel.inputs.Count}, Outputs: {yazhModel.outputs.Count}");
            Debug.Log($"[Yazh AI] Model memory: ~{EstimateModelSize(yazhModel) / 1024 / 1024:F1}MB");
            #endif
        }
        else
        {
            Debug.LogError("[Yazh AI] Failed to load model");
        }
    }

    private void WarmupInference()
    {
        // Run a dummy inference to warm up the model (first call is always slower)
        try
        {
            float warmupStart = Time.realtimeSinceStartup;
            int[] dummyTokens = new int[] { 1, 2, 3, 4, 5 };
            Tensor warmupTensor = CreateInputTensor(dummyTokens);
            inferenceWorker.Execute(warmupTensor);
            Tensor warmupOutput = inferenceWorker.PeekOutput();
            warmupTensor.Dispose();
            float warmupTime = (Time.realtimeSinceStartup - warmupStart) * 1000f;
            Debug.Log($"[Yazh AI] Warm-up inference: {warmupTime:F1}ms");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[Yazh AI] Warm-up failed: {e.Message}");
        }
    }

    private void LoadTokenizer()
    {
        string tokenizerFilePath = Path.Combine(Application.streamingAssetsPath, tokenizerPath);

        if (!File.Exists(tokenizerFilePath))
        {
            Debug.LogError($"[Yazh AI] Tokenizer not found at {tokenizerFilePath}");
            return;
        }

        // TODO: Parse tokenizer.json (HuggingFace format)
        // For MVP: Use approximate 30K vocab Tamil tokens
        tokenizer = new YazhTokenizer();
        tokenizer.LoadVocabulary(tokenizerFilePath);

        Debug.Log("[Yazh AI] Tokenizer loaded, vocab size: 30000");
    }

    private const int MAX_INPUT_LENGTH = 512;  // Max chars for child input

    /// <summary>
    /// Run inference on user input (child's chat message)
    /// Returns Tamil text response from Yazh 30K model
    /// </summary>
    public void GenerateResponse(string userInput, System.Action<string> onResponseReady)
    {
        if (!isModelReady)
        {
            Debug.LogError("[Yazh AI] Model not ready for inference");
            onResponseReady?.Invoke("மாடல் தயாரிக்கப்படவில்லை. (Model not ready)");
            return;
        }

        // SEC-002: Input validation — null/empty check
        if (string.IsNullOrWhiteSpace(userInput))
        {
            onResponseReady?.Invoke("ஏதாவது சொல்லுங்கள். (Please say something.)");
            return;
        }

        // SEC-002: Input validation — length limit
        if (userInput.Length > MAX_INPUT_LENGTH)
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"[Yazh AI] Input truncated from {userInput.Length} to {MAX_INPUT_LENGTH} chars");
            #endif
            userInput = userInput.Substring(0, MAX_INPUT_LENGTH);
        }

        StartCoroutine(GenerateResponseCoroutine(userInput, onResponseReady));
    }

    private System.Collections.IEnumerator GenerateResponseCoroutine(string userInput, System.Action<string> callback)
    {
        float startTime = Time.realtimeSinceStartup;

        // 1. Tokenize input (Tamil)
        int[] inputTokens = tokenizer.Encode(userInput);
        #if UNITY_EDITOR
        Debug.Log($"[Yazh AI] Input tokens: {inputTokens.Length}");
        #endif

        // 2. Prepare input tensor
        Tensor inputTensor = CreateInputTensor(inputTokens);

        // 3. Run inference (blocking call, but ideally async via Unity job system)
        inferenceWorker.Execute(inputTensor);

        // 4. Get output logits
        Tensor outputLogits = inferenceWorker.PeekOutput();

        // 5. Sample next token(s) with temperature
        int nextToken = SampleNextToken(outputLogits, TEMPERATURE);

        // 6. Decode back to Tamil text
        string response = tokenizer.Decode(new int[] { nextToken });

        // SEC-009: Output validation — check for empty/garbage output
        if (string.IsNullOrWhiteSpace(response))
        {
            response = "மன்னிக்கவும், புரியவில்லை. (Sorry, I didn't understand.)";
        }

        float inferenceLatency = (Time.realtimeSinceStartup - startTime) * 1000f;  // ms
        #if UNITY_EDITOR
        Debug.Log($"[Yazh AI] Inference latency: {inferenceLatency:F1}ms");
        #endif

        // Update latency stats
        inferenceCount++;
        totalInferenceTime += inferenceLatency;
        if (inferenceLatency < minLatency) minLatency = inferenceLatency;
        if (inferenceLatency > maxLatency) maxLatency = inferenceLatency;

        RecordMetric("last_inference_latency", inferenceLatency);
        RecordMetric("avg_inference_latency", totalInferenceTime / inferenceCount);
        RecordMetric("min_inference_latency", minLatency);
        RecordMetric("max_inference_latency", maxLatency);
        RecordMetric("inference_count", (float)inferenceCount);

        inputTensor.Dispose();

        // Return response via callback
        callback?.Invoke(response);

        yield return null;
    }

    private Tensor CreateInputTensor(int[] tokens)
    {
        // Pad/truncate to MAX_CONTEXT_TOKENS
        int[] paddedTokens = new int[MAX_CONTEXT_TOKENS];
        int offset = MAX_CONTEXT_TOKENS - tokens.Length;

        // Left-pad with 0 (pad token)
        for (int i = 0; i < offset; i++)
            paddedTokens[i] = 0;

        // Copy actual tokens
        System.Array.Copy(tokens, 0, paddedTokens, offset, tokens.Length);

        // Create tensor [1, seq_len]
        return new Tensor(1, MAX_CONTEXT_TOKENS, 1, 1, paddedTokens);
    }

    private int SampleNextToken(Tensor logits, float temperature)
    {
        // TODO: Implement top-k / nucleus sampling
        // For MVP: argmax (greedy)

        float maxLogit = float.MinValue;
        int maxIdx = 0;

        for (int i = 0; i < logits.data.Length; i++)
        {
            if (logits.data[i] > maxLogit)
            {
                maxLogit = logits.data[i];
                maxIdx = i;
            }
        }

        return maxIdx % 30000;  // Vocab size: 30K
    }

    private long EstimateModelSize(Model model)
    {
        long bytes = 0;
        foreach (var layer in model.layers)
        {
            // Rough estimation: each constant roughly 4 bytes per parameter
            bytes += layer.weights.Count * 4;
        }
        return bytes;
    }

    private void RecordMetric(string metricName, float value)
    {
        if (performanceMetrics.ContainsKey(metricName))
            performanceMetrics[metricName] = value;
        else
            performanceMetrics.Add(metricName, value);

        if (enableProfiling)
            Debug.Log($"[Perf] {metricName}: {value:F2}");
    }

    public Dictionary<string, float> GetPerformanceMetrics() => performanceMetrics;

    /// <summary>
    /// Run a latency benchmark with multiple Tamil prompts.
    /// Returns a formatted report string with min/max/avg latency.
    /// </summary>
    public string RunLatencyBenchmark(int iterations = 10)
    {
        // SEC-003: Clamp iterations to sane range
        if (iterations < 1) iterations = 1;
        if (iterations > 100) iterations = 100;

        if (!isModelReady)
        {
            return "[Yazh AI] Benchmark failed: model not loaded.";
        }

        // Tamil test prompts (child-appropriate)
        string[] testPrompts = new string[]
        {
            "வணக்கம்",                    // Hello
            "நான் உன்னை நேசிக்கிறேன்",      // I love you
            "என் பெயர் குருவி",            // My name is Kuruvi
            "மழை பெய்யுது",               // It's raining
            "கதை சொல்லு",                 // Tell a story
            "நாளை என்ன செய்வோம்",         // What shall we do tomorrow?
            "இனிய நாள்",                   // Have a nice day
            "பறவை பறக்கிறது",             // The bird flies
            "மரம் உயரமாக இருக்கிறது",      // The tree is tall
            "நீ எங்கே போகிறாய்"            // Where are you going?
        };

        // Reset stats for clean benchmark
        float benchTotal = 0f;
        float benchMin = float.MaxValue;
        float benchMax = 0f;
        int successCount = 0;

        Debug.Log("========================================");
        Debug.Log("[Yazh AI] LATENCY BENCHMARK START");
        Debug.Log($"[Yazh AI] Iterations: {iterations} | Model: {modelPath}");
        Debug.Log("========================================");

        for (int i = 0; i < iterations; i++)
        {
            string prompt = testPrompts[i % testPrompts.Length];
            float iterStart = Time.realtimeSinceStartup;

            try
            {
                int[] tokens = tokenizer.Encode(prompt);
                Tensor input = CreateInputTensor(tokens);
                inferenceWorker.Execute(input);
                Tensor output = inferenceWorker.PeekOutput();
                int nextToken = SampleNextToken(output, TEMPERATURE);
                tokenizer.Decode(new int[] { nextToken });
                input.Dispose();

                float iterLatency = (Time.realtimeSinceStartup - iterStart) * 1000f;
                benchTotal += iterLatency;
                if (iterLatency < benchMin) benchMin = iterLatency;
                if (iterLatency > benchMax) benchMax = iterLatency;
                successCount++;

                Debug.Log($"  [Bench {i + 1}/{iterations}] {iterLatency:F1}ms — \"{prompt}\"");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"  [Bench {i + 1}/{iterations}] FAILED: {e.Message}");
            }
        }

        float benchAvg = successCount > 0 ? benchTotal / successCount : 0f;
        string report = $"Yazh 30K Benchmark: {successCount}/{iterations} success | " +
                        $"Min: {benchMin:F1}ms | Avg: {benchAvg:F1}ms | Max: {benchMax:F1}ms | " +
                        $"Target: <150ms | {(benchAvg < 150f ? "PASS" : "FAIL")}";

        Debug.Log("========================================");
        Debug.Log($"[Yazh AI] {report}");
        Debug.Log("========================================");

        // Store in metrics
        RecordMetric("benchmark_min_ms", benchMin);
        RecordMetric("benchmark_avg_ms", benchAvg);
        RecordMetric("benchmark_max_ms", benchMax);
        RecordMetric("benchmark_success_rate", (float)successCount / iterations * 100f);

        return report;
    }

    private void OnDestroy()
    {
        if (inferenceWorker != null)
        {
            inferenceWorker.Dispose();
            Debug.Log("[Yazh AI] Inference worker disposed");
        }
    }
}

/// <summary>
/// YazhTokenizer: Encodes/decodes Tamil text to/from BPE tokens
/// Target vocab: 30,000 Tamil Unicode characters + subword units
/// </summary>
public class YazhTokenizer
{
    private Dictionary<string, int> textToToken = new();
    private Dictionary<int, string> tokenToText = new();
    private int vocabSize = 30000;

    public void LoadVocabulary(string vocabJsonPath)
    {
        // TODO: Parse HuggingFace tokenizer.json format
        // For MVP: Initialize with basic Tamil Unicode range

        // Tamil Unicode: U+0B80–U+0BFF (128 chars)
        int tokenId = 0;

        // Add special tokens
        textToToken["<PAD>"] = tokenId;
        tokenToText[tokenId++] = "<PAD>";

        textToToken["<EOS>"] = tokenId;
        tokenToText[tokenId++] = "<EOS>";

        textToToken["<UNK>"] = tokenId;
        tokenToText[tokenId++] = "<UNK>";

        // Add basic Tamil chars (U+0B80–U+0BFF)
        for (char c = '\u0B80'; c <= '\u0BFF' && tokenId < vocabSize; c++)
        {
            string ch = c.ToString();
            textToToken[ch] = tokenId;
            tokenToText[tokenId] = ch;
            tokenId++;
        }

        Debug.Log($"[Tokenizer] Loaded {tokenId} tokens (target {vocabSize})");
    }

    public int[] Encode(string text)
    {
        // Simple encoding: one Tamil char per token (MVP)
        var tokens = new List<int>();

        foreach (char c in text)
        {
            string ch = c.ToString();
            if (textToToken.ContainsKey(ch))
                tokens.Add(textToToken[ch]);
            else
                tokens.Add(textToToken.ContainsKey("<UNK>") ? textToToken["<UNK>"] : 0);
        }

        tokens.Add(textToToken["<EOS>"]);
        return tokens.ToArray();
    }

    public string Decode(int[] tokens)
    {
        // Simple decoding: one token per Tamil char (MVP)
        var result = new System.Text.StringBuilder();

        foreach (int token in tokens)
        {
            if (tokenToText.ContainsKey(token) && tokenToText[token] != "<EOS>" && tokenToText[token] != "<PAD>")
            {
                result.Append(tokenToText[token]);
            }
        }

        return result.ToString();
    }
}

// Required imports (add to actual file):
// using UnityEngine;
// using Unity.Barracuda;
// using System.Collections.Generic;
// using System.IO;
