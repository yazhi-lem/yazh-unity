using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Central game manager for Yazh XR App.
/// Handles scene state, pet lifecycle, and game progression.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ARSession arSession;
    [SerializeField] private string tamiliLanguage = "தமிழ்";
    
    private PetManager petManager;
    private DialogueSystem dialogueSystem;
    private SurvivalSystem survivalSystem;
    private YazhInferenceEngine yazhEngine;
    private YazhLife yazhLife;

    public enum GameState
    {
        Onboarding,
        MainGame,
        Runner,     // Tinai endless run (Subway Surfers-style core loop)
        Paused,
        Challenge,
        Settings,
        Achievements
    }

    private GameState currentState = GameState.Onboarding;
    private string selectedPetType; // kuruvi, maan, yanai, pulliruvi
    private int challengeDay = 1;   // 1-7 in current challenge cycle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        petManager = GetComponent<PetManager>() ?? gameObject.AddComponent<PetManager>();
        dialogueSystem = GetComponent<DialogueSystem>() ?? gameObject.AddComponent<DialogueSystem>();
        survivalSystem = GetComponent<SurvivalSystem>() ?? gameObject.AddComponent<SurvivalSystem>();
        yazhEngine = GetComponent<YazhInferenceEngine>() ?? gameObject.AddComponent<YazhInferenceEngine>();
        yazhLife = GetComponent<YazhLife>() ?? gameObject.AddComponent<YazhLife>();

        // Connect life events to game behaviour
        YazhLife.OnPhaseChanged += OnPetPhaseChanged;
        YazhLife.OnPetDormant   += OnPetEnteredDormancy;
        YazhLife.OnPetAwakened  += OnPetAwoke;

        Debug.Log("[GameManager] All managers initialized");
    }

    private async void Start()
    {
        // Load Yazh 30K model asynchronously
        await yazhEngine.InitializeAsync("Assets/Models/AI/yazh_30k.onnx");
        
        // AR Session management
        if (arSession != null)
        {
            arSession.Reset();
        }

        SetGameState(GameState.Onboarding);
    }

    /// <summary>
    /// Transition between game states
    /// </summary>
    public void SetGameState(GameState newState)
    {
        Debug.Log($"[GameManager] State transition: {currentState} → {newState}");
        currentState = newState;

        switch (newState)
        {
            case GameState.Onboarding:
                OnStateOnboarding();
                break;
            case GameState.MainGame:
                OnStateMainGame();
                break;
            case GameState.Runner:
                OnStateRunner();
                break;
            case GameState.Challenge:
                OnStateChallenge();
                break;
            case GameState.Paused:
                OnStatePaused();
                break;
            case GameState.Settings:
                OnStateSettings();
                break;
            case GameState.Achievements:
                break;
        }
    }

    private void OnStateOnboarding()
    {
        Debug.Log("[GameManager] Entering Onboarding state");
        // Load onboarding scene / UI
        Time.timeScale = 1f;
    }

    private void OnStateMainGame()
    {
        Debug.Log("[GameManager] Entering MainGame state");
        // Spawn pet, initialize dialogue
        if (!string.IsNullOrEmpty(selectedPetType))
        {
            petManager.SpawnPet(selectedPetType);
            survivalSystem.StartDaySimulation();
        }
        Time.timeScale = 1f;
    }

    private void OnStateRunner()
    {
        Debug.Log("[GameManager] Entering Runner state — tinai endless run. ஓடு!");
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TinaiRunner");
    }

    /// <summary>
    /// Start the tinai endless run with the currently selected pet.
    /// Call from the main-menu "ஓடு" (Run) button.
    /// </summary>
    public void StartEndlessRun()
    {
        SetGameState(GameState.Runner);
    }

    private void OnStateChallenge()
    {
        Debug.Log("[GameManager] Entering Challenge state - Day " + challengeDay);
        survivalSystem.StartChallenge(challengeDay);
        survivalSystem.IncreaseDifficulty();
        Time.timeScale = 1f;
    }

    private void OnStatePaused()
    {
        Time.timeScale = 0f;
    }

    private void OnStateSettings()
    {
        Time.timeScale = 0.5f; // Slow-mo while settings open
    }

    /// <summary>
    /// Called from Onboarding UI after pet selection
    /// </summary>
    public void OnPetSelected(string petType)
    {
        selectedPetType = petType;
        Debug.Log($"[GameManager] Pet selected: {petType}");
        SetGameState(GameState.MainGame);
    }

    /// <summary>
    /// Advance challenge by one day
    /// </summary>
    public void AdvanceChallenge()
    {
        challengeDay++;
        if (challengeDay > 7)
        {
            challengeDay = 1;
            Debug.Log("[GameManager] Challenge cycle complete! Unlock reward.");
            OnChallengeComplete();
        }
        else
        {
            SetGameState(GameState.Challenge);
        }
    }

    private void OnChallengeComplete()
    {
        // TODO: Unlock biome or pet ability
        SetGameState(GameState.Achievements);
    }

    public GameState GetCurrentState() => currentState;
    public string GetSelectedPet() => selectedPetType;
    public int GetChallengeDay() => challengeDay;
    public YazhLife GetYazhLife() => yazhLife;

    // ─── Life event handlers ──────────────────────────────────────────────────

    private void OnPetPhaseChanged(YazhLife.LifePhase phase)
    {
        Debug.Log($"[GameManager] Pet phase → {phase} ({yazhLife.GetTamilPhaseLabel()})");

        // When struggling, auto-trigger a gentle challenge hint
        if (phase == YazhLife.LifePhase.Struggling && currentState == GameState.MainGame)
        {
            survivalSystem.CheckChallengeTrigger();
        }
    }

    private void OnPetEnteredDormancy()
    {
        Debug.Log("[GameManager] Pet dormant — pausing game world.");
        SetGameState(GameState.Paused);
    }

    private void OnPetAwoke()
    {
        Debug.Log("[GameManager] Pet awoke — resuming MainGame.");
        SetGameState(GameState.MainGame);
    }

    // ─── Care shortcuts (call from UI buttons) ────────────────────────────────

    public void FeedPet(string resourceType)
    {
        yazhLife.Feed(resourceType);
        survivalSystem.GatherResource(resourceType, 1);
        petManager.PlayEmotionAnimation("happy");
    }

    public void PlayWithPet()
    {
        yazhLife.Play();
        petManager.PlayEmotionAnimation("celebrate");
    }

    public void TalkToPet(string inputText)
    {
        yazhLife.Talk();
        _ = dialogueSystem.ProcessInput(inputText);
    }

    private void OnDestroy()
    {
        YazhLife.OnPhaseChanged -= OnPetPhaseChanged;
        YazhLife.OnPetDormant   -= OnPetEnteredDormancy;
        YazhLife.OnPetAwakened  -= OnPetAwoke;
    }
}
