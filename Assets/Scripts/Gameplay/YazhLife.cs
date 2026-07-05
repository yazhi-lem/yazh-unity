using UnityEngine;
using System;

/// <summary>
/// YazhLife — the living heartbeat of the pet.
/// Tracks vitality (உயிர்), mood (மனநிலை), bond (பிணைப்பு), and energy (சக்தி).
/// Vitality decays over real time; interaction and care restore it.
/// When vitality hits zero the pet enters dormancy (not death — it sleeps and waits).
/// </summary>
public class YazhLife : MonoBehaviour
{
    // ─── Events ──────────────────────────────────────────────────────────────

    public static event Action<LifeStats> OnStatsChanged;
    public static event Action<LifePhase> OnPhaseChanged;
    public static event Action OnPetDormant;
    public static event Action OnPetAwakened;

    // ─── Data ────────────────────────────────────────────────────────────────

    public enum LifePhase
    {
        Thriving,   // Vitality > 75
        Alive,      // 40–75
        Struggling, // 15–39
        Dormant     // 0–14 — pet sleeps, waits for child to return
    }

    [System.Serializable]
    public class LifeStats
    {
        [Range(0, 100)] public float vitality  = 100f; // உயிர்
        [Range(0, 100)] public float mood      = 80f;  // மனநிலை
        [Range(0, 100)] public float bond      = 50f;  // பிணைப்பு (grows over sessions)
        [Range(0, 100)] public float energy    = 100f; // சக்தி
        public int totalInteractions           = 0;
        public float totalPlaytimeSeconds      = 0f;
        public DateTime lastSeenUtc            = DateTime.UtcNow;
    }

    // ─── Config ───────────────────────────────────────────────────────────────

    [Header("Decay rates per second (in-session)")]
    [SerializeField] private float vitalityDecayRate = 0.005f;  // ~30/hour
    [SerializeField] private float energyDecayRate   = 0.008f;  // ~29/hour
    [SerializeField] private float moodDecayRate     = 0.003f;  // ~11/hour

    [Header("Offline decay (applied on next session open)")]
    [SerializeField] private float offlineVitalityDecayPerHour = 2.5f;
    [SerializeField] private float offlineMoodDecayPerHour     = 1.5f;
    [SerializeField] private float maxOfflineHours             = 72f; // cap at 3 days

    [Header("Care restore amounts")]
    [SerializeField] private float feedVitality  = 15f;
    [SerializeField] private float feedEnergy    = 10f;
    [SerializeField] private float playMood      = 20f;
    [SerializeField] private float playBond      = 3f;
    [SerializeField] private float talkMood      = 10f;
    [SerializeField] private float talkBond      = 2f;

    // ─── State ────────────────────────────────────────────────────────────────

    private LifeStats stats = new();
    private LifePhase currentPhase = LifePhase.Alive;
    private bool isDormant = false;

    // ─── Unity lifecycle ──────────────────────────────────────────────────────

    private void Awake()
    {
        LoadFromPlayerPrefs();
        ApplyOfflineDecay();
    }

    private void Start()
    {
        EvaluatePhase();
        InvokeRepeating(nameof(TickDecay), 1f, 1f);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused) SaveToPlayerPrefs();
    }

    private void OnApplicationQuit()
    {
        SaveToPlayerPrefs();
    }

    // ─── Decay ────────────────────────────────────────────────────────────────

    private void TickDecay()
    {
        if (isDormant) return;

        stats.totalPlaytimeSeconds += 1f;

        stats.vitality = Mathf.Max(0, stats.vitality - vitalityDecayRate);
        stats.energy   = Mathf.Max(0, stats.energy   - energyDecayRate);
        stats.mood     = Mathf.Max(0, stats.mood      - moodDecayRate);

        EvaluatePhase();
        OnStatsChanged?.Invoke(stats);
    }

    private void ApplyOfflineDecay()
    {
        float hoursAway = (float)(DateTime.UtcNow - stats.lastSeenUtc).TotalHours;
        hoursAway = Mathf.Min(hoursAway, maxOfflineHours);

        if (hoursAway < 0.1f) return;

        stats.vitality = Mathf.Max(0, stats.vitality - offlineVitalityDecayPerHour * hoursAway);
        stats.mood     = Mathf.Max(0, stats.mood     - offlineMoodDecayPerHour     * hoursAway);

        Debug.Log($"[YazhLife] Offline for {hoursAway:F1}h. Vitality now {stats.vitality:F0}.");
    }

    // ─── Phase evaluation ─────────────────────────────────────────────────────

    private void EvaluatePhase()
    {
        LifePhase newPhase = stats.vitality switch
        {
            > 75 => LifePhase.Thriving,
            > 40 => LifePhase.Alive,
            > 14 => LifePhase.Struggling,
            _    => LifePhase.Dormant
        };

        if (newPhase == currentPhase) return;

        currentPhase = newPhase;
        OnPhaseChanged?.Invoke(currentPhase);

        if (currentPhase == LifePhase.Dormant && !isDormant)
        {
            isDormant = true;
            OnPetDormant?.Invoke();
            Debug.Log("[YazhLife] Pet entered dormancy. உறங்குகிறது.");
        }
        else if (currentPhase != LifePhase.Dormant && isDormant)
        {
            isDormant = false;
            OnPetAwakened?.Invoke();
            Debug.Log("[YazhLife] Pet awakened. எழுந்திருக்கிறது!");
        }
    }

    // ─── Care actions (called by UI / SurvivalSystem) ─────────────────────────

    /// <summary>Feed the pet (water/food). Restores vitality + energy.</summary>
    public void Feed(string resourceType)
    {
        stats.vitality = Mathf.Min(100, stats.vitality + feedVitality);
        stats.energy   = Mathf.Min(100, stats.energy   + feedEnergy);
        stats.totalInteractions++;
        EvaluatePhase();
        OnStatsChanged?.Invoke(stats);
        Debug.Log($"[YazhLife] Fed ({resourceType}). Vitality: {stats.vitality:F0}");
    }

    /// <summary>Play with the pet. Lifts mood and deepens bond.</summary>
    public void Play()
    {
        stats.mood = Mathf.Min(100, stats.mood + playMood);
        stats.bond = Mathf.Min(100, stats.bond + playBond);
        stats.totalInteractions++;
        OnStatsChanged?.Invoke(stats);
        Debug.Log($"[YazhLife] Played. Mood: {stats.mood:F0}, Bond: {stats.bond:F0}");
    }

    /// <summary>Talk to the pet (dialogue). Lifts mood, grows bond slowly.</summary>
    public void Talk()
    {
        stats.mood = Mathf.Min(100, stats.mood + talkMood);
        stats.bond = Mathf.Min(100, stats.bond + talkBond);
        stats.totalInteractions++;
        OnStatsChanged?.Invoke(stats);
    }

    /// <summary>Wake dormant pet with a care action (requires vitality boost first).</summary>
    public void Wake()
    {
        if (!isDormant) return;
        stats.vitality = 20f;
        stats.mood     = 30f;
        EvaluatePhase();
        Debug.Log("[YazhLife] Wake called. எழு!");
    }

    // ─── Accessors ────────────────────────────────────────────────────────────

    public LifeStats GetStats()    => stats;
    public LifePhase GetPhase()    => currentPhase;
    public bool      IsDormant()   => isDormant;

    /// <summary>Tamil phase label for UI.</summary>
    public string GetTamilPhaseLabel() => currentPhase switch
    {
        LifePhase.Thriving   => "செழிக்கிறது",
        LifePhase.Alive      => "வாழ்கிறது",
        LifePhase.Struggling => "சிரமப்படுகிறது",
        LifePhase.Dormant    => "உறங்குகிறது",
        _                    => ""
    };

    // ─── Persistence ──────────────────────────────────────────────────────────

    private const string PREFS_KEY = "YazhLife_v1";

    private void SaveToPlayerPrefs()
    {
        stats.lastSeenUtc = DateTime.UtcNow;
        string json = JsonUtility.ToJson(stats);
        PlayerPrefs.SetString(PREFS_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("[YazhLife] Saved.");
    }

    private void LoadFromPlayerPrefs()
    {
        string json = PlayerPrefs.GetString(PREFS_KEY, "");
        if (!string.IsNullOrEmpty(json))
        {
            stats = JsonUtility.FromJson<LifeStats>(json);
            Debug.Log("[YazhLife] Loaded saved state.");
        }
    }
}
