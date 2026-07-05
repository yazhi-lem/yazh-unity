# YAZH-UNITY Development Checklist

## Foundation (Week 1–2)
- [x] Git repo initialized on Zorba
- [x] Project structure created (Assets, Scripts, Models, Audio)
- [x] Core GameManager.cs (game state, scene transitions, pet lifecycle)
- [x] YazhInferenceManager.cs (Barracuda ONNX, tokenizer, inference pipeline)
- [x] ARSetup.cs (AR Foundation config, camera, plane detection)
- [ ] **NEXT:** Load & test Yazh 30K ONNX model locally
- [ ] **NEXT:** Create main menu scene
- [ ] **NEXT:** Create pet selection scene

## Module: AI/ML Integration
- [ ] Load ONNX model file (streaming assets)
- [ ] Test inference latency (< 150ms target)
- [ ] Implement tokenizer (Tamil → tokens → Tamil)
- [ ] Add STT pipeline (Speech-to-Text, Tamil)
- [ ] Add TTS pipeline (Text-to-Speech, Tamil voice)
- [ ] Test pet response generation loop

## Module: AR + 3D
- [ ] ARKit/ARCore plane detection working
- [ ] Import pet models (Kuruvi, Maan placeholders)
- [ ] Animation system (idle, walk, eat, sleep)
- [ ] Render pet in AR viewport with real-time tracking
- [ ] Implement light estimation (match real world)

## Module: Gameplay
- [ ] Resource collection mechanic
- [ ] Pet stat tracking (health, energy, hunger, happiness)
- [ ] 7-day survival loop
- [ ] Day/night cycles
- [ ] Weather system (sunny, rainy, stormy)
- [ ] Achievement badges

## Module: UI/UX (Tamil-first)
- [ ] Dialogue HUD (Tamil text input)
- [ ] Pet status display (stats in Tamil)
- [ ] Main menu (Tamil + English fallback)
- [ ] Settings (language, volume, graphics)
- [ ] Achievement screen

## Module: Audio
- [ ] Ambient soundscapes (biome-specific)
- [ ] Pet voice recordings (Tamil, 150+ phrases)
- [ ] Sound effects (eat, collect, complete, error)
- [ ] Background music (challenge themes)

## Week 5–8: Expansion
- [ ] All 4 pets (Kuruvi, Maan, Yanai, Pulliruvi)
- [ ] All 4 biomes (Sangam Kaadu, Oorru, Kulaathanku, Karai Parai)
- [ ] Full dialogue system (chat with pet)
- [ ] Cross-platform optimization (iOS + Android)

## Week 9–12: Polish & Shipping
- [ ] Performance profiling (frame rate, memory, battery)
- [ ] Beta testing (20–50 kids + parents)
- [ ] Hotfixes + localization
- [ ] App Store submission (iOS)
- [ ] Play Store submission (Android)

---

## Build Command (when ready)
```
# iOS (requires Xcode)
Build Settings: iOS
Build

# Android (requires Android Studio/Gradle)
Build Settings: Android
Build
```

## Test AR Setup
```
1. Open project in Unity 6 LTS
2. Window > XR > AR Foundation > Create Sample Scene
3. Assets/Scripts/AR/ARSetup.cs attached to Camera
4. Press Play → AR session should initialize
5. Check Console for [AR] logs
```

## Deploy Yazh 30K Model
```
cp ~/Yazhi/models/yazh/yazh-30k-quantized.onnx Assets/StreamingAssets/MLModels/
cp ~/Yazhi/models/yazh/tokenizer.json Assets/StreamingAssets/MLModels/
```

---

**Status:** Prototype foundation complete  
**Repo:** /home/neutron/Yazhi/apps/yazh-unity  
**Next Milestone:** Load & test Yazh 30K model inference
