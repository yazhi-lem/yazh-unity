# Tinai Endless Runner — Design & Pivot Notes

**Date:** 2026-07-14
**Status:** Playable prototype (code-first, primitive art)
**Pivot:** Yazh Unity core loop moves from 7-day XR survival to a
Subway Surfers-style endless runner. The pet stays the heart of the game —
now it *runs*.

---

## Concept

The child's Yazh pet runs endlessly through the **five tinai** — the five
landscapes of Sangam literature — which cycle forever in classical order:

| # | Tinai | தமிழ் | Landscape | Collectible | Twist |
|---|-------|-------|-----------|-------------|-------|
| 1 | Kurinji | குறிஞ்சி | Mountains | தேன் (honey) | baseline |
| 2 | Mullai | முல்லை | Forest / pasture | முல்லை மலர் (jasmine) | denser obstacles |
| 3 | Marutham | மருதம் | Farmland / riverine | நெல் (paddy) | +5% speed |
| 4 | Neithal | நெய்தல் | Seashore | முத்து (pearl) | +10% speed |
| 5 | Palai | பாலை | Arid wasteland | நீர் துளி (water drop) | +20% speed, most obstacles, scarce pickups — scarcity *is* the theme |

Each tinai stretch is **400 m** (`TinaiSystem.TinaiLengthMeters`). Distance
maps deterministically to tinai, so segments recycling to the front of the
track are always themed for where they will sit on the endless run.

## The runner is the pet

The pet chosen in PetSelection is the runner, and each pet plays differently:

| Pet | தமிழ் | Ability |
|-----|-------|---------|
| Kuruvi (sparrow) | குருவி | **Double jump** |
| Maan (deer) | மான் | **High, floaty jump** (+20% jump, −15% gravity) |
| Yanai (elephant) | யானை | **Smash** through breakable low barriers (slower lane change) |
| Pulliruvi (dove) | புள்ளிரூவி | **Song magnet** — collectibles drift to her |

### YazhLife stays load-bearing

- Finishing any run calls `GameManager.PlayWithPet()` → mood + bond rise.
  Running *is* caring for the pet.
- A tired pet (energy < 20) starts runs 20% slower — kids learn to let the
  pet rest.

## Controls

- **Swipe** left/right = lane change, up = jump, down = slide (mobile)
- **Arrows / WASD / Space** on desktop
- Works with both the new Input System and legacy input
  (`#if ENABLE_INPUT_SYSTEM`).

## Track rules

- 3 lanes at x = −2 / 0 / +2, segments 30 m long, 7 pooled segments.
- Obstacle rows always leave **at least one open lane** (fair by
  construction).
- Obstacle kinds: **LowBarrier** (jump / Yanai-smash), **HighBarrier**
  (slide), **FullBlock** (dodge).
- The **world moves, the runner stays near the origin** — no float
  precision drift on long runs.
- Speed ramps `8 → 20 m/s` over the run; obstacle density also ramps over
  the first 2000 m.
- Score = meters run + collectibles × 10 (+5 per smash).

## Architecture (`Assets/Scripts/Runner/`)

| File | Role |
|------|------|
| `TinaiSystem.cs` | The five tinai themes + distance→tinai mapping |
| `TinaiSegment.cs` | One pooled track segment, built from primitives |
| `EndlessTerrainGenerator.cs` | Segment pool, recycling, tinai transitions, sky/fog ambience |
| `YazhRunnerController.cs` | Lanes, jump, slide, swipe input, pet abilities, collisions |
| `RunnerGameManager.cs` | Run state, speed ramp, score, game over, YazhLife rewards |
| `RunnerHUD.cs` | Tamil-first HUD (score, distance, tinai banner, game over) |
| `RunnerBootstrap.cs` | One-component scene assembly |

## How to play it in the editor

1. Create a new empty scene, save as `Assets/Scenes/TinaiRunner.unity`.
2. Add an empty GameObject; attach **RunnerBootstrap**.
3. Press Play. (Optionally set *Pet Type Override* to `kuruvi` / `maan` /
   `yanai` / `pulliruvi`.)

`GameManager.StartEndlessRun()` loads the `TinaiRunner` scene by name —
add the scene to Build Settings and wire an "ஓடு" (Run) button in the main
menu to it.

## What we kept from the survival build

- Pet selection, personalities, YazhLife, UIStyles palette, Tamil-first UI,
  AI dialogue stack (chat with your pet between runs), COPPA scaffolding.
- `SurvivalSystem` remains in the tree but is no longer the core loop;
  candidates for its future: post-run camp/meta layer, or removal.

## Next steps

- Real pet models + run/jump/slide animations (replace placeholder capsule
  body in `RunnerBootstrap`).
- Tinai-specific decor meshes (crags, jasmine, paddy, palms, thorn trees).
- Missions ("collect 30 முத்து in Neithal"), daily runs, revive with தேன்.
- Pet voice lines during runs via the existing dialogue/TTS stack.
