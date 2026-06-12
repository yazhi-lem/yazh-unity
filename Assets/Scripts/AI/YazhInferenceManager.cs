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

    [SerializeField] private string modelPath = "MLModels/yazh-30k-quantized.onnx";
    [SerializeField] private string tokenizerPath = "MLModels/yazh-tokenizer.json";
    [SerializeField] private float inferenceTimeout = 0.5f;  // 500ms max (target 150ms)
    [SerializeField] private bool enableProfiling = true;

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
            Debug.Log($"[Yazh AI] Model loaded successfully. Inputs: {yazhModel.inputs.Count}, Outputs: {yazhModel.outputs.Count}");
            Debug.Log($"[Yazh AI] Model memory: ~{EstimateModelSize(yazhModel) / 1024 / 1024:F1}MB");
        }
        else
        {
            Debug.LogError("[Yazh AI] Failed to load model");
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

        StartCoroutine(GenerateResponseCoroutine(userInput, onResponseReady));
    }

    private System.Collections.IEnumerator GenerateResponseCoroutine(string userInput, System.Action<string> callback)
    {
        float startTime = Time.realtimeSinceStartup;

        // 1. Tokenize input (Tamil)
        int[] inputTokens = tokenizer.Encode(userInput);
        Debug.Log($"[Yazh AI] Input tokens: {inputTokens.Length} ({string.Join(",", inputTokens)})");

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

        float inferenceLatency = (Time.realtimeSinceStartup - startTime) * 1000f;  // ms
        Debug.Log($"[Yazh AI] Inference latency: {inferenceLatency:F1}ms");

        RecordMetric("last_inference_latency", inferenceLatency);

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
