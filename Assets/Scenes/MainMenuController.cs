// MainMenuController.cs — YAZH-UNITY
// THOZHAR | Rotation 25 | Jun 17, 2026
// Controls the main menu scene: title, start, settings

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button startButton;
    public Button settingsButton;
    public Text titleText;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Toggle tamilToggle; // Tamil-first mode

    void Start()
    {
        // Wire buttons
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);

        // Set title
        if (titleText != null)
            titleText.text = "யாழ்"; // Yazh in Tamil

        // Hide settings panel
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Initialize GameManager if not present
        if (GameManager.Instance == null)
        {
            Debug.Log("[MainMenu] GameManager not found — creating...");
            new GameObject("GameManager").AddComponent<GameManager>();
        }
    }

    void OnStartClicked()
    {
        Debug.Log("[MainMenu] Start clicked — loading PetSelection");
        SceneManager.LoadScene("PetSelection");
    }

    void OnSettingsClicked()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public void OnLanguageToggle(bool tamil)
    {
        // Tamil-first: always true by default
        PlayerPrefs.SetInt("TamilFirst", tamil ? 1 : 0);
    }
}
