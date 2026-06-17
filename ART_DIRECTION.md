# YAZH-UNITY Art Direction Document
> OVIYA | Rotation 25 | Jun 17, 2026

## Visual Identity: Vahana UI

### Design Philosophy
Geometric synthesis rooted in Dravidian proportions. Every element draws from
Tamil visual heritage — temple gopuram silhouettes, kolam patterns, Sangam
landscape imagery — reimagined through a brutalist, kid-friendly lens.

### Color Palette

**Primary (Sangam Earth):**
- Deep Red Earth: #8B2500 (terracotta, temple stone)
- Temple Gold: #D4A843 (gopuram accents, celebration)
- Palm Leaf Green: #2D5A27 (nature, growth, health)

**Secondary (Coastal Sky):**
- Monsoon Grey: #6B7B8C (clouds, rain, storm weather)
- Lagoon Blue: #3A7CA5 (water, calm, trust)
- Night Indigo: #1A1A2E (menus, text backgrounds)

**Accent (Festival):**
- Marigold Orange: #FF8C00 (CTAs, highlights, energy)
- Jasmine White: #FFF8E7 (text, backgrounds, purity)
- Kumkum Red: #C41E3A (alerts, danger, urgency)

**Semantic:**
- Health: #E74C3C (red)
- Energy: #F39C12 (amber)
- Hunger: #27AE60 (green)
- Happiness: #9B59B6 (purple)

### Typography
- **Primary:** Noto Sans Tamil (Google Fonts, OFL license)
- **Display:** Custom geometric Tamil display font (TBD — commission from Tamil type designer)
- **Size hierarchy:** 16pt body / 24pt headers / 48pt display / 12pt captions
- **Weight:** Regular for body, Bold for headers, Light for captions

### Pet Designs

#### Kuruvi (குறுவி) — Sparrow
- **Personality:** Curious, fast, energetic
- **Silhouette:** Round body, small beak, perky tail
- **Color:** Warm brown (#8B6914) with cream belly (#FFF8E7)
- **Eyes:** Large, round, black — expressive
- **Size reference:** Fits in child's palm (in-game scale)
- **Animations:** Idle (head tilts), Walk (hop), Eat (peck), Fly (short hops), Sleep (fluffed)
- **Sound:** Quick chirps, 2-3 tone patterns

#### Maan (மான்) — Deer
- **Personality:** Thoughtful, gentle, strategic
- **Silhouette:** Graceful, slender legs, small antlers (both sexes)
- **Color:** Tawny (#C4956A) with white spots (juvenile look)
- **Eyes:** Large, dark brown, calm
- **Size reference:** Knee-height to child
- **Animations:** Idle (ear flicks), Walk (graceful), Eat (graze), Alert (head up), Sleep (curled)
- **Sound:** Soft bleats, gentle calls

#### Yanai (யானை) — Elephant
- **Personality:** Wise, patient, memory-keeper
- **Silhouette:** Rounded, friendly proportions (not realistic — stylized)
- **Color:** Warm grey (#808080) with pink inner ears (#FFB6C1)
- **Eyes:** Small, wise, half-lidded default
- **Size reference:** Waist-height to child
- **Animations:** Idle (trunk sway), Walk (slow, heavy), Eat (trunk grab), Spray (water), Sleep (lying)
- **Sound:** Low rumbles, trumpet (rare, celebratory)

#### Pulliruvi (புல்லுருவி) — Tiger Cat
- **Personality:** Playful, independent, unpredictable
- **Silhouette:** Sleek, long tail, pointed ears
- **Color:** Orange (#FF8C00) with black stripes (#1A1A2E)
- **Eyes:** Green (#2D5A27), slit pupils, mischievous
- **Size reference:** House-cat sized
- **Animations:** Idle (tail swish), Walk (stalk), Eat (pounce), Play (roll), Sleep (curled, tail over nose)
- **Sound:** Meows, purrs, hisses (playful, not scary)

### Biome Designs

#### Oorru (ஊற்று) — Village Spring (Biome 1, Week 1)
- **Setting:** Tamil village center, ancient banyan tree, stone well
- **Palette:** Palm Leaf Green + Temple Gold + Deep Red Earth
- **Key elements:**
  - Banyan tree (central landmark, shade provider)
  - Stone well (water source)
  - Kolam patterns on ground (procedural, daily)
  - Clay houses (background, silhouette)
  - Pumpkin vines, banana trees (resource nodes)
- **Lighting:** Warm afternoon sun, dappled shadows
- **Soundscape:** Birds, distant temple bells, water trickling
- **Weather effects:** Rain creates puddles, sun creates dust particles

#### Kulaathanku (குளத்தங்கு) — Farmland Pond (Biome 2, Week 5)
- **Setting:** Rice paddies, irrigation channels, palm groves
- **Palette:** Green gold + water blue + earth brown
- **Key elements:** Rice shoots, water channels, palm trees, scarecrow
- **Soundscape:** Frogs, crickets, wind through palms

#### Karai Parai (கரைப்பாறை) — Coastal Cliffs (Biome 3, Week 6)
- **Setting:** Rocky coastline, fishing boats, lighthouse
- **Palette:** Lagoon Blue + Monsoon Grey + Temple Gold
- **Key elements:** Rock formations, fishing nets, boat wrecks, tide pools
- **Soundscape:** Waves, seabirds, wind

### UI Style

#### Principles
1. **Tamil-first:** All UI text in Tamil, English only for technical terms
2. **Geometric:** Kolam-inspired patterns for borders, dividers, backgrounds
3. **Brutalist:** No gradients, no shadows, no rounded corners (except pet portraits)
4. **Accessible:** Minimum 16pt text, high contrast, color-blind safe palette
5. **Kid-friendly:** Large touch targets (min 44x44pt), clear iconography

#### Component Specifications

**Buttons:**
- Background: Marigold Orange (#FF8C00) default, Deep Red Earth (#8B2500) pressed
- Text: Jasmine White (#FFF8E7), Bold, 24pt
- Border: 2px solid Temple Gold (#D4A843)
- Size: Min 44x44pt touch target
- Corner radius: 0 (brutalist — sharp corners)

**Panels:**
- Background: Night Indigo (#1A1A2E) at 90% opacity
- Border: 2px solid Palm Leaf Green (#2D5A27)
- Padding: 16pt all sides

**Health/Energy Bars:**
- Background: #333333
- Fill: Semantic colors (Health=red, Energy=amber, Hunger=green, Happiness=purple)
- Height: 12pt
- Border: 1px solid #666666

**Pet Portrait Frames:**
- Shape: Circle (kolam-inspired border pattern)
- Size: 120x120pt (selection), 60x60pt (HUD)
- Border: 3px solid Temple Gold

### Asset Specifications

#### 3D Models
- Format: FBX (Unity compatible)
- Poly count: < 5,000 per pet (mobile target)
- Texture size: 1024x1024 max
- Rig: Humanoid or Generic (Unity Mecanim compatible)
- Animations: FBX clips, 30 FPS
- Naming: `{pet}_{action}.fbx` (e.g., `kuruvi_idle.fbx`)

#### Textures
- Format: PNG (lossless) or ASTC (compressed, mobile)
- Size: 512x512 (props), 1024x1024 (pets, key elements)
- Style: Hand-painted, stylized (not photorealistic)
- Color space: sRGB

#### Audio
- Voice: Recorded Tamil, native speakers (kids' voices for pets)
- Format: OGG Vorbis (compressed) or WAV (uncompressed, < 10MB)
- Sample rate: 44.1 kHz
- Naming: `{pet}_{phrase_id}.ogg` (e.g., `kuruvi_greeting_01.ogg`)
- SFX: 16-bit, mono, 22 kHz acceptable
- Music: Looping, 60-90 second clips, OGG

#### UI Assets
- Format: SVG (source), PNG (runtime)
- Size: @1x, @2x, @3x for mobile
- Naming: `{component}_{state}.png` (e.g., `button_default.png`)

### Naming Conventions
- All files: lowercase, underscore separated
- Prefix by type: `pet_`, `biome_`, `ui_`, `sfx_`, `music_`, `voice_`
- Version suffix: `_v1`, `_v2` for iterations
- No spaces, no special characters, no Tamil in filenames

---
*Created: 2026-06-17 | OVIYA | Rotation 25*
*Format: .yz (Yazhi documentation standard)*
