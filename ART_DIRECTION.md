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
SECTION 7: CULTURAL REFERENCE LIBRARY
═══════════════════════════════════════════════════════════════════════════════

For artists and developers — key Tamil visual references:

Architecture:
  - Meenakshi Temple, Madurai (gopuram proportions)
  - Brihadeeswarar Temple, Thanjavur (vimana geometry)
  - Dravidian temple kolam patterns (floor designs)

Textiles:
  - Kanchipuram silk borders (geometric motifs)
  - Chettinad diamond lattice patterns
  - Tamil Nadu palm leaf weaving patterns

Nature:
  - Western Ghats mountain silhouettes
  - Palmyra palm (state tree of Tamil Nadu)
  - Marigold flower (ubiquitous in Tamil culture)
  - Peacock (national bird, Tamil Nadu state symbol)

Art:
  - Tanjore painting style (rich colors, gold leaf, geometric framing)
  - Chola bronze sculpture proportions
  - Sangam-era pottery motifs

Colors (cultural significance):
  - Ochre/Turmeric: Purity, auspiciousness
  - Red: Power, celebration, temple color
  - Green: Nature, life, agriculture
  - White: Peace, simplicity, palmyra
  - Black: Protection, depth, night sky

═══════════════════════════════════════════════════════════════════════════════
END OF ART DIRECTION DOCUMENT
═══════════════════════════════════════════════════════════════════════════════

Version: 1.0
Author: OVIYA (Art Direction, Rotation 25)
Approved by: Yazhi Leadership
Next review: Week 4 milestone
