#!/usr/bin/env python3
"""
Yazh 30K ONNX Model — Latency Validation Script
Runs inference on the ONNX model using ONNX Runtime and measures latency.
Can be run independently of Unity.

Usage:
    python3 latency_test.py [--model MODEL_PATH] [--iterations N] [--warmup N]
"""

import argparse
import json
import os
import sys
import time
import statistics
from pathlib import Path

def check_onnxruntime():
    """Check if onnxruntime is installed."""
    try:
        import onnxruntime as ort
        print(f"[OK] onnxruntime {ort.__version__}")
        return True
    except ImportError:
        print("[WARN] onnxruntime not installed. Install with: pip install onnxruntime")
        print("[INFO] Falling back to ONNX file validation only.")
        return False

def validate_model(model_path):
    """Validate the ONNX model structure without running inference."""
    try:
        import onnx
        model = onnx.load(model_path)
        onnx.checker.check_model(model)

        print(f"[OK] Model valid: {model_path}")
        print(f"  IR version: {model.ir_version}")
        opsets = [f"{o.domain or 'ai.onnx'}:{o.version}" for o in model.opset_import]
        print(f"  Opset: {opsets}")
        print(f"  Graph name: {model.graph.name}")
        print(f"  Inputs:")
        for inp in model.graph.input:
            shape = [d.dim_value if d.dim_value else d.dim_param for d in inp.type.tensor_type.shape.dim]
            dtype = inp.type.tensor_type.elem_type
            print(f"    - {inp.name}: shape={shape}, dtype={dtype}")
        print(f"  Outputs:")
        for out in model.graph.output:
            shape = [d.dim_value if d.dim_value else d.dim_param for d in out.type.tensor_type.shape.dim]
            dtype = out.type.tensor_type.elem_type
            print(f"    - {out.name}: shape={shape}, dtype={dtype}")

        # Count parameters
        total_params = 0
        for initializer in model.graph.initializer:
            params = 1
            for d in initializer.dims:
                params *= d
            total_params += params
        print(f"  Total parameters: {total_params:,} ({total_params / 1e6:.1f}M)")

        # Model size
        size_mb = os.path.getsize(model_path) / (1024 * 1024)
        print(f"  File size: {size_mb:.1f} MB")

        return True
    except ImportError:
        print("[WARN] onnx package not installed. Skipping structural validation.")
        size_mb = os.path.getsize(model_path) / (1024 * 1024) if os.path.exists(model_path) else 0
        print(f"  File exists: {os.path.exists(model_path)}, Size: {size_mb:.1f} MB")
        return True
    except Exception as e:
        print(f"[ERROR] Model validation failed: {e}")
        return False

def run_inference_benchmark(model_path, iterations=20, warmup=3):
    """Run inference benchmark using ONNX Runtime."""
    import onnxruntime as ort
    import numpy as np

    print(f"\n{'='*50}")
    print(f"  YAZH 30K LATENCY BENCHMARK")
    print(f"{'='*50}")
    print(f"  Model: {model_path}")
    print(f"  Iterations: {iterations}, Warmup: {warmup}")

    # Create session
    sess_options = ort.SessionOptions()
    sess_options.graph_optimization_level = ort.GraphOptimizationLevel.ORT_ENABLE_ALL
    sess_options.intra_op_num_threads = 4

    try:
        session = ort.InferenceSession(model_path, sess_options)
    except Exception as e:
        print(f"[ERROR] Failed to create session: {e}")
        return None

    # Get input info
    input_name = session.get_inputs()[0].name
    input_shape = session.get_inputs()[0].shape
    input_dtype = session.get_inputs()[0].type
    print(f"  Input: {input_name} shape={input_shape} dtype={input_dtype}")

    output_name = session.get_outputs()[0].name
    output_shape = session.get_outputs()[0].shape
    print(f"  Output: {output_name} shape={output_shape}")

    seq_len = input_shape[1] if len(input_shape) > 1 else 256

    # Warmup
    print(f"\n  [WARMUP] {warmup} iterations...")
    dummy_input = np.random.randint(0, 30000, size=(1, seq_len)).astype(np.int64)
    for i in range(warmup):
        try:
            session.run(None, {input_name: dummy_input})
            print(f"    Warmup {i+1}/{warmup} — OK")
        except Exception as e:
            print(f"    Warmup {i+1}/{warmup} — FAILED: {e}")
            return None

    # Benchmark
    print(f"\n  [BENCHMARK] {iterations} iterations...")
    latencies = []
    success = 0

    for i in range(iterations):
        # Random Tamil-like token IDs
        test_input = np.random.randint(0, 30000, size=(1, seq_len)).astype(np.int64)

        start = time.perf_counter()
        try:
            output = session.run(None, {input_name: test_input})
            end = time.perf_counter()
            latency_ms = (end - start) * 1000
            latencies.append(latency_ms)
            success += 1
            print(f"    [{i+1:02d}/{iterations}] {latency_ms:8.2f} ms")
        except Exception as e:
            print(f"    [{i+1:02d}/{iterations}] FAILED: {e}")

    # Statistics
    if latencies:
        latencies.sort()
        avg = statistics.mean(latencies)
        med = statistics.median(latencies)
        std = statistics.stdev(latencies) if len(latencies) > 1 else 0
        p50 = latencies[int(len(latencies) * 0.50)]
        p90 = latencies[int(len(latencies) * 0.90)]
        p95 = latencies[int(len(latencies) * 0.95)]
        min_l = min(latencies)
        max_l = max(latencies)

        passed = avg < 150
        verdict = "PASS" if passed else "FAIL"

        print(f"\n{'='*50}")
        print(f"  RESULTS")
        print(f"{'='*50}")
        print(f"  Success:     {success}/{iterations}")
        print(f"  Min:         {min_l:.2f} ms")
        print(f"  Avg:         {avg:.2f} ms")
        print(f"  Median:      {med:.2f} ms")
        print(f"  P50:         {p50:.2f} ms")
        print(f"  P90:         {p90:.2f} ms")
        print(f"  P95:         {p95:.2f} ms")
        print(f"  Max:         {max_l:.2f} ms")
        print(f"  Std Dev:     {std:.2f} ms")
        print(f"  Target:      < 150 ms")
        print(f"  Verdict:     {verdict}")
        print(f"{'='*50}")

        # Save results
        results = {
            "model": model_path,
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "iterations": iterations,
            "success_count": success,
            "latency_ms": {
                "min": round(min_l, 2),
                "avg": round(avg, 2),
                "median": round(med, 2),
                "p50": round(p50, 2),
                "p90": round(p90, 2),
                "p95": round(p95, 2),
                "max": round(max_l, 2),
                "std": round(std, 2)
            },
            "target_ms": 150,
            "passed": passed,
            "raw_latencies": [round(l, 2) for l in latencies]
        }

        results_path = Path(model_path).parent / "latency_results.json"
        with open(results_path, 'w') as f:
            json.dump(results, f, indent=2)
        print(f"\n  [SAVE] Results saved to: {results_path}")

        return results
    else:
        print("\n  [ERROR] No successful inferences.")
        return None

def main():
    parser = argparse.ArgumentParser(description="Yazh 30K ONNX Latency Test")
    parser.add_argument("--model", type=str,
                        default="MLModels/yazh-30k-int8.onnx",
                        help="Path to ONNX model (relative to StreamingAssets)")
    parser.add_argument("--iterations", type=int, default=20,
                        help="Number of benchmark iterations")
    parser.add_argument("--warmup", type=int, default=3,
                        help="Number of warmup iterations")
    parser.add_argument("--streaming-assets", type=str,
                        default="/home/neutron/Yazhi/apps/yazh-unity/Assets/StreamingAssets",
                        help="Path to StreamingAssets directory")
    args = parser.parse_args()

    # Resolve model path
    model_path = os.path.join(args.streaming_assets, args.model)
    if not os.path.exists(model_path):
        print(f"[ERROR] Model not found: {model_path}")
        sys.exit(1)

    print(f"Model: {model_path}")
    print(f"Size: {os.path.getsize(model_path) / (1024*1024):.1f} MB")

    # Step 1: Validate model structure
    print(f"\n--- Step 1: Model Validation ---")
    if not validate_model(model_path):
        sys.exit(1)

    # Step 2: Run inference benchmark
    print(f"\n--- Step 2: Inference Benchmark ---")
    has_ort = check_onnxruntime()
    if has_ort:
        results = run_inference_benchmark(model_path, args.iterations, args.warmup)
        if results is None:
            sys.exit(1)
    else:
        print("[SKIP] Cannot run inference without onnxruntime.")
        print("[INFO] The Unity LatencyTest.cs component will run the benchmark in-editor.")

    print("\n[DONE] Latency test complete.")

if __name__ == "__main__":
    main()
