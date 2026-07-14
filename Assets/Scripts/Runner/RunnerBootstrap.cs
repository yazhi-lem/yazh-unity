// RunnerBootstrap.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// Drop this single component on an empty GameObject in an empty scene and
// press Play: it assembles the whole runner (camera, light, pet, terrain,
// game manager, HUD) from code. Matches the project's code-first style —
// no prefab or scene wiring required.

using UnityEngine;
using UnityEngine.EventSystems;

public class RunnerBootstrap : MonoBehaviour
{
    [Header("Pet (falls back to GameManager selection, then kuruvi)")]
    [SerializeField] private string petTypeOverride = "";

    private void Awake()
    {
        string petType = ResolvePetType();

        // ── Camera: classic runner chase view ────────────────────────────────
        var cam = Camera.main;
        if (cam == null)
        {
            var camGO = new GameObject("Main Camera") { tag = "MainCamera" };
            cam = camGO.AddComponent<Camera>();
            camGO.AddComponent<AudioListener>();
        }
        cam.transform.position = new Vector3(0f, 4.5f, -8f);
        cam.transform.rotation = Quaternion.Euler(18f, 0f, 0f);

        // ── Light ─────────────────────────────────────────────────────────────
        if (FindFirstObjectByType<Light>() == null)
        {
            var lightGO = new GameObject("Sun");
            var sun = lightGO.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.1f;
            lightGO.transform.rotation = Quaternion.Euler(55f, -30f, 0f);
        }

        // ── The runner: the Yazh pet ─────────────────────────────────────────
        // Root pivot sits at foot level (y = 0); the placeholder capsule body
        // is a child until pet models land — PetManager prefabs can replace
        // the body later without touching the controller.
        var petGO = new GameObject($"YazhRunner_{petType}");
        petGO.transform.position = Vector3.zero;

        var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.name = "Body";
        body.transform.SetParent(petGO.transform, false);
        body.transform.localPosition = new Vector3(0f, 1f, 0f);
        Destroy(body.GetComponent<Collider>());
        body.GetComponent<Renderer>().material.color = PetColor(petType);

        var petCollider = petGO.AddComponent<CapsuleCollider>();
        petCollider.center = new Vector3(0f, 1f, 0f);
        petCollider.height = 2f;
        petCollider.radius = 0.45f;

        var runner = petGO.AddComponent<YazhRunnerController>();
        runner.petType = petType;
        runner.ApplyPetProfile();

        // ── Terrain ───────────────────────────────────────────────────────────
        var terrainGO = new GameObject("EndlessTinaiTerrain");
        var terrain = terrainGO.AddComponent<EndlessTerrainGenerator>();
        terrain.runner = petGO.transform;
        if (runner.ability == YazhRunnerController.PetAbility.Magnet)
            terrain.magnetRadius = runner.magnetRadius;

        // ── Game manager + HUD ────────────────────────────────────────────────
        var managerGO = new GameObject("RunnerGameManager");
        var manager = managerGO.AddComponent<RunnerGameManager>();
        manager.terrain = terrain;
        manager.runner = runner;

        new GameObject("RunnerHUD").AddComponent<RunnerHUD>();

        // uGUI needs an EventSystem for the restart button.
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            es.AddComponent<StandaloneInputModule>();
#endif
        }

        Debug.Log($"[RunnerBootstrap] Tinai endless runner assembled. Pet: {petType}. ஓடு!");
    }

    private string ResolvePetType()
    {
        if (!string.IsNullOrEmpty(petTypeOverride)) return petTypeOverride;
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.GetSelectedPet()))
            return GameManager.Instance.GetSelectedPet().ToLowerInvariant();
        return "kuruvi";
    }

    private static Color PetColor(string petType) => petType switch
    {
        "kuruvi"    => new Color(0.72f, 0.48f, 0.25f), // sparrow brown
        "maan"      => new Color(0.85f, 0.65f, 0.40f), // deer fawn
        "yanai"     => new Color(0.55f, 0.55f, 0.60f), // elephant grey
        "pulliruvi" => new Color(0.80f, 0.75f, 0.78f), // dove rose-grey
        _           => Color.white
    };
}
