# CI Setup — Android APK on GitHub Actions

**Date:** 2026-07-14
**Workflow:** `.github/workflows/build.yml` (game-ci / unity-builder)

Every push to `main`/`develop` (or a manual *Run workflow*) produces a
debug-signed **`Yazh-Android-APK`** artifact you can install directly on a
device. Builds are **model-less by default** — the ONNX files are stripped
before building and the app falls back to scripted Tamil dialogue, so the
pipeline needs no model weights, no signing key, no founder gates.

---

## One-time setup (repo admin, ~10 minutes)

CI needs a Unity license. Free Personal licenses work.

1. **Get the activation file**
   Actions → **Unity License Activation (one-time)** → *Run workflow* →
   download the `Unity-Activation-File` artifact (an `.alf` file).

2. **Convert it to a license**
   Upload the `.alf` at <https://license.unity3d.com/manual> (log in with
   the Unity account CI should use) → download the `.ulf` file.

3. **Add repository secrets** (Settings → Secrets and variables → Actions):

   | Secret | Value |
   |--------|-------|
   | `UNITY_LICENSE` | Full text contents of the `.ulf` file |
   | `UNITY_EMAIL` | Unity account email |
   | `UNITY_PASSWORD` | Unity account password |
   | `UNITY_SERIAL` | *(Pro/Plus licenses only — optional)* |

That's it. The next push builds a real APK. If secrets are missing the
job fails fast with a pointer to this file instead of pretending to pass.

---

## How "works without model" is enforced

| Layer | Behaviour |
|-------|-----------|
| CI (`build.yml`) | Strips `Assets/StreamingAssets/MLModels/*.onnx` before building (≈18 MB slimmer APK). Re-run manually with **include_models = true** to bundle them. |
| Build gate (`BuildScript.VerifyAllModelHashes`) | Missing model → **warning**, build proceeds. Present-but-hash-mismatched model → **hard fail** (tampering is never shipped). `YAZH_REQUIRE_MODELS=1` makes missing models fatal for release builds. |
| Runtime (`YazhInferenceManager` / `YazhInferenceEngine`) | Model file absent → `IsModelReady()` stays false; no crash. |
| Dialogue (`DialogueSystem`, `BiomeArenaController`) | Falls back to scripted Tamil responses ("சரி! ஓடலாம் வா!" …). |
| Runner (`TinaiRunner` scene) | Never touches the model — fully playable. |

## Build entry point

CI calls `BuildScript.BuildAndroidCI` (headless):

- Output path from `-customBuildPath` (passed by game-ci)
- Forces a valid Android identity (`org.yazhi.yazh`, product `Yazh`) since
  the hand-authored ProjectSettings has none
- APK (not AAB), debug keystore — a testable artifact, not a store upload
- Exits non-zero on a failed build so CI reflects reality

## Not yet wired (needs founder input)

- **Release signing:** upload keystore → secrets `ANDROID_KEYSTORE_BASE64`,
  `ANDROID_KEYSTORE_PASS`, `ANDROID_KEYALIAS_NAME`, `ANDROID_KEYALIAS_PASS`,
  then switch `useCustomKeystore` on in `BuildAndroidCI`.
- **AAB for Play Store:** flip `androidExportType: androidAppBundle` once
  signing exists.
- **iOS:** manual `workflow_dispatch` with *build_ios = true* exports an
  Xcode project (unsigned; needs a macOS runner minute budget).
