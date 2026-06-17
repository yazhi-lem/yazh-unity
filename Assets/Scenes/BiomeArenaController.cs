// BiomeArenaController.cs — YAZH-UNITY
// THOZHAR | Rotation 25 | Jun 17, 2026
// Controls the main gameplay scene: AR viewport, dialogue HUD, resource UI

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BiomeArenaController : MonoBehaviour
{
    [Header("AR")]
    public GameObject arSession;
    public GameObject arCamera;

    [Header("Dialogue HUD")]
    public InputField chatInput;
    public Button sendButton;
    public Text responseText;
    public ScrollRect chatScroll;

    [Header("Pet HUD")]
    public Text petNameText;
    public Image petPortrait;
    public Slider healthBar;
    public Slider energyBar;
    public Slider hungerBar;
    public Slider happinessBar;

    [Header("Resource HUD")]
    public Text waterText;
    public Text foodText;
    public Text shelterText;
    public Text herbText;

    [Header("Weather")]
    public Text weatherText;
    public Image weatherIcon;

    [Header("Day Counter")]
    public Text dayText;

    private string playerPet;
    private bool isProcessing = false;

    void Start()
    {
        // Get selected pet from GameManager
        if (GameManager.Instance != null)
        {
            playerPet = GameManager.Instance.selectedPet;
        }
        else
        {
            playerPet = "Kuruvi"; // fallback
            Debug.LogWarning("[BiomeArena] No GameManager — defaulting to Kuruvi");
        }

        // Set pet name
        if (petNameText != null)
            petNameText.text = playerPet;

        // Wire chat
        if (sendButton != null)
            sendButton.onClick.AddListener(OnSendClicked);

        // Initialize AR
        InitializeAR();

        // Start game loop
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(playerPet);
        }

        UpdateHUD();
    }

    void Update()
    {
        // Update HUD every frame
        UpdateHUD();
    }

    void InitializeAR()
    {
        if (arSession != null)
            arSession.SetActive(true);
        if (arCamera != null)
            arCamera.SetActive(true);

        Debug.Log("[BiomeArena] AR session initialized");
    }

    void OnSendClicked()
    {
        if (isProcessing) return;
        if (chatInput == null || string.IsNullOrEmpty(chatInput.text)) return;

        string input = chatInput.text.Trim();
        chatInput.text = "";

        Debug.Log("[BiomeArena] Player says: " + input);
        StartCoroutine(ProcessDialogue(input));
    }

    IEnumerator ProcessDialogue(string input)
    {
        isProcessing = true;

        // Show thinking indicator
        if (responseText != null)
            responseText.text = "...";

        // Send to Yazh inference manager
        if (YazhInferenceManager.Instance != null && YazhInferenceManager.Instance.IsModelReady())
        {
            // Async inference
            bool done = false;
            string response = "";

            YazhInferenceManager.Instance.GenerateResponse(input, (result) => {
                response = result;
                done = true;
            });

            // Wait for response (max 2 seconds)
            float timeout = 2f;
            while (!done && timeout > 0)
            {
                yield return null;
                timeout -= Time.deltaTime;
            }

            if (done && responseText != null)
            {
                responseText.text = response;
            }
            else if (responseText != null)
            {
                responseText.text = "மன்னிக்கவும், மீண்டும் முயற்சிக்கவும்"; // "Sorry, try again"
            }
        }
        else
        {
            // Fallback response
            yield return new WaitForSeconds(0.5f);
            if (responseText != null)
                responseText.text = GetFallbackResponse(input);
        }

        isProcessing = false;
    }

    string GetFallbackResponse(string input)
    {
        // Simple keyword matching fallback
        if (input.Contains("வணக்கம்") || input.Contains("hello"))
            return "வணக்கம்! நான் உன் " + playerPet + ".";
        if (input.Contains("பெயர்"))
            return "என் பெயர் " + playerPet + ".";
        return "சரி, நாம் சுற்றுலா செல்லலாம்!"; // "Okay, let's go explore!"
    }

    void UpdateHUD()
    {
        if (GameManager.Instance == null) return;

        // Stats
        if (healthBar != null)
            healthBar.value = GameManager.Instance.GetHealth() / 100f;
        if (energyBar != null)
            energyBar.value = GameManager.Instance.GetEnergy() / 100f;
        if (hungerBar != null)
            hungerBar.value = GameManager.Instance.GetHunger() / 100f;
        if (happinessBar != null)
            happinessBar.value = GameManager.Instance.GetHappiness() / 100f;

        // Resources
        if (waterText != null)
            waterText.text = "💧 " + GameManager.Instance.GetWater();
        if (foodText != null)
            foodText.text = "🍎 " + GameManager.Instance.GetFood();
        if (shelterText != null)
            shelterText.text = "🏠 " + GameManager.Instance.GetShelter();
        if (herbText != null)
            herbText.text = "🌿 " + GameManager.Instance.GetHerb();

        // Day
        if (dayText != null)
            dayText.text = "நாள் " + GameManager.Instance.GetCurrentDay();

        // Weather
        if (weatherText != null)
            weatherText.text = GameManager.Instance.GetWeatherName();
    }

    public void OnMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
