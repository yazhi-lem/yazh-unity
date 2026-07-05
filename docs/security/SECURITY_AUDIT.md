# SECURITY AUDIT REPORT — Yazh-Unity (YAZH-UNITY)
**Auditor:** KARUPPU (Security Agent, Rotation 25)
**Date:** 2026-06-17
**Scope:** GameManager.cs, YazhInferenceManager.cs, ARSetup.cs, LatencyTest.cs, BuildScript.yml
**Project:** /home/neutron/Yazhi/apps/yazh-unity/
**Principle:** Sovereign, zero-cloud, all data on-device, no telemetry

---

## EXECUTIVE SUMMARY

Overall security posture: **MODERATE** — The codebase adheres to the zero-cloud principle with no network calls found, but has critical gaps in input validation, ONNX model integrity verification, data persistence encryption, and AR data handling.

**Score: 5.5/10** (up from baseline 0 — no prior security controls)

| Category | Rating | Status |
|----------|--------|--------|
| Input Validation | WEAK | Multiple unvalidated inputs |
| ONNX Model Integrity | WEAK | No signature/hash verification |
| Data Persistence | NONE | No save system exists (no encryption needed yet) |
| Network Calls | STRONG | Zero outbound connections found |
| AR Data Handling | MODERATE | Camera permission checked, but no data leakage guards |
| CI/CD Security | MODERATE | No secret management, no build signing |
| Logging/Profiling | WEAK | Verbose debug logging in production, potential data leak |

---

## FINDINGS

### CRITICAL (Fix immediately)

#### SEC-001: No ONNX Model Integrity Verification
**File:** YazhInferenceManager.cs (Lines 78-101), LatencyTest.cs (Lines 172-207)
**Severity:** CRITICAL
**Description:** ONNX models are loaded from StreamingAssets with zero integrity verification. No checksum, hash, or digital signature is validated. A tampered model file (supply-chain attack or on-device modification) would be loaded and executed without detection.
**Impact:** Model poisoning → malicious output to children, data exfiltration via crafted responses.
**Status:** ✅ FIXED (2026-06-18)
**Solution:** 
- Embedded SHA-256 hashes in BuildScript.cs (build-time whitelist)
- Embedded SHA-256 hashes in YazhInferenceManager.cs (runtime verification)
- VerifyModelHash() method validates hash before load
- VerifyAllModelHashes() gates CI/CD pipeline
- See: ONNX_HASH_VERIFICATION.md

#### SEC-002: No Input Validation on User Chat Input
**File:** YazhInferenceManager.cs (Lines 145-201)
**Severity:** CRITICAL
**Description:** The GenerateResponse() method accepts arbitrary string input with no length limits, character filtering, or sanitization. The tokenizer processes raw input directly. No maximum input length is enforced before tokenization.
**Impact:** Buffer overflow via extremely long input (tokens array unbounded), potential tokenizer crash, denial of service. Crafted input could trigger unexpected model behavior.
**Recommendation:** Enforce max input length (e.g., 512 chars). Validate input is within expected Tamil Unicode range. Reject or truncate oversized input before tokenization.

#### SEC-003: No Input Validation on Benchmark Iterations Parameter
**File:** YazhInferenceManager.cs (Line 268)
**Severity:** CRITICAL
**Description:** RunLatencyBenchmark(int iterations = 10) accepts an unbounded integer. A caller could pass int.MaxValue, causing an infinite loop that freezes the app.
**Impact:** Denial of service. The method is public and could be called by any code.
**Recommendation:** Clamp iterations to a sane range (1-100). Add parameter validation at method entry.

### HIGH (Fix before production)

#### SEC-004: Verbose Debug Logging in Production Builds
**File:** All C# files
**Severity:** HIGH
**Description:** Debug.Log() is used extensively throughout all scripts, including logging user input tokens (YazhInferenceManager.cs Line 163), pet stats, session data, and internal state. The enableLogging flag in GameManager only controls GameManager's own logs — all other classes log unconditionally.
**Impact:** Sensitive data (user chat input, pet stats, session duration, inference latency) visible in device logs. On Android, logs are readable by other apps with READ_LOGS permission. On iOS, visible via Xcode.
**Recommendation:** Wrap all Debug.Log calls in #if UNITY_EDITOR or a compile-time flag. Remove user input content from all log messages. Create a centralized logging gate.

#### SEC-005: AR Camera Data Not Protected
**File:** ARSetup.cs (Lines 27-40)
**Severity:** HIGH
**Description:** ARSetup checks camera permission but does not verify that camera frames are processed solely on-device. No guard against AR frame data being written to disk, shared via IPC, or accessed by other app components. The AROcclusionManager enables human segmentation — processing human images requires explicit consent and data handling guarantees.
**Impact:** Potential privacy violation. Camera frames of children (target demographic: 6-14) could leak if not properly handled. Human segmentation data is especially sensitive.
**Recommendation:** Add explicit documentation that all AR processing is on-device. Ensure no AR frame data is persisted to disk. Add runtime check that camera data is not being routed to any network endpoint. Consider adding a privacy consent screen before AR activation.

#### SEC-006: No Data Persistence Encryption
**File:** GameManager.cs (entire file)
**Severity:** HIGH
**Description:** While no save/load system is implemented yet, the game design includes pet stats, session metrics, and a 7-day challenge loop that will need persistence. No encryption strategy exists for PlayerPrefs or file-based saves.
**Impact:** When saves are implemented, pet data, session metrics, and potentially chat history will be stored in plaintext. On rooted/jailbroken devices, this data is trivially accessible.
**Recommendation:** Design encryption into the save system from the start. Use AES-256 for file-based saves. Avoid PlayerPrefs for sensitive data (it's plaintext XML on both platforms). Derive encryption keys from device-specific identifiers.

#### SEC-007: LatencyTest Saves Results to Unprotected File
**File:** LatencyTest.cs (Lines 267-300)
**Severity:** HIGH
**Description:** SaveResults() writes a JSON file containing model info, timestamps, and raw latency data to disk without encryption. The file path is constructed relative to Application.dataPath with no access controls.
**Impact:** Performance data and model metadata accessible to other apps (Android) or via backup extraction.
**Recommendation:** Encrypt benchmark results. Store in app-private directory. Add option to disable file output in production builds.

### MEDIUM (Fix during development)

#### SEC-008: Singleton Pattern Without Thread Safety
**File:** GameManager.cs (Lines 10-51), YazhInferenceManager.cs (Lines 13-54)
**Severity:** MEDIUM
**Description:** Both managers use singleton pattern with instance checking but no thread safety. In a multi-threaded inference scenario, race conditions could occur.
**Impact:** Potential null reference, duplicate instances, or corrupted state under concurrent access.
**Recommendation:** Add lock-based thread safety or document that singletons must be accessed only from main thread.

#### SEC-009: No Model Output Validation
**File:** YazhInferenceManager.cs (Lines 175-178)
**Severity:** MEDIUM
**Description:** Model output tokens are decoded directly to string and returned to the caller without any content filtering or validation. If the model produces unexpected output (garbage, special tokens as text, out-of-range tokens), it's passed directly to the UI.
**Impact:** UI display of garbage text, potential crash on invalid token IDs, or display of special tokens (<PAD>, <UNK>) to children.
**Recommendation:** Validate decoded output before returning. Filter special tokens from output. Add fallback response for empty/garbage output.

#### SEC-010: CI/CD Pipeline Has No Security Hardening
**File:** .github/workflows/build.yml
**Severity:** MEDIUM
**Description:** The GitHub Actions workflow uses `continue-on-error: true` on build steps, which silently swallows build failures. No secret scanning, no dependency vulnerability checks, no build artifact signing. The workflow triggers on any push to main/develop.
**Impact:** Compromised builds could go undetected. No guarantee of build artifact integrity.
**Recommendation:** Add secret scanning step. Remove continue-on-error or make it explicit. Add build artifact checksums. Consider requiring signed commits.

#### SEC-011: iOS ARKit Version Check is Fragile
**File:** ARSetup.cs (Line 31)
**Severity:** MEDIUM → HIGH (for kids app with COPPA requirements)
**Description:** Previous code: `UnityEngine.iOS.Device.systemVersion.Contains("ARKit")` is not a valid ARKit detection method. System version strings don't contain "ARKit". This check will always return false on iOS, disabling AR entirely. Additionally, no parental consent flow for camera access (COPPA violation for under-13 users).
**Impact:** AR never activates on iOS. Silent failure with no user-facing error. COPPA violations for kids app.
**Status:** ✅ FIXED (2026-06-18)
**Solution:**
- Fixed ARKit detection: Use ARSession.state at runtime instead of string matching
- Implemented complete permission + consent flow (RequestPermissionsAndConsent coroutine)
- Platform-specific permission requests (iOS system dialog + Android runtime permission)
- Parental consent dialog with audit trail logging (COPPA compliance)
- AR initialization gated on both permission AND consent
- See: COPPA_COMPLIANCE.md

#### SEC-012: Android Camera Permission Check is Insufficient
**File:** ARSetup.cs (Line 34)
**Severity:** MEDIUM
**Description:** Previous code: Permission.HasUserAuthorizedPermission only checks if permission was previously granted. It does not request permission if not granted. On first launch, this returns false and AR is silently disabled.
**Impact:** AR never works on first app launch. No user prompt to grant camera permission.
**Status:** ✅ FIXED (2026-06-18)
**Solution:**
- Implemented Permission.RequestUserPermission(Permission.Camera) coroutine
- Waits for user response in coroutine (async, non-blocking)
- Integrated with iOS permission flow for consistency
- Audit trail: Logs permission response (Android + iOS unified)
- See: COPPA_COMPLIANCE.md

### LOW (Address when implementing features)

#### SEC-013: No Certificate Pinning (Future-Proofing)
**Severity:** LOW
**Description:** Currently no network calls exist, so certificate pinning is not needed. If any network features are added (model updates, analytics opt-in), certificate pinning must be implemented.
**Recommendation:** Document that any future network code must implement certificate pinning.

#### SEC-014: No Anti-Tampering Measures
**Severity:** LOW
**Description:** No integrity checks on the app binary itself. No jailbreak/root detection.
**Recommendation:** For a kids' app handling camera data, consider adding root/jailbreak detection and app integrity verification.

#### SEC-015: BuildScript References Non-Existent Scenes
**File:** BuildScript.cs (Lines 11-16)
**Severity:** LOW
**Description:** BuildScript references 3 scenes (MainMenu.unity, PetSelection.unity, BiomeArena.unity) that don't exist yet. The GetExistingScenes() method handles this gracefully, but the hardcoded paths could cause confusion.
**Recommendation:** Add comment that scenes must be created by Thozhar before builds will include them.

---

## POSITIVE FINDINGS

1. **Zero Network Calls:** No WebRequest, HttpClient, UnityWebRequest, or socket code found in any C# script. The app is truly offline-first. This is excellent.

2. **No Telemetry/Analytics:** No analytics SDKs, crash reporters, or telemetry code found. No data leaves the device.

3. **ONNX Models Exist On-Device:** All 3 model variants (FP32, INT8, INT4) are bundled in StreamingAssets. No runtime model downloading.

4. **Secrets in .gitignore:** .env files and config.secrets.json are properly gitignored.

5. **No Hardcoded Credentials:** No API keys, tokens, or credentials found in any source file.

6. **Minimal Attack Surface:** The codebase is small (~1,300 lines C#) with no external service dependencies.

---

## FIXES APPLIED

The following fixes were applied during this audit and subsequent security hardening:

1. **SEC-001:** Added ONNX model SHA-256 hash verification (build-time + runtime)
   - BuildScript.cs: VerifyAllModelHashes() gates CI/CD pipeline
   - YazhInferenceManager.cs: VerifyModelHash() validates before load
   - Embedded hashes for all 3 models (INT8, INT4, FP32)
   - See: ONNX_HASH_VERIFICATION.md

2. **SEC-002:** Added input validation to GenerateResponse() — max length check, null/empty check

3. **SEC-003:** Added parameter validation to RunLatencyBenchmark() — iterations clamped to [1, 100]

4. **SEC-004:** Added compile-time logging gate (#if UNITY_EDITOR) to YazhInferenceManager.cs

5. **SEC-009:** Added output validation to GenerateResponse() — empty/garbage output fallback

6. **SEC-011:** Implemented iOS ARKit permission flow + COPPA parental consent
   - ARSetup.cs: RequestPermissionsAndConsent() orchestrates full flow
   - Platform-specific permission requests (iOS native + Android runtime)
   - Parental consent dialog with audit trail
   - AR initialization gated on permission AND consent
   - See: COPPA_COMPLIANCE.md

7. **SEC-012:** Fixed Android camera permission request
   - Implemented Permission.RequestUserPermission() coroutine
   - Unified iOS + Android permission flow
   - Audit logging for compliance

---

## RECOMMENDATIONS (Priority Order) — Updated 2026-06-18

**COMPLETED (✅):**
1. ✅ Implement ONNX model hash verification (SEC-001) — CRITICAL
2. ✅ Fix AR permission flow for both iOS and Android (SEC-011, SEC-012) — MEDIUM

**REMAINING (🔄):**
1. Input validation on all public API methods (SEC-002 partial) — **CRITICAL**
   - GenerateResponse() validated (done)
   - Need: Validate other public method inputs
2. Design encrypted save system before implementing persistence (SEC-006) — **HIGH**
3. Add privacy consent screen before AR activation (SEC-005) — **HIGH**
   - Note: SEC-011 parental consent dialog now serves this purpose
4. Harden CI/CD pipeline (SEC-010) — **MEDIUM**
5. Add anti-tampering measures before App Store submission (SEC-014) — **LOW**

---

## COMPLIANCE NOTES

**DPDP (India Digital Personal Data Protection Act):**
- Camera data of minors requires verifiable parental consent — NOT yet implemented
- Data minimization principle: only collect necessary data — COMPLIANT (no data collection)
- Right to erasure: must be able to delete user data — NOT yet implemented (no data stored)
- Data localization: all processing on-device — COMPLIANT

**COPPA (if targeting US market):**
- Parental consent required for under-13 data collection — NOT implemented
- Camera access requires clear disclosure — NOT implemented

---

## CONCLUSION

The Yazh-Unity codebase has a solid zero-cloud foundation with no network calls or telemetry. The primary security gaps are in input validation (CRITICAL), model integrity verification (CRITICAL), and data protection for the AR camera pipeline (HIGH). The fixes applied address the most urgent input validation issues. Model hash verification and encrypted persistence should be the next security priorities before any data is stored or models are updated.

**Next audit:** After Thozhar creates scenes and Arivu integrates the model, re-audit with focus on scene transition security and model inference output handling.

---

*Audited by KARUPPU | Rotation 25 | 2026-06-17*
