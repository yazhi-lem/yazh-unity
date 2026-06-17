#!/bin/bash
# build-yazh.sh — Build script for yazh-unity on iOS/Android
# Usage: ./build-yazh.sh [ios|android] [debug=0/1]
#
# Environment detection:
#   - Detects Unity installation (editor, hub, manual path)
#   - Validates Android SDK/NDK for Android builds
#   - Validates Xcode for iOS builds (macOS only)
#   - Falls back to CI/CD instructions if local build impossible
#
# Project: Yazh XR Pet App (Unity 6 LTS + AR Foundation + Barracuda ONNX)
# Author: UYIR (DevOps, Rotation 25) — 2026-06-17

set -e

PROJECT_ROOT=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
UNITY_PROJ="${PROJECT_ROOT}"
BUILD_DIR="${PROJECT_ROOT}/Build"
LOGS_DIR="${PROJECT_ROOT}/Logs"

TARGET_PLATFORM="${1:-ios}"
DEBUG_MODE="${2:-0}"

echo "───────────────────────────────────────────────────────────"
echo "  YAZH XR APP BUILD SCRIPT"
echo "───────────────────────────────────────────────────────────"
echo "  Platform: $TARGET_PLATFORM"
echo "  Mode: $([ "$DEBUG_MODE" = "1" ] && echo "DEBUG" || echo "RELEASE")"
echo "  Project: $UNITY_PROJ"
echo "  OS: $(uname -s) ($(uname -m))"
echo "───────────────────────────────────────────────────────────"

mkdir -p "$BUILD_DIR" "$LOGS_DIR"

# ─── Detect Unity ───────────────────────────────────────────────
detect_unity() {
    UNITY_CMD=""

    # Check common Unity paths
    local unity_paths=(
        "/usr/bin/Unity"
        "/usr/local/bin/Unity"
        "/opt/Unity/Editor/Unity"
        "$HOME/Unity/Editor/Unity"
        "/Applications/Unity/Hub/Editor/*/Unity.app/Contents/MacOS/Unity"
    )

    for p in "${unity_paths[@]}"; do
        # Handle glob paths
        for resolved in $p; do
            if [ -x "$resolved" ]; then
                UNITY_CMD="$resolved"
                echo "  Unity found: $UNITY_CMD"
                return 0
            fi
        done
    done

    # Check PATH
    if command -v Unity &>/dev/null; then
        UNITY_CMD="$(command -v Unity)"
        echo "  Unity found in PATH: $UNITY_CMD"
        return 0
    fi

    return 1
}

# ─── Validate Android SDK ──────────────────────────────────────
validate_android_sdk() {
    local has_sdk=false
    local has_ndk=false

    if [ -n "$ANDROID_HOME" ] && [ -d "$ANDROID_HOME" ]; then
        echo "  ANDROID_HOME: $ANDROID_HOME"
        has_sdk=true
    elif [ -n "$ANDROID_SDK_ROOT" ] && [ -d "$ANDROID_SDK_ROOT" ]; then
        echo "  ANDROID_SDK_ROOT: $ANDROID_SDK_ROOT"
        has_sdk=true
    else
        echo "  ⚠️  ANDROID_HOME / ANDROID_SDK_ROOT not set"
    fi

    if [ -n "$ANDROID_NDK_HOME" ] && [ -d "$ANDROID_NDK_HOME" ]; then
        echo "  ANDROID_NDK_HOME: $ANDROID_NDK_HOME"
        has_ndk=true
    elif [ -n "$ANDROID_HOME" ] && [ -d "$ANDROID_HOME/ndk" ]; then
        echo "  NDK found in ANDROID_HOME/ndk"
        has_ndk=true
    else
        echo "  ⚠️  Android NDK not found"
    fi

    if $has_sdk && $has_ndk; then
        return 0
    fi
    return 1
}

# ─── Validate iOS (macOS only) ─────────────────────────────────
validate_ios() {
    if [ "$(uname -s)" != "Darwin" ]; then
        echo "  ❌ iOS builds require macOS (current: $(uname -s))"
        return 1
    fi

    if ! command -v xcodebuild &>/dev/null; then
        echo "  ❌ Xcode not found. Install from App Store."
        return 1
    fi

    echo "  Xcode: $(xcodebuild -version 2>/dev/null | head -1)"
    return 0
}

# ─── CI/CD Fallback ────────────────────────────────────────────
ci_fallback() {
    echo ""
    echo "═══════════════════════════════════════════════════════════"
    echo "  LOCAL BUILD NOT AVAILABLE"
    echo "═══════════════════════════════════════════════════════════"
    echo ""
    echo "  Cannot build $TARGET_PLATFORM on this machine."
    echo ""
    echo "  Options:"
    echo ""
    echo "  1. GitHub Actions (recommended):"
    echo "     Push to main/develop branch → CI builds automatically"
    echo "     Workflow: .github/workflows/build.yml"
    echo ""
    echo "  2. Remote build machine:"
    echo "     - Android: x86_64 Linux + Unity 6 LTS + Android module"
    echo "     - iOS: macOS + Xcode + Unity 6 LTS + iOS module"
    echo ""
    echo "  3. Unity Cloud Build:"
    echo "     https://dashboard.unity3d.com → Cloud Build"
    echo ""
    echo "  See BUILD_ENVIRONMENT.md for full setup instructions."
    echo "═══════════════════════════════════════════════════════════"
}

# ─── Build iOS ─────────────────────────────────────────────────
build_ios() {
    echo "🔨 Building for iOS (ARKit)..."

    if ! validate_ios; then
        ci_fallback
        exit 1
    fi

    if ! detect_unity; then
        echo "❌ Unity not found."
        ci_fallback
        exit 1
    fi

    local build_path="$BUILD_DIR/Yazh-iOS"
    local log_file="$LOGS_DIR/ios_build_$(date +%Y%m%d_%H%M%S).log"

    echo "  Output: $build_path"
    echo "  Log: $log_file"

    "$UNITY_CMD" -quit -batchmode \
        -projectPath "$UNITY_PROJ" \
        -executeMethod BuildScript.BuildiOS \
        -debug="$DEBUG_MODE" \
        -logFile "$log_file"

    echo "✅ iOS build complete: $build_path"
    echo "   Open in Xcode for signing and App Store submission"
}

# ─── Build Android ─────────────────────────────────────────────
build_android() {
    echo "🔨 Building for Android (ARCore)..."

    if ! detect_unity; then
        echo "❌ Unity not found."
        ci_fallback
        exit 1
    fi

    if ! validate_android_sdk; then
        echo "⚠️  Android SDK/NDK not fully configured."
        echo "   Unity may use bundled SDK. Attempting build anyway..."
    fi

    local build_path="$BUILD_DIR/Yazh-Android.apk"
    local log_file="$LOGS_DIR/android_build_$(date +%Y%m%d_%H%M%S).log"

    echo "  Output: $build_path"
    echo "  Log: $log_file"

    "$UNITY_CMD" -quit -batchmode \
        -projectPath "$UNITY_PROJ" \
        -executeMethod BuildScript.BuildAndroid \
        -debug="$DEBUG_MODE" \
        -logFile "$log_file"

    if [ -f "$build_path" ]; then
        echo "✅ Android build complete: $build_path"
        ls -lh "$build_path"
    else
        echo "⚠️  Build may have failed. Check log: $log_file"
    fi
}

# ─── Main ──────────────────────────────────────────────────────
case "$TARGET_PLATFORM" in
    ios)
        build_ios
        ;;
    android)
        build_android
        ;;
    *)
        echo "❌ Unknown platform: $TARGET_PLATFORM"
        echo "   Usage: ./build-yazh.sh [ios|android] [debug=0/1]"
        exit 1
        ;;
esac

echo ""
echo "📊 Build Summary:"
echo "   Platform: $TARGET_PLATFORM"
echo "   Mode: $([ "$DEBUG_MODE" = "1" ] && echo "DEBUG" || echo "RELEASE")"
echo "   Output: $BUILD_DIR/"
echo "   Logs: $LOGS_DIR/"
echo "───────────────────────────────────────────────────────────"
