// YazhRunnerController.cs — YAZH-UNITY
// Endless Runner Pivot — Jul 14, 2026
// The Yazh pet as endless runner: 3 lanes, jump, slide, swipe/keyboard input.
// Each pet runs differently:
//   குருவி Kuruvi   — double jump (small wings, big heart)
//   மான்   Maan     — higher, floatier jumps
//   யானை  Yanai    — smashes breakable obstacles
//   புள்ளிரூவி Pulliruvi — song-magnet pulls collectibles in

using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class YazhRunnerController : MonoBehaviour
{
    public enum PetAbility { DoubleJump, HighJump, Smash, Magnet }

    [Header("Movement")]
    [SerializeField] private float laneChangeSpeed = 12f;
    [SerializeField] private float jumpVelocity = 8.5f;
    [SerializeField] private float gravity = 24f;
    [SerializeField] private float slideDuration = 0.8f;

    [Header("Pet")]
    public string petType = "kuruvi";
    public PetAbility ability = PetAbility.DoubleJump;
    public float magnetRadius = 4f; // used when ability == Magnet

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private int lane = 1;              // 0 = left, 1 = center, 2 = right
    private float verticalVelocity;
    private bool grounded = true;
    private int jumpsUsed;
    private float slideTimer;

    private float normalHeight;
    private Vector3 normalCenter;

    private bool profileApplied;
    private Vector2 touchStart;
    private bool touchTracking;
    private const float SwipeThresholdPx = 60f;

    public bool IsSliding => slideTimer > 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        capsule = GetComponent<CapsuleCollider>();
        normalHeight = capsule.height;
        normalCenter = capsule.center;
    }

    private void Start()
    {
        // Fallback for scene-wired setups; RunnerBootstrap calls this directly.
        if (!profileApplied) ApplyPetProfile();
    }

    /// <summary>Configure the runner from the selected pet type.</summary>
    public void ApplyPetProfile()
    {
        if (profileApplied) return;
        profileApplied = true;
        switch (petType.ToLowerInvariant())
        {
            case "kuruvi":
                ability = PetAbility.DoubleJump;
                break;
            case "maan":
                ability = PetAbility.HighJump;
                jumpVelocity *= 1.2f;
                gravity *= 0.85f;
                break;
            case "yanai":
                ability = PetAbility.Smash;
                laneChangeSpeed *= 0.85f; // big body, wide turns
                break;
            case "pulliruvi":
                ability = PetAbility.Magnet;
                break;
        }
        Debug.Log($"[YazhRunner] Pet {petType} → ability {ability}");
    }

    private void Update()
    {
        if (RunnerGameManager.Instance != null && !RunnerGameManager.Instance.IsRunning)
            return;

        ReadKeyboard();
        ReadTouch();

        // Vertical motion (manual gravity — kinematic body).
        if (!grounded || verticalVelocity > 0f)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Slide countdown.
        if (slideTimer > 0f)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f) EndSlide();
        }
    }

    private void FixedUpdate()
    {
        Vector3 pos = rb.position;

        // Lane easing.
        float targetX = (lane - 1) * TinaiSegment.LaneWidth;
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        // Vertical.
        pos.y += verticalVelocity * Time.fixedDeltaTime;
        if (pos.y <= 0f)
        {
            pos.y = 0f;
            verticalVelocity = 0f;
            grounded = true;
            jumpsUsed = 0;
        }
        else
        {
            grounded = false;
        }

        rb.MovePosition(pos);
    }

    // ─── Input ────────────────────────────────────────────────────────────────

    private void ReadKeyboard()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        if (kb == null) return;
        if (kb.leftArrowKey.wasPressedThisFrame  || kb.aKey.wasPressedThisFrame) MoveLane(-1);
        if (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame) MoveLane(+1);
        if (kb.upArrowKey.wasPressedThisFrame    || kb.wKey.wasPressedThisFrame ||
            kb.spaceKey.wasPressedThisFrame) Jump();
        if (kb.downArrowKey.wasPressedThisFrame  || kb.sKey.wasPressedThisFrame) Slide();
#else
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A)) MoveLane(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) MoveLane(+1);
        if (Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S)) Slide();
#endif
    }

    private void ReadTouch()
    {
#if ENABLE_INPUT_SYSTEM
        var ts = Touchscreen.current;
        if (ts == null) return;
        var touch = ts.primaryTouch;
        if (touch.press.wasPressedThisFrame)
        {
            touchStart = touch.position.ReadValue();
            touchTracking = true;
        }
        else if (touch.press.wasReleasedThisFrame && touchTracking)
        {
            ResolveSwipe(touch.position.ReadValue() - touchStart);
            touchTracking = false;
        }
#else
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
            touchTracking = true;
        }
        else if (touch.phase == TouchPhase.Ended && touchTracking)
        {
            ResolveSwipe(touch.position - touchStart);
            touchTracking = false;
        }
#endif
    }

    private void ResolveSwipe(Vector2 delta)
    {
        if (delta.magnitude < SwipeThresholdPx) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            MoveLane(delta.x > 0 ? +1 : -1);
        }
        else if (delta.y > 0) Jump();
        else Slide();
    }

    // ─── Actions ──────────────────────────────────────────────────────────────

    public void MoveLane(int direction)
    {
        lane = Mathf.Clamp(lane + direction, 0, TinaiSegment.LaneCount - 1);
    }

    public void Jump()
    {
        int maxJumps = ability == PetAbility.DoubleJump ? 2 : 1;
        if (jumpsUsed >= maxJumps) return;

        if (IsSliding) EndSlideEarly();
        verticalVelocity = jumpVelocity;
        grounded = false;
        jumpsUsed++;
    }

    public void Slide()
    {
        if (!grounded || IsSliding) return;
        slideTimer = slideDuration;
        capsule.height = normalHeight * 0.5f;
        capsule.center = new Vector3(normalCenter.x, normalCenter.y - normalHeight * 0.25f, normalCenter.z);
    }

    private void EndSlide()
    {
        capsule.height = normalHeight;
        capsule.center = normalCenter;
    }

    private void EndSlideEarly()
    {
        slideTimer = 0f;
        EndSlide();
    }

    // ─── Collisions ───────────────────────────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        if (RunnerGameManager.Instance == null || !RunnerGameManager.Instance.IsRunning)
            return;

        var collectible = other.GetComponent<RunnerCollectible>();
        if (collectible != null)
        {
            if (collectible.Collect())
                RunnerGameManager.Instance.OnCollect(collectible);
            return;
        }

        var obstacle = other.GetComponent<RunnerObstacle>();
        if (obstacle != null)
        {
            if (ability == PetAbility.Smash && obstacle.IsBreakable)
            {
                obstacle.Smash();
                RunnerGameManager.Instance.OnSmash();
                return;
            }
            RunnerGameManager.Instance.OnPlayerHit(obstacle);
        }
    }
}
