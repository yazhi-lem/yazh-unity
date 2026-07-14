# Yazh XR Pet App — Unity Implementation

**Status:** Prototype / Foundation  
**Engine:** Unity 6 LTS (C#)  
**Platforms:** iOS 12+ / Android 7+ (ARKit/ARCore)  
**AI Model:** Yazh 30K (Tamil, on-device ONNX via Barracuda)

---

## Game Overview

**Pivot (Jul 2026):** Endless runner (Subway Surfers-style). The child's Yazh pet runs through the five **tinai** landscapes of Sangam literature, cycling endlessly: குறிஞ்சி Kurinji (mountains) → முல்லை Mullai (forest) → மருதம் Marutham (farmland) → நெய்தல் Neithal (seashore) → பாலை Palai (wasteland). Child still chats with the pet in Tamil between runs; pet thinks natively in Tamil (no translation).

**Runner pets (each with a distinct ability):** Kuruvi (double jump), Maan (high jump), Yanai (smashes obstacles), Pulliruvi (collectible magnet)
**Terrain:** Tinai system endless — see [docs/gameplay/ENDLESS_RUNNER_DESIGN.md](docs/gameplay/ENDLESS_RUNNER_DESIGN.md)
**Care loop:** Runs feed the pet's mood and bond (YazhLife); a tired pet runs slower

---

## Project Structure

```
yazh-unity/
├── README.md                  # This file
├── package.json
├── ProjectSettings.json
├── .gitignore
│
├── Assets/                    # Unity assets
│   ├── Scenes/                # MainMenu, PetSelection, BiomeArena, SettingsMenu
│   ├── Scripts/
│   │   ├── AI/                # YazhInferenceEngine (ONNX Tamil inference)
│   │   ├── AR/                # ARSessionManager (ARKit/ARCore)
│   │   ├── Audio/             # AudioSyncManager (TTS + SFX)
│   │   ├── Core/              # GameManager, DialogueSystem, PetManager
│   │   ├── Gameplay/          # SurvivalSystem, YazhLife
│   │   ├── Runner/            # Tinai endless runner (core loop)
│   │   ├── UI/                # UIStyles (Material Design)
│   │   ├── Editor/            # BuildScript (CI/CD automation)
│   │   └── WebGL/             # Web fallback build
│   ├── Resources/             # Icons, textures, biome assets
│   └── StreamingAssets/
│       └── MLModels/          # ONNX models (yazh-30k.onnx, int4, int8)
│
├── Packages/                  # Unity package dependencies
├── ProjectSettings/           # Unity project settings
│
└── docs/                      # All documentation
    ├── architecture/          # ARCHITECTURE.md, ARCHITECTURE_AUDIT.yz, BUILD_ENVIRONMENT.md
    ├── security/              # SECURITY_AUDIT.md, COPPA_COMPLIANCE.md, ONNX_HASH_VERIFICATION.md
    ├── deployment/            # DEPLOY.md, PLAY_STORE_PLAN.md, build-yazh.sh
    ├── gameplay/              # ART_DIRECTION.md, ONBOARDING_DESIGN.md
    ├── SETUP.md               # Development setup guide
    ├── START_HERE.md          # Quick start for new developers
    ├── TEAM.md                # Team roles (8 production agents)
    ├── INDEX.md               # Documentation index
    ├── CHECKLIST.md           # Feature checklist
    ├── COMPLETION_SUMMARY.md  # Implementation status
    ├── IMPLEMENTATION_COMPLETE.md
    ├── LATENCY_BENCHMARK.md   # Performance benchmarks
    ├── MODEL_STATUS.md        # ONNX model deployment status
    ├── DELIVERY.md            # Delivery checklist
    └── README.root.md         # Original README (archived)
```

---

## Quick Start

See [docs/SETUP.md](docs/SETUP.md) for full setup guide.

1. Clone: `git clone https://github.com/yazhi-lem/yazh-unity.git`
2. Open in Unity 6 LTS
3. Open `Assets/Scenes/MainMenu.unity`
4. Press Play

---

## Documentation

| Document | Location | Description |
|----------|----------|-------------|
| Architecture | [docs/architecture/](docs/architecture/) | System design, audit, build env |
| Security | [docs/security/](docs/security/) | COPPA, ONNX verification, audit |
| Deployment | [docs/deployment/](docs/deployment/) | Play Store, build scripts |
| Gameplay | [docs/gameplay/](docs/gameplay/) | Art direction, onboarding |
| Setup | [docs/SETUP.md](docs/SETUP.md) | Dev environment setup |
| Team | [docs/TEAM.md](docs/TEAM.md) | 8 production roles |

---

## Codebase Stats

- **2,485 lines** of C# across 8+ systems
- **4 scenes** (MainMenu, PetSelection, BiomeArena, SettingsMenu)
- **4 pets** with distinct AI personalities
- **ONNX inference** (Yazh 30K, on-device, <50ms latency)
- **COPPA compliant** (children's privacy)

---

## Key Systems

| System | File | Lines | Description |
|--------|------|-------|-------------|
| Game Manager | `Assets/Scripts/Core/GameManager.cs` | — | Game loop, scene transitions |
| Pet Manager | `Assets/Scripts/Core/PetManager.cs` | — | Pet state machine (4 pets) |
| Dialogue | `Assets/Scripts/Core/DialogueSystem.cs` | — | Tamil chat UI + history |
| AI Inference | `Assets/Scripts/AI/YazhInferenceEngine.cs` | — | ONNX Tamil inference (Barracuda) |
| AR Session | `Assets/Scripts/AR/ARSessionManager.cs` | — | ARKit/ARCore lifecycle |
| Audio | `Assets/Scripts/Audio/AudioSyncManager.cs` | — | Piper TTS + sound effects |
| Tinai Terrain | `Assets/Scripts/Runner/EndlessTerrainGenerator.cs` | — | Endless tinai track (5 Sangam landscapes) |
| Runner | `Assets/Scripts/Runner/YazhRunnerController.cs` | — | Pet runner: lanes, jump, slide, abilities |
| Run Manager | `Assets/Scripts/Runner/RunnerGameManager.cs` | — | Speed ramp, score, YazhLife rewards |
| Survival | `Assets/Scripts/Gameplay/SurvivalSystem.cs` | 152 | Resource management (legacy loop) |
| Pet Life | `Assets/Scripts/Gameplay/YazhLife.cs` | 231 | Pet lifecycle + survival state |
| UI Styles | `Assets/Scripts/UI/UIStyles.cs` | 107 | Material Design system |
| Build CI/CD | `Assets/Scripts/Editor/BuildScript.cs` | 192 | Automated Debug/Release builds |

---

## CI/CD

GitHub Actions builds a debug-signed APK on every push to `main`/`develop`
(artifact: `Yazh-Android-APK`). Builds are **model-less by default** — the app
falls back to scripted Tamil dialogue without the ONNX weights. One-time
setup (Unity license secrets): [docs/deployment/CI_SETUP.md](docs/deployment/CI_SETUP.md).

## Blockers

- **Play Store:** Awaiting signing key (founder gate) — CI ships unsigned test APKs meanwhile
- **App Store:** Awaiting signing certificate (founder gate)

---

## License

MIT — See LICENSE file

---

**Last Updated:** 2026-07-05  
**Status:** Production-ready code, awaiting deployment credentials
