# Yazh XR - Setup & Build Guide

## Environment Setup

### Prerequisites

- **Unity 2022.3 LTS** (not 2023+, for stability)
- **Barracuda** package (via Package Manager)
- **AR Foundation** package (built-in option)
- **TextMesh Pro** (for UI text rendering)
- **iOS:** Xcode 14+ (if targeting iOS)
- **Android:** Android Studio + NDK (if targeting Android)

### Step 1: Open Project in Unity

```bash
# Clone/extract project
cd yazh-unity

# Open with Unity Hub
# Select Unity 2022.3 LTS as engine version
```

### Step 2: Install Required Packages

In Unity Editor:
```
Window > TextMeshPro > Import TMP Essential Resources
```

Then via Package Manager (Window > Package Manager):
```
Add package from git URL:
  - com.unity.barracuda@3.0.0
  - com.unity.xr.arfoundation@5.1.0
  - com.unity.xr.arcore@5.1.0
  - com.unity.xr.arkit@5.1.0
```

Or use Packages/manifest.json:
```json
{
  "dependencies": {
    "com.unity.barracuda": "3.0.0",
    "com.unity.xr.arfoundation": "5.1.0",
    "com.unity.xr.arcore": "5.1.0",
    "com.unity.xr.arkit": "5.1.0"
  }
}
```

### Step 3: Import Yazh 30K Model

1. Place ONNX model at: `Assets/Models/AI/yazh_30k.onnx`
2. In Project window, select the .onnx file
3. Inspector settings:
   - Type: ONNX
   - Execution: GPU (ComputePrecompiled)
4. Apply

### Step 4: Configure AR Foundation

#### For iOS (ARKit)
```
Edit > Project Settings > XR Plugin Management
  - iOS 
    ✓ ARKit enabled
```

Player Settings (iOS):
```
- Minimum iOS Version: 12.0
- Architecture: ARM64
- Camera: Required (Info.plist)
```

#### For Android (ARCore)
```
Edit > Project Settings > XR Plugin Management
  - Android
    ✓ ARCore enabled
```

Player Settings (Android):
```
- Minimum API Level: 24
- Target API Level: 33
- ARCore: Required
```

### Step 5: Load Sample Scene

Open `Assets/Scenes/MainAR.unity` to verify setup.

Expected:
- AR camera feed visible
- Grid overlay showing plane detection
- Hierarchy shows GameManager, ARSession, etc.

Press Play (Editor) to test. You should see:
- AR plane detection working
- Sample dialogue in console logs

## Development Workflow

### Adding a New Pet

1. **3D Model:**
   - Create/import FBX (rigged + animations)
   - Place in `Assets/Models/Pets/`

2. **Animator Controller:**
   - Create Animator asset (`Assets/Animators/`)
   - Add states: idle, walk, run, talk, emotion_*, interact_*
   - Connect with transitions (parameters: trigger, speed)

3. **Prefab:**
   - Drag model into scene
   - Add AudioSource component
   - Drag prefab to `Assets/Prefabs/Pets/`

4. **Config:**
   - Add entry to `Assets/Resources/Config/PetPersonalities.json`
   - Include: name, voice pitch, animation speed, personality

5. **Code:**
   - Update `PetManager.LoadPetPrefabs()` to include new pet
   - Test: `GameManager.Instance.OnPetSelected("new_pet_name")`

### Adding Dialogue Context

Expand dialogue in `Assets/Resources/Config/` JSON files:
- Pet strategy quotes (used in dialogue context)
- Specific domain vocabulary (survival, biome, emotions)

Example context enrichment in `DialogueSystem.cs`:
```csharp
string context = BuildContextWindow();
context += $"\nPet personality: {petPersonality}\n";
context += $"Current weather: {currentWeather}\n";
context += $"Resources: Water {water}%, Food {food}%\n";
```

### Running Inference Tests

In Editor Play mode:
```csharp
// Copy to temporary script for testing
private async Task TestInference() {
    var engine = YazhInferenceEngine.Instance;
    var tokens = await engine.InferenceAsync("நன்றி", "");
    var response = engine.DecodeTokens(tokens);
    Debug.Log("Response: " + response);
}
```

## Building for Device

### iOS Build

```bash
# In Unity Editor:
# File > Build Settings > iOS
# Scenes: [MainAR.unity, ...] (check order matters)
# Build

# In resulting Xcode project:
xcodebuild -scheme Unity-iPhone -configuration Release archive
```

Debug on real device:
```
Xcode > Product > Run (requires dev certificate + provisioning profile)
Then deploy to iPhone 12+ with ARKit support
```

### Android Build

```bash
# In Unity Editor:
# File > Build Settings > Android
# Build APK

# Or via Gradle:
cd [ProjectPath]/Builds/Android
./gradlew assembleRelease
```

Debug on real device:
```
Android: adb install -r yazh.apk
Or: Play Store internal testing
```

## Performance Optimization

### Profiler Checks

Window > Analysis > Profiler

Priority metrics:
- **CPU** (Game + UI threads)
- **Memory** (Total RAM, GC allocation)
- **GPU** (Render pipeline time)

Target: Game thread <16ms (60 FPS)

### Common Bottlenecks

| Issue | Fix |
|-------|-----|
| Inference slow | Check GPU vs CPU execution (ComputePrecompiled vs others) |
| Memory leak | Profile GC.Alloc, check UnityEngine.Object disposal |
| AR jank | Reduce plane detection frequency in ARPlaneManager |
| Animation lag | Lower pet animation count if >4 overlapping |
| UI stutter | Use UI batching (Canvas groups) |

### Optimization Checklist

- [ ] Model quantized to 150MB
- [ ] Inference runs <150ms
- [ ] Memory <200MB sustained
- [ ] FPS 60 on target device (Pixel 6, iPhone 12)
- [ ] Battery drain <10%/hour
- [ ] No thermal throttling after 4 hours

## Troubleshooting

### Model Not Loading
```
Error: "Model not found: Assets/Models/AI/yazh_30k.onnx"
Fix: Verify file exists + check Resources path
```

### AR Not Working
```
iOS: Camera permission denied
Fix: Check Info.plist has NSCameraUsageDescription

Android: ARCore not installed
Fix: Google Play Services for AR required
```

### Inference Timeout
```
Error: "Inference task timeout"
Fix: Check token window size (max 512), reduce batch size
```

### Audio Not Syncing
```
Issue: Lip-sync out of sync
Fix: Ensure audio clip duration matches animation length
Check: AudioSyncManager.PlayDialogueWithSync()
```

## Documentation References

- **Barracuda:** https://docs.unity3d.com/Packages/com.unity.barracuda@3.0/
- **AR Foundation:** https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest/
- **TextMeshPro:** https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest/

## Next Steps

1. **Import 3D assets:** Transfer FBX models from design folder
2. **Record voices:** Capture Tamil voice actor audio
3. **Test dialogue:** Load Yazh 30K model, run sample inferences
4. **Profile on target:** Test on Pixel 6 + iPhone 12+
5. **Beta deployment:** Submit to TestFlight + Play Store internal testing
