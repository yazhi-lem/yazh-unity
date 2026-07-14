// RunnerHUD.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// Tamil-first runner HUD: score, distance, collectibles, tinai banner,
// game-over panel. Builds its own uGUI canvas so no scene wiring is needed.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RunnerHUD : MonoBehaviour
{
    private Text scoreText;
    private Text distanceText;
    private Text collectibleText;
    private Text tinaiBanner;
    private GameObject gameOverPanel;
    private Text gameOverText;

    private Coroutine bannerRoutine;

    private void Start()
    {
        BuildCanvas();
    }

    private void OnEnable()
    {
        RunnerGameManager.OnScoreChanged += HandleScore;
        RunnerGameManager.OnCollected += HandleCollected;
        RunnerGameManager.OnGameOver += HandleGameOver;
        EndlessTerrainGenerator.OnTinaiChanged += HandleTinaiChanged;
    }

    private void OnDisable()
    {
        RunnerGameManager.OnScoreChanged -= HandleScore;
        RunnerGameManager.OnCollected -= HandleCollected;
        RunnerGameManager.OnGameOver -= HandleGameOver;
        EndlessTerrainGenerator.OnTinaiChanged -= HandleTinaiChanged;
    }

    private void Update()
    {
        if (RunnerGameManager.Instance != null && distanceText != null)
        {
            distanceText.text = $"{RunnerGameManager.Instance.Distance:F0} மீ";
        }
    }

    // ─── Event handlers ───────────────────────────────────────────────────────

    private void HandleScore(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString("N0");
    }

    private void HandleCollected(int count, string tamilName)
    {
        if (collectibleText != null) collectibleText.text = $"{tamilName} × {count}";
    }

    private void HandleTinaiChanged(TinaiTheme theme)
    {
        if (tinaiBanner == null) return;
        tinaiBanner.text = $"{theme.tamilName}\n{theme.englishName}";
        if (bannerRoutine != null) StopCoroutine(bannerRoutine);
        bannerRoutine = StartCoroutine(ShowBanner());
    }

    private IEnumerator ShowBanner()
    {
        tinaiBanner.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        tinaiBanner.gameObject.SetActive(false);
    }

    private void HandleGameOver(int finalScore)
    {
        if (gameOverPanel == null) return;
        gameOverText.text = $"ஓட்டம் முடிந்தது!\nமதிப்பெண்: {finalScore:N0}\n" +
                            $"தூரம்: {RunnerGameManager.Instance.Distance:F0} மீ";
        gameOverPanel.SetActive(true);
    }

    // ─── Canvas construction ──────────────────────────────────────────────────

    private void BuildCanvas()
    {
        var canvasGO = new GameObject("RunnerHUD_Canvas");
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        canvasGO.AddComponent<GraphicRaycaster>();

        scoreText = MakeText(canvasGO.transform, "Score", new Vector2(0.5f, 1f),
            new Vector2(0f, -80f), 56, "0");
        UIStyles.ApplyTamilDisplayStyle(scoreText);
        scoreText.fontSize = 56;

        distanceText = MakeText(canvasGO.transform, "Distance", new Vector2(0f, 1f),
            new Vector2(140f, -80f), 32, "0 மீ");
        UIStyles.ApplyTamilBodyStyle(distanceText);
        distanceText.fontSize = 32;

        collectibleText = MakeText(canvasGO.transform, "Collectibles", new Vector2(1f, 1f),
            new Vector2(-140f, -80f), 32, "");
        UIStyles.ApplyTamilBodyStyle(collectibleText);
        collectibleText.fontSize = 32;

        tinaiBanner = MakeText(canvasGO.transform, "TinaiBanner", new Vector2(0.5f, 0.7f),
            Vector2.zero, 64, "");
        UIStyles.ApplyTamilDisplayStyle(tinaiBanner);
        tinaiBanner.color = UIStyles.TempleGold;
        tinaiBanner.gameObject.SetActive(false);

        BuildGameOverPanel(canvasGO.transform);

        // Show the opening tinai banner (the generator's first OnTinaiChanged
        // may fire before this canvas exists, depending on Start order).
        var manager = RunnerGameManager.Instance;
        if (manager != null && manager.terrain != null)
            HandleTinaiChanged(manager.terrain.CurrentTheme);
    }

    private void BuildGameOverPanel(Transform parent)
    {
        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(parent, false);
        var rect = gameOverPanel.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.1f, 0.3f);
        rect.anchorMax = new Vector2(0.9f, 0.7f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        gameOverPanel.AddComponent<Image>();
        UIStyles.ApplyPanelStyle(gameOverPanel);

        gameOverText = MakeText(gameOverPanel.transform, "GameOverText",
            new Vector2(0.5f, 0.65f), Vector2.zero, 44, "");
        UIStyles.ApplyTamilDisplayStyle(gameOverText);
        gameOverText.fontSize = 44;

        // "Run again" button — மீண்டும் ஓடு
        var buttonGO = new GameObject("RestartButton");
        buttonGO.transform.SetParent(gameOverPanel.transform, false);
        var buttonRect = buttonGO.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.25f, 0.1f);
        buttonRect.anchorMax = new Vector2(0.75f, 0.3f);
        buttonRect.offsetMin = buttonRect.offsetMax = Vector2.zero;
        buttonGO.AddComponent<Image>();
        var button = buttonGO.AddComponent<Button>();

        var label = MakeText(buttonGO.transform, "Label", new Vector2(0.5f, 0.5f),
            Vector2.zero, 32, "மீண்டும் ஓடு");
        var labelRect = label.rectTransform;
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = labelRect.offsetMax = Vector2.zero;

        UIStyles.ApplyTamilFirstStyle(button);
        button.onClick.AddListener(() => RunnerGameManager.Instance.Restart());

        gameOverPanel.SetActive(false);
    }

    private static Text MakeText(Transform parent, string name, Vector2 anchor,
                                 Vector2 offset, int fontSize, string content)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.anchoredPosition = offset;
        rect.sizeDelta = new Vector2(600f, 160f);

        var text = go.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.text = content;
        return text;
    }
}
