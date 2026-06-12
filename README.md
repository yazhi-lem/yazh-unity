# Yazh XR Pet App — Unity Implementation

**Status:** Prototype / Foundation  
**Engine:** Unity 6 LTS (C#)  
**Platform:** iOS 12+ / Android 7+ (ARKit/ARCore)  
**AI Model:** Yazh 30K (Tamil, on-device ONNX via Barracuda)

---

## 🎮 Game Overview

Intelligent pet survival game in XR biomes. Child chats with pet in Tamil. Pet thinks natively in Tamil (no translation).

**Pets:** Kuruvi (bird), Maan (deer), Yanai (elephant), Pulliruvi (cat)  
**Biomes:** Sangam Kaadu (forest), Oorru (village), Kulaathanku (palace), Karai Parai (beach)  
**Mechanic:** 7-day survival challenge (resources, weather, pet AI)

---

## 📂 Project Structure

```
Assets/
├── Scenes/             # All gameplay scenes
│   ├── MainMenu.unity
│   ├── PetSelection.unity
│   ├── BiomeArena.unity
│   └── SettingsMenu.unity
├── Scripts/
│   ├── Core/           # Engine systems (GameManager, StateManager, etc)
│   ├── AR/             # AR Foundation integration
│   ├── AI/             # Yazh 30K inference pipeline
│   ├── Gameplay/       # Pet logic, survival mechanics, weather
│   ├── UI/             # Menus, HUD, dialogue UI
│   └── Audio/          # Tamil TTS, ambient audio
├── Models/             # 3D models (FBX, rigged)
├── Materials/          # PBR materials
├── Prefabs/            # Reusable game objects
└── StreamingAssets/
    ├── Models/         # Model assets (included in build)
    └── MLModels/       # ONNX models (Yazh 30K quantized)
```

---

## 🔧 Setup Instructions

### Prerequisites
- Unity 6 LTS
- AR Foundation + ARKit/ARCore
- Barracuda ONNX Runtime (NuGet package)
- C# 9.0+

### Quick Start

1. **Clone & open in Unity:**
   ```bash
   git clone /home/neutron/Yazhi/apps/yazh-unity
   cd yazh-unity
   # Open in Unity 6 LTS
   ```

2. **Install Barracuda:**
   ```
   Window > TextMesh Pro > Import TMP Essential Resources
   Window > Barracuda > Download from GitHub
   ```

3. **Load Yazh 30K model:**
   - Copy ONNX model to `Assets/StreamingAssets/MLModels/`
   - Scene auto-loads at startup

4. **Run AR scene:**
   - iOS: Product > Build Settings > iOS > Build & Run
   - Android: Product > Build Settings > Android > Build & Run

---

## 🎯 Development Priorities (12-Week Roadmap)

### Week 1–4: Foundation
- [ ] AR camera setup (plane detection, lighting estimation)
- [ ] Pet Kuruvi model + idle/walk/eat/sleep animations
- [ ] Sangam Kaadu biome (simplified)
- [ ] Basic dialogue UI (Tamil text + TTS)
- [ ] Yazh 30K ONNX load test (inference latency benchmark)

### Week 5–8: Expansion
- [ ] Pets 2–4 (Maan, Yanai, Pulliruvi)
- [ ] Biomes 2–4
- [ ] Survival mechanics (resource tracking, weather)
- [ ] Voice chat pipeline (STT Tamil → Yazh 30K → TTS Tamil)
- [ ] Achievement system

### Week 9–12: Polish & Ship
- [ ] Cross-platform optimization (frame rate smoothing)
- [ ] Beta testing (20–50 kids)
- [ ] Hotfixes + localization
- [ ] App Store + Play Store submission

---

## 🤖 AI Integration (Yazh 30K)

**Model:** ONNX format (quantized to INT8 or INT4)  
**Inference:** Barracuda Runtime (on-device, < 150ms latency)  
**Input:** Tamil text (child's chat message)  
**Output:** Tamil text (pet's response)

**Pipeline:**
```
User Input (STT) → Yazh 30K (Barracuda) → Pet Voice Response (TTS)
```

See `Assets/Scripts/AI/YazhInferenceManager.cs` for implementation.

---

## 📋 Build Checklist

- [ ] All scenes compile without errors
- [ ] AR Foundation initials on target device
- [ ] Yazh model loads within 2 seconds
- [ ] Inference latency < 200ms
- [ ] Frame rate stable 30fps+ (AR)
- [ ] Audio plays correctly (Tamil voice)

---

## 🔗 Related Docs

- **App Experience Spec:** `/home/neutron/Yazhi/product/yazh/YAZH_XR_APP_EXPERIENCE.yz`
- **Production Schedule:** `/home/neutron/Yazhi/product/yazh/YAZH_PRODUCTION_SCHEDULE.yz`
- **Engine Analysis:** `/home/neutron/Yazhi/product/yazh/ENGINE_SELECTION_ANALYSIS.yz`
- **Yazh Model Spec:** `/home/neutron/Yazhi/models/yazh/YAZH_MODEL.yz`

---

## 👥 Team

- **Lead:** (Assign lead developer)
- **3D/Art:** (Assign 3D artist)
- **Audio:** (Assign audio engineer)
- **ML/AI:** (Assign ML engineer for Barracuda integration)

---

**Last updated:** 2026-06-13  
**Status:** Prototype initialization complete
