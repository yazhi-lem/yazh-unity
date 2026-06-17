# YAZH-UNITY — Production Team Assignments

**Project:** YAZH-UNITY XR App (Kids Pet Game, Tamil-First, On-Device AI)
**Engine:** Unity 6 LTS (C# + Barracuda ONNX)
**Timeline:** 12 Weeks (Jun 13 – Sep 5, 2026)
**Last Updated:** 2026-06-17 | ANBU (Team Coordination)

---

## 1. Team Roster — 8 Production Roles

### VAJRA — Tech Lead
**Reports to:** Project Owner (Yazhi)
**Role:** Technical architecture, code quality, build pipeline, cross-team technical decisions

**Responsibilities:**
- Own the architecture audit and maintain technical documentation
- Manage CI/CD pipeline (GitHub Actions, build validation)
- Review all C# code for correctness, performance, and maintainability
- Resolve cross-cutting technical issues (AR setup, model integration, platform builds)
- Make final calls on engine-level decisions (package versions, compilation targets)
- Coordinate with ARIVU on model integration, with THOZHAR on scene architecture, with UYIR on build targets

**Current Status:** ✅ COMPLETE (Architecture audit + CI/CD setup done)
**Key Deliverables:** ARCHITECTURE_AUDIT.yz, CI/CD workflow, BuildScript.cs

---

### THOZHAR — UI Designer
**Reports to:** VAJRA (technical), OVIYA (visual direction)
**Role:** Unity scene creation, UI/UX implementation, frontend mobile development

**Responsibilities:**
- Create and maintain all Unity scenes: MainMenu, PetSelection, BiomeArena
- Implement UI layouts, transitions, animations, and responsive design
- Build the 7-day game loop UI (pet care, dialogue, survival mechanics)
- Ensure UI works across iOS and Android screen sizes
- Collaborate with OVIYA on Dravidian visual style implementation
- Collaborate with VAJRA on scene architecture and asmdef boundaries

**Current Status:** 🔴 START (Jun 17 target)
**Key Deliverables:** 3 Unity scenes, UI prefab library, transition system

---

### ARIVU — ML Integration
**Reports to:** VAJRA (technical)
**Role:** ONNX model deployment, AI inference pipeline, latency optimization

**Responsibilities:**
- Deploy Yazh 30K model into Unity via Barracuda ONNX runtime
- Benchmark and optimize model latency (target: <150ms on mobile CPU)
- Validate ONNX model loading, tokenization, and output quality
- Manage model variants (FP32, INT8, INT4) and select per-platform
- Integrate with GameManager.cs for AI-driven pet dialogue
- Ensure offline-first operation (no runtime model downloads)

**Current Status:** 🔴 START (Jun 17 target)
**Key Deliverables:** Model integration, latency report, tokenizer validation

---

### UYIR — DevOps
**Reports to:** VAJRA (technical)
**Role:** Build environment, platform tooling, deployment pipeline

**Responsibilities:**
- Maintain build environment documentation (BUILD_ENVIRONMENT.md)
- Manage build-yazh.sh and local build tooling
- Coordinate CI/CD pipeline secrets and configuration (UNITY_LICENSE, etc.)
- Set up Android SDK/NDK and iOS/Xcode build chains when hardware available
- Monitor disk space, toolchain versions, and dependency updates
- Support THOZHAR and ARIVU with platform-specific build issues

**Current Status:** 🟡 IN_PROGRESS (Jun 17 target)
**Key Deliverables:** BUILD_ENVIRONMENT.md, build-yazh.sh, CI/CD secrets config

**Known Blockers:** No Unity Editor for armhf Linux; no macOS for iOS builds. CI/CD via GitHub Actions is the primary build path.

---

### KAAVAL — Security
**Reports to:** VAJRA (technical), ANBU (compliance)
**Role:** Security auditing, code hardening, privacy compliance

**Responsibilities:**
- Conduct security audits of all C# code (14 findings in initial audit)
- Verify ONNX model integrity (SHA-256 hash verification at load time)
- Ensure COPPA/DPDP compliance for minors' data (camera, voice, chat input)
- Review CI/CD pipeline for supply chain security
- Validate offline-first architecture (no unintended network calls)
- Re-audit after major feature additions (scenes, model integration)

**Current Status:** ✅ COMPLETE (Security audit + 4 code fixes done)
**Key Deliverables:** SECURITY_AUDIT.md, 4 code fixes in YazhInferenceManager.cs

**Open Items:** SEC-001 (ONNX hash verification), SEC-005 (AR camera consent), SEC-006 (encrypted save system)

---

### KANAKU — Go-To-Market
**Reports to:** ANBU (coordination)
**Role:** Market strategy, pricing, diaspora outreach, App Store submission

**Responsibilities:**
- Define pricing strategy (free tier + premium features)
- Create diaspora marketing plan (Tamil-speaking communities worldwide)
- Manage App Store + Play Store submission process (Week 9-12)
- Coordinate beta testing recruitment (20-50 kids, ages 6-14)
- Track competitor landscape (Tamil kids apps, pet games)
- Own the revenue path: ₹1 Crore target (Agam + Adhan + diaspora)

**Current Status:** 🟡 IN_PROGRESS (Jun 24 target) — LICENSING LOCKED
**Key Deliverables:** Marketing strategy, pricing doc, App Store submission plan

**Key Decision:** Corpus licensing — Agam-private strategy with 80/20 split (open + premium), executed Jun 15.

---

### OVIYA — Art Direction
**Reports to:** ANBU (coordination), THOZHAR (implementation)
**Role:** Visual identity, Dravidian design language, asset specification

**Responsibilities:**
- Define the Dravidian visual style (colors, patterns, motifs, typography)
- Create asset specification document for all game assets
- Design pet characters (Kuruvi, Maan, Yanai, Pulliruvi) with cultural authenticity
- Design biome environments (Oorru, Kulaathanku, Karai Parai)
- Ensure art direction supports the 6-14 age group
- Review THOZHAR's scene implementations for visual consistency

**Current Status:** 🔴 START (Jun 20 target)
**Key Deliverables:** ART_DIRECTION.md, asset spec, character designs

---

### ANBU — Team Coordination
**Reports to:** Project Owner (Yazhi)
**Role:** Sprint planning, blocker escalation, cross-team communication, progress tracking

**Responsibilities:**
- Assign and track all 8 production roles (this document)
- Run daily standup summaries and weekly sprint reviews
- Escalate blockers to Project Owner (Yazhi) within 24 hours
- Maintain tasks.md master tracker with current statuses
- Coordinate inter-team dependencies (e.g., THOZHAR needs OVIYA's art spec before scene styling)
- Manage the 12-week sprint schedule and milestone tracking
- Write daily memory logs and ensure knowledge persistence across rotations

**Current Status:** ✅ COMPLETE (Team assignments done)
**Key Deliverables:** TEAM.md, daily memory logs, tasks.md updates

---

## 2. Reporting Structure

```
Project Owner (Yazhi)
├── VAJRA (Tech Lead)
│   ├── THOZHAR (UI Designer) — technical guidance
│   ├── ARIVU (ML Integration) — technical guidance
│   ├── UYIR (DevOps) — technical guidance
│   └── KAAVAL (Security) — technical guidance
└── ANBU (Team Coord)
    ├── KANAKU (Go-To-Market) — coordination
    └── OVIYA (Art Direction) — coordination
```

**Key principle:** VAJRA owns all technical decisions. ANBU owns all coordination, scheduling, and cross-team communication. KAAVAL reports to VAJRA on technical security matters and to ANBU on compliance matters.

---

## 3. Communication Protocol

### Daily Standups
- **Format:** Each agent posts a daily summary to `memory/YYYY-MM-DD.md`
- **Content:** What was done today, what's planned tomorrow, any blockers
- **ANBU responsibility:** Read all daily logs, update tasks.md, escalate blockers

### Blocker Escalation
1. **Level 1:** Agent raises blocker in daily log → ANBU reviews within 24 hours
2. **Level 2:** ANBU escalates to VAJRA (technical) or Project Owner (strategic)
3. **Level 3:** Project Owner makes final decision, documents in tasks.md
4. **Rule:** No blocker sits unaddressed for more than 48 hours

### Cross-Team Dependencies
| Dependency | From | To | Status |
|------------|------|----|--------|
| Art spec → Scene styling | OVIYA | THOZHAR | 🔴 Not started |
| Model integration → GameManager | ARIVU | VAJRA | 🔴 Not started |
| Scene architecture → UI implementation | VAJRA | THOZHAR | 🔴 Not started |
| Security re-audit → Post-feature | KAAVAL | All | ⏳ After Week 4 |
| Build env → CI/CD testing | UYIR | VAJRA | 🟡 In progress |

### File Conventions
- Architecture decisions → `ARCHITECTURE_AUDIT.yz`
- Security findings → `SECURITY_AUDIT.md`
- Build docs → `BUILD_ENVIRONMENT.md`
- Art direction → `ART_DIRECTION.md`
- Daily work → `memory/YYYY-MM-DD.md`
- Task status → `src/data/tasks.md`

---

## 4. Sprint Schedule

### Weeks 1–4: Foundation (Jun 13 – Jul 11)
**Goal:** Playable prototype (Kuruvi pet, basic dialogue, iOS/Android build)

| Week | Focus | Key Deliverables |
|------|-------|-----------------|
| 1 (Jun 13-19) | Team setup, architecture finalization | TEAM.md, CI/CD live, scenes started |
| 2 (Jun 20-26) | Core scene implementation | MainMenu + PetSelection scenes functional |
| 3 (Jun 27-Jul 3) | Model integration + AI dialogue | Yazh 30K loaded, latency <150ms |
| 4 (Jul 4-11) | Vertical slice complete | BiomeArena playable, Kuruvi pet interactive |

**Milestone:** Playable prototype with Kuruvi pet, basic Tamil dialogue, builds on iOS/Android

**Dependencies:**
- OVIYA must deliver art spec by end of Week 2 for THOZHAR to style scenes in Week 3
- ARIVU must validate model loading by Week 3 for VAJRA to integrate with GameManager
- UYIR must have CI/CD secrets configured by Week 4 for automated builds

---

### Weeks 5–8: Expansion (Jul 11 – Aug 8)
**Goal:** Feature-complete game (full 7-day loop, all pets, all biomes)

| Week | Focus | Key Deliverables |
|------|-------|-----------------|
| 5 (Jul 11-17) | Pet expansion | Maan + Yanai pets implemented |
| 6 (Jul 18-24) | Biome expansion | Oorru + Kulaathanku biomes |
| 7 (Jul 25-31) | Voice + phrases | 500+ Tamil phrases recorded and integrated |
| 8 (Aug 1-8) | Survival mechanics | Full 7-day loop, resource management |

**Milestone:** Feature-complete game with all 4 pets, 4 biomes, voice, survival mechanics

**Dependencies:**
- THOZHAR needs all pet/biome art assets from OVIYA by Week 5
- ARIVU needs voice data pipeline ready by Week 7
- KANAKU needs beta tester recruitment plan by Week 6

---

### Weeks 9–12: Polish & Ship (Aug 8 – Sep 5)
**Goal:** Live on iOS App Store + Android Play Store

| Week | Focus | Key Deliverables |
|------|-------|-----------------|
| 9 (Aug 8-14) | Cross-platform optimization | Performance targets met on both platforms |
| 10 (Aug 15-21) | Beta testing | 20-50 kids testing, feedback collected |
| 11 (Aug 22-28) | Store submission prep | Screenshots, descriptions, privacy policy |
| 12 (Aug 29-Sep 5) | Launch | Live on App Store + Play Store |

**Milestone:** Live on iOS App Store + Android Play Store

**Dependencies:**
- KAAVAL must complete final security/privacy audit by Week 9
- KANAKU must finalize pricing and store listings by Week 10
- UYIR must ensure both platform builds pass CI/CD by Week 9
- OVIYA must deliver store assets (screenshots, icons) by Week 10

---

## 5. Risk Register

| Risk | Owner | Impact | Mitigation |
|------|-------|--------|------------|
| No Unity Editor on armhf Linux | UYIR | HIGH | Use GitHub Actions CI/CD as primary build path |
| No macOS for iOS builds | UYIR | HIGH | Acquire Mac Mini M1+ or use cloud macOS |
| Barracuda ONNX operator incompatibility | ARIVU | HIGH | Validate all operators early (Week 2) |
| Model latency >150ms on mobile | ARIVU | MEDIUM | Use INT8/INT4 quantization, benchmark early |
| ARSetup.cs enum conflicts | VAJRA | MEDIUM | Fix once AR Foundation package version known |
| OVIYA art spec delays | OVIYA/ANBU | MEDIUM | THOZHAR can build unstyled scenes first |
| COPPA/DPDP non-compliance | KAAVAL | HIGH | Implement parental consent flow before beta |
| Disk space exhaustion on Zorba | UYIR | LOW | Monitor usage, clean build artifacts |

---

## 6. Success Criteria

**Week 4 Gate:** Playable prototype with Kuruvi pet, basic Tamil dialogue, builds on both platforms
**Week 8 Gate:** All 4 pets, 4 biomes, 500+ phrases, full 7-day loop
**Week 12 Gate:** Live on App Store + Play Store, 20-50 beta testers completed

**Quality Targets:**
- Model latency: <150ms on mobile CPU
- App size: <200MB (with INT8 model)
- Crash rate: <1% (post-launch)
- Security: Zero CRITICAL findings at launch

---

*YAZH-UNITY is Yazhi's P0 project. All 8 agents are committed to the full 12-week sprint. ANBU coordinates. VAJRA leads technically. Everyone ships.*
