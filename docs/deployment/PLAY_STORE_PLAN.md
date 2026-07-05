# YAZH-UNITY — Play Store Launch Plan
> Cycle 10 | Jun 18, 2026

## Goal
Publish YAZH-UNITY on Google Play Store with in-app subscriptions, **gated by long privacy-first onboarding**.

## Timeline (Aggressive)
- **Week 1 (Jun 18-24):** Play Console setup, store listing, APK build
- **Week 2 (Jun 25-Jul 1):** Internal Testing track → Closed Testing
- **Week 3 (Jul 2-8):** Production track, marketing push
- **Week 4 (Jul 9-15):** Public launch, diaspora outreach activation

## Play Console Setup

### Account Requirements
- Google Play Developer account: $25 one-time
- Identity verification (D-U-N-S for organizations)
- Payment profile for IAP revenue

### App Registration
```
App name:        YAZH-UNITY — Tamil Pet Game
Package name:    org.yazhi.unity
Default language: Tamil (ta)
Free / Paid:      Free (with in-app purchases)
```

## Store Listing

### App Title (50 chars max)
```
YAZH-UNITY — Tamil Pet Game
```

### Short Description (80 chars max)
```
On-device Tamil AI pet game for kids. Sovereign, private, no cloud.
```

### Full Description (4000 chars max)

```
YAZH-UNITY is the first Tamil-first on-device AI pet game for children
ages 6-14. Every pet speaks Tamil. Every story is Tamil. Every UI
element honors Tamil cultural heritage.

FOUR PETS, FOUR PERSONALITIES:
• குறுவி (Kuruvi) — Curious sparrow, fast learner
• மான் (Maan) — Thoughtful deer, strategic guide
• யானை (Yanai) — Wise elephant, memory keeper
• புல்லுருவி (Pulliruvi) — Playful tiger cat, independent spirit

SOVEREIGN BY DESIGN:
Unlike cloud-based AI, YAZH-UNITY runs entirely on your device. No
data leaves your phone. No analytics. No tracking. No cloud accounts.
All processing happens locally using the Yazh 30K Tamil language model.

PRIVACY FIRST:
• Zero cloud inference (works offline)
• Zero telemetry, zero analytics
• COPPA, DPDP, GDPR-K compliant
• Parental consent built-in (long onboarding)
• Open source code (auditable)

BUILT BY THE TAMIL DIASPORA, FOR THE TAMIL DIASPORA:
Yazhi is a sovereign Tamil tech initiative building language-preservation
tools, on-device AI, and mesh communications. We build for our mothers,
our workers, our children — not for Silicon Valley.

SUBSCRIPTION TIERS:
• Free: Kuruvi pet, 1 biome, 100 phrases (limited)
• Family ($4.99/mo): All 4 pets, all biomes, unlimited dialogue
• School ($49.99/yr): Teacher dashboard, 30 child profiles

COMMUNITY:
• Matrix chat: matrix.yazhi.org
• Mastodon: social.yazhi.org
• Source: github.com/yazhi-lem

நம் குழந்தைகளுக்கு தமிழ்! (Tamil for our children!)
```

### Screenshots (8 required, 16:9 or 9:16)

| # | Description |
|---|-------------|
| 1 | MainMenu (Tamil title, "யாழ்") |
| 2 | PetSelection (4 pets in Tamil) |
| 3 | BiomeArena (AR viewport + pet) |
| 4 | Dialogue example (Tamil conversation) |
| 5 | Stats HUD (health/energy/hunger/happiness) |
| 6 | Parent dashboard |
| 7 | Subscription tiers |
| 8 | COPPA consent screen (transparency) |

### Feature Graphic (1024x500)
- Hero: Yazhi logo + YAZH-UNITY tagline
- 4 pet icons in Tamil
- "On-Device Tamil AI" callout

### Icon (512x512)
- Stylized Yazh (Tamil harp) silhouette in Temple Gold
- Night Indigo background
- Brutalist (no rounded corners)

### Age Rating
- **ESRB:** Everyone (mild fantasy violence — pet survival themes)
- **PEGI:** 7+
- **Google:** Everyone 10+ (parental guidance recommended)

### Content Rating Questionnaire
- Violence: Cartoon/fantasy (pet dangers like weather, hunger)
- Language: None
- Sexual content: None
- Drugs: None
- User-generated content: No (single-player)
- Location sharing: No
- Personal data: Minimal (parental email for consent only)

### Privacy Policy
Hosted at: yazhi.dev/privacy
- What we collect: Parent email (for consent record), device locale
- What we don't collect: Usage analytics, child data, location, contacts
- Data location: On-device only
- Third parties: None
- Children's privacy: COPPA + DPDP + GDPR-K compliant

### In-App Subscriptions (Play Billing) — TAMILNADU PRICING (INR)

> **Strategic Pivot v2:** Pricing in INR for Tamilnadu market (primary). USD shown for diaspora (Phase 2).

```
SKU: yazhi_family_monthly_inr
Title: Yazhi Family (Monthly) — Tamilnadu
Price: ₹199 INR / month (~$2.40)
Description: All 4 pets, all biomes, unlimited dialogue for 1 family

SKU: yazhi_family_annual_inr
Title: Yazhi Family (Annual) — Tamilnadu
Price: ₹1,999 INR / year (~$24, save ~16%)
Description: Same as monthly, billed yearly

SKU: yazhi_school_annual_inr
Title: Yazhi School (Annual) — Tamilnadu
Price: ₹4,999 INR / year per school (~$60)
Description: Teacher dashboard, 30 child profiles, offline APK deployment
```

**USD pricing (for diaspora, Phase 2):**
```
SKU: yazhi_family_monthly_usd
Price: $4.99 USD / month
SKU: yazhi_family_annual_usd
Price: $49.99 USD / year
SKU: yazhi_school_annual_usd
Price: $199.99 USD / year per school
```

### Free Tier
- Kuruvi pet only
- 1 biome (Oorru)
- 100 dialogue phrases
- 7-day survival loop
- No time limit (but content capped)

### Family Plan Benefits
- All 4 pets (Kuruvi, Maan, Yanai, Pulliruvi)
- All 4 biomes
- Unlimited dialogue
- Up to 4 child profiles
- Parent dashboard
- New content monthly

### School Plan Benefits
- All Family features
- 30 child profiles
- Teacher dashboard with progress tracking
- Admin controls
- Offline APK deployment (for low-bandwidth schools)
- Tamil curriculum integration

### Trial Period
- 7-day free trial (no credit card required)
- Cancel anytime
- No automatic charge after trial

## Long Onboarding Flow

### Why "Long"?
Yazhi's sovereignty principle demands informed consent. The onboarding is intentionally thorough:

**Total time:** 3-5 minutes (target <5 min)

### Onboarding Screens (8 screens)

```
[1] Welcome
    - "Welcome to YAZH-UNITY"
    - Brief intro (3 sentences)
    - [Continue] button

[2] Language Selection
    - Tamil primary, English secondary
    - "Choose your preferred language"
    - [Tamil] [English] buttons

[3] Sovereignty Disclosure
    - "How your data is handled"
    - Bullet points:
      * All AI runs on your device
      * We never see your child's conversations
      * No analytics, no tracking
      * Code is open source
    - [I understand] button

[4] COPPA Parental Consent (gating)
    - Parent email input
    - Parent attestation checkbox
    - [Accept] [Decline] buttons
    - If declined: app exits

[5] Child Profile
    - Nickname (optional)
    - Age (6-14)
    - Avatar selection (Tamil pet icons)
    - [Continue]

[6] Pet Selection
    - 4 pets with Tamil names + descriptions
    - Tap to select
    - [Continue]

[7] Subscription Tier
    - Free / Family / School options
    - "Start 7-day free trial" CTA on Family
    - Or "Continue with Free" for limited version
    - Skip option for now, unlock later in settings

[8] Ready
    - "You're ready to begin!"
    - [Enter YAZH-UNITY]
```

### Skip Behavior
- Can skip steps 5-7 (use defaults)
- Cannot skip steps 1-4 (sovereignty disclosure + COPPA are mandatory)
- Settings accessible post-onboarding to change tier

## Build & Deploy

### APK Build
```bash
cd apps/yazh-unity
./build-yazh.sh android release=1

# Output: build/yazh-unity-release.apk
```

### Upload to Play Console
1. Open Play Console → Internal Testing → Create Release
2. Upload APK + AAB (App Bundle)
3. Add release notes
4. Promote to Closed Testing (beta users)
5. After QA → Production

### Store Listing Review
- Google review: 1-7 days for first submission
- Common rejections: Privacy policy issues, age rating, COPPA compliance
- Mitigation: We've prepared full disclosure docs

## Required Permissions (Android)

```
INTERNET (optional, for analytics — but we DON'T use)
CAMERA (for AR)
READ_EXTERNAL_STORAGE (optional, for custom pets)
WRITE_EXTERNAL_STORAGE (optional)
WAKE_LOCK (for background inference)
```

**NOTE:** No location, contacts, microphone, SMS, or other sensitive permissions.

## Risk Register

| Risk | Severity | Mitigation |
|------|----------|-----------|
| Play Store rejection (privacy) | HIGH | Full privacy policy, COPPA flow, minimal permissions |
| IAP integration bugs | MEDIUM | Test with internal testing track first |
| Subscription confusion | MEDIUM | Clear UI, no hidden charges, easy cancel |
| Long onboarding drop-off | MEDIUM | A/B test, reduce to 4 screens if needed |
| Negative reviews on Tamil-first | LOW | Diaspora support, Tamil-language responses |
| Server-side validation required | HIGH | Use Play Billing's server-side verification (Play Developer API) |

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| APK size | < 50 MB | Build output |
| Install to first play | < 30s | User testing |
| Onboarding completion | > 70% | Play Console analytics (opt-in) |
| Trial start rate | > 30% | Play Billing |
| Trial → paid conversion | > 5% | Play Billing |
| D7 retention | > 40% | Play Console |
| D30 retention | > 20% | Play Console |
| MRR by week 8 | $5K | Play Console revenue |

## Files Referenced

- `apps/yazh-unity/Assets/Plugins/Android/COPPA_CONSENT_*` — COPPA flow (Cycle 9)
- `apps/yazh-unity/Assets/Scripts/UI/UIStyles.cs` — Tamil-first styling (Cycle 7)
- `models/yazh/MODEL_CARD.md` — Model card for citation
- `product/yazh/GO_TO_MARKET.md` — Pricing strategy (Cycle 5)
- `product/yazh/PRESS_KIT.md` — Press materials (Cycle 9)
- `memory/2026-06-18-STRATEGIC-PIVOT.md` — This cycle's pivot

---
*Created: 2026-06-18 | Cycle 10 | Strategic pivot to Play Store + IAP*
