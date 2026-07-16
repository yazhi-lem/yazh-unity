# TTS Integration Readiness

**Checked:** 2026-07-14 (Rotation 26)  
**Scope:** Offline inspection of the Unity project; no provider credentials or network access required.

## Current implementation

- `Assets/Scripts/Core/DialogueSystem.cs` generates a `DialogueResponse`, but `audioClip` is explicitly `null` (`// TODO: actual TTS synthesis`).
- `Assets/Scripts/Audio/AudioSyncManager.cs` can play an already-created Unity `AudioClip` and trigger the pet's `talk` animation. It does not synthesize audio or select a TTS provider.
- No Amudh client, endpoint, API key configuration, local model, or generated Tamil audio assets are present in this repository.
- The README's “Piper TTS” description is therefore a capability reference, not an active integration.

## Readiness verdict

**Not integrated; provider selection is still blocked.** The Unity-side playback seam is present, but an Amudh adapter (or another offline/local provider) must produce an `AudioClip` before dialogue can be voiced. Do not mark TTS complete based on the existing `AudioSyncManager` alone.

## Offline acceptance gate for the next integration change

1. Add a provider-neutral adapter that accepts Tamil text and returns an `AudioClip` (or an explicit failure).
2. Keep credentials and provider URLs outside the repository; offline/local operation must remain possible.
3. Connect the adapter from `DialogueSystem.InferResponse` and only invoke `AudioSyncManager.PlayDialogueWithSync` when a non-null clip is returned.
4. Add a deterministic failure-path test for missing provider/model configuration; text dialogue must still be delivered.
5. Validate on a device or Unity Editor with a real Tamil clip: playback starts, `talk` is triggered, and `StopDialogue` returns the pet to `idle`.

## Current blockers

- Unity Editor is not installed on this host, so C# compilation, scene wiring, and audio playback cannot be exercised here.
- No Amudh integration artifact or provider contract is present locally, so implementing an adapter would require guessing the API and is intentionally out of scope for this offline cycle.
- Device credentials/signing are unrelated to this readiness check and remain deployment blockers.

## Offline checks completed

- Confirmed the only runtime TTS seam is `AudioSyncManager.PlayDialogueWithSync(AudioClip)`.
- Confirmed `DialogueSystem` currently sets `audioClip = null`.
- Confirmed no `amudh` reference exists in tracked project source or documentation.
- Confirmed project JSON files are parseable except `Packages/manifest.json`, which contains a UTF-8 BOM that Python's default `json.tool` rejects; Unity may still accept it, but it should be normalized in a separate build-hygiene change.
