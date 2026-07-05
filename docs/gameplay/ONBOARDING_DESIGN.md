# YAZH-UNITY Long Onboarding Design
> Cycle 10 | Jun 18, 2026

## Why "Long"?
The onboarding is intentionally thorough (~3-5 min) because Yazhi's sovereignty principle demands **informed consent** before any subscription or data handling. This is non-negotiable.

## Principles
1. **No dark patterns** — every screen is honest about what's happening
2. **Privacy-first** — disclose BEFORE collecting anything
3. **Reversible** — user can change tier/settings anytime
4. **Tamil-first** — primary language Tamil, English secondary
5. **No data exfil** — all processing on-device
6. **Audit-trail** — COPPA consent logged for regulatory review

## Onboarding Flow (8 screens)

### Screen 1: Welcome
```
┌──────────────────────────────────┐
│                                  │
│         யாழ் (Yazh)              │
│                                  │
│   Welcome to YAZH-UNITY          │
│                                  │
│   Tamil AI pet game for kids.    │
│   Sovereign. Private. Yours.     │
│                                  │
│      [   Continue   ]            │
│                                  │
│   Already have an account?        │
│   [Restore subscription]          │
└──────────────────────────────────┘
```

**Trigger:** First app launch
**Action:** [Continue] → Screen 2
**Skip:** [Restore subscription] → Screen 7 (subscription tier)

---

### Screen 2: Language Selection
```
┌──────────────────────────────────┐
│                                  │
│   Choose your language           │
│                                  │
│      ┌─────────┐                 │
│      │  தமிழ்   │                 │
│      │  Tamil   │                 │
│      └─────────┘                 │
│      ┌─────────┐                 │
│      │ English │                 │
│      └─────────┘                 │
│                                  │
└──────────────────────────────────┘
```

**Default:** Tamil (Yazhi's primary language)
**Note:** Affects all UI from this point forward

---

### Screen 3: Sovereignty Disclosure (CRITICAL)
```
┌──────────────────────────────────┐
│                                  │
│   How your data is handled       │
│                                  │
│   ✓ All AI runs on YOUR device   │
│   ✓ We never see conversations   │
│   ✓ No analytics, no tracking    │
│   ✓ Code is open source          │
│   ✓ COPPA + DPDP compliant       │
│   ✓ 7-day free trial, cancel any │
│                                  │
│   Read full policy:              │
│   yazhi.dev/privacy              │
│                                  │
│      [ I understand ]            │
│                                  │
└──────────────────────────────────┘
```

**Why mandatory:** User must understand zero-cloud before using app
**Action:** [I understand] → Screen 4

---

### Screen 4: COPPA Parental Consent (GATING)
```
┌──────────────────────────────────┐
│                                  │
│   Parental Consent Required      │
│   (for users under 18)           │
│                                  │
│   Yazh uses your camera for AR.  │
│   All processing stays on-device.│
│                                  │
│   Parent/Guardian Email:         │
│   [_________________]            │
│                                  │
│   ☐ I am the parent/guardian    │
│     and consent to data          │
│     practices above              │
│                                  │
│   [Decline]    [Accept & Continue]│
│                                  │
└──────────────────────────────────┘
```

**Behavior:**
- [Decline] → App exits (no app usage without consent)
- [Accept] → Email validated + consent logged → Screen 5

**Audit log:** Timestamp + platform + parent email + consent status
**File:** `coppa_consent_audit.log` (on-device, regulatory review-ready)

---

### Screen 5: Child Profile
```
┌──────────────────────────────────┐
│                                  │
│   Tell us about your child       │
│                                  │
│   Nickname (optional):           │
│   [_________________]            │
│                                  │
│   Age:                           │
│   [6] [7] [8] [9] [10] [11] [12] [13] [14]│
│                                  │
│   Favorite pet:                  │
│   🐦    🦌    🐘    🐅           │
│   Kuruvi Maan Yanai Pulliruvi   │
│                                  │
│      [ Continue ]                │
│                                  │
│   [ Skip for now ]               │
└──────────────────────────────────┘
```

**Optional:** All fields optional, defaults applied if skipped
**Storage:** All on-device (no cloud)

---

### Screen 6: Pet Selection
```
┌──────────────────────────────────┐
│                                  │
│   Choose your companion          │
│                                  │
│   ┌──────────────────────────┐   │
│   │ 🐦 குறுவி (Kuruvi)      │   │
│   │ Curious sparrow           │   │
│   │ Fast learner, explorer    │   │
│   │ [ Select ]                │   │
│   └──────────────────────────┘   │
│   ┌──────────────────────────┐   │
│   │ 🦌 மான் (Maan)           │   │
│   │ Thoughtful deer           │   │
│   │ Strategic guide           │   │
│   │ [ Select ]                │   │
│   └──────────────────────────┘   │
│   ┌──────────────────────────┐   │
│   │ 🐘 யானை (Yanai)          │   │
│   │ Wise elephant             │   │
│   │ Memory keeper             │   │
│   │ [ Select ]                │   │
│   └──────────────────────────┘   │
│   ┌──────────────────────────┐   │
│   │ 🐅 புல்லுருவி (Pulliruvi)  │   │
│   │ Playful tiger cat         │   │
│   │ Independent spirit        │   │
│   │ [ Select ]                │   │
│   └──────────────────────────┘   │
│                                  │
└──────────────────────────────────┘
```

**Note:** Free tier = Kuruvi only. Others locked until subscription.
**Visual cue:** Locked pets shown but disabled with crown icon.

---

### Screen 7: Subscription Tier
```
┌──────────────────────────────────┐
│                                  │
│   Choose your plan               │
│                                  │
│   ┌──────────────────────────┐   │
│   │ FREE                      │   │
│   │ Kuruvi pet, 1 biome      │   │
│   │ 100 dialogue phrases      │   │
│   │ [ Continue with Free ]    │   │
│   └──────────────────────────┘   │
│   ┌──────────────────────────┐   │
│   │ FAMILY — $4.99/mo         │ ← Highlighted │
│   │ All 4 pets, all biomes   │   │
│   │ Unlimited dialogue        │   │
│   │ 4 child profiles          │   │
│   │ [ Start 7-day Free Trial ]│   │
│   └──────────────────────────┘   │
│   ┌──────────────────────────┐   │
│   │ SCHOOL — $199.99/yr       │   │
│   │ 30 child profiles         │   │
│   │ Teacher dashboard         │   │
│   │ [ Learn more ]            │   │
│   └──────────────────────────┘   │
│                                  │
│   Cancel anytime. No hidden fees.│
│                                  │
│   [ Skip — decide later ]        │
└──────────────────────────────────┘
```

**Behavior:**
- [Continue with Free] → Screen 8
- [Start Free Trial] → Play Billing flow → Screen 8
- [Skip] → Use Free tier, can upgrade later in Settings

---

### Screen 8: Ready
```
┌──────────────────────────────────┐
│                                  │
│        ✨ You're ready! ✨        │
│                                  │
│   Your companion awaits.         │
│                                  │
│      [ Enter YAZH-UNITY ]        │
│                                  │
│   (First pet: Kuruvi)            │
│                                  │
└──────────────────────────────────┘
```

**Action:** → MainMenu scene with selected pet

---

## Implementation Details

### State Machine
```
enum OnboardingState {
    Welcome,
    Language,
    Sovereignty,
    ParentalConsent,
    ChildProfile,
    PetSelection,
    Subscription,
    Complete
}

OnboardingState currentState = OnboardingState.Welcome;
```

### Persistence
```
SharedPreferences ("yazhi_onboarding"):
  onboarding_complete: bool
  onboarding_version: int
  selected_language: string ("ta" | "en")
  selected_pet: string ("Kuruvi" | "Maan" | "Yanai" | "Pulliruvi")
  parental_consent: bool
  parental_email: string
  parental_consent_timestamp: ISO8601
  child_nickname: string (optional)
  child_age: int (optional)
  subscription_tier: string ("free" | "family" | "school")
  subscription_started: ISO8601 (if any)
```

### Skip Logic
- Cannot skip: Welcome, Language, Sovereignty, ParentalConsent
- Can skip: ChildProfile, PetSelection (defaults), Subscription (use Free)

### Restore Flow (Returning Users)
```
if onboarding_complete and subscription_tier == "family":
    → MainMenu directly
elif onboarding_complete and subscription_tier == "free":
    → MainMenu with Free tier restrictions
else:
    → Start onboarding from Welcome
```

### Conversion Tracking
- Track each screen transition (locally only)
- Aggregate stats: drop-off at each screen
- Help iterate UX (off-device, opt-in)
- Never send to cloud (zero telemetry principle)

## Compliance Notes

### COPPA (US)
- Verifiable parental consent: ✅ Email + attestation
- Data minimization: ✅ Only parent email stored
- Right to delete: ✅ Settings → "Delete all data"
- No behavioral advertising: ✅
- No third-party SDKs: ✅

### DPDP (India)
- Verifiable consent: ✅ Same as COPPA
- Data principal rights: ✅ Access, correction, erasure
- Children's data: ✅ Special protections
- Data localization: ✅ On-device only

### GDPR-K (EU)
- Age-appropriate design: ✅
- Parental consent: ✅
- Privacy by default: ✅ Zero telemetry
- Right to erasure: ✅

## A/B Testing Plan (Future)

After launch, test:
- 8 screens vs 6 screens (combine Language + Sovereignty)
- Onboarding time target: 3-5 min (acceptable), >5 min (drop-off)
- COPPA gate placement: Before profile vs after profile

## Success Metrics

- Onboarding completion rate: >70%
- Average time to complete: 3-5 min
- Drop-off screen: Track per-screen
- Subscription conversion from onboarding: >5%
- Day-1 retention: >60%

## Files Referenced

- `apps/yazh-unity/Assets/Plugins/Android/COPPA_CONSENT_ACTIVITY.java` (Cycle 9)
- `apps/yazh-unity/Assets/Plugins/Android/values/strings.xml` (Cycle 9)
- `apps/yazh-unity/Assets/Scripts/UI/UIStyles.cs` (Cycle 7)
- `PLAY_STORE_PLAN.md` (this cycle)
- `memory/2026-06-18-STRATEGIC-PIVOT.md` (this cycle)

---
*Created: 2026-06-18 | Cycle 10 | Privacy-first onboarding for Play Store launch*
