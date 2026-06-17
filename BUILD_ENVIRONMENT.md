════════════════════════════════════════════════════════════════════════════════
                    YAZH-UNITY BUILD ENVIRONMENT SETUP
                    DevOps Audit — UYIR Rotation 25
                    2026-06-17
════════════════════════════════════════════════════════════════════════════════

📋 EXECUTIVE SUMMARY
─────────────────────────────────────────────────────────────────────────
  Machine:      Zorba (Raspberry Pi 5, 8GB RAM, 58GB SSD)
  OS:           Debian Bookworm (armhf userspace, aarch64 kernel)
  Unity Editor: ❌ NOT INSTALLED (not available for armhf Linux)
  Android SDK:  ❌ NOT INSTALLED (available: apt install android-sdk)
  Java:         ✅ OpenJDK 17.0.19
  Node.js:      ✅ v22.15.0
  Docker:       ✅ 20.10.24
  CMake:        ✅ 3.25.1
  Disk Free:    12 GB (79% used)
  Git:          ✅ 2.39.5

  CONCLUSION: Full iOS/Android builds require a macOS/x86_64 Linux dev
  machine. This machine CAN set up the Android SDK toolchain but CANNOT
  run the Unity Editor to compile builds.

════════════════════════════════════════════════════════════════════════════════

🟢 ANDROID BUILD PIPELINE
════════════════════════════════════════════════════════════════════════════════

PLATFORM SUPPORT:
  Target:         Android 7.0+ (API 24+, ARCore compatible)
  Engine:         Unity 6 LTS + AR Foundation
  Build output:   .apk (debug) / .aab (store)
  Architecture:   ARM64 (primary), ARMv7 (secondary)

PREREQUISITES (Dev Machine — Linux x86_64 or macOS):
─────────────────────────────────────────────────────────────────────────
  1. Unity 6 LTS (6000.x) with Android Build Support module
     - Install via Unity Hub → Add Modules → Android Build Support
     - Includes: Android SDK & NDK tools, OpenJDK
  2. Android SDK (API 24–34)
  3. Android NDK (r23+ recommended)
  4. Java 17 (bundled with Unity or system)

ANDROID BUILD STEPS:
─────────────────────────────────────────────────────────────────────────
  Step 1: Open project in Unity Editor
    $ /path/to/Unity -projectPath /home/neutron/Yazh-unity

  Step 2: Switch platform
    File → Build Settings → Android → Switch Platform

  Step 3: Configure Player Settings
    - Package: com.zoo.yazh
    - Min API: Android 7.0 (API 24)
    - Target API: Android 14 (API 34)
    - Architecture: ARM64 + ARMv7
    - Scripting Backend: IL2CPP

  Step 4: Add scenes to build
    - Assets/Scenes/MainMenu.unity
    - Assets/Scenes/PetSelection.unity
    - Assets/Scenes/BiomeArena.unity

  Step 5: Build
    $ ./build-yazh.sh android 1   # debug APK
    $ ./build-yazh.sh android 0   # release AAB

  Output: Build/Yazh-Android.apk (debug) or Build/Yazh-Android.aab (release)

ANDROID SDK INSTALLATION (for local toolchain testing on Zorba):
─────────────────────────────────────────────────────────────────────────
  # Check available packages
  $ apt-cache policy android-sdk
    Candidate: 28.0.2+9 (armhf)

  # Install Android SDK base
  $ sudo apt-get install -y android-sdk android-sdk-build-tools \
      android-sdk-common

  # Install platform tools (adb, fastboot)
  $ sudo apt-get install -y adb

  # Verify
  $ adb version
  $ which sdkmanager

  NOTE: This installs only the SDK TOOLS (adb, fastboot). The Unity
  Editor requires NDK/SDK that Unity Hub installs separately. The
  system android-sdk package is insufficient for standalone Unity builds.

BUILD CONFIGURATION:
─────────────────────────────────────────────────────────────────────────
  Bundle ID:          com.zoo.yazh
  Min SDK:            API 24 (Android 7.0)
  Target SDK:         API 34 (Android 14)
  Scripting Backend:  IL2CPP
  Target Arch:        ARM64 primary, ARMv7 fallback
  Gradle Version:     Bundled with Unity Android module
  Proguard:           Enabled (release builds only)

════════════════════════════════════════════════════════════════════════════════

🔴 iOS BUILD PIPELINE
════════════════════════════════════════════════════════════════════════════════

PLATFORM SUPPORT:
  Target:         iOS 12.0+ (ARKit compatible)
  Engine:         Unity 6 LTS + AR Foundation
  Build output:   .ipa (App Store) or Xcode project

⚠️  iOS CANNOT be built from Linux. Requires macOS + Xcode.

PREREQUISITES (macOS Dev Machine):
─────────────────────────────────────────────────────────────────────────
  1. macOS 13+ (Ventura or later)
  2. Xcode 15+ (includes iOS SDK, Swift, Clang)
  3. Unity 6 LTS with iOS Build Support module
  4. Apple Developer Account ($99/year for App Store distribution)
  5. Signing certificates + provisioning profiles

iOS BUILD STEPS:
─────────────────────────────────────────────────────────────────────────
  Step 1: Build from Unity (macOS)
    $ ./build-yazh.sh ios 0

    This generates an Xcode project at: Build/Yazh-iOS/

  Step 2: Open Xcode project
    $ open Build/Yazh-iOS/YazhUnity.xcodeproj

  Step 3: Configure signing
    - Select team (Apple Developer account)
    - Set bundle ID: com.zoo.yazh
    - Auto-signing recommended for development

  Step 4: Build archive
    Product → Archive → Distribute App → App Store Connect

  Output: Build/Yazh-iOS.ipa (via Xcode archive export)

iOS CAPabilities USED:
─────────────────────────────────────────────────────────────────────────
  - ARKit (augmented reality)
  - Camera (AR session)
  - Microphone (voice input for Tamil STT)
  - Voice Processing (AVFoundation)
  Required Info.plist entries:
    - NSCameraUsageDescription
    - NSMicrophoneUsageDescription
    - NSLocationWhenInUseUsageDescription (optional, for AR geo)
    - UIRequiredDeviceCapabilities: arkit

════════════════════════════════════════════════════════════════════════════════

🔧 CI/CD PIPELINE (GitHub Actions)
════════════════════════════════════════════════════════════════════════════════

Location: .github/workflows/build.yml (created by VAJRA)

Pipeline:
  1. validate  → Project structure + C# brace check + ONNX integrity
  2. build-android → ubuntu-latest + game-ci/unity-builder@v4
  3. build-ios  → macos-latest + game-ci/unity-builder@v4
  4. summary    → Artifact links

Triggers: push to main/develop, PRs, manual dispatch

NOTE: GitHub Actions requires Unity license activation via:
  UNITY_LICENSE (repo secret) or Unity Serial (CI activation)
  See: https://game.ci/docs/github/activation

════════════════════════════════════════════════════════════════════════════════

⚠️  KNOWN BLOCKERS ON ZORBA (RPi 5 armhf)
════════════════════════════════════════════════════════════════════════════════

  1. NO UNITY EDITOR FOR armhf
     Unity does not ship armhf Linux builds. Only x86_64 Linux.
     Impossible to run Unity Editor natively on Zorba.

  2. NO XCODE ON LINUX
     iOS builds absolutely require macOS + Xcode.
     No workaround exists.

  3. ANDROID SDK NOT INSTALLED
     Available via apt (android-sdk 28.0.2) but not installed.
     Even if installed, cannot build without Unity Editor.

  4. 12 GB FREE DISK
     Unity + Android SDK + NDK + Xcode would need ~30+ GB.
     Zorba's 58 GB disk is insufficient for full toolchain.

════════════════════════════════════════════════════════════════════════════════

📋 RECOMMENDED ACTIONS
════════════════════════════════════════════════════════════════════════════════

  FOR CURRENT DEVELOPMENT (Rotation 25):
  ──────────────────────────────────────────────────────────────────────
  1. Use a cloud CI/CD provider (GitHub Actions) for actual builds
  2. OR use a macOS device (Mac Mini M1+ recommended) for local builds
  3. OR use a x86_64 Linux machine for Android builds
  4. Keep Zorba for: development, code editing, git, planning, ML inference

  IF MACOS MACHINE ACQUIRED:
  ──────────────────────────────────────────────────────────────────────
  1. Install Homebrew
  $ /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

  2. Install Unity Hub
  $ brew install --cask unity-hub

  3. Install Unity 6 LTS via Hub
     - Add modules: Android Build Support, iOS Build Support, Linux Build Support
     - Note: Unity Hub auto-downloads SDK/NDK for Android

  4. Install Xcode from App Store
     $ xcode-select --install

  5. Clone repo and build
  $ cd /path/to/yazh-unity
  $ ./build-yazh.sh android 1   # test Android debug build
  $ ./build-yazh.sh ios 1       # test iOS debug build

════════════════════════════════════════════════════════════════════════════════

📊 BUILD ENVIRONMENT STATUS
─────────────────────────────────────────────────────────────────────────
| Component          | Available on Zorba | Required For        | Status |
|--------------------|--------------------|---------------------|--------|
| Unity 6 Editor     | ❌ armhf not supp. | All builds          | BLOCKED|
| Android SDK/NDK    | ❌ (apt available) | Android builds      | PENDING|
| Xcode              | ❌ macOS only      | iOS builds          | BLOCKED|
| Java 17            | ✅ OpenJDK 17      | Unity + Android     | OK     |
| Git                | ✅ 2.39.5          | Version control     | OK     |
| Node.js            | ✅ v22.15.0        | CI/CD tooling       | OK     |
| Docker             | ✅ 20.10.24        | CI containers       | OK     |
| CMake              | ✅ 3.25.1          | Native builds       | OK     |
| GitHub Actions N/A | ✅ cloud-based     | CI/CD pipeline      | READY* |

* GitHub Actions requires Unity license secret configured.

════════════════════════════════════════════════════════════════════════════════

Author: UYIR (DevOps, Rotation 25)
Date: 2026-06-17
Last Updated: 2026-06-17
════════════════════════════════════════════════════════════════════════════════
