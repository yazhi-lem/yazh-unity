// PetSelectionController.cs — YAZH-UNITY
// THOZHAR | Rotation 25 | Jun 17, 2026
// Cycle 9: Wired UIStyles (Tamil-first brutalist) — Jun 18, 2026
// Controls pet selection: Kuruvi, Maan, Yanai, Pulliruvi

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PetSelectionController : MonoBehaviour
{
    [Header("Pet Buttons")]
    public Button kuruviButton;   // Sparrow
    public Button maanButton;     // Deer
    public Button yanaiButton;    // Elephant
    public Button pulliruviButton; // Tiger Cat

    [Header("Pet Info Panel")]
    public GameObject infoPanel;
    public Text petNameText;
    public Text petDescText;
    public Image petIcon;

    [Header("Pet Data")]
    public Sprite kuruviSprite;
    public Sprite maanSprite;
    public Sprite yanaiSprite;
    public Sprite pulliruviSprite;

    private string selectedPet = "";

    void Start()
    {
        // Apply Tamil-first brutalist style (Cycle 9)
        ApplyYazhiStyle();

        // Wire buttons
        if (kuruviButton != null)
            kuruviButton.onClick.AddListener(() => SelectPet("Kuruvi"));
        if (maanButton != null)
            maanButton.onClick.AddListener(() => SelectPet("Maan"));
        if (yanaiButton != null)
            yanaiButton.onClick.AddListener(() => SelectPet("Yanai"));
        if (pulliruviButton != null)
            pulliruviButton.onClick.AddListener(() => SelectPet("Pulliruvi"));

        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            UIStyles.ApplyPanelStyle(infoPanel);
        }
    }

    void ApplyYazhiStyle()
    {
        // Cycle 9: Tamil-first styling per ART_DIRECTION.md
        UIStyles.ApplyTamilFirstStyle(kuruviButton);
        UIStyles.ApplyTamilFirstStyle(maanButton);
        UIStyles.ApplyTamilFirstStyle(yanaiButton);
        UIStyles.ApplyTamilFirstStyle(pulliruviButton);
        UIStyles.ApplyTamilDisplayStyle(petNameText);
        UIStyles.ApplyTamilBodyStyle(petDescText);
        Debug.Log("[PetSelection] Tamil-first style applied");
    }

    void SelectPet(string petName)
    {
        selectedPet = petName;
        Debug.Log("[PetSelection] Selected: " + petName);

        // Show info panel
        if (infoPanel != null)
            infoPanel.SetActive(true);

        // Update info
        if (petNameText != null)
            petNameText.text = GetPetDisplayName(petName);
        if (petDescText != null)
            petDescText.text = GetPetDescription(petName);
        if (petIcon != null)
            petIcon.sprite = GetPetSprite(petName);

        // Store selection in GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.selectedPet = petName;
        }
    }

    public void OnConfirmClicked()
    {
        if (string.IsNullOrEmpty(selectedPet))
        {
            Debug.LogWarning("[PetSelection] No pet selected!");
            return;
        }

        Debug.Log("[PetSelection] Confirmed: " + selectedPet + " — loading BiomeArena");
        SceneManager.LoadScene("BiomeArena");
    }

    public void OnBackClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    string GetPetDisplayName(string pet)
    {
        switch (pet)
        {
            case "Kuruvi": return "குறுவி";
            case "Maan": return "மான்";
            case "Yanai": return "யானை";
            case "Pulliruvi": return "புல்லுருவி";
            default: return pet;
        }
    }

    string GetPetDescription(string pet)
    {
        switch (pet)
        {
            case "Kuruvi": return "Curious and fast. Loves to explore.";
            case "Maan": return "Thoughtful and gentle. Good at survival strategy.";
            case "Yanai": return "Wise and patient. Remembers everything.";
            case "Pulliruvi": return "Playful and independent. Full of surprises.";
            default: return "";
        }
    }

    Sprite GetPetSprite(string pet)
    {
        switch (pet)
        {
            case "Kuruvi": return kuruviSprite;
            case "Maan": return maanSprite;
            case "Yanai": return yanaiSprite;
            case "Pulliruvi": return pulliruviSprite;
            default: return null;
        }
    }
}
