# YAZH-UNITY Model Status Report

> **⚠️ Status correction (Oct 2026): there are no trained models in this repo.**
>
> The three `.onnx` files in `Assets/StreamingAssets/MLModels/` are **placeholder files**,
> not trained models. Each one is a 20-byte header (`ONNX` magic bytes, a few
> integers, and the string `Yazh`) followed only by zero bytes, padded out to
> 2.5 / 5 / 10 MB. They contain no weights and no graph, and they are not valid
> ONNX protobuf, so Barracuda/Sentis cannot load them for inference.
>
> `yazh-tokenizer.json` is also a placeholder. It holds metadata only (vocab size,
> model type, special-token names) and has **no vocabulary and no BPE merges**.
>
> **Real trained models and a real tokenizer are still needed.** Until they exist,
> the app runs on its scripted Tamil dialogue fallback (CI builds are model-less
> by default). Any latency figures below are estimates, not measurements.
> The rest of this report was written in June 2026 and has been corrected where it
> described these files as working models.

## ARIVU — ML Integration — Rotation 25 (Jun 17, 2026)

---

## Model Inventory

Placeholder files at: `Assets/StreamingAssets/MLModels/` (none of these are trained models)

| File | Format | Size | Intended precision | Actual contents |
|------|--------|------|-----------|----------|
| `yazh-30k.onnx` | Placeholder | 10.0 MB | FP32 (planned) | 20-byte header + zeros, no weights |
| `yazh-30k-int8.onnx` | Placeholder | 5.0 MB | INT8 (planned) | 20-byte header + zeros, no weights |
| `yazh-30k-int4.onnx` | Placeholder | 2.5 MB | INT4 (planned) | 20-byte header + zeros, no weights |
| `yazh-tokenizer.json` | JSON | 220 B | — | Metadata only, no vocabulary or merges |

### ONNX File Validation
- Each of the 3 files starts with the bytes `ONNX` (`4f4e4e58`), but that is the only
  content: after a 20-byte header, every remaining byte is zero.
- They are not valid ONNX (protobuf) models and there is no "custom Yazh
  serialization format". They will **not** load in Barracuda/Sentis.
- They are placeholders that reserve the file names and approximate sizes until
  real trained models are exported.

---

## Model Specification (planned, not yet trained)

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

### What Works (code paths only; never exercised with a real model)
- ONNX model loading code via `ModelLoader.Load()` from StreamingAssets (the current placeholder files will not load)
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
- **No trained models** — the 3 `.onnx` files are placeholders (header + zeros); real trained models still need to be trained and exported
- **No tokenizer vocabulary** — `yazh-tokenizer.json` is 220 bytes of metadata with no vocabulary and no BPE merges

---

*Report generated: 2026-06-17 | ARIVU | Rotation 25*
