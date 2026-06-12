using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// GameManager: Central orchestrator for Yazh XR app
/// Manages game state, scene transitions, pet lifecycle, and resource allocation
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string gameVersion = "0.1.0-prototype";
    [SerializeField] private int targetFPS = 60;  // AR baseline
    [SerializeField] private bool enableLogging = true;

    // Game state
    private GameState currentGameState = GameState.MainMenu;
    private Pet selectedPet = null;
    private BiomeController currentBiome = null;
    private ResourceManager resourceManager;
    private WeatherSystem weatherSystem;

    // Session tracking
    private float sessionStartTime;
    private int currentDayCount = 1;
    private Dictionary<string, float> sessionMetrics;

    public enum GameState
    {
        MainMenu,
        PetSelection,
        BiomeActive,
        PetChat,
        Paused,
        GameOver,
        Settings
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        Application.targetFrameRate = targetFPS;
        sessionStartTime = Time.time;
        sessionMetrics = new Dictionary<string, float>();

        Log($"[Yazh XR] Initialized v{gameVersion}");
        Log($"[Yazh XR] Target FPS: {targetFPS}");
        Log($"[Yazh XR] Platform: {Application.platform}");
    }

    private void Update()
    {
        if (currentGameState == GameState.BiomeActive)
        {
            UpdateGameLoop();
        }

        // ESC to pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void UpdateGameLoop()
    {
        // Update active systems
        if (currentBiome != null)
        {
            currentBiome.UpdateBiome(Time.deltaTime);
        }

        if (selectedPet != null)
        {
            selectedPet.UpdatePet(Time.deltaTime);
        }

        if (weatherSystem != null)
        {
            weatherSystem.UpdateWeather(Time.deltaTime);
        }
    }

    /// <summary>
    /// Transitions to a new game state
    /// </summary>
    public void ChangeGameState(GameState newState)
    {
        GameState previousState = currentGameState;
        currentGameState = newState;

        Log($"[State Transition] {previousState} → {newState}");

        switch (newState)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.PetSelection:
                OnPetSelection();
                break;
            case GameState.BiomeActive:
                OnBiomeStarted();
                break;
            case GameState.PetChat:
                OnPetChatStarted();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.Settings:
                OnSettingsOpened();
                break;
        }
    }

    public void SelectPet(PetType petType)
    {
        selectedPet = new Pet(petType);
        Log($"[Pet Selected] {petType} - Health: {selectedPet.Health}, Energy: {selectedPet.Energy}");
        ChangeGameState(GameState.BiomeActive);
    }

    public void StartBiome(BiomeType biomeType)
    {
        if (currentBiome != null)
        {
            Destroy(currentBiome.gameObject);
        }

        GameObject biomeObj = new GameObject($"Biome_{biomeType}");
        currentBiome = biomeObj.AddComponent<BiomeController>();
        currentBiome.Initialize(biomeType);

        weatherSystem = gameObject.AddComponent<WeatherSystem>();
        weatherSystem.Initialize(biomeType);

        Log($"[Biome Loaded] {biomeType} - Day {currentDayCount}/7");
    }

    public void TogglePause()
    {
        if (currentGameState == GameState.BiomeActive)
        {
            ChangeGameState(GameState.Paused);
        }
        else if (currentGameState == GameState.Paused)
        {
            ChangeGameState(GameState.BiomeActive);
            Time.timeScale = 1f;
        }
    }

    private void OnMainMenu()
    {
        // Load MainMenu scene
        Log("[UI] Loading MainMenu");
    }

    private void OnPetSelection()
    {
        // Load PetSelection scene
        Log("[UI] Loading PetSelection");
    }

    private void OnBiomeStarted()
    {
        StartBiome(BiomeType.SangamKaadu);  // Week 1 default
        Log("[Gameplay] Biome started, resources initialized");
    }

    private void OnPetChatStarted()
    {
        Log("[Chat] Pet chat UI activated");
        // Trigger dialogue HUD
    }

    private void OnSettingsOpened()
    {
        Log("[UI] Settings menu opened");
    }

    public void EndDay()
    {
        currentDayCount++;
        Log($"[Day Cycle] Completed day, now on Day {currentDayCount}");

        if (currentDayCount > 7)
        {
            CompleteChallenge();
        }
    }

    private void CompleteChallenge()
    {
        Log($"[Achievement] 7-day challenge completed!");
        Log($"[Pet Stats] Final - Health: {selectedPet.Health}, Happiness: {selectedPet.Happiness}");
        ChangeGameState(GameState.GameOver);
    }

    public GameState GetCurrentState() => currentGameState;
    public Pet GetSelectedPet() => selectedPet;
    public BiomeController GetCurrentBiome() => currentBiome;
    public int GetCurrentDay() => currentDayCount;

    private void Log(string message)
    {
        if (enableLogging)
        {
            Debug.Log(message);
        }
    }

    private void OnApplicationQuit()
    {
        float sessionDuration = Time.time - sessionStartTime;
        Log($"[Session End] Duration: {sessionDuration:F1}s, Days completed: {currentDayCount}");
    }
}

/// <summary>
/// Pet: Core pet entity with stats, behaviors, and AI
/// </summary>
public class Pet
{
    public PetType Type { get; private set; }
    public float Health { get; set; } = 100f;
    public float Energy { get; set; } = 100f;
    public float Hunger { get; set; } = 50f;
    public float Happiness { get; set; } = 75f;
    public string Name { get; set; } = "Companion";

    // Pet personality affects response style
    public PetPersonality Personality { get; private set; }

    public enum PetPersonality
    {
        Curious,      // Kuruvi (bird) - fast, curious
        Thoughtful,   // Maan (deer) - cautious, logical
        Wise,         // Yanai (elephant) - patient, strategic
        Playful       // Pulliruvi (cat) - independent, tricky
    }

    public Pet(PetType type)
    {
        Type = type;
        Personality = type switch
        {
            PetType.Kuruvi => PetPersonality.Curious,
            PetType.Maan => PetPersonality.Thoughtful,
            PetType.Yanai => PetPersonality.Wise,
            PetType.Pulliruvi => PetPersonality.Playful,
            _ => PetPersonality.Curious
        };
    }

    public void UpdatePet(float deltaTime)
    {
        // Decay stats naturally
        Energy = Mathf.Max(0, Energy - deltaTime * 5f);
        Hunger = Mathf.Min(100, Hunger + deltaTime * 3f);

        // Mood impacts happiness
        if (Hunger > 80) Happiness = Mathf.Max(0, Happiness - deltaTime * 2f);
        if (Energy < 20) Happiness = Mathf.Max(0, Happiness - deltaTime * 1f);

        // Health linked to happiness
        Health = Mathf.Lerp(Health, Happiness, deltaTime * 0.1f);
    }

    public string GetStatusMessage()
    {
        return $"{Name} ({Type})\nHealth: {Health:F0}% | Energy: {Energy:F0}% | Happiness: {Happiness:F0}%";
    }
}

public enum PetType { Kuruvi, Maan, Yanai, Pulliruvi }
public enum BiomeType { SangamKaadu, Oorru, Kulaathanku, KaraiParai }

/// <summary>
/// BiomeController: Manages individual biome environment and interactive elements
/// </summary>
public class BiomeController : MonoBehaviour
{
    public BiomeType BiomeType { get; private set; }
    private List<Resource> availableResources = new();
    private ARPlaneManager arPlaneManager;

    public void Initialize(BiomeType biomeType)
    {
        BiomeType = biomeType;
        SetupBiomeEnvironment();
    }

    private void SetupBiomeEnvironment()
    {
        Debug.Log($"[Biome] Setting up {BiomeType}");

        // AR Foundation setup
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        if (arPlaneManager != null)
        {
            arPlaneManager.planesChanged += OnARPlanesChanged;
        }

        // Spawn biome-specific resources
        InitializeResources();
    }

    private void InitializeResources()
    {
        // Week 1: Static prop placement (no procedural gen yet)
        availableResources.Clear();

        switch (BiomeType)
        {
            case BiomeType.SangamKaadu:
                // Forest: water streams, trees, herbs
                availableResources.Add(new Resource(ResourceType.Water, 10, "Stream"));
                availableResources.Add(new Resource(ResourceType.Food, 5, "Berries"));
                availableResources.Add(new Resource(ResourceType.Shelter, 3, "Rock Outcrop"));
                availableResources.Add(new Resource(ResourceType.Herb, 7, "Medicinal Plants"));
                break;
        }

        Debug.Log($"[Resources] Spawned {availableResources.Count} resource nodes in {BiomeType}");
    }

    public void UpdateBiome(float deltaTime)
    {
        // Update AR rendering, weather effects, etc.
    }

    private void OnARPlanesChanged(ARPlanesChangedEventArgs args)
    {
        Debug.Log($"[AR] Planes detected: {args.added.Count} added, {args.updated.Count} updated");
    }
}

/// <summary>
/// Resource: Collectable items in the biome
/// </summary>
public class Resource
{
    public ResourceType Type { get; set; }
    public int Quantity { get; set; }
    public string Name { get; set; }

    public Resource(ResourceType type, int quantity, string name)
    {
        Type = type;
        Quantity = quantity;
        Name = name;
    }
}

public enum ResourceType { Water, Food, Shelter, Herb }

/// <summary>
/// WeatherSystem: Manages dynamic weather affecting gameplay
/// </summary>
public class WeatherSystem : MonoBehaviour
{
    public WeatherCondition CurrentWeather { get; private set; } = WeatherCondition.Sunny;
    private float weatherCycleTime = 0f;
    private float weatherCycleLength = 180f;  // 3 min cycle

    public enum WeatherCondition { Sunny, Cloudy, Rainy, Stormy }

    public void Initialize(BiomeType biomeType)
    {
        Debug.Log($"[Weather] Initialized for {biomeType}");
    }

    public void UpdateWeather(float deltaTime)
    {
        weatherCycleTime += deltaTime;

        if (weatherCycleTime >= weatherCycleLength)
        {
            // Cycle to next weather
            CurrentWeather = (WeatherCondition)(((int)CurrentWeather + 1) % 4);
            weatherCycleTime = 0f;
            Debug.Log($"[Weather] Changed to {CurrentWeather}");

            // Apply gameplay effects
            ApplyWeatherEffects();
        }
    }

    private void ApplyWeatherEffects()
    {
        var pet = GameManager.Instance.GetSelectedPet();
        if (pet == null) return;

        switch (CurrentWeather)
        {
            case WeatherCondition.Rainy:
                pet.Health -= 5f;  // Wet and cold
                break;
            case WeatherCondition.Stormy:
                pet.Energy -= 10f;  // Stressed
                break;
            case WeatherCondition.Sunny:
                pet.Happiness += 5f;  // Happy
                break;
        }
    }
}

// Required using statements (add to top of actual file):
// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using System.Collections.Generic;
