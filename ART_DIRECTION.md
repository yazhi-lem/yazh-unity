# YAZH-UNITY — ART DIRECTION DOCUMENT
# Dravidian Geometric Synthesis | Tamil-First | Kids 6-14
# Status: COMPLETE | Date: 2026-06-18 | Author: OVIYA (Rotation 25)

═══════════════════════════════════════════════════════════════════════════════
SECTION 1: VISUAL PHILOSOPHY
═══════════════════════════════════════════════════════════════════════════════

## 1.1 Design Language: Vahana UI — Dravidian Geometric Synthesis

Yazh-Unity uses the Vahana UI design system: geometric synthesis rooted in
Dravidian proportions, NOT western design tropes. Every visual element traces
back to Tamil material culture — temple architecture, Sangam-era art, textile
patterns, and the Tamil landscape.

Core principles:
- GEOMETRIC PURITY: Circles, diamonds, stepped pyramids, kolam-inspired grids
- TAMIL PROPORTIONS: 1:1.618 (golden ratio) applied to Dravidian temple vimanam
- FLAT-DEPTH HYBRID: 2D geometric precision with subtle 3D material warmth
- NO WESTERN TROPES: No rounded-corner cards, no drop shadows, no glassmorphism
- CULTURAL GROUNDING: Every color, pattern, and motif has Tamil significance

## 1.2 Color Palette — "Sangam Earth"

Primary palette (60% of UI):

  SANGAM OCHRE      #C8840A  — Turmeric gold, primary accent
  TEMPLE RED        #8B1A1A  — Saffron/sindoor red, CTA buttons
  DEEP BLACK        #1A1A1A  — Text, primary UI elements
  PALMYRA WHITE     #F5F0E8  — Background, parchment tone
  RIVER BLUE        #2E5090  — Water, info states, links

Secondary palette (30% of UI):

  NEEM GREEN        #2D5A27  — Nature, health, positive states
  COCONUT BROWN     #6B4226  — Earth, grounding, secondary text
  MARIGOLD ORANGE   #E8941A  — Warmth, achievement, celebration
  INDIGO            #3D2B56  — Night sky, mystery, premium states

Tertiary accents (10% of UI):

  TURMERIC YELLOW   #F0C040  — Highlights, sparkle effects
  LOTUS PINK        #D4738A  — Emotional warmth, celebration
  JADE              #00A86B  — Success states, completion
  PEACOCK BLUE      #004D6B  — Special items, rare finds

Biome-specific palettes:

  Sangam Kaadu:   Neem Green + River Blue + Coconut Brown (forest)
  Oorru:          Sangam Ochre + Marigold + Temple Red (village warmth)
  Kulaathanku:    Indigo + Peacock Blue + Palmyra White (mountain mist)
  Karai Parai:    River Blue + Turmeric Yellow + Coconut Brown (coast)

## 1.3 Typography

Primary: Noto Sans Tamil (Google Fonts, OFL license)
  - Display: Bold 700 (headers, titles)
  - Body: Regular 400 (dialogue, descriptions)
  - UI: Medium 500 (buttons, labels)

Secondary: System fallback for English accessibility text
  - SF Pro (iOS) / Roboto (Android) — only for English fallback

Tamil script sizing rule: Minimum 16sp for body text. Tamil characters are
complex and need larger sizes for legibility. Line height: 1.6x for Tamil.

## 1.4 Geometric Motifs

Kolam Grid: Dot-based grid patterns used as background textures and loading
  animations. 4x4, 8x8, and 16x16 dot grids for different densities.

Vimana Stepped Pyramid: Used for progress bars, level indicators, and
  achievement tiers. Each step = one level of progression.

Temple Gopuram Arch: Used for modal frames, dialogue boxes, and card
  containers. NOT rounded rectangles — stepped arch shapes.

Paisley (Mango/Konnai): Used for decorative accents, button backgrounds, and
  selection indicators. Stylized, geometric — NOT ornate.

Diamond Lattice: Used for inventory grids, achievement layouts, and biome
  selection cards. Interlocking diamond pattern from Tamil textile tradition.

═══════════════════════════════════════════════════════════════════════════════
SECTION 2: PET DESIGNS — STYLIZED, KID-FRIENDLY
═══════════════════════════════════════════════════════════════════════════════

## 2.1 Design Rules (All Pets)

- STYLIZED, NOT REALISTIC: 2.5D illustrated look, geometric forms
- LARGE EYES: 30-40% of face area — expressive, kid-friendly
- SOFT EDGES: No sharp angles — all forms rounded but geometric
- CLEAR SILHOUETTES: Instantly recognizable at 64x64px
- EMOTIONAL RANGE: 4 core expressions (happy, curious, tired, celebrate)
- ANIMATION-READY: Separate body parts for rigging (limbs, ears, tail, etc.)
- CULTURAL MARKERS: Each pet has a Tamil cultural accessory (see below)

## 2.2 Kuruvi (Sparrow) — குருவி

Personality: Curious, fast, energetic
Role: Exploration bonus, rapid dialogue

Design:
  - Body: Compact oval, ~8K triangles
  - Color: Warm brown (#6B4226) with ochre belly (#C8840A)
  - Eyes: Large, round, black with white highlight — 40% of face
  - Beak: Small orange triangle, rigged for speech animation
  - Tail: 4 articulated feathers, fan out on excitement
  - Wings: Fold/unfold mechanic, flutter during speech
  - Cultural marker: Tiny kolam-pattern patch on chest
  - Size reference: 1/10th of child character height (small, fits on shoulder)

Expressions:
  Happy: Wings spread, head tilt up, beak open (chirping)
  Curious: Head tilt 30 degrees, one eye larger, body lean forward
  Tired: Wings droop, eyes half-closed, feathers ruffled
  Celebrate: Full wing spread, rapid bobbing, tail fan

Animation priorities:
  1. Idle: Subtle breathing, head turns, feather ruffle
  2. Chirp: Beak open/close sync with audio, head bob
  3. Wing flutter: Fast wing cycle during speech
  4. Hop: Small jumps when excited
  5. Perch: Sitting pose for rest/idle states

## 2.3 Maan (Deer) — மான்

Personality: Thoughtful, graceful, observant
Role: Survival strategy, tracking

Design:
  - Body: Elegant quadruped, ~12K triangles
  - Color: Tawny gold (#C8840A) with white spots (#F5F0E8)
  - Eyes: Large, deep brown (#3D2B56), soulful — tracks child position
  - Antlers: 2 symmetric racks, small (kid-friendly), tilt for emotion
  - Ears: Large, rotate independently (attention direction)
  - Legs: Slim, IK solver for terrain adaptation
  - Cultural marker: Marigold flower garland around neck
  - Size reference: 1/3rd of child character height (companion height)

Expressions:
  Happy: Prance pose, ears forward, tail up
  Curious: One ear forward, one back, head tilt, alert stance
  Tired: Head down, ears back, slow blink
  Celebrate: Leap pose, antlers raised, all legs off ground

Animation priorities:
  1. Walk: Graceful 4-beat gait, slight head bob
  2. Ear swivel: Independent ear rotation (listening mechanic)
  3. Head tilt: 15-degree tilt for curiosity/questioning
  4. Prance: Light, bouncy walk for happy state
  5. Grazing: Head down, slow chewing, tail swish

## 2.4 Yanai (Elephant) — யானை

Personality: Wise, patient, nurturing
Role: Memory of past decisions, building

Design:
  - Body: Rounded, friendly, ~15K triangles
  - Color: Warm grey (#808080) with turmeric-smeared forehead (#F0C040)
  - Eyes: Small relative to body but expressive, wise brow ridges
  - Trunk: 12-bone IK chain, expressive gestures (sway, curl, point)
  - Ears: Fan-shaped, animate for cooling/emotion (gentle flap)
  - Tusks: Small, smooth, rounded — child-safe, non-aggressive
  - Cultural marker: Temple bell on red cord around neck
  - Size reference: 2x child character height (large, protective presence)

Expressions:
  Happy: Trunk curl up, ears flap, head tilt
  Curious: Trunk extended forward, ears forward, slight head tilt
  Tired: Trunk droop, ears still, slow blink, sitting pose
  Celebrate: Trunk raised high, ears full spread, trumpet pose

Animation priorities:
  1. Trunk sway: Idle trunk movement, gentle pendulum
  2. Ear flap: Slow, rhythmic ear movement
  3. Head tilt: Slow, wise head tilt (sagacity)
  4. Slow walk: Weighted, deliberate steps
  5. Spray/dust: Trunk raise + particle effect

## 2.5 Pulliruvi (Spotted Dove/Tiger) — புள்ளிரூவி

NOTE: "Pulliruvi" in this context = spotted cat (tiger cub), not dove.
The COMPLETION_SUMMARY.md lists Pulliruvi as "cat" with Playful personality.

Personality: Playful, independent, unpredictable
Role: Rhythm tasks, creative challenges

Design:
  - Body: Sleek cub form, ~9K triangles
  - Color: Orange (#E8941A) with dark stripes (#1A1A1A) and white belly
  - Eyes: Large, amber (#C8840A), color-shifting for mood (green=calm, red=excited)
  - Spots: Distinctive pulli (dots) pattern — stylized, geometric circles
  - Tail: Long, curved, expressive (puff when excited, droop when sad)
  - Ears: Pointed, rotate for emotion
  - Cultural marker: Peacock feather tucked behind ear
  - Size reference: 1/4th of child character height (cub, fits in arms)

Expressions:
  Happy: Pounce pose, tail up and curved, eyes wide
  Curious: Crouched stalking pose, eyes locked forward, tail low
  Tired: Curled up, tail wrapped, eyes half-closed
  Celebrate: Back flip or spin, tail puffed, eyes bright

Animation priorities:
  1. Pounce: Crouch → leap → land cycle
  2. Head bob: Musical rhythm sync (for rhythm challenges)
  3. Tail swish: Emotional state indicator
  4. Stalk: Low crouch walk for focused/curious state
  5. Roll: Playful roll for celebration

═══════════════════════════════════════════════════════════════════════════════
SECTION 3: BIOME DESIGNS
═══════════════════════════════════════════════════════════════════════════════

## 3.1 Oorru (Village) — ஊர்ரு

Theme: Tamil agricultural village, warm and welcoming
Mood: Community, harvest, daily life

Environment:
  - Ground: Red earth (#8B4513) with patches of green grass
  - Sky: Warm blue (#2E5090) with white clouds
  - Structures: Tiled-roof houses, granary, well, fence sections
  - Vegetation: Coconut palms, banana trees, marigold patches
  - Water: Central well with bucket mechanism, small irrigation channels
  - NPCs: Farmer (plowing), water carrier (bucket), weaver (loom)

Key structures:
  1. Tiled House (0-2 story, modular)
     - Terracotta roof tiles, white-walled
     - Door: Wooden, with kolam pattern carved
     - PBR: Tile roughness 0.8, wall roughness 0.6
  
  2. Granary (வாயக்கட்டு)
     - Cylindrical, raised on stones
     - Woven palm walls, conical roof
     - PBR: Organic roughness 0.9
  
  3. Temple Gopuram (background)
     - 3-tier stepped pyramid
     - Colorful (not white — Tamil temple colors)
     - Visible from all parts of biome (landmark)
  
  4. Well (functional)
     - Stone circular wall, wooden beam, rope+bucket
     - Interactive: Pull rope animation, water collect

Color palette: Sangam Ochre + Marigold + Coconut Brown + Neem Green

## 3.2 Kulaathanku (Farmland/Mountain Path) — குலாதங்கு

Theme: Highland terraces, misty, sparse resources
Mood: Challenge, perseverance, discovery

Environment:
  - Terrain: Steep slopes, terraced fields, rocky outcroppings
  - Sky: Misty grey-blue (#6B7B8D) with fog layers
  - Vegetation: Sparse — hardy shrubs, medicinal herbs, wild flowers
  - Structures: Stone steps, hermit cave entrance, temple ruins
  - Hazards: Crevasses, unstable slopes, wind zones

Key structures:
  1. Terraced Fields (படுக்கை)
     - Stepped rice paddies going up the mountain
     - Water flow between levels (animated)
     - PBR: Wet earth roughness 0.4, water reflective
  
  2. Hermit's Cave
     - Dark entrance, warm interior glow
     - Interior: Simple platform, oil lamp, scrolls
     - NPC: Hermit (storytelling hub)
  
  3. Stone Steps
     - Worn, weathered granite
     - Broken sections (navigation challenge)
     - Moss patches for visual interest
  
  4. Temple Ruins
     - Partial columns, broken gopuram base
     - Ancient Tamil inscriptions on stones
     - Collectible: Ancient text fragments

Color palette: Indigo + Peacock Blue + Coconut Brown + Neem Green

## 3.3 Karai Parai (Coastal Cliffs) — கரை பாறை

Theme: Ocean-side, fishing village, dramatic cliffs
Mood: Adventure, monsoon survival, maritime culture

Environment:
  - Water: Ocean with wave animation, foam line, tide mechanic
  - Shore: Sandy beach, rocky cliffs, salt ponds
  - Sky: Dynamic — clear to stormy (weather system integration)
  - Structures: Fishing hut, boat, salt ponds, pier/jetty
  - Flora: Coconut palms (windproof), seaweed, mangroves
  - Fauna: Crabs (small, scurrying), seabirds (flying), fish (splashes)

Key structures:
  1. Fishing Hut (woven palm)
     - Open-sided, palm-thatch roof
     - Interior: Fishing net, oars, small fire pit
     - PBR: Palm roughness 0.9, weathered
  
  2. Boat (floating)
     - Traditional Tamil fishing boat (vallam)
     - Tilts with waves (physics)
     - Can be used for exploration
  
  3. Salt Ponds
     - Shallow, crystalline water
     - White salt deposits on edges
     - Collectible: Salt crystals
  
  4. Pier/Jetty
     - Wooden, weathered, extends into water
     - Posts for boat tying
     - Seagull perches

Color palette: River Blue + Turmeric Yellow + Coconut Brown + Palmyra White

## 3.4 Sangam Kaadu (Forest) — சங்கம் காடு

Theme: Dense Tamil forest, Thirukkural-inspired wisdom
Mood: Mystery, learning, nature immersion

Environment:
  - Trees: Palm, banyan, neem, coconut, fig (5 variants)
  - Understory: Ferns, mushrooms, vines, flowers (jasmine, marigold)
  - Water: River with flow animation, rope bridge
  - Structures: Thirukkural stone markers, wooden signs, hidden cave
  - Fauna: Squirrels, monitor lizards (உழவிளான்), birds (non-interactive)

Key structures:
  1. Trees (5 variants, 3K-8K tris each)
     - Palm: Tall, single trunk, fan leaves
     - Banyan: Multiple trunks, aerial roots
     - Neem: Dense canopy, small leaves
     - Coconut: Curved trunk, clustered coconuts
     - Fig: Wide canopy, exposed roots
     - Billboard LOD for distance
  
  2. Thirukkural Stone Markers
     - Carved stone pillars with Tamil script
     - Glowing inscription (readable by child)
     - Interactive: Touch to hear Thirukkural verse
  
  3. River
     - Water shader with flow animation
     - Rope bridge crossing (sway mechanic)
     - Collectible: Water vessels on banks
  
  4. Hidden Cave
     - Revealed on approach (particle effect)
     - Interior: Dark, resource cache inside

Color palette: Neem Green + River Blue + Coconut Brown + Deep Black

═══════════════════════════════════════════════════════════════════════════════
SECTION 4: UI STYLE — BRUTALIST TAMIL-FIRST
═══════════════════════════════════════════════════════════════════════════════

## 4.1 Design Principles

- BRUTALIST: Raw, honest, no decorative excess — geometric precision
- TAMIL-FIRST: All UI text defaults to Tamil; English is fallback
- NO WESTERN TROPES: No rounded corners, no gradients, no shadows
- GRID-BASED: Kolam-dot grid system for all layouts
- HIGH CONTRAST: Deep black on palmyra white (or reverse for night mode)
- ACCESSIBLE: Large touch targets (min 48x48dp), high contrast ratios

## 4.2 UI Components

### Buttons
  - Shape: Gopuram arch (stepped pyramid top, rectangular body)
  - Color: Temple Red (#8B1A1A) for primary, Sangam Ochre (#C8840A) for secondary
  - Text: Noto Sans Tamil Bold, 18sp, Palmyra White
  - States: Normal / Pressed (darker shade) / Disabled (grey)
  - No rounded corners — sharp geometric edges

### Cards / Panels
  - Shape: Diamond lattice border, rectangular body
  - Background: Palmyra White (#F5F0E8) with subtle kolam pattern overlay
  - Border: 2px Deep Black (#1A1A1A)
  - Header: Temple Red background with Tamil title

### Dialogue Bubbles
  - Shape: Geometric speech bubble with pointed tail
  - Child messages: River Blue (#2E5090) background, right-aligned
  - Pet responses: Neem Green (#2D5A27) background, left-aligned
  - Text: Noto Sans Tamil Regular, 16sp, Deep Black
  - Pet name: Bold, with pet icon (16x16px)

### Progress Bars
  - Shape: Vimana stepped pyramid — each segment fills with progress
  - Fill color: Sangam Ochre (#C8840A)
  - Background: Coconut Brown (#6B4226) at 30% opacity
  - Label: Tamil percentage text (e.g., "முடிந்தது 70%")

### Resource Icons
  - Grid: Diamond lattice layout
  - Each resource: Icon (256x256px) + count (Tamil numerals)
  - Water: Drop icon, River Blue
  - Food: Rice bowl icon, Sangam Ochre
  - Shelter: House icon, Coconut Brown
  - Herb: Leaf icon, Neem Green

### Navigation
  - Bottom bar: 5 action buttons (Talk, Explore, Build, Rest, Achievements)
  - Shape: Temple Gopuram arch frame
  - Active state: Temple Red highlight
  - Inactive state: Deep Black outline only
  - Icons: 64x64px SVG, geometric line-art style

## 4.3 Screen Layouts

### Main Menu
  - Background: Animated kolam grid on Palmyra White
  - Center: Yazh logo (geometric, Tamil script)
  - Buttons: Vertical stack, Gopuram-arch buttons
  - Bottom: Language toggle (தமிழ் | English), Settings gear

### Pet Selection
  - Layout: 2x2 diamond grid (4 pets)
  - Each card: Pet portrait (2048x2048px) + Tamil name + description
  - Selected: Temple Red border, slight scale-up animation
  - Locked: Grey overlay with lock icon

### Biome Arena (AR Viewport)
  - Top-left: Pet name + day streak (Tamil)
  - Top-right: Health bar + resource count
  - Center: AR viewport (pet in environment)
  - Bottom: Action buttons (5) + dialogue input
  - Dialogue: Slide-up panel, chat bubbles

### Settings
  - Language: தமிழ் (default) / English
  - Difficulty: எளிய / நடுத்தர / கடினம் (Easy/Medium/Hard)
  - Volume: Slider (geometric, stepped)
  - Parental controls: PIN-protected section

═══════════════════════════════════════════════════════════════════════════════
SECTION 5: ASSET SPECIFICATION
═══════════════════════════════════════════════════════════════════════════════

## 5.1 Resolution Standards

Pet Models:
  - High LOD:   15K-20K triangles (close-up, high-end devices)
  - Medium LOD: 8K-12K triangles (standard)
  - Low LOD:    3K-5K triangles (low-end devices, distance)
  - Textures:   2048x2048 PBR (Albedo, Normal, Roughness, Metallic)
  - Format:     USDZ (iOS) + GLB (Android)

Environment Assets:
  - Hero objects (well, gopuram, etc.): 5K-10K tris, 2048x2048 textures
  - Standard objects (trees, houses): 3K-8K tris, 1024x1024 textures
  - Background objects: 1K-3K tris, 512x512 textures
  - Billboard LOD: 2D sprite, 512x512px

UI Assets:
  - Pet portraits: 2048x2048px, PNG + WEBP
  - Biome key art: 3840x2160px (16:9), PNG + WEBP
  - Achievement badges: 512x512px, PNG (transparent)
  - UI icons: 256x256px, SVG source + PNG export
  - Screen mockups: 1920x1080px, PNG @ 2x

Audio:
  - Voice: 44.1kHz, 24-bit WAV (source), OGG (runtime)
  - SFX: 44.1kHz, 16-bit WAV (source), OGG (runtime)
  - Music: 44.1kHz, 16-bit WAV (source), OGG (runtime)
  - Normalize: -18dBFS (voice), -12dBFS (music), -6dBFS (SFX)

## 5.2 File Formats

3D Models:
  - Source: .blend (Blender) — keep for editing
  - Export: .fbx (Unity import) + .glb (Android) + .usdz (iOS)
  - Rig: Humanoid-compatible or Generic (Unity Mecanim)

Textures:
  - Source: .psd / .krita — keep for editing
  - Export: .png (lossless) for UI, .jpg (quality 90) for 3D textures
  - PBR maps: Albedo, Normal, Roughness, Metallic, AO

Audio:
  - Source: .wav (uncompressed)
  - Runtime: .ogg (compressed, mobile-friendly)

UI:
  - Source: .fig (Figma) / .xd (Adobe XD)
  - Export: .png @ 1x, 2x, 3x + .svg for icons

## 5.3 Naming Conventions

Format: {category}_{name}_{variant}_{resolution}.{ext}

Categories:
  pet_      — Pet models and textures
  env_      — Environment assets (biome objects)
  ui_      — UI elements (icons, badges, portraits)
  sfx_      — Sound effects
  music_    — Music tracks
  voice_    — Voice recordings
  anim_     — Animation clips
  mat_      — Materials
  shader_   — Shaders

Examples:
  pet_kuruvi_high.fbx          — Kuruvi high LOD model
  pet_kuruvi_low.fbx           — Kuruvi low LOD model
  pet_kuruvi_albedo.png        — Kuruvi albedo texture
  pet_kuruvi_portrait.png      — Kuruvi character portrait
  env_oorru_house_tiled.fbx    — Village tiled house
  env_karai_parai_boat.fbx     — Coastal boat
  ui_icon_water_256.png        — Water resource icon
  ui_badge_survival_512.png    — Survival achievement badge
  sfx_kuruvi_chirp.ogg         — Kuruvi chirp sound
  music_sangam_loop.ogg        — Survival theme
  voice_kuruvi_hello.ogg       — Kuruvi "hello" voice
  anim_kuruvi_idle.anim        — Kuruvi idle animation
  mat_earth_red.mat            — Red earth material

Tamil names in filenames: Use English transliteration (kuruvi, maan, yanai,
pulliruvi, oorru, kulaathanku, karai_parai, sangam_kaadu).

## 5.4 Directory Structure

Assets/
  Models/
    Pets/
      Kuruvi/          — FBX, textures, materials
      Maan/
      Yanai/
      Pulliruvi/
    Environment/
      Oorru/           — Village structures, props
      Kulaathanku/     — Mountain terrain, structures
      Karai Parai/     — Coastal structures, water
      Sangam Kaadu/    — Forest trees, rocks, markers
  Textures/
    Pets/              — Pet texture atlases
    Environment/       — Environment texture atlases
    UI/                — UI sprite atlases
  Materials/
    Pets/
    Environment/
    UI/
  Audio/
    Voice/
      Kuruvi/          — 150+ phrases
      Maan/
      Yanai/
      Pulliruvi/
    SFX/
      UI/              — Button clicks, notifications
      Gameplay/        — Collect, build, weather
      Ambient/         — Biome ambient loops
    Music/
      Themes/          — Challenge themes
      Biome/           — Biome-specific music
  Animations/
    Pets/
      Kuruvi/          — 20+ animation clips
      Maan/
      Yanai/
      Pulliruvi/
    Environment/       — Animated props (water, wind, etc.)
    UI/                — UI transition animations
  Prefabs/
    Pets/              — Pre-configured pet prefabs
    Environment/       — Pre-configured environment prefabs
    UI/                — Pre-configured UI prefabs
  Scenes/
    MainMenu.unity
    PetSelection.unity
    BiomeArena.unity
    SettingsMenu.unity
  Scripts/
    Core/
    AI/
    AR/
    Gameplay/
    UI/
    Editor/
  StreamingAssets/
    MLModels/          — ONNX models + tokenizer
  UI/
    Portraits/         — Pet character portraits (PNG)
    BiomeArt/          — Biome key art (PNG)
    Badges/            — Achievement badges (PNG)
    Icons/             — UI icons (SVG + PNG)

## 5.5 Asset Budget (Performance Targets)

Target devices: iPhone 12+ / Pixel 6-class Android
Target FPS: 60 (menus), 30+ (AR)

Triangle budget per frame:
  - Pet model (high LOD):     20K tris max
  - Environment (visible):    100K tris max
  - UI:                       10K tris max
  - Total:                    130K tris budget

Texture memory:
  - Pet textures:             8 MB (2048x2048 x 4 maps, compressed)
  - Environment textures:     32 MB (atlased, compressed)
  - UI textures:              8 MB (atlased)
  - Total:                    48 MB texture budget

Audio memory:
  - Voice (loaded phrases):   10 MB
  - SFX:                      5 MB
  - Music (streaming):        2 MB
  - Total:                    17 MB audio budget

═══════════════════════════════════════════════════════════════════════════════
SECTION 6: PRODUCTION WORKFLOW
═══════════════════════════════════════════════════════════════════════════════

## 6.1 Asset Pipeline

1. CONCEPT → 2D illustration (Krita/Photoshop) → Approved by OVIYA
2. MODEL → 3D model (Blender) → Export FBX → Import Unity
3. TEXTURE → PBR maps (Substance Painter/Blender) → Export PNG → Import Unity
4. RIG → Unity Mecanim rig → Test animations → Finalize
5. ANIM → Animation clips (Blender/Motion capture) → Export → Import Unity
6. AUDIO → Record (44.1kHz/24-bit) → Edit → Normalize → Export OGG → Import Unity
7. UI → Figma mockup → Approved → Export PNG/SVG → Import Unity

## 6.2 Quality Checklist (Per Asset)

Before any asset is committed:
  [ ] Follows naming convention (Section 5.3)
  [ ] Correct resolution (Section 5.1)
  [ ] Correct format (Section 5.2)
  [ ] Fits in budget (Section 5.5)
  [ ] Matches art direction (this document)
  [ ] Has all LOD levels (3D models)
  [ ] Has all expression variants (pets)
  [ ] PBR maps complete (Albedo, Normal, Roughness, Metallic)
  [ ] Animation rig tested
  [ ] No western design tropes
  [ ] Tamil cultural markers present
  [ ] Kid-friendly (no sharp edges, no aggressive features)

## 6.3 Priority Order (Week 1-4)

Week 1-2:
  1. Kuruvi model + textures (high priority — first pet)
  2. Kuruvi animations (idle, walk, eat, sleep, talk)
  3. Sangam Kaadu biome (simplified — 3 tree variants, 1 structure)
  4. UI icon set (core 20 icons: actions, resources, emotions)
  5. Pet portraits (4 pets, 1 expression each)

Week 3-4:
  6. Maan model + textures
  7. Maan animations
  8. Oorru biome (simplified)
  9. UI mockups (MainMenu, PetSelection, BiomeArena)
  10. Achievement badges (first 10)

═══════════════════════════════════════════════════════════════════════════════
SECTION 7: ANIMATION SPECIFICATIONS
═══════════════════════════════════════════════════════════════════════════════

## 7.1 Animation Standards (All Pets)

**Frame Rate:** 30 FPS (mobile-friendly, syncs with Tamil speech)
**Rig Type:** Unity Humanoid or Generic (Mecanim-compatible)
**Blend Space:** 2D blending for locomotion (walk speed, turning)
**IK Targets:** Position + Rotation (for feet, ears, trunk, etc.)

## 7.2 Core Animation Cycles

### IDLE (Looping, 2 seconds)
Duration: 60 frames (at 30 FPS)
Content: Breathing motion, subtle head turns, blinks
Facial: Eyes blink every 30-40 frames, smooth eye rotation
Key poses: T-pose base, slight weight shift
Loop: Seamless transition (frame 60 → frame 1)

### WALK (Looping, varies by pet)
Duration: 40-60 frames (1.3-2.0 seconds per step cycle)
Content: Quadrupeds (4-beat gait), birds (hop/flutter), large animals (weighted steps)
Blend parameter: Speed (0-1 normalized)
Key poses: Contact → passing → contact (4-leg cycle)
Loop: Seamless, weight-distributed

### TALK / DIALOGUE
Duration: 0.5-3 seconds (matches voice line length)
Content: Mouth open/close sync, head bob, ear twitch, eye contact
Event markers: Voice start, voice end (for TTS sync)
Facial: Mouth shapes (open, closed, mid) at key frames
Pro tip: Offset mouth animation by 2 frames behind audio for lip-sync

### CELEBRATE / VICTORY
Duration: 1.5-2.5 seconds
Content: Jump, spin, tail puff, wing spread
Key poses: crouch, apex, land
Sound effect sync: Footstep at apex, land at frame 45+

### SLEEP / REST
Duration: 2-4 seconds (looping)
Content: Eyes close, body relaxes, breathing slows
Transition: From IDLE at 0.5s fade
Wake: Reverse animation + head shake

### EAT / DRINK
Duration: 2-3 seconds
Content: Head down motion, chewing (if applicable), consume gesture
Key poses: Neutral → consume → neutral
Interaction: Triggered by player action, non-looping

### DAMAGED / HURT
Duration: 1 second
Content: Flinch, small hop back, hurt sound
Non-looping: Returns to IDLE at end
Flash: Red tint (0.3s) during animation

### CURIOUS / QUESTIONING
Duration: 1-1.5 seconds (can loop)
Content: Head tilt, ear perk, eye focus forward
Direction: Face toward player or object of interest

## 7.3 Per-Pet Animation Priorities

### Kuruvi (Bird)
1. Idle (breathing + feather ruffle)
2. Hop (forward, backward, left, right)
3. Wing flutter (during talk + celebrate)
4. Chirp (beak + head bob)
5. Hop to perch (vertical leap + landing)

### Maan (Deer)
1. Walk (elegant 4-beat gait)
2. Prance (happy version with head bob)
3. Ear swivel (independent ear rotation, 40 variants)
4. Head tilt (15° left/right on talk)
5. Leap (celebrate jump with antler toss)

### Yanai (Elephant)
1. Slow walk (weighted 4-beat with slight delay per leg)
2. Trunk sway (continuous idle motion, 20 variants)
3. Ear flap (rhythmic cooling motion)
4. Head tilt (wise, slow head rotation)
5. Trumpet pose (trunk up + celebrate animation)

### Pulliruvi (Tiger Cub)
1. Pounce (crouch → leap → land)
2. Stalk (low walk, stalking pose)
3. Tail swish (emotional indicator, 30 FPS for snappiness)
4. Head bob (beats sync, for rhythm challenges)
5. Roll (playful, 360° spin on back)

## 7.4 Animation Delivery Format

**File format:** FBX with embedded animations (Blender export)
OR separate .anim clips (Unity Mecanim format)

**Naming format:** anim_{petname}_{animation_name}_{variant}.fbx
  Examples:
  - anim_kuruvi_idle_001.fbx
  - anim_maan_walk_slow.fbx
  - anim_yanai_trumpet_celebrate.fbx
  - anim_pulliruvi_pounce_landing.fbx

**Per-animation metadata file (JSON):**
```json
{
  "animation_name": "kuruvi_chirp",
  "pet": "kuruvi",
  "duration_frames": 30,
  "fps": 30,
  "duration_seconds": 1.0,
  "looping": false,
  "parameters": {
    "blend_parameter": "Talk",
    "facial_morphs": ["mouth_open", "blink"]
  },
  "events": [
    {"frame": 15, "event": "sound_play", "data": "voice_kuruvi_chirp"},
    {"frame": 5, "event": "particle_emit", "data": "feather_dust"}
  ],
  "ik_targets": ["left_foot", "right_foot"],
  "notes": "Sync mouth_open to audio at frame 5-25"
}
```

## 7.5 Facial Animation (Rigging)

All pet faces must include:
- Eyes: Open/close + direction (up/down/left/right/focus)
- Mouth: Open/close + variations (smile, surprised, determined)
- Ears: Rotate, tilt, fold (if applicable)
- Brow: Raise, furrow (if applicable)
- Tail: Position, curl, puff (if applicable)

**Morph targets (blend shapes):** Create for all facial features
Each morph at 0.0 (neutral) to 1.0 (full)

**Animation blend tree (Mecanim):**
```
Root
├── Locomotion (1D blend: speed 0-1)
│   ├── Idle
│   ├── Walk
│   └── Run
├── Actions (Trigger-based)
│   ├── Talk
│   ├── Celebrate
│   ├── Eat
│   └── Sleep
└── Emotional (Continuous blend)
    ├── Happy (eyes wide, mouth up)
    ├── Sad (eyes droopy, mouth down)
    └── Curious (head tilt, ears forward)
```

═══════════════════════════════════════════════════════════════════════════════
SECTION 8: ACCESSIBILITY & COPPA COMPLIANCE
═══════════════════════════════════════════════════════════════════════════════

## 8.1 Target Audience & Regulations

**Target age:** 6-14 years old
**Primary markets:** Canada, UK, Malaysia, Singapore, Sri Lanka
**Applicable regulations:** COPPA (USA), GDPR (EU/UK), DPDP (India), PIPEDA (Canada)

## 8.2 COPPA Compliance (USA market)

**Key requirements:**
- No collection of personal information without verifiable parental consent
- No behavioral advertising (targeting based on activity)
- No persistent tracking or third-party data sharing
- Clear, simple privacy notice in language kids understand
- Parental access to see what data is stored

**Implementation for Yazh-Unity:**
- [✓] NO data collection (all on-device)
- [✓] NO analytics or telemetry
- [✓] NO third-party SDKs
- [ ] Parental consent flow (to be implemented Week 6)
- [ ] Privacy notice (age-appropriate, Tamil + English)

## 8.3 Accessibility Standards (WCAG 2.1 AA)

### Visual Accessibility
- **Color contrast:** All text min 4.5:1 contrast ratio
  - Dark text on light background: #1A1A1A on #F5F0E8 = 14.3:1 ✓
  - Light text on dark background: #F5F0E8 on #1A1A1A = 14.3:1 ✓
  - UI accent colors tested against backgrounds

- **Text size:** Minimum 14sp for body text (Tamil), 12sp for UI labels
  - Large fonts reduce eye strain for kids
  - Tamil script requires larger sizes than Latin

- **Color blindness:** Do not rely on color alone to convey information
  - Resource indicators: Use icon + color (water drop + River Blue)
  - Status indicators: Use shape + color (health bar stepped pyramid)

### Audio Accessibility
- **Captions:** All dialogue has on-screen subtitles (Tamil text below pet response)
- **Audio descriptions:** Environmental sounds labeled in accessibility menu
- **Volume control:** Independent sliders for voice, SFX, music

### Motor Accessibility
- **Touch targets:** Minimum 48x48 dp (physical size on screen)
  - All buttons at least 48x48 dp
  - Dialogue bubbles have large tap zones
  - Resource icons spaced 56x56 dp in grid

- **Gesture alternatives:** All multi-touch gestures have single-tap alternatives
  - Pinch-to-zoom → buttons (+ / -)
  - Swipe navigation → arrow buttons

- **No timed interactions:** Players never forced to respond within time limit
  - Chat dialogue: No "response timeout"
  - Resource collection: No time pressure

### Cognitive Accessibility
- **Language:** Simple Tamil + English sentences, age-appropriate
  - Avoid complex grammatical structures
  - Define unfamiliar Tamil words
  - Use visual metaphors (e.g., health bar labeled "உயிர்" with heart icon)

- **Consistency:** Buttons, icons, layouts consistent throughout app
  - Action buttons always at bottom
  - Pet name always top-left
  - Settings always accessed via gear icon

- **No flashing:** Avoid rapid flashing (>3 Hz) which can trigger seizures
  - Achievement animations fade-in, not flash
  - UI transitions smooth (0.3-0.5s duration)

## 8.4 For Tamil Diaspora Kids

- **Cultural appropriateness:** All content reviewed for cultural sensitivity
  - No stereotypes of Tamil people/culture
  - Sangam Corpus represents diverse perspectives
  - NPCs show modern + traditional life equally

- **Language naturalness:** Pet dialogue written by Tamil native speakers
  - Not direct translations (AI-generated or otherwise)
  - Reflects living Tamil language (not archaic)
  - Includes modern tech vocabulary when appropriate

- **Representation:** Diverse child characters in UI mockups
  - South Asian features represented
  - Mix of traditional + modern clothing
  - Inclusive family structures

## 8.5 Accessibility Checklist (Before Submission)

Before submitting to App Store / Play Store:
- [ ] All text contrast ratios tested (min 4.5:1)
- [ ] All buttons/touchable areas 48x48dp minimum
- [ ] All animations avoid flashing (>3Hz)
- [ ] All voice dialogue has subtitles
- [ ] Settings include: Font size, contrast mode, captions toggle, volume sliders
- [ ] Privacy notice in Tamil + English (no legal jargon)
- [ ] No data stored without clear parent consent mechanism
- [ ] Tested with screen readers (iOS VoiceOver, Android TalkBack)
- [ ] Tested with kids (6-8, 9-11, 12-14 age groups)

═══════════════════════════════════════════════════════════════════════════════
SECTION 9: PRODUCTION SCHEDULE & ASSET DELIVERY TIMELINE
═══════════════════════════════════════════════════════════════════════════════

## 9.1 Week-by-Week Asset Delivery (For THOZHAR Styling)

### WEEK 1 (Jun 17-23) — FOUNDATION LOCKED
**Target completion: Jun 20** (so THOZHAR can style scenes by Jun 24)

**3D Models:**
- [ ] Kuruvi model (high + medium + low LOD) — 100% complete
- [ ] Kuruvi rig (Mecanim-compatible) — ready for animation
- [ ] Sangam Kaadu biome base (1 tree variant, 1 rock, 1 structure) — greybox
- [ ] Oorru biome base (1 house, 1 well structure) — greybox

**Textures & Materials:**
- [ ] Kuruvi PBR textures (albedo, normal, roughness, metallic) — 2048x2048
- [ ] Earth material (red earth + grass blend) — for biome ground
- [ ] Wood material (terracotta, palm thatch) — for structures

**Animations:**
- [ ] Kuruvi idle animation (60 frames, 2s loop)
- [ ] Kuruvi walk animation (40 frames, looping)

**UI Assets:**
- [ ] 20 core UI icons (SVG + 256px PNG):
  - 5 action buttons (Talk, Explore, Build, Rest, Achievements)
  - 4 resource icons (Water, Food, Shelter, Herb)
  - 5 emotion states (Happy, Curious, Tired, Scared, Celebrate)
  - 2 nav icons (Settings, Menu)
  - 4 system icons (Back, Close, Info, Help)
- [ ] Kuruvi portrait (2048x2048px PNG)
- [ ] Yazh logo (geometric, SVG + PNG)
- [ ] Kolam grid background pattern (SVG, for main menu)

**Audio:**
- [ ] Kuruvi voice set (50 phrases, OGG format):
  - 10 greetings
  - 10 responses to player
  - 10 emotional sounds
  - 10 gameplay sounds (eating, drinking, sleeping)
  - 10 miscellaneous
- [ ] Button click SFX (3 variants)
- [ ] Ambient forest loop (15s, for Sangam Kaadu)

**Total deliverables: 12 art assets (ready for Unity import)**

### WEEK 2 (Jun 24-30) — SCENE STYLING COMPLETE
**Blocker resolution: THOZHAR needs Week 1 assets to complete this**

**3D Models:**
- [ ] Maan model (high + medium + low LOD) — 100% complete
- [ ] Maan rig — ready for animation
- [ ] Sangam Kaadu biome detailed (5 tree variants, props, hidden cave)
- [ ] Oorru biome detailed (5 house variants, NPCs, well + irrigation)
- [ ] Karai Parai biome base (boat, beach, cliffs)

**Textures:**
- [ ] Maan PBR textures (2048x2048)
- [ ] Vegetation textures (palm leaves, bark, ferns)
- [ ] Water shader setup (river, ocean, ripples)
- [ ] Stone material (temple ruins, cliff faces)

**Animations:**
- [ ] Kuruvi talk animation (with facial animation metadata)
- [ ] Kuruvi celebrate animation
- [ ] Maan idle animation
- [ ] Maan walk animation
- [ ] Maan prance animation

**UI Assets:**
- [ ] UI screen mockups (PNG):
  - MainMenu (1920x1080)
  - PetSelection (1920x1080)
  - BiomeArena (1920x1080)
  - Settings (1920x1080)
- [ ] Maan portrait (2048x2048px)
- [ ] Biome keyart (4 biomes, 3840x2160px each)
- [ ] Achievement badge templates (20 designs)

**Audio:**
- [ ] Maan voice set (50 phrases)
- [ ] Forest ambient SFX (5 sounds: birds, wind, water, creatures, insects)
- [ ] Village ambient loop (Oorru biome)
- [ ] Resource collection SFX (3 variants: water, food, shelter)

**Total deliverables: 18 art assets**

### WEEK 3 (Jul 1-7) — CHARACTER PACK 2 BEGINS
**THOZHAR: Scenes fully styled, ready for Week 2 expansion**

**3D Models:**
- [ ] Yanai model (high + medium + low LOD)
- [ ] Yanai rig
- [ ] Pulliruvi model (high + medium + low LOD)
- [ ] Pulliruvi rig
- [ ] Kulaathanku biome detailed (terrace, cave, hermit NPC)
- [ ] Karai Parai biome detailed (salt ponds, pier, fishing boat props)

**Animations:**
- [ ] Maan talk, celebrate, eat, sleep (all core cycles)
- [ ] Yanai idle, walk, trunk sway, ear flap
- [ ] Pulliruvi pounce, stalk, tail swish

**Asset packs:**
- [ ] Pet pack 2 UI (portraits + selection screens for Yanai + Pulliruvi)
- [ ] Difficulty selection UI mockup
- [ ] Parental controls mockup (Week 6 feature)

**Total deliverables: 15 new assets**

### WEEK 4 (Jul 8-14) — FEATURE COMPLETION
**Milestone: Vertical slice fully playable with 2 pets, multiple biomes**

**3D Models:**
- [ ] Environmental props refinement (all biomes)
- [ ] NPC models (farmer, water carrier, weaver, etc.) — 8 characters
- [ ] Weather particle effects (rain, dust, fog)

**Animations:**
- [ ] Yanai all core cycles (talk, celebrate, eat, sleep, etc.)
- [ ] Pulliruvi all core cycles
- [ ] Environmental animations (water flow, wind sway, door open/close)

**UI Polish:**
- [ ] Dialogue UI mockup (chat bubble design, Tamil font rendering)
- [ ] Health/resource HUD mockup
- [ ] Achievement unlock animation mockup

**Total deliverables: 12 assets**

### WEEKS 5-8 (Jul 15-Aug 11) — EXPANSION PHASE
(Handled by expanded art team after Week 1-4 foundation)

**3D Models:**
- [ ] All 4 pets: Complete expression sets (happy, curious, tired, celebrate)
- [ ] 6+ environment props per biome
- [ ] VFX models (rare items, collectibles)

**Animations:**
- [ ] All pets: 15-20 animation cycles each
- [ ] Environmental sequences (day/night cycle, weather transitions)

**Audio:**
- [ ] Full voice recording: 150 phrases per pet (600 total)
- [ ] Ambient soundscapes per biome
- [ ] Challenge/boss music tracks

**UI:**
- [ ] Complete achievement badge set (40+)
- [ ] Localization assets (Tamil + English text variations)

**Total deliverables: 80+ assets (expansion pack)**

---

## 9.2 Asset Handoff Checklist (THOZHAR Entry Point)

**THOZHAR receives from Art by EOD Jun 20:**

Folder: `/home/neutron/Yazhi/apps/yazh-unity/Assets/`
```
Models/
  Pets/Kuruvi/
    - kuruvi_high.fbx ✓
    - kuruvi_med.fbx ✓
    - kuruvi_low.fbx ✓
    - kuruvi_rig.fbx (Mecanim ready) ✓
  Environment/
    - greybox_sangam_tree_001.fbx ✓
    - greybox_sangam_tree_002.fbx ✓
    - greybox_oorru_house_001.fbx ✓
    - greybox_oorru_well.fbx ✓

Textures/
  Pets/
    - kuruvi_albedo.png (2048x2048) ✓
    - kuruvi_normal.png ✓
    - kuruvi_roughness.png ✓
    - kuruvi_metallic.png ✓
  Environment/
    - mat_earth_red.png ✓
    - mat_wood_palm.png ✓

Materials/
  - pet_kuruvi.mat (pre-configured) ✓
  - env_earth.mat ✓

Animations/
  - anim_kuruvi_idle.anim ✓
  - anim_kuruvi_walk.anim ✓

Audio/
  Voice/Kuruvi/
    - voice_kuruvi_greeting_001.ogg ✓
    - voice_kuruvi_greeting_002.ogg ✓
    - ... (50 phrases total) ✓
  SFX/
    - sfx_button_click_001.ogg ✓
    - sfx_ambient_forest.ogg ✓

UI/
  Icons/ (20 SVG + PNG pairs)
    - ui_icon_talk_256.svg + .png ✓
    - ui_icon_explore_256.svg + .png ✓
    - ... (18 more) ✓
  Portraits/
    - ui_portrait_kuruvi_2048.png ✓
  Backgrounds/
    - ui_bg_kolam_grid.svg ✓
```

**Validation checklist:**
- [ ] All FBX files import without errors in Unity 6
- [ ] All textures are correct resolution (2048 max for pets)
- [ ] All audio files are OGG format, normalized correctly
- [ ] All SVG icons have been exported to PNG @ 256px + 512px
- [ ] Naming conventions followed exactly (pet_{name}_{variant})
- [ ] No western design elements (round corners, drop shadows)
- [ ] Tamil cultural markers visible on all pet models
- [ ] At least 3 LOD levels for all 3D models

## 9.3 Communication Protocol

**OVIYA (Art Lead) → THOZHAR (Scene Styling):**
1. Assets committed to `/Assets/` folder
2. Slack notification: "Week 1 art assets ready for styling"
3. Spreadsheet updated: asset filename → styling status
4. Daily async standup (Matrix room `#yazh-art`)

**THOZHAR feedback → OVIYA:**
- If asset doesn't fit scene: Report in #yazh-art with screenshot
- If animation timing is off: Note exact frame where issue occurs
- If colors don't fit palette: Suggest adjustment (hex code)

**Approval flow:**
- THOZHAR tests asset in scene
- Posts screenshot in #yazh-united-approvals
- OVIYA reviews + approves OR requests revision
- If approved: Asset marked final in spreadsheet
- If revision needed: OVIYA pushes update within 24h

═══════════════════════════════════════════════════════════════════════════════
SECTION 10: CULTURAL REFERENCE LIBRARY
═══════════════════════════════════════════════════════════════════════════════

For artists and developers — key Tamil visual references:

Architecture:
  - Meenakshi Temple, Madurai (gopuram proportions, 195ft height)
  - Brihadeeswarar Temple, Thanjavur (vimana geometry, 1010 CE, no mortar)
  - Dravidian temple kolam patterns (floor designs, dot-based grids)
  - Chettinad palace architecture (diamond lattice windows, timber work)

Textiles:
  - Kanchipuram silk borders (geometric motifs, phoenix + peacock patterns)
  - Chettinad diamond lattice patterns (interlocking geometric design)
  - Tamil Nadu palm leaf weaving patterns (striped, natural fiber colors)
  - Temple wall murals (Tanjore style — rich earth tones + gold leaf)

Nature:
  - Western Ghats mountain silhouettes (layered, misty)
  - Palmyra palm (state tree of Tamil Nadu, distinct fan-shaped crown)
  - Marigold flower (ubiquitous in Tamil culture, bright orange)
  - Peacock (national bird, Tamil Nadu state symbol, distinctive plumage)
  - Banyan tree (cultural/spiritual significance, multiple trunks, aerial roots)

Art:
  - Tanjore painting style (rich colors, gold leaf, geometric framing, 200+ years old)
  - Chola bronze sculpture proportions (1:1.618 human form, balanced pose)
  - Sangam-era pottery motifs (ancient Tamil art, 300 BCE - 300 CE)
  - Tamil script calligraphy (fluid, geometric letterforms)

Colors (cultural significance):
  - Ochre/Turmeric (#C8840A): Purity, auspiciousness, worship readiness
  - Red (#8B1A1A): Power, celebration, temple color, female life-force
  - Green (#2D5A27): Nature, life, agriculture, growth, monsoon
  - White (#F5F0E8): Peace, simplicity, palmyra parchment, mourning (historically)
  - Black (#1A1A1A): Protection, depth, night sky, cosmic void
  - Indigo (#3D2B56): Dye tradition, mystery, twilight
  - Gold (#C8840A or F0C040): Prosperity, divinity, turmeric, saffron

Historical periods to reference:
  - **Sangam Era (300 BCE - 300 CE)**: Earliest Tamil literature + art
    - Simple geometries, pottery patterns, palm tree motifs
    - Used for game world background lore
  - **Chola Era (848-1279 CE)**: Peak Dravidian architecture
    - Temple gopuram proportions, bronze sculpture refined forms
    - Interior temple wall percentages used for UI proportions
  - **18th-19th Century (Tanjore School)**: Painting style
    - Rich earth-tone palettes, geometric framing (our color palette)

Visual research materials:
  - Meenakshi Temple virtual tour (YouTube: "VR Meenakshi Temple walkthrough")
  - Tanjore paintings on Google Arts (see color harmony techniques)
  - Tamil Nadu textile museums (Chettinad palace Wikipedia images)
  - Ancient Tamil pottery database (Tamil Language Archives)

DO NOT reference:
  - Western fantasy tropes (castles, wizards, unicorns)
  - Bollywood/Hindi visual language (different region)
  - South Asian generics (avoid "exotic" stereotypes)
  - Colonial-era depictions of Tamil life

═══════════════════════════════════════════════════════════════════════════════
END OF ART DIRECTION DOCUMENT
═══════════════════════════════════════════════════════════════════════════════

Version: 2.0 (COMPLETE)
Author: OVIYA (Art Direction Lead, Rotation 25)
Approved by: Yazhi Leadership
Last updated: 2026-06-18, 14:30 IST
Status: PRODUCTION READY (blocking resolved)
Next review: Week 4 milestone (Jul 8)
