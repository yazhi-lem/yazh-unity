# Yazh XR Pet - Unity Project

Intelligent Tamil-first XR app for children (ages 6-14).

## Features

- **4 Pet Companions:** Kuruvi (sparrow), Maan (deer), Yanai (elephant), Pulliruvi (dove)
- **4 Sangam-inspired Biomes:** Forest, Village, Mountain, Coast
- **Tamil-First AI:** Yazh 30K-token model, native Tamil only
- **On-Device Inference:** Barracuda ONNX Runtime (no cloud)
- **Survival Mechanics:** 7-day challenge loops, resource gathering
- **AR-First:** iOS ARKit + Android ARCore support

## Quick Start

### Prerequisites
- Unity 2022.3 LTS+
- iOS/Android development kit
- Barracuda package (via Package Manager)
- ONNX Runtime plugin

### Setup

```bash
# Clone this repo (or initialize new project)
cd yazh-unity

# Open in Unity Hub
# Select Unity 2022.3 LTS

# In Unity Editor:
# 1. Window > TextMeshPro > Import TMP Essential Resources
# 2. Window > XR Plugin Management > enable ARKit (iOS) + ARCore (Android)
# 3. Add Barracuda via Package Manager
# 4. Import Yazh 30K ONNX model to Assets/Models/
```

### Project Structure

```
yazh-unity/
├── Assets/
│   ├── Models/
│   │   ├── Pets/          # Pet 3D models (FBX rigged)
│   │   ├── Biomes/        # Environment assets
│   │   ├── AI/            # Yazh 30K ONNX model
│   │   └── Audio/         # Voice recordings, ambience
│   ├── Scenes/
│   │   ├── Onboarding.unity
│   │   ├── MainAR.unity   # Primary gameplay scene
│   │   ├── Biome_Forest.unity
│   │   └── Settings.unity
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── PetManager.cs
│   │   │   └── DialogueSystem.cs
│   │   ├── AR/
│   │   │   ├── ARSessionManager.cs
│   │   │   └── PetPlacementController.cs
│   │   ├── AI/
│   │   │   ├── YazhInferenceEngine.cs
│   │   │   └── DialogueContext.cs
│   │   ├── Gameplay/
│   │   │   ├── SurvivalSystem.cs
│   │   │   ├── ResourceManager.cs
│   │   │   └── ChallengeSystem.cs
│   │   ├── Audio/
│   │   │   ├── TTSManager.cs
│   │   │   └── AudioSyncManager.cs
│   │   └── UI/
│   │       ├── OnboardingUI.cs
│   │       ├── DialogueUI.cs
│   │       └── AchievementUI.cs
│   ├── Prefabs/
│   │   ├── Pets/
│   │   ├── UI/
│   │   └── Environment/
│   ├── Resources/
│   │   ├── Config/
│   │   │   ├── SurvivalConfig.json
│   │   │   ├── PetPersonalities.json
│   │   │   └── Achievements.json
│   │   └── Localization/
│   │       ├── Tamil/
│   │       └── English/
│   ├── Shaders/
│   ├── Materials/
│   └── Editor/
│       └── BuildPostProcessor.cs
├── Packages/
│   └── manifest.json
├── ProjectSettings/
├── README.md
├── ARCHITECTURE.md
├── SETUP.md
└── package.json
```

## Development Roadmap

**Week 1-2:** AR Foundation setup, onboarding UI, pet model import
**Week 3-4:** Dialogue system integration, Yazh 30K model loading
**Week 5-6:** First biome, survival mechanics, resource system
**Week 7-8:** Animation sync, TTS integration, achievement system
**Week 9-10:** Cross-platform optimization, performance profiling
**Week 11-12:** Beta testing, localization QA, app store prep

## Key Scripts (Documented Below)

### Core Systems
- `GameManager.cs` — Central game state
- `PetManager.cs` — Multiple pet lifecycle
- `DialogueSystem.cs` — Conversation flow + state retention
- `YazhInferenceEngine.cs` — ONNX model inference (<150ms latency)
- `SurvivalSystem.cs` — Resource tracking, weather, challenge logic

### AR Integration
- `ARSessionManager.cs` — Plane detection, anchoring
- `PetPlacementController.cs` — Gesture-based pet placement

### UI
- `OnboardingUI.cs` — Pet selection, difficulty, language choice
- `DialogueUI.cs` — Chat stack, voice input, animation sync
- `AchievementUI.cs` — Badge display, progression tracking

## Building for Deployment

### iOS
```bash
# In Unity: File > Build Settings
# Platform: iOS
# Scenes in Build: All (.unity files)
# Player Settings: Minimum iOS 12.0, ARKit enabled
# Build → Select folder → Xcode opens
# In Xcode: Product > Archive → Distribute (App Store)
```

### Android
```bash
# In Unity: File > Build Settings
# Platform: Android
# API Level: 24+ (ARCore requirement)
# Player Settings: Min SDK 21, Target SDK 33, ARCore enabled
# Build → Select folder → Gradle builds .apk/.aab
# Upload to Google Play Console
```

## Configuration Files

### SurvivalConfig.json
```json
{
  "resources": {
    "water": { "maxCapacity": 5, "dailyLoss": 1 },
    "food": { "maxCapacity": 10, "dailyLoss": 2 },
    "shelter": { "maxDurability": 100, "decayPerDay": 2 }
  },
  "difficulty": {
    "easy": { "resourceScarcity": 0.5, "challengeFrequency": 0.3 },
    "medium": { "resourceScarcity": 1.0, "challengeFrequency": 0.6 },
    "hard": { "resourceScarcity": 1.5, "challengeFrequency": 1.0 }
  }
}
```

### PetPersonalities.json
```json
{
  "kuruvi": {
    "name": "குருவி",
    "english": "Sparrow",
    "personality": "energetic",
    "voicePitch": 1.3,
    "animationSpeed": 1.1
  },
  "maan": {
    "name": "மான்",
    "english": "Deer",
    "personality": "graceful",
    "voicePitch": 0.8,
    "animationSpeed": 0.9
  },
  "yanai": {
    "name": "யானை",
    "english": "Elephant",
    "personality": "wise",
    "voicePitch": 0.6,
    "animationSpeed": 0.8
  },
  "pulliruvi": {
    "name": "புள்ளிரூவி",
    "english": "Dove",
    "personality": "artistic",
    "voicePitch": 1.2,
    "animationSpeed": 1.0
  }
}
```

## API Reference

### Dialogue System
```csharp
// Queue dialogue input (voice or text)
DialogueSystem.Instance.ProcessInput("தண்ணீர் தேட வேண்டுமா?");

// Listen for response
DialogueSystem.Instance.OnResponseGenerated += HandleResponse;

private void HandleResponse(DialogueResponse response) {
    // response.tamiText
    // response.animationTrigger
    // response.audioClip
}
```

### Yazh Inference
```csharp
// Load model
await YazhInferenceEngine.Instance.InitializeAsync("yazh_30k.onnx");

// Run inference
var tokens = await YazhInferenceEngine.Instance.InferenceAsync(input);
var response = YazhInferenceEngine.Instance.DecodeTokens(tokens);
```

### Survival System
```csharp
// Track resources
SurvivalSystem.Instance.ConsumResource("water", 1);
SurvivalSystem.Instance.GatherResource("food", 2);

// Check challenge state
bool isChallengeDue = SurvivalSystem.Instance.CheckChallengeTrigger();
```

## Testing

### Unity Editor Play Mode
- Test onboarding flow
- Verify dialogue system (mock inference)
- Check survival mechanics

### Device Testing
- Deploy to Android/iOS device with ARCore/ARKit support
- Profile inference latency (target <150ms)
- Test input performance (voice STT, gesture recognition)

## Performance Targets

- **FPS:** 60 (smooth AR experience)
- **Model Inference:** <150ms per token
- **Memory:** <200MB RAM (including model)
- **Battery Drain:** <10% per hour (sustained play)
- **Thermal:** No throttling after 4+ hours continuous play

## Troubleshooting

### Model Not Loading
- Verify ONNX Runtime plugin installed: Window > Barracuda > Info
- Check model path: Assets/Models/AI/yazh_30k.onnx
- Ensure Barracuda compatible with Unity version

### AR Not Working
- iOS: Verify ARKit enabled (Player Settings > iOS > AR Foundation)
- Android: Verify ARCore installed on test device, API 24+
- Check camera permissions in platform settings

### Voice Input Missing
- Android: Verify Whisper plugin installed
- iOS: Verify STT permissions granted
- Test microphone permissions in device settings

## Contributing

Development follows Yazhi standards:
- Code style: C# Google style guide
- Commit messages: Conventional Commits
- PR reviews: At least one approval before merge
- Testing: Play mode + device testing required

## License

MIT License (aligned with Yazhi open-source commitment)

## Support

- Issues: GitHub Issues (if public repo) or internal tickets
- Documentation: See ARCHITECTURE.md, SETUP.md
- Questions: Reach out to Yazhi core team

---

**Status:** Alpha (0.1.0) — Ready for production team onboarding
**Last Updated:** 2026-06-12
**Maintainer:** Yazhi Founder
