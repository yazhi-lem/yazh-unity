#!/bin/bash
# build-yazh.sh — Build script for yazh-unity on iOS/Android

set -e

PROJECT_ROOT=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
UNITY_PROJ="${PROJECT_ROOT}"
BUILD_DIR="${PROJECT_ROOT}/Build"
LOGS_DIR="${PROJECT_ROOT}/Logs"

TARGET_PLATFORM="${1:-ios}"  # ios or android
DEBUG_MODE="${2:-0}"         # 0=release, 1=debug

echo "———————————————————————————————————————————————————————————"
echo "  YAZH XR APP BUILD SCRIPT"
echo "———————————————————————————————————————————————————————————"
echo "  Platform: $TARGET_PLATFORM"
echo "  Mode: $([ "$DEBUG_MODE" = "1" ] && echo "DEBUG" || echo "RELEASE")"
echo "  Project: $UNITY_PROJ"
echo "———————————————————————————————————————————————————————————"

mkdir -p "$BUILD_DIR" "$LOGS_DIR"

# Check for Unity
if ! command -v unity &>/dev/null; then
    echo "❌ Unity not found. Install Unity 6 LTS and add to PATH."
    exit 1
fi

# Build iOS
if [ "$TARGET_PLATFORM" = "ios" ]; then
    echo "🔨 Building for iOS (ARKit)..."
    
    unity -quit -batchmode \
        -projectPath "$UNITY_PROJ" \
        -executeMethod BuildScript.BuildiOS \
        -debug="$DEBUG_MODE" \
        -logFile "$LOGS_DIR/ios_build.log"
    
    BUILD_OUTPUT="$BUILD_DIR/Yazh-iOS.ipa"
    echo "✅ iOS build complete: $BUILD_OUTPUT"

# Build Android
elif [ "$TARGET_PLATFORM" = "android" ]; then
    echo "🔨 Building for Android (ARCore)..."
    
    unity -quit -batchmode \
        -projectPath "$UNITY_PROJ" \
        -executeMethod BuildScript.BuildAndroid \
        -debug="$DEBUG_MODE" \
        -logFile "$LOGS_DIR/android_build.log"
    
    BUILD_OUTPUT="$BUILD_DIR/Yazh-Android.apk"
    echo "✅ Android build complete: $BUILD_OUTPUT"

else
    echo "❌ Unknown platform: $TARGET_PLATFORM"
    echo "   Usage: ./build-yazh.sh [ios|android] [debug=0/1]"
    exit 1
fi

echo ""
echo "📊 Build Summary:"
echo "   Output: $BUILD_OUTPUT"
echo "   Logs: $LOGS_DIR/*.log"
echo "———————————————————————————————————————————————————————————"
