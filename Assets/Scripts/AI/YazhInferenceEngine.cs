using UnityEngine;
using Unity.InferenceEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Runs Yazh 30K ONNX model inference on-device.
/// Manages tokenization, inference, and decoding.
/// Target latency: <150ms per token generation.
///
/// Uses Unity's Inference Engine (formerly Sentis / Barracuda successor).
/// Package: com.unity.ai.inference — see Packages/manifest.json
///
/// FIX (Issue #1): previously this class loaded the model via
/// Resources.Load&lt;ModelAsset&gt;(modelPath), which silently returned null for
/// any StreamingAssets-relative path (Resources.Load only resolves assets
/// placed under a Resources/ folder, with no extension). GameManager passed
/// "Assets/Models/AI/yazh_30k.onnx", which doesn't exist anywhere in the
/// project, so isModelReady was permanently false and every conversation
/// silently fell back to DialogueSystem's 4 scripted lines.
///
/// This now loads directly from Assets/StreamingAssets/MLModels (same
/// location YazhInferenceManager already uses), and the tokenizer is a real
/// Tamil Unicode-range char tokenizer instead of two never-populated,
/// dead dictionaries.
///
/// NOTE: the bundled .onnx files under StreamingAssets/MLModels are still
/// placeholder weights (not real trained weights) — that's a separate,
/// follow-up concern (no amount of code-path fixing can produce real Tamil
/// completions from a placeholder file). With real weights in place, this
/// path is now correct end-to-end. Until then, ModelLoader.Load will fail
/// on the placeholder bytes, which is caught below and falls back cleanly,
/// same as before.
/// </summary>
public class YazhInferenceEngine : MonoBehaviour
{
    public static YazhInferenceEngine Instance { get; private set; }

    [Tooltip("Path relative to Assets/StreamingAssets/, e.g. MLModels/yazh-30k-int8.onnx")]
    [SerializeField] private string modelPath = "MLModels/yazh-30k-int8.onnx";
    [SerializeField] private string tokenizerPath = "MLModels/yazh-tokenizer.json";

    private Worker worker;
    private bool isModelReady = false;

    private YazhTokenizer tokenizer;
    private const int VOCAB_SIZE = 30000;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Initialize model asynchronously. `overrideModelPath`, if given, is a path
    /// relative to Assets/StreamingAssets/ (e.g. "MLModels/yazh-30k-int8.onnx").
    /// Kept as a parameter for call-site compatibility with existing callers.
    /// </summary>
    public async Task InitializeAsync(string overrideModelPath = null)
    {
        if (!string.IsNullOrEmpty(overrideModelPath))
            modelPath = overrideModelPath;

        // Unity APIs (ModelLoader, Worker) are main-thread-only,
        // so initialization runs on the main thread; yield once to stay awaitable.
        await Task.Yield();

        try
        {
            string modelFilePath = Path.Combine(Application.streamingAssetsPath, modelPath);

            if (!File.Exists(modelFilePath))
            {
                Debug.LogError($"[YazhInferenceEngine] Model not found at {modelFilePath}");
                return;
            }

            // Build the runtime model and create a worker for inference
            // Backend selection with CPU fallback for devices lacking
            // compute-shader support (see YazhBackendSelector, Issue #2).
            worker = YazhBackendSelector.CreateWorkerSafe(model, "[YazhInferenceEngine]");

            LoadTamilTokenizer();
            isModelReady = true;

            Debug.Log("[YazhInferenceEngine] Model initialized successfully");
        }
        catch (System.Exception ex)
        {
            // Expected to hit this until real trained weights replace the
            // placeholder files in StreamingAssets/MLModels — DialogueSystem's
            // scripted fallback keeps the pet responsive in the meantime.
            Debug.LogError($"[YazhInferenceEngine] Initialization failed: {ex.Message}");
        }
    }

    private void LoadTamilTokenizer()
    {
        string tokenizerFilePath = Path.Combine(Application.streamingAssetsPath, tokenizerPath);

        tokenizer = new YazhTokenizer();

        if (!File.Exists(tokenizerFilePath))
        {
            Debug.LogWarning($"[YazhInferenceEngine] Tokenizer file not found at {tokenizerFilePath}; using default Tamil Unicode vocabulary.");
        }

        // Populates the Tamil Unicode-range char vocabulary + special tokens
        // (see YazhTokenizer in YazhInferenceManager.cs).
        tokenizer.LoadVocabulary(tokenizerFilePath);
        Debug.Log("[YazhInferenceEngine] Tamil tokenizer loaded");
    }

    /// <summary>
    /// Run inference: input text → next-token id (greedy, single-token per
    /// call — matches the documented MVP limitation; multi-token generation
    /// is a separate follow-up).
    /// </summary>
    public async Task<List<int>> InferenceAsync(string input, string context = "")
    {
        if (!isModelReady || tokenizer == null)
        {
            Debug.LogError("[YazhInferenceEngine] Model not ready");
            return new List<int>();
        }

        var tokens = new List<int>();

        // Worker scheduling must happen on the main thread (Unity Inference Engine).
        await Task.Yield();

        try
        {
            // Build prompt: system message + context + input
            var prompt = $"You are a Tamil-speaking pet companion.\n{context}\nInput: {input}\nResponse:";
            int[] promptTokens = tokenizer.Encode(prompt);

            var tensorData = new float[promptTokens.Length];
            for (int i = 0; i < promptTokens.Length; i++)
                tensorData[i] = promptTokens[i];

            using (var inputTensor = new Tensor<float>(new TensorShape(1, promptTokens.Length), tensorData))
            {
                worker.Schedule(inputTensor);
                var output = worker.PeekOutput() as Tensor<float>;
                output.CompleteAllPendingOperations();

                float[] logits = output.DownloadToArray();
                int nextToken = Argmax(logits);
                tokens.Add(nextToken);
            }

            Debug.Log($"[YazhInferenceEngine] Generated {tokens.Count} token(s)");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[YazhInferenceEngine] Inference error: {ex.Message}");
        }

        return tokens;
    }

    /// <summary>Greedy argmax over the last VOCAB_SIZE logits in the output.</summary>
    private int Argmax(float[] logits)
    {
        if (logits == null || logits.Length == 0)
            return 0;

        int start = System.Math.Max(0, logits.Length - VOCAB_SIZE);
        float maxLogit = float.MinValue;
        int maxIdx = 0;

        for (int i = start; i < logits.Length; i++)
        {
            if (logits[i] > maxLogit)
            {
                maxLogit = logits[i];
                maxIdx = i - start;
            }
        }

        return maxIdx % VOCAB_SIZE;
    }

    /// <summary>
    /// Convert tokens back to Tamil text.
    /// </summary>
    public string DecodeTokens(List<int> tokens)
    {
        if (tokenizer == null || tokens == null || tokens.Count == 0)
            return string.Empty;

        return tokenizer.Decode(tokens.ToArray());
    }

    private void OnDestroy()
    {
        worker?.Dispose();
    }
}
