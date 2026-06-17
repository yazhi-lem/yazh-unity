// MaanScene.cs — YAZH-UNITY
// THOZHAR | Rotation 25 Cycle 7 | Jun 18, 2026
// Second pet scene: Maan (deer) — thoughtful personality

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MaanScene : MonoBehaviour
{
    [Header("UI")]
    public Text titleText;
    public Text dialogueText;
    public Text petNameText;
    public Image petPortrait;

    [Header("Maan-specific")]
    public Button wisdomButton;  // Shows a thoughtful response
    public Button memoryButton;  // Remembers past interactions

    [Header("Stats (thoughtful deer has higher energy, lower happiness)")]
    public Slider healthBar;
    public Slider energyBar;

    private int conversationCount = 0;
    private string[] memories = new string[10]; // Remember last 10 interactions

    void Start()
    {
        if (titleText != null)
        {
            titleText.text = "மான்"; // Maan in Tamil
            UIStyles.ApplyTamilDisplayStyle(titleText);
        }

        if (petNameText != null)
        {
            petNameText.text = "மான் - அறிவுரை";
            UIStyles.ApplyTamilBodyStyle(petNameText);
        }

        if (dialogueText != null)
        {
            dialogueText.text = "வணக்கம்! நான் மான். பொறுமையாக யோசிப்பேன், நீங்கள் சொல்வதை நினைவில் வைப்பேன்.";
            UIStyles.ApplyTamilBodyStyle(dialogueText);
        }

        if (wisdomButton != null)
        {
            wisdomButton.onClick.AddListener(OnWisdomClicked);
            UIStyles.ApplyTamilFirstStyle(wisdomButton);
        }

        if (memoryButton != null)
        {
            memoryButton.onClick.AddListener(OnMemoryClicked);
            UIStyles.ApplyTamilFirstStyle(memoryButton);
        }

        // Maan stats: thoughtful, patient — high energy, calm
        if (healthBar != null)
            UIStyles.ApplyStatBarStyle(healthBar, UIStyles.HealthRed);
        if (energyBar != null)
            UIStyles.ApplyStatBarStyle(energyBar, UIStyles.EnergyAmber);

        Debug.Log("[MaanScene] Initialized — thoughtful deer ready");
    }

    void OnWisdomClicked()
    {
        // Maan gives thoughtful, strategic responses
        string[] wisdoms = new string[]
        {
            "நீர் நன்றாக செய்கிறீர். தொடர்ந்து முயற்சி செய்யுங்கள்.",
            "பொறுமை மிக முக்கியம். காத்திருப்போம்.",
            "நீங்கள் சொன்னதை நினைவில் வைத்துள்ளேன்.",
            "ஆழமாக யோசித்தால், சரியான வழி கிடைக்கும்."
        };
        string wisdom = wisdoms[Random.Range(0, wisdoms.Length)];
        if (dialogueText != null)
            dialogueText.text = wisdom;
        Remember("Wisdom: " + wisdom);
    }

    void OnMemoryClicked()
    {
        // Maan remembers past interactions
        if (dialogueText != null)
        {
            if (conversationCount == 0)
            {
                dialogueText.text = "இது நமது முதல் சந்திப்பு. ஆனால் நீங்கள் நல்லவர் என்று தெரிகிறது.";
            }
            else
            {
                dialogueText.text = "நாம் " + conversationCount + " முறை பேசியுள்ளோம். கடைசியாக நீங்கள் '" +
                                    memories[(conversationCount - 1) % memories.Length] + "' என்று சொன்னீர்கள்.";
            }
        }
    }

    void Remember(string memory)
    {
        memories[conversationCount % memories.Length] = memory;
        conversationCount++;
        Debug.Log("[MaanScene] Memory " + conversationCount + ": " + memory);
    }

    public void OnBackClicked()
    {
        SceneManager.LoadScene("PetSelection");
    }
}
