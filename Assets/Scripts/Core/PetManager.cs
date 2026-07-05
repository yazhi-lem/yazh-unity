using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages pet instances, their state, and animation.
/// Each pet has personality override for dialogue tone.
/// </summary>
public class PetManager : MonoBehaviour
{
    [System.Serializable]
    public class PetPersonality
    {
        public string petType;      // kuruvi, maan, yanai, pulliruvi
        public string tamilName;    // குருவி, மான், யானை, புள்ளிரூவி
        public string personality;  // energetic, graceful, wise, artistic
        public float voicePitch;
        public float animationSpeed;
    }

    [SerializeField] private List<PetPersonality> petPersonalities = new();
    [SerializeField] private Transform petSpawnPoint;
    
    private GameObject activePet;
    private Animator petAnimator;
    private PetPersonality activePetPersonality;

    private Dictionary<string, GameObject> petPrefabs = new();

    private void OnEnable()
    {
        LoadPetPersonalities();
        LoadPetPrefabs();
    }

    private void LoadPetPersonalities()
    {
        // Load from JSON: Assets/Resources/Config/PetPersonalities.json
        TextAsset configAsset = Resources.Load<TextAsset>("Config/PetPersonalities");
        if (configAsset != null)
        {
            // Parse JSON (use JsonUtility or Newtonsoft.Json)
            Debug.Log("[PetManager] Pet personalities loaded");
        }
    }

    private void LoadPetPrefabs()
    {
        // Load from Resources/Prefabs/Pets/
        petPrefabs["kuruvi"] = Resources.Load<GameObject>("Prefabs/Pets/Kuruvi");
        petPrefabs["maan"] = Resources.Load<GameObject>("Prefabs/Pets/Maan");
        petPrefabs["yanai"] = Resources.Load<GameObject>("Prefabs/Pets/Yanai");
        petPrefabs["pulliruvi"] = Resources.Load<GameObject>("Prefabs/Pets/Pulliruvi");
        
        Debug.Log("[PetManager] Pet prefabs loaded");
    }

    public void SpawnPet(string petType)
    {
        if (activePet != null)
            Destroy(activePet);

        if (!petPrefabs.TryGetValue(petType, out var prefab))
        {
            Debug.LogError($"[PetManager] Pet prefab not found: {petType}");
            return;
        }

        Vector3 spawnPos = petSpawnPoint != null ? petSpawnPoint.position : Vector3.zero;
        activePet = Instantiate(prefab, spawnPos, Quaternion.identity);
        petAnimator = activePet.GetComponent<Animator>();

        // Set personality
        activePetPersonality = petPersonalities.Find(p => p.petType == petType);
        if (activePetPersonality != null)
        {
            petAnimator.speed = activePetPersonality.animationSpeed;
        }

        Debug.Log($"[PetManager] Spawned pet: {petType} at {spawnPos}");
    }

    public void PlayAnimation(string triggerName)
    {
        if (petAnimator != null)
        {
            petAnimator.SetTrigger(triggerName);
            Debug.Log($"[PetManager] Playing animation: {triggerName}");
        }
    }

    public void PlayEmotionAnimation(string emotion)
    {
        // emotion: happy, sad, curious, tired, angry, celebrate
        PlayAnimation($"emotion_{emotion}");
    }

    public void SyncLipSync(float audioLength)
    {
        // Timeline sync: audio length → animation length
        if (petAnimator != null)
        {
            // Set animation clip length to match audio
            petAnimator.SetFloat("TalkDuration", audioLength);
        }
    }

    public GameObject GetActivePet() => activePet;
    public PetPersonality GetActivePetPersonality() => activePetPersonality;
}
