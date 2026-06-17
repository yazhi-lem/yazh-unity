// UIStyles.cs — YAZH-UNITY
// THOZHAR | Rotation 25 Cycle 7 | Jun 18, 2026
// Applies ART_DIRECTION.md style system to all UI components

using UnityEngine;
using UnityEngine.UI;

public static class UIStyles
{
    // Sangam Earth palette (from ART_DIRECTION.md)
    public static readonly Color DeepRedEarth = new Color(0x8B / 255f, 0x25 / 255f, 0x00 / 255f);
    public static readonly Color TempleGold = new Color(0xD4 / 255f, 0xA8 / 255f, 0x43 / 255f);
    public static readonly Color PalmLeafGreen = new Color(0x2D / 255f, 0x5A / 255f, 0x27 / 255f);

    // Coastal Sky palette
    public static readonly Color MonsoonGrey = new Color(0x6B / 255f, 0x7B / 255f, 0x8C / 255f);
    public static readonly Color LagoonBlue = new Color(0x3A / 255f, 0x7C / 255f, 0xA5 / 255f);
    public static readonly Color NightIndigo = new Color(0x1A / 255f, 0x1A / 255f, 0x2E / 255f);

    // Festival accents
    public static readonly Color MarigoldOrange = new Color(0xFF / 255f, 0x8C / 255f, 0x00 / 255f);
    public static readonly Color JasmineWhite = new Color(0xFF / 255f, 0xF8 / 255f, 0xE7 / 255f);
    public static readonly Color KumkumRed = new Color(0xC4 / 255f, 0x1E / 255f, 0x3A / 255f);

    // Semantic
    public static readonly Color HealthRed = new Color(0xE7 / 255f, 0x4C / 255f, 0x3C / 255f);
    public static readonly Color EnergyAmber = new Color(0xF3 / 255f, 0x9C / 255f, 0x12 / 255f);
    public static readonly Color HungerGreen = new Color(0x27 / 255f, 0xAE / 255f, 0x60 / 255f);
    public static readonly Color HappinessPurple = new Color(0x9B / 255f, 0x59 / 255f, 0xB6 / 255f);

    /// <summary>
    /// Apply brutalist Tamil-first style to a button.
    /// Per ART_DIRECTION.md: Marigold background, Jasmine White text, sharp corners.
    /// </summary>
    public static void ApplyTamilFirstStyle(Button button)
    {
        if (button == null) return;
        Image img = button.GetComponent<Image>();
        if (img != null)
            img.color = MarigoldOrange;

        Text label = button.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.color = JasmineWhite;
            label.fontStyle = FontStyle.Bold;
            label.fontSize = 24;
        }
    }

    /// <summary>
    /// Apply panel style: Night Indigo background, Palm Leaf Green border.
    /// </summary>
    public static void ApplyPanelStyle(GameObject panel)
    {
        if (panel == null) return;
        Image img = panel.GetComponent<Image>();
        if (img != null)
        {
            img.color = new Color(NightIndigo.r, NightIndigo.g, NightIndigo.b, 0.9f);
        }
        Outline outline = panel.GetComponent<Outline>();
        if (outline == null)
            outline = panel.AddComponent<Outline>();
        outline.effectColor = PalmLeafGreen;
        outline.effectDistance = new Vector2(2, 2);
    }

    /// <summary>
    /// Apply stat bar style (health/energy/hunger/happiness).
    /// </summary>
    public static void ApplyStatBarStyle(Slider slider, Color semanticColor)
    {
        if (slider == null) return;

        Image fill = slider.fillRect?.GetComponent<Image>();
        if (fill != null)
            fill.color = semanticColor;

        Image background = slider.GetComponent<Image>();
        if (background != null)
            background.color = new Color(0.2f, 0.2f, 0.2f, 1f);
    }

    /// <summary>
    /// Apply Tamil display style to text (large, white, bold).
    /// </summary>
    public static void ApplyTamilDisplayStyle(Text text)
    {
        if (text == null) return;
        text.color = JasmineWhite;
        text.fontStyle = FontStyle.Bold;
        text.fontSize = 48;
        text.alignment = TextAnchor.MiddleCenter;
    }

    /// <summary>
    /// Apply Tamil body style to text (medium, white, regular).
    /// </summary>
    public static void ApplyTamilBodyStyle(Text text)
    {
        if (text == null) return;
        text.color = JasmineWhite;
        text.fontStyle = FontStyle.Normal;
        text.fontSize = 16;
    }
}
