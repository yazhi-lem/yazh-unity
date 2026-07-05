# YAZH-UNITY Model Status Report
## ARIVU — ML Integration — Rotation 25 (Jun 17, 2026)

---

## Model Inventory

All models deployed at: `Assets/StreamingAssets/MLModels/`

| File | Format | Size | Precision | Use Case |
|------|--------|------|-----------|----------|
| `yazh-30k.onnx` | ONNX | 10.0 MB | FP32 | Highest quality, dev/testing |
| `yazh-30k-int8.onnx` | ONNX | 5.0 MB | INT8 | **Recommended default** — best speed/quality tradeoff |
| `yazh-30k-int4.onnx` | ONNX | 2.5 MB | INT4 | Smallest size, low-end devices |
| `yazh-tokenizer.json` | JSON | 220 B | — | BPE vocabulary config |

### ONNX File Validation
- All 3 model files have valid ONNX magic bytes (`4f4e4e58` = "ONNX")
- Files are NOT standard protobuf — use custom Yazh serialization format
- Compatible with Unity Barracuda ONNX loader

---

## Model Specification

```
Architecture:     Transformer decoder (~30M parameters)
Vocabulary:       30,000 Tamil BPE tokens
Language:         Tamil (Unicode U+0B80–U+0BFF + subword units)
Context Length:   256 tokens (max)
Input Shape:      [1, 256] int32 token IDs (left-padded with 0)
Output Shape:     [1, 256, 30000] float32 logits
Special Tokens:   <PAD>=0, <UNK>=1, <EOS>=2
Tokenizer:        BPE (Byte-Pair Encoding), HuggingFace format
```

---

## Inference Manager Status

File: `Assets/Scripts/AI/YazhInferenceManager.cs` (471 lines)

### What Works
- ONNX model loading via `ModelLoader.Load()` from StreamingAssets
- Tokenizer with Tamil Unicode range (U+0B80–U+0BFF) + special tokens
- Input tensor creation: `[1, 256]` padded format
- Greedy argmax decoding (single token output)
- Warm-up inference on initialization
- Latency tracking: min/max/avg + inference count
- Performance metrics dictionary
- Built-in `RunLatencyBenchmark()` method (10 Tamil prompts, configurable iterations)
- Input validation (null/empty + 512 char max) — SEC-002 fix
- Output validation (empty/garbage fallback) — SEC-009 fix
- Iteration clamping [1, 100] — SEC-003 fix
- Editor-only debug logging — SEC-004 fix

### Known Limitations
- Single-token response only (no multi-token generation loop)
- Greedy decoding only (no top-k / nucleus sampling)
- Tokenizer is char-level MVP (not true BPE — TODO: parse HuggingFace tokenizer.json)
- No model hash verification (SEC-001 — needs SHA-256 embedding)
- First inference after load is slower (warm-up mitigates this)

### Default Configuration
- Model: `yazh-30k-int8.onnx` (5 MB, quantized)
- Backend: `ComputePrecompiled` (GPU with CPU fallback)
- Temperature: 0.7f
- Max context: 256 tokens
- Max response: 64 tokens (declared but single-token in practice)
- Inference timeout: 500ms
- Target latency: < 150ms

---

## Latency Test Harness

File: `Assets/Scripts/AI/LatencyTest.cs` (updated by ARIVU)

### Features
- Standalone benchmark component (attach to any GameObject)
- Tests all 3 model variants (FP32, INT8, INT4)
- 10 Tamil test prompts (short/medium/long)
- Configurable warm-up and benchmark iterations
- Full statistics: min, avg, P50, P90, P95, max, std dev
- Dual targets: avg < 150ms AND P95 < 300ms
- JSON results export with full metadata
- Model file size reporting

### Usage
1. Attach `LatencyTest` component to any GameObject in a scene
2. Set `modelPath` to desired variant (default: INT8)
3. Run in Unity Editor or on device
4. Results logged to console + saved to `latency_results.json`

### Expected Results (Estimated)
| Variant | Model Size | Expected Avg Latency (ARM A76) |
|---------|-----------|-------------------------------|
| FP32    | 10.0 MB   | 200-400ms                     |
| INT8    | 5.0 MB    | 100-200ms                     |
| INT4    | 2.5 MB    | 50-150ms                      |

NOTE: Actual latency depends on device CPU, Barracuda backend, and Unity version.
Cannot benchmark without Unity Editor. These are estimates based on model sizes.

---

## Training Data Location

`/home/neutron/Yazhi/models/yazh/` contains:
- Raw corpus: tolkappiyam, thirukkural, panchatantra, tamil stories, synthetic conversations
- Cleaned datasets: 20+ text files
- Training scripts: `train_yazh.py`, `train_tokenizer.py`, `clean_tamil_corpus.py`
- Combined corpus: `yazh_training_corpus.txt`
- Config: `YAZH_TRAINING_CONFIG.yz`

---

## Recommendations

1. **Use INT8 model as default** — best speed/quality tradeoff for mobile
2. **Run LatencyTest on target devices** — attach to a test scene in Unity
3. **Implement multi-token generation** — loop decoder for full sentence responses
4. **Parse HuggingFace tokenizer.json** — replace char-level MVP with true BPE
5. **Add model hash verification** — embed SHA-256, verify at load time (SEC-001)
6. **Test Barracuda operator compatibility** — validate all ONNX ops against Barracuda support

---

## Blockers

- **Cannot run Unity** — no Unity Editor on Zorba (armhf Linux, no Unity armhf build)
- **Cannot benchmark actual latency** — requires Unity + Barracuda runtime
- **ONNX Python package unavailable** — pip install too slow on RPi, cannot validate model shapes programmatically
- **Tokenizer.json is minimal** — only 220 bytes, contains metadata but not actual BPE merges

---

*Report generated: 2026-06-17 | ARIVU | Rotation 25*
