// TinaiSystem.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// The five Sangam tinai landscapes as endless-runner terrain themes.
// குறிஞ்சி → முல்லை → மருதம் → நெய்தல் → பாலை, cycling forever.

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// The five landscapes (tinai) of Sangam literature.
/// Each tinai themes a stretch of the endless track: colors, obstacles,
/// collectibles, and difficulty.
/// </summary>
public enum TinaiType
{
    Kurinji,   // குறிஞ்சி — mountains
    Mullai,    // முல்லை — forest / pasture
    Marutham,  // மருதம் — farmland / riverine
    Neithal,   // நெய்தல் — seashore
    Palai      // பாலை — arid wasteland
}

/// <summary>
/// Visual + gameplay theme for one tinai stretch of track.
/// </summary>
[System.Serializable]
public class TinaiTheme
{
    public TinaiType type;
    public string tamilName;        // குறிஞ்சி …
    public string englishName;      // Mountains …
    public string flowerName;       // the tinai's namesake flower (Tamil)
    public string collectibleName;  // தேன், முத்து …

    public Color groundColor;
    public Color skyColor;
    public Color decorColor;        // side props (trees, rocks, palms)
    public Color obstacleColor;
    public Color collectibleColor;

    [Range(0f, 1f)] public float obstacleDensity;    // chance of obstacle per lane row
    [Range(0f, 1f)] public float collectibleDensity; // chance of collectible run per segment
    public float speedModifier = 1f;                 // Palai runs hotter/faster
}

/// <summary>
/// Static provider of tinai themes and the endless tinai cycle.
/// Distance along the run maps deterministically to a tinai, so the same
/// run distance always shows the same landscape.
/// </summary>
public static class TinaiSystem
{
    /// <summary>Meters of track per tinai before transitioning to the next.</summary>
    public const float TinaiLengthMeters = 400f;

    // Classical ordering of the five akam landscapes.
    public static readonly TinaiType[] Cycle =
    {
        TinaiType.Kurinji,
        TinaiType.Mullai,
        TinaiType.Marutham,
        TinaiType.Neithal,
        TinaiType.Palai
    };

    private static Dictionary<TinaiType, TinaiTheme> themes;

    /// <summary>Which tinai the track shows at a given distance from the start.</summary>
    public static TinaiType GetTinaiForDistance(float meters)
    {
        int index = Mathf.FloorToInt(Mathf.Max(0f, meters) / TinaiLengthMeters) % Cycle.Length;
        return Cycle[index];
    }

    /// <summary>How far (0–1) the runner is through the current tinai stretch.</summary>
    public static float GetTinaiProgress(float meters)
    {
        return Mathf.Repeat(Mathf.Max(0f, meters), TinaiLengthMeters) / TinaiLengthMeters;
    }

    public static TinaiTheme GetTheme(TinaiType type)
    {
        EnsureThemes();
        return themes[type];
    }

    public static TinaiTheme GetThemeForDistance(float meters)
    {
        return GetTheme(GetTinaiForDistance(meters));
    }

    private static void EnsureThemes()
    {
        if (themes != null) return;
        themes = new Dictionary<TinaiType, TinaiTheme>();

        // Sangam Earth palette anchors (see UIStyles / ART_DIRECTION.md).
        themes[TinaiType.Kurinji] = new TinaiTheme
        {
            type = TinaiType.Kurinji,
            tamilName = "குறிஞ்சி",
            englishName = "Mountains",
            flowerName = "குறிஞ்சி மலர்",
            collectibleName = "தேன்",            // honey
            groundColor = Hex("5A4632"),
            skyColor = Hex("7BA7C9"),
            decorColor = Hex("4E5A50"),          // grey-green crags
            obstacleColor = Hex("6E6258"),       // boulders
            collectibleColor = Hex("E8A825"),    // honey gold
            obstacleDensity = 0.45f,
            collectibleDensity = 0.55f,
            speedModifier = 1.0f
        };

        themes[TinaiType.Mullai] = new TinaiTheme
        {
            type = TinaiType.Mullai,
            tamilName = "முல்லை",
            englishName = "Forest",
            flowerName = "முல்லை மலர்",
            collectibleName = "முல்லை மலர்",     // jasmine
            groundColor = Hex("2D5A27"),         // PalmLeafGreen
            skyColor = Hex("9FCBE0"),
            decorColor = Hex("1E3D1A"),          // deep forest
            obstacleColor = Hex("5B4327"),       // fallen logs
            collectibleColor = Hex("FFF8E7"),    // JasmineWhite
            obstacleDensity = 0.50f,
            collectibleDensity = 0.55f,
            speedModifier = 1.0f
        };

        themes[TinaiType.Marutham] = new TinaiTheme
        {
            type = TinaiType.Marutham,
            tamilName = "மருதம்",
            englishName = "Farmland",
            flowerName = "மருத மலர்",
            collectibleName = "நெல்",            // paddy grain
            groundColor = Hex("7A8F3C"),         // paddy green-gold
            skyColor = Hex("C9DDEB"),
            decorColor = Hex("3A7CA5"),          // LagoonBlue canals
            obstacleColor = Hex("8B5A2B"),       // bullock carts
            collectibleColor = Hex("D4A843"),    // TempleGold
            obstacleDensity = 0.55f,
            collectibleDensity = 0.60f,
            speedModifier = 1.05f
        };

        themes[TinaiType.Neithal] = new TinaiTheme
        {
            type = TinaiType.Neithal,
            tamilName = "நெய்தல்",
            englishName = "Seashore",
            flowerName = "நெய்தல் மலர்",
            collectibleName = "முத்து",           // pearl
            groundColor = Hex("D9C79A"),          // sand
            skyColor = Hex("3A7CA5"),             // LagoonBlue
            decorColor = Hex("2D5A27"),           // palms
            obstacleColor = Hex("6B4A2F"),        // beached boats / nets
            collectibleColor = Hex("F2F0EA"),     // pearl white
            obstacleDensity = 0.60f,
            collectibleDensity = 0.50f,
            speedModifier = 1.10f
        };

        themes[TinaiType.Palai] = new TinaiTheme
        {
            type = TinaiType.Palai,
            tamilName = "பாலை",
            englishName = "Wasteland",
            flowerName = "பாலை மலர்",
            collectibleName = "நீர் துளி",        // water drop (scarce, precious)
            groundColor = Hex("8B2500"),          // DeepRedEarth
            skyColor = Hex("E8B87A"),             // heat haze
            decorColor = Hex("5E4934"),           // dry thorn trees
            obstacleColor = Hex("4A3626"),        // sun-cracked rock
            collectibleColor = Hex("6FC3DF"),     // water blue
            obstacleDensity = 0.70f,              // hardest stretch
            collectibleDensity = 0.35f,           // scarcity is the theme
            speedModifier = 1.20f
        };
    }

    private static Color Hex(string hex)
    {
        return ColorUtility.TryParseHtmlString("#" + hex, out var c) ? c : Color.magenta;
    }
}
