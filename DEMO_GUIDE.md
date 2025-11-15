# TUMBLE RUMBLE - PLAYABLE DEMO GUIDE
## How to Play the Game Right Now

This guide will get you playing the game in **under 10 minutes**.

---

## 🚀 QUICK START (3 Steps)

### Step 1: Open in Unity (2 minutes)

1. **Open Unity Hub**
2. Click **"Add"** → Select this folder (`Gang-Beast-Clone`)
3. Unity version: **2022.3 LTS** (if not installed, Unity Hub will prompt you)
4. Click **"Open"**
5. Wait for Unity to import packages (2-5 minutes first time)

### Step 2: Run Auto-Setup (1 minute)

Once Unity opens:

1. In Unity menu bar: **`Tumble Rumble → Setup Project`**
2. A window appears - click **"Create All Assets & Scenes"**
3. Wait 30 seconds while it creates everything
4. Click **OK** when "Setup Complete" dialog appears

### Step 3: Play! (Immediate)

1. In Project window, navigate to: `Assets/_Project/Scenes/`
2. Double-click **`TestArena.unity`**
3. Press **Play ▶️** button (or press `F5` / `Ctrl+P`)

**You're now playing the game!**

---

## 🎮 CONTROLS

### Player 1 (Keyboard & Mouse)

| Action | Control |
|--------|---------|
| **Move** | WASD |
| **Jump** | Space |
| **Grab** | E |
| **Punch** | Left Mouse Button |
| **Ability** | Q |
| **Crouch** | Ctrl |

### Player 2 (Gamepad - if connected)

| Action | Control |
|--------|---------|
| **Move** | Left Stick |
| **Jump** | A (South Button) |
| **Grab** | X (West Button) |
| **Punch** | B (East Button) |
| **Ability** | Y (North Button) |
| **Crouch** | L1 (Left Shoulder) |

---

## 🎯 WHAT YOU'LL SEE

### Test Arena Scene

The demo includes:
- ✅ **2 Players**: One cyan, one pink
- ✅ **Circular Platform**: Fight arena
- ✅ **Physics System**: Full ragdoll physics
- ✅ **Void Zone**: Fall off the edge = elimination
- ✅ **Dynamic Camera**: Auto-frames both players

### How to Play

1. **Move around** with WASD
2. **Jump** with Space
3. **Punch** your opponent (left mouse)
4. **Grab** them with E
5. **Try to knock them off the platform!**

---

## 🧪 TESTING THE GAME

### What Works Right Now

✅ **Player Movement** - Physics-driven locomotion
✅ **Combat** - Punching and grabbing
✅ **Ragdoll Physics** - 15-bone system
✅ **Void Elimination** - Fall off = KO
✅ **Dynamic Camera** - Tracks both players
✅ **Score System** - Backend tracking
✅ **Match Manager** - Round system ready

### What's Placeholder

⚠️ **Visuals** - Simple cubes (assets pending)
⚠️ **Audio** - No sound yet (implementation ready)
⚠️ **UI** - Minimal (HUD system ready)
⚠️ **AI** - Manual only (bot code ready)
⚠️ **Animations** - Static poses (system ready)

---

## 🔧 CUSTOMIZATION

### Add More Players

1. In Hierarchy, right-click → Duplicate **Player1** or **Player2**
2. Rename to **Player3**, etc.
3. Change position (`Transform → Position` in Inspector)
4. Change color (assign different material to Visual child)
5. Press Play

### Modify Physics

**Make players heavier:**
1. Select Player in Hierarchy
2. Inspector → Rigidbody component
3. Increase **Mass** (default: 40)

**Change gravity:**
1. Edit → Project Settings → Physics
2. Gravity Y: Change from -20 to any value

**Adjust jump:**
1. Select Player → Inspector
2. PlayerController component
3. Jump Force: Increase/decrease (default: 8)

### Change Arena Size

1. Select **MainPlatform** in Hierarchy
2. Inspector → Transform → Scale
3. Adjust X and Z (default: 15, 0.5, 15)

---

## 📊 PERFORMANCE

### Expected Performance

- **Target:** 60 FPS
- **Resolution:** 1920×1080
- **Physics:** 50 Hz (0.02s fixed timestep)
- **Players:** 2 (can add up to 8)

### If Performance is Low

1. Edit → Project Settings → Quality
2. Select **"Low"** preset
3. Or: Reduce Shadow Distance, disable Anti-aliasing

### Monitor Performance

1. Window → Analysis → Profiler
2. Press Play
3. Watch CPU, Rendering, Physics graphs
4. Target: < 16.6ms per frame

---

## 🏗️ WHAT WAS AUTO-CREATED

The setup script created:

### Materials
- `MAT_Player_Cyan.mat` - Cyan glowing player
- `MAT_Player_Pink.mat` - Pink glowing player
- `MAT_Platform.mat` - Dark gray platform

### Physics Materials
- `PhysicsMaterial_Player.physicMaterial`
- `PhysicsMaterial_Environment.physicMaterial`

### Prefabs
- `Player.prefab` - Complete player with all systems

### Scenes
- `TestArena.unity` - Playable test scene
- `MainMenu.unity` - Basic menu (not fully functional yet)

---

## 🐛 TROUBLESHOOTING

### "Cannot find PlayerController script"

**Fix:**
- Check that all scripts compiled
- Window → Console → Clear errors
- If errors persist, reimport all scripts

### "Input System not found"

**Fix:**
- Window → Package Manager
- Search "Input System" → Install
- Unity will prompt restart → Click Yes

### "Gravity doesn't work"

**Fix:**
- Edit → Project Settings → Physics
- Gravity Y should be **-20** (or negative)

### "Players fall through platform"

**Fix:**
- Select Platform → Inspector → Add Component → **Box Collider**
- Or: Check that Rigidbody on Player has **Use Gravity** enabled

### "Camera doesn't follow players"

**Fix:**
- Select Main Camera
- Inspector → Add Component → **DynamicCamera**
- DynamicCamera → Targets → Add Player1 and Player2

---

## 🎨 NEXT STEPS: Making it Beautiful

### Replace Placeholder Visuals

1. Import 3D models (see `ASSET_SPECIFICATIONS_PHASE4.md`)
2. Replace the "Visual" child cube with your model
3. Apply materials from spec

### Add Audio

1. Import sound files
2. AudioManager → Assign clips in Inspector
3. Sounds will play automatically

### Complete UI

1. Open `MainMenu.unity`
2. Add TextMeshPro components
3. Connect buttons to scripts

### Create Full Arenas

1. Use ProBuilder to model arenas
2. Add arena-specific hazard scripts
3. Configure evolution settings

See `BUILD_INSTRUCTIONS_PHASE6.md` for complete instructions.

---

## 🌐 MULTIPLAYER TESTING

### Local Multiplayer (Same Machine)

**Already works!** Just:
1. Connect multiple gamepads
2. Auto-assigns to players
3. Press Play

### Online Multiplayer

Requires additional setup:
1. Install Unity Netcode package (in manifest)
2. Follow `BUILD_INSTRUCTIONS_PHASE6.md` Section 8
3. Build and test

---

## 📝 DEMO LIMITATIONS

This is a **functional prototype demo**, not the final game:

### What's Missing

- [ ] High-quality 3D character models
- [ ] Full animation sets
- [ ] Audio (music & SFX)
- [ ] Polished UI/HUD
- [ ] All 8 arena scenes
- [ ] Networked multiplayer
- [ ] AI bots (code ready, not enabled)
- [ ] Particle effects/VFX
- [ ] Menu flow

### What's Complete

- [x] All core gameplay systems
- [x] Physics engine (full ragdoll)
- [x] Player controls
- [x] Combat mechanics
- [x] Match/round management
- [x] Score tracking
- [x] Camera system
- [x] Project structure
- [x] **Fully playable foundation**

---

## ⚡ QUICK TIPS

### Best Test Scenarios

1. **Physics Test**: Jump around, fall off edges
2. **Combat Test**: Punch opponent repeatedly
3. **Grab Test**: Press E near opponent, move while holding
4. **Push Off Test**: Try to knock opponent into void
5. **Camera Test**: Run far apart, watch camera zoom out

### Fun Experiments

- **High Jump**: Set Jump Force to 20+
- **Low Gravity**: Set Gravity Y to -5
- **Super Heavy**: Set Mass to 200
- **Bouncy Players**: Set Bounciness to 0.8

---

## 📚 DOCUMENTATION REFERENCE

| Doc | Purpose |
|-----|---------|
| `README.md` | Project overview |
| `DESIGN_DOC_PHASE1.md` | Game design vision |
| `DESIGN_DOC_PHASE2.md` | Technical architecture |
| `COMPLETE_SCRIPTS_PHASE3.md` | All game scripts |
| `ASSET_SPECIFICATIONS_PHASE4.md` | Asset creation specs |
| `SELF_HEALING_SYSTEM_PHASE5.md` | Auto-optimization |
| `BUILD_INSTRUCTIONS_PHASE6.md` | Complete build guide |
| **`DEMO_GUIDE.md`** | **You are here!** |

---

## ✅ VERIFICATION CHECKLIST

After setup, verify everything works:

- [ ] Unity opens project without errors
- [ ] Console shows "Setup complete" message
- [ ] TestArena scene loads
- [ ] Press Play - no errors in Console
- [ ] Player 1 (cyan) moves with WASD
- [ ] Player can jump with Space
- [ ] Player can punch (left click shows animation)
- [ ] Camera follows player
- [ ] Falling off platform causes elimination message in Console
- [ ] FPS > 30 (check Stats window)

If all checked ✅ **THE DEMO WORKS!**

---

## 🎮 READY TO PLAY!

**That's it - you now have a fully functional ragdoll physics brawler demo running in Unity!**

Press Play and start brawling! 🥊

---

## 🔄 FUTURE UPDATES

To complete the full game, work through:

1. **Asset Creation** (Phase 4 specs) - 3D models, animations, audio
2. **Scene Building** (Phase 2 architecture) - All 8 arenas
3. **UI Polish** (Phase 3 scripts) - Menus, HUD, transitions
4. **Multiplayer** (Phase 6 build guide) - Online play
5. **Balance & Polish** - Playtesting and refinement

**Estimated time to full release:** 2-3 months with dedicated art pipeline

---

*Demo Guide Version: 1.0*
*Last Updated: 2025-11-15*
*Status: FUNCTIONAL DEMO READY ✓*

**Enjoy playing TUMBLE RUMBLE!** 🎉
