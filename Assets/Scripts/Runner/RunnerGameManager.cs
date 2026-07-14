// RunnerGameManager.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// Run state: speed ramp, distance, score, collectibles, game over.
// The run IS playtime with your pet — finishing a run lifts the pet's mood
// and deepens the bond through YazhLife, and a tired pet starts slower.

using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerGameManager : MonoBehaviour
{
    public static RunnerGameManager Instance { get; private set; }

    [Header("Speed")]
    [SerializeField] private float baseSpeed = 8f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float speedRampPerSecond = 0.12f;

    [Header("Scoring")]
    [SerializeField] private int collectibleScore = 10;
    [SerializeField] private int smashScore = 5;

    [Header("References")]
    public EndlessTerrainGenerator terrain;
    public YazhRunnerController runner;

    // ─── Events (HUD listens) ─────────────────────────────────────────────────
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int, string> OnCollected;   // count, Tamil name
    public static event System.Action<int> OnGameOver;             // final score

    private float currentSpeed;
    private float runSeconds;
    private int score;
    private int collectibleCount;
    private bool running;

    public bool IsRunning => running;
    public float CurrentSpeed => currentSpeed;
    public int Score => score;
    public int Collectibles => collectibleCount;
    public float Distance => terrain != null ? terrain.DistanceTraveled : 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartRun();
    }

    public void StartRun()
    {
        currentSpeed = baseSpeed * TiredPenalty();
        runSeconds = 0f;
        score = 0;
        collectibleCount = 0;
        running = true;
        Time.timeScale = 1f;
        Debug.Log($"[RunnerGame] Run started. ஓடு! (start speed {currentSpeed:F1} m/s)");
    }

    private void Update()
    {
        if (!running || terrain == null) return;

        runSeconds += Time.deltaTime;

        // Ramp speed over time; Palai (wasteland) stretches run hotter.
        float tinaiMod = terrain.CurrentTheme.speedModifier;
        currentSpeed = Mathf.Min(maxSpeed, (baseSpeed + runSeconds * speedRampPerSecond)) * tinaiMod;

        terrain.Advance(currentSpeed * Time.deltaTime);

        // Distance score: 1 point per meter.
        int newScore = Mathf.FloorToInt(Distance) + collectibleCount * collectibleScore;
        if (newScore != score)
        {
            score = newScore;
            OnScoreChanged?.Invoke(score);
        }
    }

    // ─── Gameplay callbacks ───────────────────────────────────────────────────

    public void OnCollect(RunnerCollectible collectible)
    {
        collectibleCount += collectible.value;
        OnCollected?.Invoke(collectibleCount, collectible.tamilName);
    }

    public void OnSmash()
    {
        score += smashScore;
        OnScoreChanged?.Invoke(score);
    }

    public void OnPlayerHit(RunnerObstacle obstacle)
    {
        if (!running) return;
        running = false;
        Debug.Log($"[RunnerGame] Hit {obstacle.kind} at {Distance:F0}m. Run over. Score: {score}");

        RewardPet();
        OnGameOver?.Invoke(score);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ─── YazhLife integration ─────────────────────────────────────────────────

    /// <summary>A tired pet (low energy) starts the run a little slower.</summary>
    private float TiredPenalty()
    {
        var life = GameManager.Instance != null ? GameManager.Instance.GetYazhLife() : null;
        if (life == null) return 1f;
        return life.GetStats().energy < 20f ? 0.8f : 1f;
    }

    /// <summary>Every run counts as play: mood up, bond deepens.</summary>
    private void RewardPet()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.PlayWithPet();
        Debug.Log("[RunnerGame] Run rewarded pet mood + bond via YazhLife.");
    }
}
