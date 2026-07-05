# Yazh XR Project Architecture

## Overview

Yazh is a Tamil-first XR app built with Unity 2022.3 LTS + C#. It integrates:
- **AR Foundation** (iOS ARKit + Android ARCore)
- **Barracuda ONNX Runtime** (Yazh 30K model inference)
- **Custom dialogue & survival systems**

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    YAZH XR APP (Unity)                      │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  GameManager (Central State Machine)                        │
│  ├─ PetManager (Pet lifecycle, animations)                 │
│  ├─ DialogueSystem (Conversation flow, context)            │
│  ├─ YazhInferenceEngine (ONNX model, tokenization)          │
│  ├─ SurvivalSystem (Resources, weather, challenges)        │
│  ├─ ARSessionManager (Plane detection, placement)          │
│  ├─ AudioSyncManager (TTS audio ↔ animation sync)          │
│  └─ UI Controllers (Onboarding, chat, achievements)        │
│                                                              │
│  Asset Imports:                                             │
│  ├─ 3D Models (FBX rigged pets, biome props)                │
│  ├─ Audio (Voice clips, ambience, challenge theme)         │
│  ├─ Animations (Mecanim state machines)                    │
│  └─ Textures (2K PBR materials)                             │
│                                                              │
└─────────────────────────────────────────────────────────────┘
         ↓
    AR Foundation Layer (Native)
    ├─ iOS: ARKit (plane detection, light estimation)
    └─ Android: ARCore (plane detection, light estimation)
         ↓
    Device Camera + Sensors
```

## Core Scripts

### GameManager.cs
- **Role:** Central state machine, scene orchestration
- **States:** Onboarding → MainGame → Challenge → Paused → Settings → Achievements
- **Manages:** Pet spawning, dialogue initialization, challenge progression
- **Pattern:** Singleton (DontDestroyOnLoad)

### PetManager.cs
- **Role:** Pet instance lifecycle, animation state
- **Features:** Multi-pet support, personality loading, animation triggering
- **Data:** 4 pet types (Kuruvi, Maan, Yanai, Pulliruvi) + personalities (JSON config)
- **Example:** `petManager.SpawnPet("kuruvi")` → loads prefab, sets animations, applies personality

### DialogueSystem.cs
- **Role:** Conversation flow, context retention, model calls
- **Context Window:** Last 8 exchanges (≈512 tokens, sufficient for dialogue coherence)
- **Flow:** Input → YazhInference → Response → TTS → Animation sync
- **Event:** `OnResponseGenerated` callback triggers dialogue UI + animation

### YazhInferenceEngine.cs
- **Role:** ONNX model inference via Barracuda
- **Model:** Yazh 30K (150MB quantized ONNX)
- **Latency Target:** <150ms per token generation
- **Process:** Input text → Tokenization (Tamil) → Inference → Token ID list → Decoding → Tamil text
- **Fallback:** Unknown tokens → graceful paraphrase (no English)

### SurvivalSystem.cs
- **Role:** Resource tracking, weather simulation, challenge progression
- **Resources:** Water (5 capacity), Food (10 capacity), Shelter (100 durability)
- **Daily Loss:** Water -1, Food -2, Shelter -2 (scales with difficulty)
- **Weather:** Random cycles (sunny, cloudy, rainy, stormy) with resource impact
- **Challenge:** 7-day loop (3 days learning, 3 days challenge, 1 day boss)

### ARSessionManager.cs
- **Role:** AR Foundation lifecycle, plane detection, pet placement
- **Platforms:** iOS ARKit + Android ARCore (unified via AR Foundation)
- **Raycast:** Screen tap → world position on detected plane → pet placement

### AudioSyncManager.cs
- **Role:** Lip-sync (audio duration → animation length)
- **Pattern:** Calculate audio duration → Scale animation speed → Play simultaneously
- **Event-Driven:** `PlayDialogueWithSync(audioClip)` → animation + audio locked

## Data Flow Examples

### Example 1: Child Speaks → Pet Responds

```
Child voice input (STT by OS)
         ↓
DialogueSystem.ProcessInput("தண்ணீர் தேட வேண்டுமா?")
         ↓
YazhInferenceEngine.InferenceAsync(input, context)
  - Build prompt (system + context + input)
  - Tokenize Tamil → token IDs
  - Barracuda inference (<150ms)
  - Decode tokens → Tamil response
         ↓
DialogueResponse event fired
         ↓
UI updates: Show bubble + PetManager.PlayAnimation("talk")
         ↓
TTS provider synthesizes Tamil audio
         ↓
AudioSyncManager.PlayDialogueWithSync(audioClip)
  - Audio duration → animate pet mouth for that duration
  - Sync: audio plays + animation lip-moves together
         ↓
Pet responds visually + audibly
```

### Example 2: Challenge Progression

```
Day 1-3: Learning Phase
  - SurvivalSystem: low difficulty, frequent hints
  - Pet: teaches survival basics (தண்ணீர் அவசியம்)
  - Resources: abundant, decay slow
         ↓
Day 4-6: Challenge Phase
  - SurvivalSystem: medium difficulty, fewer hints
  - Pet: strategic advice (resource optimization)
  - Resources: scarcer, decay accelerates
         ↓
Day 7: Boss Challenge
  - SurvivalSystem: high difficulty, rare hints
  - Pet: minimal guidance, child must apply learned skills
  - Resources: critical scarcity
  - Weather: often adverse (rainy/stormy)
         ↓
Challenge Complete
  - Unlock new biome OR pet ability
  - Day counter resets
```

## Configuration Architecture

### SurvivalConfig.json
Defines resource dynamics, weather cycles, difficulty tiers.
Loaded at startup → `SurvivalSystem` applies at runtime.

### PetPersonalities.json
4 pets, each with:
- Tamil + English name
- Voice pitch (affects TTS)
- Animation speed (affects Mecanim playback)
- Personality strategy quotes (used in dialogue context)

### Achievements.json
50 badges with unlock conditions, reward types, progression tiers.

## Performance Profiling Targets

| Metric | Target | Notes |
|--------|--------|-------|
| FPS | 60 | Smooth AR without jank |
| Model Inference | <150ms | Per-response latency |
| Memory (RAM) | <200MB | Including model + scene |
| Model Size (Disk) | <150MB | Quantized ONNX on device |
| Battery | <10%/hour | Sustained 4+ hour play |
| Thermal | No throttle | After 4+ hours continuous |

## Testing Strategy

### Play Mode (Editor)
- Onboarding flow (pet selection)
- Dialogue I/O (mock inference)
- Survival mechanics (day simulation)
- Achievement unlock logic

### Device Testing
- ARCore/ARKit plane detection
- Model inference latency (profile)
- Voice input accuracy
- Battery/thermal behavior

## Build & Deployment

### iOS
```
File > Build Settings > iOS
Player Settings:
  - Min iOS: 12.0
  - ARKit: Enabled
  - Camera: Required
Build → Xcode → Archive → AppStore
```

### Android
```
File > Build Settings > Android
Player Settings:
  - Min API: 24
  - Target API: 33
  - ARCore: Enabled
  - Camera: Required
Build → Gradle → APK/AAB → Play Console
```

## Future Extensions

- **Amudh Integration:** Scene narration via vision model
- **Adhan Fallback:** Knowledge augmentation for complex queries
- **Multiplayer:** Shared anchors via Cloud Anchors
- **Offline Mode:** Cached responses, no inference
- **Meta Quest Support:** VR headset deployment
