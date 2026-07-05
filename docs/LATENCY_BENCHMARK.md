# YAZH-UNITY ONNX Latency Benchmark
> ARIVU | Rotation 25 Cycle 7 | Jun 18, 2026

## Overview
This document describes how to benchmark the Yazh 30K ONNX model inference latency on iOS/Android devices. Target: **< 150ms average**.

## Test Methodology

### Benchmark Script
Located at: `Assets/Scripts/AI/LatencyTest.cs`

**Phases:**
1. **Warmup** (5 iterations) — Prime caches, JIT, GPU buffers
2. **Benchmark** (20 iterations, clamped to [1, 100]) — Measure actual latency
3. **Statistics** — Min, Max, Avg, Median, P95

**Test input:** `"வணக்கம்"` (Tamil for "Hello") — representative short user input.

### Configuration
```csharp
warmupIterations = 5;
benchmarkIterations = 20; // Clamped to [1, 100]
testInput = "வணக்கம்";
```

## Running the Benchmark

### On iOS Device
1. Build to iOS device (requires Mac with Xcode):
   ```bash
   ./build-yazh.sh ios debug=0
   ```
2. Open `.xcodeproj` in Xcode
3. Deploy to iPhone (iOS 12+)
4. Launch app, navigate to BiomeArena scene
5. The LatencyTest script auto-runs on first inference call (or trigger via UI button)
6. Results saved to: `Application.persistentDataPath/latency_results.json`

### On Android Device
1. Build APK:
   ```bash
   ./build-yazh.sh android debug=0
   ```
2. Install:
   ```bash
   adb install -r build/yazh-unity.apk
   ```
3. Launch via `adb shell am start -n com.yazhi.unity/.MainActivity`
4. Same as iOS — results in `Application.persistentDataPath`

### On Editor (Development Only)
- LatencyTest can run in Editor for development testing
- Results are NOT representative (Editor overhead is different from device)

## Results Interpretation

| Average Latency | Rating | Action |
|-----------------|--------|--------|
| < 100ms | Excellent ✓ | Ship as-is |
| 100-150ms | Acceptable ✓ | Ship, monitor |
| 150-250ms | Marginal ⚠️ | Optimize before ship |
| > 250ms | Poor ✗ | Must optimize |

## Optimization Strategies

If latency exceeds 150ms:

### 1. Model Quantization
- Switch from FP32 → INT8 (already deployed: `yazh-30k-int8.onnx`)
- Switch from INT8 → INT4 (already deployed: `yazh-30k-int4.onnx`)
- Tradeoff: smaller model = faster but lower accuracy

### 2. Backend Selection
- **CPU:** Default, works everywhere
- **GPU Compute Shader:** Faster on modern devices
- **GPU Pixel Shader:** Deprecated, avoid
- Configure in YazhInferenceManager.cs:
  ```csharp
  worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
  ```

### 3. Context Length
- Current: 256 tokens (max)
- Reduce to 128 if shorter dialogues are sufficient
- Speeds up token generation significantly

### 4. Token Sampling
- Greedy (current): Fastest, deterministic
- Top-K (K=10): Slightly slower, more diverse
- Top-P (P=0.9): Slowest, most diverse
- For kids' game, greedy is fine

### 5. Caching
- Cache tokenizer outputs
- Cache common response templates
- Memoize frequently-used inference results

## CI/CD Integration

### Automated Benchmark (Optional)
Add to `.github/workflows/build.yml`:
```yaml
benchmark:
  runs-on: ubuntu-latest
  steps:
    - uses: actions/checkout@v4
    - name: Build for benchmark
      run: ./build-yazh.sh android debug=0
    - name: Run latency test
      run: |
        # Requires connected Android device in CI farm
        adb install -r build/yazh-unity.apk
        adb shell am start -n com.yazhi.unity/.MainActivity
        sleep 30
        adb pull /sdcard/Android/data/com.yazhi.unity/files/latency_results.json
    - name: Check results
      run: |
        AVG=$(jq '.results_ms.avg' latency_results.json)
        if (( $(echo "$AVG > 150" | bc -l) )); then
          echo "❌ Latency exceeds 150ms target: ${AVG}ms"
          exit 1
        fi
        echo "✅ Latency OK: ${AVG}ms"
```

### Manual Benchmark Workflow
For now, benchmarks run manually on developer devices. CI integration requires:
- Connected device farm (GitHub Actions runners don't have devices)
- Or: cloud device testing service (Firebase Test Lab, BrowserStack)

## Current Status (Jun 18, 2026)

- ✅ ONNX models deployed: FP32, INT8, INT4
- ✅ LatencyTest.cs benchmark script ready
- ✅ YazhInferenceManager.cs loads from StreamingAssets
- ⏳ Results pending: Cannot run on actual device (no Unity Editor + no iOS/Android hardware)
- 📋 CI/CD integration: Documented, awaiting device farm setup

## Files Referenced
- `Assets/Scripts/AI/LatencyTest.cs` — Benchmark script
- `Assets/Scripts/AI/YazhInferenceManager.cs` — Inference pipeline (with SEC-001 hash verification)
- `Assets/StreamingAssets/MLModels/yazh-30k*.onnx` — Model files
- `Assets/StreamingAssets/MLModels/yazh-tokenizer.json` — Tokenizer
- `.github/workflows/build.yml` — CI/CD pipeline
- `Assets/Scripts/UI/UIStyles.cs` — Tamil-first styling

## Recommendations
1. Acquire at least one iOS + one Android device for testing
2. Run benchmark on real hardware before Week 3 (beta testing)
3. Add CI benchmark job once device farm is set up
4. Document actual latency results in this file after first device run

---
*Created: 2026-06-18 | ARIVU | Rotation 25 Cycle 7*
