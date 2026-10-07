# Next Actions: Yazh learning pet (Q4 2026)

> This file mirrors the Yazhi Q4 2026 launch plan as of **8 Oct 2026**.

| | |
|---|---|
| Federal | **Yazh** |
| Goals | Guard-kid guardrail module open-sourced, ≥98% pass on the kid suite · one-command model rollback |
| Q4 launch | **Closed beta for 15–20 founding families, 19 Dec 2026**. No public store listing |
| What v1 is | A bilingual (Tamil–English) **learning pet**: rhymes, word-of-the-day, one STEM track, pet memory and growth stages, with the Guard-kid safety layer and a parent daily cap |
| Out of v1 | Storyteller submissions, third language, payments, child voice input, public store listing |
| Data kept | First name and age band only |

## Gates: no child uses Yazh before all of these pass

1. Verified parental consent flow in place
2. Kid red-team suite ≥98% pass
3. Zero open safety bugs

## Plan by milestone

Milestones: Safety first → Core loop → Guard → Founding families → Closed beta → Beta operations.

| Target date | Milestone | What |
|---|---|---|
| 1 Nov 2026 | Safety first | Build the verified parental consent flow |
| 8 Nov 2026 | Core loop | Build pet memory and growth stages in Yazh |
| 12 Nov 2026 | Core loop | Add native-reviewed rhymes and word-of-the-day to Yazh |
| 18 Nov 2026 | Core loop | Build one STEM track (numbers or plants) in Yazh |
| 22 Nov 2026 | Core loop | Add Tamil–English code-switching and a parent daily cap to Yazh |
| 28 Nov 2026 | Guard | Build Guard-kid input/output classifiers and hard rules in code |
| 28 Nov 2026 | Guard | Build the distress handler that flags the parent |
| 5 Dec 2026 | Guard | Run the kid red-team suite and reach ≥98% pass |
| 5 Dec 2026 | Founding families | Recruit 15–20 founding families for the Yazh closed beta |
| 6 Dec 2026 | Guard | Build one-command model rollback for Yazh |
| 12 Dec 2026 | Founding families | Hold a consent and onboarding call with each founding family |
| 12 Dec 2026 | Founding families | Build parent access to Yazh conversation logs |
| 19 Dec 2026 | Closed beta | Open the Yazh closed beta once every safety gate has passed |
| 26 Dec 2026 | Beta operations | Send the weekly parent summary to founding families |
| 31 Dec 2026 | Beta operations | Review flagged Yazh conversations daily and log each review |
| 31 Dec 2026 | Beta operations | Open-source the Guard-kid module, keeping the live blocklist private |
| 31 Dec 2026 | Beta operations | Measure Yazh D7 retention |
| TBD | Core loop | Add CPU fallback to the Yazh inference worker for min-spec devices |

## Gaps between this repo and the Q4 plan

- **No real model yet.** The bundled ONNX models and tokenizer are **placeholders**. PR
  [#23](https://github.com/yazhi-lem/yazh-unity/pull/23) (open, awaiting merge) documents this. A trained, red-teamed model is
  needed before the 19 Dec beta. Which model Yazh runs (Adhan Kutty, a separate Yazh model, or a served model) is **unclear**.
- **Game direction.** The previous version of this file planned an **endless runner** across five Tiṇai biomes with four runner
  pets, and an App Store / Play Store launch in December. The Q4 v1 is a learning pet in a closed beta, with no store listing.
  Whether runner code stays in the build is **unclear**.

## Immediate next actions

1. Record the runner-vs-learning-pet decision and which Unity scenes stay.
2. Build the verified parental consent flow (due 1 Nov).
3. Start pet memory and growth stages (due 8 Nov).
4. Decide whether the CPU fallback for min-spec devices (PR [#7](https://github.com/yazhi-lem/yazh-unity/pull/7)) lands in v1.
5. Work through the mirrored GitHub issues (#12, #14, #17, #19, #20, #22) in milestone order.
