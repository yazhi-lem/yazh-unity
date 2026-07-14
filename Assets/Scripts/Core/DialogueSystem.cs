using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Manages dialogue flow, context retention, and model inference calls.
/// Maintains last 8 exchanges for context window.
/// </summary>
public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }

    [System.Serializable]
    public class DialogueExchange
    {
        public string childInput;
        public string petResponse;
        public string animationTrigger;
        public string emotionalTone;
        public float responseTime;
    }

    public delegate void OnDialogueResponse(DialogueResponse response);
    public event OnDialogueResponse OnResponseGenerated;

    public class DialogueResponse
    {
        public string tamilText;
        public string englishTranslation;
        public string animationTrigger;
        public AudioClip audioClip;
        public float duration;
    }

    [SerializeField] private int contextWindowSize = 8;
    private Queue<DialogueExchange> dialogueHistory = new();
    private YazhInferenceEngine yazhEngine;
    private AudioSyncManager audioSync;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        yazhEngine = GetComponent<YazhInferenceEngine>() ?? FindFirstObjectByType<YazhInferenceEngine>();
        audioSync = GetComponent<AudioSyncManager>() ?? FindFirstObjectByType<AudioSyncManager>();
    }

    /// <summary>
    /// Process child's input (voice or text) and generate response
    /// </summary>
    public async Task ProcessInput(string inputText)
    {
        Debug.Log($"[DialogueSystem] Processing input: {inputText}");

        // Build context from last 8 exchanges
        string context = BuildContextWindow();

        // Inference via Yazh 30K model
        var response = await InferResponse(inputText, context);

        // Add to history
        var exchange = new DialogueExchange
        {
            childInput = inputText,
            petResponse = response.tamilText,
            animationTrigger = response.animationTrigger,
            emotionalTone = response.animationTrigger,
            responseTime = Time.realtimeSinceStartup
        };
        dialogueHistory.Enqueue(exchange);

        // Trim to context window
        if (dialogueHistory.Count > contextWindowSize)
            dialogueHistory.Dequeue();

        // Trigger response callback
        OnResponseGenerated?.Invoke(response);

        Debug.Log($"[DialogueSystem] Response: {response.tamilText}");
    }

    private string BuildContextWindow()
    {
        string context = "";
        foreach (var exchange in dialogueHistory)
        {
            context += $"Child: {exchange.childInput}\nPet: {exchange.petResponse}\n";
        }
        return context;
    }

    // Scripted fallbacks when the ML model is not bundled/loaded (model-less builds).
    private static readonly string[] FallbackResponses =
    {
        "சரி! ஓடலாம் வா!",              // Okay! Let's run!
        "நான் உன் கூடவே இருக்கேன்.",     // I'm right here with you.
        "அது நல்ல யோசனை!",              // That's a good idea!
        "இன்னும் கொஞ்சம் விளையாடலாமா?"   // Shall we play a bit more?
    };

    private async Task<DialogueResponse> InferResponse(string input, string context)
    {
        string tamilResponse = null;

        // Inference via the on-device Yazh 30K model when available.
        if (yazhEngine != null)
        {
            var tokens = await yazhEngine.InferenceAsync(input, context);
            tamilResponse = yazhEngine.DecodeTokens(tokens);
        }

        // Model missing or returned nothing → scripted fallback keeps the pet alive.
        if (string.IsNullOrWhiteSpace(tamilResponse))
        {
            tamilResponse = FallbackResponses[Random.Range(0, FallbackResponses.Length)];
            Debug.Log("[DialogueSystem] Model unavailable — using scripted fallback response.");
        }

        // Generate TTS audio (placeholder)
        AudioClip audioClip = null; // TODO: actual TTS synthesis
        
        return new DialogueResponse
        {
            tamilText = tamilResponse,
            animationTrigger = "talk",
            audioClip = audioClip,
            duration = 2f
        };
    }

    public List<DialogueExchange> GetDialogueHistory()
    {
        return new List<DialogueExchange>(dialogueHistory);
    }

    public void ClearHistory()
    {
        dialogueHistory.Clear();
    }
}
