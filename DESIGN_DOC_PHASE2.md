# TUMBLE RUMBLE: TECHNICAL ARCHITECTURE
## Complete Technical Specification - Phase 2

---

## ENGINE SELECTION

**Chosen Engine: Unity 2022.3 LTS**

**Rationale:**
- **Physics Engine**: Built-in PhysX provides robust ragdoll and joint systems
- **Networking**: Multiple options (Unity Netcode for GameObjects, Photon Fusion, Mirror)
- **Cross-platform**: Native support for Windows, Mac, Linux
- **Performance**: Optimized for physics-heavy scenarios with Job System and Burst Compiler
- **Animation**: Mecanim animation system with blend trees and layering
- **Rendering**: URP (Universal Render Pipeline) for stylized cel-shaded graphics
- **Community**: Large asset store and extensive documentation
- **Multiplayer**: Proven solutions for 8-player physics synchronization

**Alternative Considered:**
- Unreal Engine 5: More powerful graphics but heavier overhead for physics party game
- Godot: Open-source but less mature networking solutions for 8-player physics sync

**Version Requirements:**
- Unity 2022.3.x LTS (Long Term Support)
- .NET Standard 2.1
- URP (Universal Render Pipeline) 14.x
- Unity Netcode for GameObjects 1.5.x OR Photon Fusion 2.x

---

## PROJECT STRUCTURE

### Root Directory Layout

```
TumbleRumble/
├── .git/
├── .gitignore
├── .gitattributes
├── Assets/
│   ├── _Project/                          # Main project assets
│   │   ├── Art/
│   │   │   ├── Characters/
│   │   │   │   ├── Gelatinous/
│   │   │   │   │   ├── Materials/
│   │   │   │   │   ├── Models/
│   │   │   │   │   ├── Textures/
│   │   │   │   │   ├── Prefabs/
│   │   │   │   │   └── Animations/
│   │   │   │   ├── Tentacled/
│   │   │   │   ├── Crystalline/
│   │   │   │   ├── Gaseous/
│   │   │   │   ├── Symbiotic/
│   │   │   │   └── Shared/
│   │   │   │       ├── Eyes/
│   │   │   │       ├── Accessories/
│   │   │   │       └── VFX/
│   │   │   ├── Environments/
│   │   │   │   ├── GravityWell/
│   │   │   │   ├── CrystalCavern/
│   │   │   │   ├── NebulaNexus/
│   │   │   │   ├── MeteorShower/
│   │   │   │   ├── BlackHole/
│   │   │   │   ├── PlasmaFountains/
│   │   │   │   ├── MagneticRings/
│   │   │   │   ├── TimeDilation/
│   │   │   │   └── Shared/
│   │   │   │       ├── Skyboxes/
│   │   │   │       ├── Props/
│   │   │   │       └── VFX/
│   │   │   ├── UI/
│   │   │   │   ├── Sprites/
│   │   │   │   ├── Fonts/
│   │   │   │   ├── Icons/
│   │   │   │   └── Materials/
│   │   │   ├── VFX/
│   │   │   │   ├── Particles/
│   │   │   │   ├── Shaders/
│   │   │   │   └── Textures/
│   │   │   └── Shaders/
│   │   │       ├── Character/
│   │   │       ├── Environment/
│   │   │       └── PostProcess/
│   │   ├── Audio/
│   │   │   ├── Music/
│   │   │   │   ├── Menu/
│   │   │   │   ├── Gameplay/
│   │   │   │   └── Victory/
│   │   │   ├── SFX/
│   │   │   │   ├── Character/
│   │   │   │   │   ├── Gelatinous/
│   │   │   │   │   ├── Tentacled/
│   │   │   │   │   ├── Crystalline/
│   │   │   │   │   ├── Gaseous/
│   │   │   │   │   └── Symbiotic/
│   │   │   │   ├── Impacts/
│   │   │   │   ├── Abilities/
│   │   │   │   ├── Hazards/
│   │   │   │   └── UI/
│   │   │   └── Mixers/
│   │   ├── Scripts/
│   │   │   ├── Core/
│   │   │   │   ├── GameManager.cs
│   │   │   │   ├── MatchManager.cs
│   │   │   │   ├── RoundManager.cs
│   │   │   │   ├── ScoreManager.cs
│   │   │   │   └── GameStateManager.cs
│   │   │   ├── Player/
│   │   │   │   ├── PlayerController.cs
│   │   │   │   ├── RagdollController.cs
│   │   │   │   ├── PlayerInput.cs
│   │   │   │   ├── PlayerAnimator.cs
│   │   │   │   ├── GrabSystem.cs
│   │   │   │   ├── CombatSystem.cs
│   │   │   │   ├── AbilitySystem.cs
│   │   │   │   └── SpeciesPhysics/
│   │   │   │       ├── GelatinousPhysics.cs
│   │   │   │       ├── TentacledPhysics.cs
│   │   │   │       ├── CrystallinePhysics.cs
│   │   │   │       ├── GaseousPhysics.cs
│   │   │   │       └── SymbioticPhysics.cs
│   │   │   ├── Camera/
│   │   │   │   ├── DynamicCamera.cs
│   │   │   │   ├── CameraZoom.cs
│   │   │   │   └── CameraTarget.cs
│   │   │   ├── Environment/
│   │   │   │   ├── Hazards/
│   │   │   │   │   ├── HazardBase.cs
│   │   │   │   │   ├── VoidZone.cs
│   │   │   │   │   ├── DamageZone.cs
│   │   │   │   │   ├── LaunchPad.cs
│   │   │   │   │   └── TrapZone.cs
│   │   │   │   ├── ArenaSpecific/
│   │   │   │   │   ├── GravityRotator.cs
│   │   │   │   │   ├── CrystalGrowth.cs
│   │   │   │   │   ├── PhaseShiftPlatform.cs
│   │   │   │   │   ├── MeteorSpawner.cs
│   │   │   │   │   ├── BlackHolePull.cs
│   │   │   │   │   ├── PlasmaGeyser.cs
│   │   │   │   │   ├── MagneticRing.cs
│   │   │   │   │   └── TimeDilationZone.cs
│   │   │   │   └── ArenaEvolution.cs
│   │   │   ├── UI/
│   │   │   │   ├── MainMenu.cs
│   │   │   │   ├── CharacterCustomization.cs
│   │   │   │   ├── LobbyUI.cs
│   │   │   │   ├── HUD.cs
│   │   │   │   ├── ScoreDisplay.cs
│   │   │   │   ├── RoundTransition.cs
│   │   │   │   └── PauseMenu.cs
│   │   │   ├── Networking/
│   │   │   │   ├── NetworkManager.cs
│   │   │   │   ├── LobbyManager.cs
│   │   │   │   ├── PlayerNetworkBehavior.cs
│   │   │   │   ├── MatchmakingManager.cs
│   │   │   │   └── NetworkedPhysicsSync.cs
│   │   │   ├── AI/
│   │   │   │   ├── BotController.cs
│   │   │   │   ├── BotBehaviorTree.cs
│   │   │   │   └── BotDifficulty.cs
│   │   │   ├── Audio/
│   │   │   │   ├── AudioManager.cs
│   │   │   │   ├── MusicManager.cs
│   │   │   │   └── SFXManager.cs
│   │   │   ├── Utilities/
│   │   │   │   ├── ObjectPool.cs
│   │   │   │   ├── Extensions.cs
│   │   │   │   ├── PhysicsHelper.cs
│   │   │   │   └── MathHelper.cs
│   │   │   └── SelfHealing/
│   │   │       ├── ErrorDetector.cs
│   │   │       ├── AutoFixer.cs
│   │   │       ├── PerformanceProfiler.cs
│   │   │       └── AutoOptimizer.cs
│   │   ├── Prefabs/
│   │   │   ├── Characters/
│   │   │   ├── Arenas/
│   │   │   ├── Hazards/
│   │   │   ├── VFX/
│   │   │   └── UI/
│   │   ├── Scenes/
│   │   │   ├── MainMenu.unity
│   │   │   ├── CharacterCustomization.unity
│   │   │   ├── Lobby.unity
│   │   │   ├── Arenas/
│   │   │   │   ├── GravityWell.unity
│   │   │   │   ├── CrystalCavern.unity
│   │   │   │   ├── NebulaNexus.unity
│   │   │   │   ├── MeteorShower.unity
│   │   │   │   ├── BlackHole.unity
│   │   │   │   ├── PlasmaFountains.unity
│   │   │   │   ├── MagneticRings.unity
│   │   │   │   └── TimeDilation.unity
│   │   │   └── TestScenes/
│   │   │       ├── PhysicsTest.unity
│   │   │       ├── NetworkingTest.unity
│   │   │       └── PerformanceTest.unity
│   │   ├── Resources/
│   │   │   ├── DefaultPlayerData/
│   │   │   └── GameSettings/
│   │   ├── Settings/
│   │   │   ├── URP_Asset.asset
│   │   │   ├── InputActions.inputactions
│   │   │   ├── Physics/
│   │   │   │   ├── PhysicsMaterial_Player.physicMaterial
│   │   │   │   ├── PhysicsMaterial_Environment.physicMaterial
│   │   │   │   └── PhysicsSettings.asset
│   │   │   └── Quality/
│   │   │       ├── QualitySettings_Low.asset
│   │   │       ├── QualitySettings_Medium.asset
│   │   │       └── QualitySettings_High.asset
│   │   └── StreamingAssets/
│   └── Plugins/                            # Third-party plugins
│       ├── Netcode/                        # Networking solution
│       └── PostProcessing/                 # Visual effects
├── Packages/
│   └── manifest.json                       # Package dependencies
├── ProjectSettings/
│   ├── EditorSettings.asset
│   ├── InputManager.asset
│   ├── Physics.asset
│   ├── ProjectSettings.asset
│   ├── QualitySettings.asset
│   └── TagManager.asset
├── UserSettings/
├── README.md
├── LICENSE
└── .vscode/                                # IDE configuration
    └── settings.json
```

---

## PHYSICS CONFIGURATION

### Physics Settings

**Global Physics Settings (Edit > Project Settings > Physics):**

```
Gravity: (0, -20, 0)  // Stronger than default for weighty feel
Default Solver Iterations: 10  // Higher for stable joints
Default Solver Velocity Iterations: 8
Bounce Threshold: 2
Sleep Threshold: 0.005
Default Contact Offset: 0.01
Default Max Depenetration Velocity: 10
Queries Hit Triggers: False
Queries Hit Backfaces: True
Enable Adaptive Force: True
Enable PCM: True  // Persistent Contact Manifold for better stability
Layer Collision Matrix: (configured below)
```

**Physics Layers:**

```
Layer 8: Player
Layer 9: PlayerRagdoll
Layer 10: Environment
Layer 11: Hazard
Layer 12: Projectile
Layer 13: Trigger
Layer 14: GrabTarget
Layer 15: IgnoreDuringRagdoll
```

**Layer Collision Matrix:**

| Layer | Player | PlayerRagdoll | Environment | Hazard | Projectile | Trigger | GrabTarget |
|-------|--------|---------------|-------------|---------|------------|---------|------------|
| Player | ✓ | ✗ | ✓ | ✓ | ✓ | ✗ | ✗ |
| PlayerRagdoll | ✗ | ✓ | ✓ | ✓ | ✓ | ✗ | ✓ |
| Environment | ✓ | ✓ | ✓ | ✗ | ✓ | ✗ | ✓ |
| Hazard | ✓ | ✓ | ✗ | ✗ | ✗ | ✗ | ✗ |
| Projectile | ✓ | ✓ | ✓ | ✗ | ✓ | ✗ | ✗ |
| Trigger | ✗ | ✗ | ✗ | ✗ | ✗ | ✗ | ✗ |
| GrabTarget | ✗ | ✓ | ✓ | ✗ | ✗ | ✗ | ✗ |

---

### Ragdoll Joint Configuration

**Character Rigidbody Setup:**

Each character consists of **15 rigidbodies** connected by **14 configurable joints**:

**Rigidbody Hierarchy:**
```
Pelvis (Root) [10kg]
├── Spine_Lower [3kg]
│   ├── Spine_Mid [3kg]
│   │   └── Spine_Upper [3kg]
│   │       ├── Head [2kg]
│   │       ├── Shoulder_L [1kg]
│   │       │   ├── UpperArm_L [1.5kg]
│   │       │   │   └── LowerArm_L [1kg]
│   │       │   │       └── Hand_L [0.5kg]
│   │       └── Shoulder_R [1kg]
│   │           ├── UpperArm_R [1.5kg]
│   │           │   └── LowerArm_R [1kg]
│   │           │       └── Hand_R [0.5kg]
├── UpperLeg_L [3kg]
│   └── LowerLeg_L [2kg]
│       └── Foot_L [1kg]
└── UpperLeg_R [3kg]
    └── LowerLeg_R [2kg]
        └── Foot_R [1kg]

Total Mass: ~40kg (baseline for Tentacled species)
```

**Rigidbody Settings (Per Part):**

```csharp
// Example: Pelvis Rigidbody
Mass: 10 (species-modified)
Drag: 0.5
Angular Drag: 5
Use Gravity: true
Is Kinematic: false
Interpolation: Interpolate
Collision Detection: Continuous Dynamic
Constraints: None
```

**Configurable Joint Settings:**

**Joint Type: Character Joint (Unity component)**

**Example: Spine_Lower to Pelvis**
```csharp
Connected Body: Pelvis
Anchor: (0, 0.3, 0)  // Local position on this body
Axis: (1, 0, 0)      // Twist axis
Secondary Axis: (0, 1, 0)
Auto Configure Connected Anchor: true

// Limits
Low Twist Limit: -20
High Twist Limit: 20
Swing 1 Limit: 30
Swing 2 Limit: 30

// Spring/Damper (for active control)
Spring: 100
Damper: 5
Max Force: Infinity

// Break Forces
Break Force: Infinity  // Never break joints
Break Torque: Infinity
```

**Joint Stiffness States:**

The ragdoll has two modes controlled dynamically:

**1. Active Control Mode (Player has control):**
```csharp
Spring: 200
Damper: 10
// Joints resist movement, allowing controlled animation
```

**2. Ragdoll Mode (Player knocked out):**
```csharp
Spring: 10
Damper: 1
// Joints are loose, full physics simulation
```

**Limb Joint Limits:**

| Joint | Twist (°) | Swing 1 (°) | Swing 2 (°) |
|-------|-----------|-------------|-------------|
| Spine_Lower | ±20 | ±30 | ±30 |
| Spine_Mid | ±20 | ±30 | ±30 |
| Spine_Upper | ±20 | ±30 | ±30 |
| Head | ±45 | ±70 | ±30 |
| Shoulder | ±30 | ±90 | ±160 |
| Elbow | ±0 | ±140 | ±0 |
| Wrist | ±90 | ±60 | ±60 |
| Hip | ±30 | ±100 | ±80 |
| Knee | ±0 | ±140 | ±0 |
| Ankle | ±30 | ±40 | ±40 |

---

### Physics Materials

**Player Physics Material:**
```
Dynamic Friction: 0.6
Static Friction: 0.6
Bounciness: 0.1
Friction Combine: Average
Bounce Combine: Average
```

**Environment Physics Material:**
```
Dynamic Friction: 0.7
Static Friction: 0.8
Bounciness: 0.0
Friction Combine: Maximum
Bounce Combine: Average
```

**Ice Physics Material (special arenas):**
```
Dynamic Friction: 0.05
Static Friction: 0.05
Bounciness: 0.0
```

---

### Force-Based Movement

**Player Movement System:**

Instead of directly setting velocity, movement applies forces to the pelvis rigidbody:

```csharp
// Pseudocode for movement
Vector3 inputDirection = new Vector3(input.x, 0, input.y);
Vector3 desiredVelocity = inputDirection * moveSpeed;
Vector3 velocityDelta = desiredVelocity - pelvisRigidbody.velocity;
Vector3 force = velocityDelta * movementForceMultiplier;

pelvisRigidbody.AddForce(force, ForceMode.Acceleration);
```

**Movement Parameters:**
```
Base Move Speed: 5 m/s
Movement Force Multiplier: 20
Max Ground Slope: 45°
Step Height: 0.4m
Ground Check Distance: 0.1m
```

**Jump System:**
```csharp
// Apply upward impulse to pelvis
pelvisRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

Jump Force: 8 (species-modified)
Coyote Time: 0.15s  // Grace period after leaving ground
Jump Buffer: 0.1s   // Early input buffering
```

---

## ANIMATION SYSTEM

### Animation Rigging

**Skeleton Structure:**

Characters use a **hybrid animation + physics system**:

1. **Animated Skeleton**: Drives visual mesh
2. **Physics Skeleton**: Handles collisions and forces
3. **Rig Constraint**: Visual skeleton follows physics skeleton

**Rig Hierarchy:**
```
Rig
├── Deform (skinned mesh)
│   └── Root
│       └── Pelvis
│           └── [Same hierarchy as physics skeleton]
└── PhysicsBones (Rigidbody hierarchy)
    └── [Physics skeleton from above]
```

**Animation Rigging Setup:**

Using Unity's **Animation Rigging package**:

```
Multi-Parent Constraint per visual bone:
- Source: Corresponding physics rigidbody
- Weight: 1.0 during ragdoll
- Weight: 0.0-1.0 blend during active control
```

---

### Animation State Machine

**Locomotion Layer:**

```
Entry → Idle
Idle → Walk (speed > 0.1)
Walk → Run (speed > 3.0)
Run → Walk (speed < 2.5)
Walk → Idle (speed < 0.1)
Idle/Walk/Run → Jump (jump trigger)
Jump → Fall (vertical velocity < -1)
Fall → Land (grounded)
Land → Idle (after 0.3s)
```

**Combat Layer (Additive):**

```
Entry → Neutral
Neutral → Punch (punch trigger)
Punch → Neutral (after animation)
Neutral → Grab (grab trigger)
Grab → GrabHold (holding object)
GrabHold → Throw (release trigger)
Throw → Neutral (after animation)
```

**Ragdoll Layer (Override):**

```
Entry → Active
Active → Ragdoll (knockout trigger)
Ragdoll → Recovery (recovery trigger)
Recovery → Active (after 1s)
```

**Blend Trees:**

**Locomotion Blend Tree (2D Freeform Directional):**
```
Parameters: Speed (0-10), Direction (-180 to 180)

Animations:
- Idle (0, 0)
- WalkForward (3, 0)
- WalkBackward (3, 180)
- WalkLeft (3, -90)
- WalkRight (3, 90)
- RunForward (7, 0)
- Strafe blends (intermediate angles)
```

---

### Animation Clips

**Required Animation Clips per Species:**

1. **Locomotion:**
   - Idle (looping)
   - Walk Forward/Back/Left/Right (looping)
   - Run Forward (looping)
   - Jump Start (oneshot)
   - Jump Loop (looping)
   - Fall (looping)
   - Land Light/Heavy (oneshot)

2. **Combat:**
   - Punch Left/Right (oneshot)
   - Kick (oneshot)
   - Grab Initiate (oneshot)
   - Grab Hold (looping)
   - Throw (oneshot)
   - Grabbed (reaction, looping)

3. **Ragdoll:**
   - Knockout (transition to physics)
   - Recovery GetUp Front/Back (oneshot)

4. **Emotes:**
   - Victory Pose (looping)
   - Taunt (oneshot)
   - Round Start (oneshot)

**Animation Technical Specs:**
- Frame rate: 60 FPS
- Root motion: Disabled (physics-driven movement)
- Loop time: Enabled for looping clips
- Bake into pose: Root transform position (Y), Root transform rotation

---

## RENDERING & VISUAL EFFECTS

### Universal Render Pipeline (URP) Configuration

**URP Asset Settings:**

```
Rendering Path: Forward
Depth Texture: Enabled
Opaque Texture: Enabled
Opaque Downsampling: None
Terrain Holes: Disabled

Lighting:
- Main Light: Per Pixel
- Additional Lights: Per Pixel
- Additional Lights Per Object Limit: 4
- Cast Shadows: Enabled
- Shadow Resolution: 2048
- Shadow Distance: 50
- Cascade Count: 2

Post Processing:
- Post Processing Feature Set: Integrated
- Grading Mode: High Dynamic Range
- LUT Size: 32
```

**Renderer Features:**

1. **Screen Space Ambient Occlusion**
   - Intensity: 0.5
   - Radius: 0.25
   - Sample Count: Medium

2. **Bloom**
   - Intensity: 0.3
   - Threshold: 1.0
   - Scatter: 0.7

3. **Color Adjustments**
   - Saturation: +10
   - Contrast: +5

4. **Chromatic Aberration** (UI only)
   - Intensity: 0.2

---

### Shader System

**Character Shader: Cel-Shaded with Bioluminescence**

**Shader Graph Structure:**

```
Inputs:
- Base Color (RGB)
- Emissive Color (RGB)
- Emissive Strength (Float)
- Rim Light Color (RGB)
- Rim Light Power (Float)

Main Pass:
1. Calculate stepped lighting (cel-shading)
   - Light levels: 3 (shadow, mid, highlight)
   - Smooth step transitions
2. Add Fresnel rim light (edge glow)
3. Add emissive (bioluminescence)
4. Apply outline (inverted hull technique)

Output: Unlit Base Pass
```

**Shader Features:**
- Stepped lighting (toon shading)
- Configurable emissive intensity
- Pulsing emissive (animated via script)
- Outline thickness: 0.02 units
- Fresnel rim lighting

**Environment Shader: Stylized PBR**

```
Properties:
- Albedo Map
- Normal Map
- Metallic/Smoothness Map
- Emission Map
- Tiling (Vector2)

Features:
- Triplanar mapping option (for large surfaces)
- Scrolling UVs (for energy effects)
- Vertex color support (for variation)
```

**VFX Shaders:**

1. **Particle Additive** - For energy effects, explosions
2. **Particle Alpha Blended** - For smoke, dust
3. **Distortion** - For heat waves, gravity warps
4. **Hologram** - For UI elements, scan lines

---

### Visual Effects (VFX Graph)

**Key VFX Systems:**

**1. Impact Effect**
```
Spawn: Burst of 20-50 particles on collision
Lifetime: 0.5-1.0s
Size: 0.1-0.3m
Color: Gradient (bright → transparent)
Forces: Radial outward velocity
```

**2. Movement Trail**
```
Spawn: Continuous while moving fast
Rate: 30/second
Lifetime: 0.3s
Size: 0.05-0.1m
Color: Character's bioluminescent color
Shape: Ribbon following character
```

**3. Ability Activation**
```
Spawn: Burst on ability use
Count: 100 particles
Lifetime: 1.0s
Size: 0.05-0.2m
Shape: Sphere expanding from character
Color: Energy color gradient
```

**4. Knockdown Dust**
```
Spawn: On landing impact
Count: 20-40
Lifetime: 1.0-2.0s
Size: 0.2-0.5m
Color: Arena-specific dust color
Physics: Affected by wind
```

---

## NETWORKING ARCHITECTURE

### Network Solution: Unity Netcode for GameObjects

**Rationale:**
- Official Unity solution
- Client-server architecture
- Built-in lag compensation
- Supports up to 100 CCU (more than enough for 8 players)
- Free and open-source

**Alternative:** Photon Fusion (if Unity Netcode proves insufficient)

---

### Network Topology

**Architecture: Client-Server (Authoritative Server)**

```
Host/Dedicated Server
├── NetworkManager
├── Match State Authority
├── Physics Simulation
└── Clients (1-8)
    ├── Input Sending
    ├── State Receiving
    └── Client-Side Prediction
```

**Why Client-Server:**
- Physics simulation requires authority
- Prevents cheating
- Consistent game state
- Better for 8-player physics

---

### Networked Object Hierarchy

**NetworkManager Responsibilities:**
- Connection handling
- Player spawning/despawning
- Scene management
- Lobby management
- Matchmaking

**Networked Components:**

**1. PlayerNetworkBehavior (NetworkBehaviour)**
```csharp
NetworkVariable<Vector3> position;
NetworkVariable<Quaternion> rotation;
NetworkVariable<int> health;
NetworkVariable<int> energy;
NetworkVariable<int> score;

// Input
ClientNetworkTransform (client authoritative position)
// or
ServerNetworkTransform (server authoritative, for physics)

// RPCs
[ServerRpc]
void MoveServerRpc(Vector3 direction);

[ServerRpc]
void GrabServerRpc(ulong targetNetworkId);

[ClientRpc]
void PlayAnimationClientRpc(string animationName);
```

**2. MatchNetworkManager**
```csharp
NetworkVariable<MatchState> currentState;
NetworkVariable<int> roundNumber;
NetworkVariable<float> roundTimer;
NetworkList<PlayerScore> scores;

[ServerRpc(RequireOwnership = false)]
void PlayerReadyServerRpc(ulong playerId);

[ClientRpc]
void StartRoundClientRpc();
```

---

### Physics Synchronization Strategy

**Problem:** Ragdoll physics with 15 rigidbodies per player × 8 players = 120 rigidbodies to sync

**Solution: Hybrid Synchronization**

**1. Primary Rigidbody (Pelvis) - Full Sync**
- Position synced every FixedUpdate (50Hz)
- Rotation synced every FixedUpdate
- Velocity synced every FixedUpdate
- Uses NetworkTransform

**2. Secondary Rigidbodies (Limbs) - State-Based**
- Sync when:
  - Player enters ragdoll mode (full sync all limbs)
  - Player exits ragdoll mode (sync pelvis only)
  - Limb grabbed by another player (sync that limb)
- Otherwise: Client-side prediction based on pelvis movement

**3. Optimization: Interest Management**
- Only sync players within 30m of each other
- Reduce update rate for distant players (50Hz → 20Hz)

**Code Example:**
```csharp
// RagdollNetworkSync.cs
public class RagdollNetworkSync : NetworkBehaviour
{
    [SerializeField] private Rigidbody pelvisRb;
    [SerializeField] private Rigidbody[] limbRbs;

    private NetworkVariable<bool> isRagdoll = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(SyncPrimaryRigidbody());
        }
    }

    private IEnumerator SyncPrimaryRigidbody()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            SyncPelvisClientRpc(pelvisRb.position, pelvisRb.rotation, pelvisRb.velocity);
        }
    }

    [ClientRpc]
    private void SyncPelvisClientRpc(Vector3 pos, Quaternion rot, Vector3 vel)
    {
        if (!IsOwner)
        {
            pelvisRb.position = pos;
            pelvisRb.rotation = rot;
            pelvisRb.velocity = vel;
        }
    }

    public void EnterRagdoll()
    {
        if (IsServer)
        {
            isRagdoll.Value = true;
            SyncAllLimbsClientRpc(GetLimbStates());
        }
    }
}
```

---

### Lobby & Matchmaking Flow

**Lobby System:**

```
1. Main Menu
   ↓
2. Play Button
   ↓
3. Choose Mode:
   - Host Game (become server)
   - Join Game (find available lobbies)
   - Quick Match (automatic matchmaking)
   ↓
4. Lobby Screen
   - Show connected players (1-8)
   - Character customization preview
   - Arena voting
   - Ready toggle
   ↓
5. All Players Ready
   ↓
6. Start Match (load arena scene)
```

**Matchmaking Implementation:**

**Option A: Simple Relay (Unity Relay Service)**
- Host creates relay code
- Other players join via code
- No dedicated servers needed
- Free tier: 1GB/month

**Option B: Steam Networking (if Steam release)**
- Steam P2P matchmaking
- Friends can join via invite
- Integrated with Steam overlay

**Code Structure:**
```csharp
// LobbyManager.cs
public class LobbyManager : NetworkBehaviour
{
    private NetworkList<PlayerLobbyData> players;

    [ServerRpc(RequireOwnership = false)]
    public void PlayerReadyServerRpc(ulong playerId)
    {
        // Mark player ready
        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        if (AllPlayersReady() && players.Count >= 2)
        {
            StartMatchClientRpc();
        }
    }

    [ClientRpc]
    private void StartMatchClientRpc()
    {
        NetworkManager.SceneManager.LoadScene("Arena_GravityWell", LoadSceneMode.Single);
    }
}
```

---

## INPUT SYSTEM

**Using Unity's New Input System**

**Input Actions Asset:**

```
Action Maps:
1. Gameplay
   - Move (Vector2, Gamepad Left Stick / WASD)
   - Jump (Button, Gamepad South / Space)
   - Grab (Button, Gamepad West / E)
   - Punch (Button, Gamepad East / LMB)
   - Ability (Button, Gamepad North / Q)
   - Crouch (Button, Gamepad L1 / Ctrl)

2. UI
   - Navigate (Vector2, Gamepad DPad / Arrow Keys)
   - Submit (Button, Gamepad South / Enter)
   - Cancel (Button, Gamepad East / Escape)

3. Pause
   - Pause (Button, Gamepad Start / Escape)
```

**Control Schemes:**
- Keyboard & Mouse
- Gamepad (Xbox/PlayStation/Generic)

**Multiplayer Input:**
- Uses PlayerInput component
- Auto-assigns control schemes to players
- Supports up to 8 local players (split controllers)

---

## ASSET PIPELINE

### 3D Model Workflow

**Character Models:**
```
Creation: Blender 3.6+
Poly Count: 5,000-8,000 tris per character
UV Layout: Single UV map, 2048x2048 texture
Rig: Exported as FBX with skeleton
Export Settings:
  - Scale: 1.0
  - Forward: -Z Forward
  - Up: Y Up
  - Apply Transform: Yes
  - Bake Animation: No (separate files)
```

**Environment Models:**
```
Creation: Blender / Procedural generation
Poly Count Budget:
  - Small props: 500-2,000 tris
  - Medium props: 2,000-5,000 tris
  - Large structures: 5,000-15,000 tris
  - Arenas: 30,000-50,000 tris total
LOD System: 3 levels (100%, 50%, 25% poly count)
Texture Resolution:
  - Props: 512x512 or 1024x1024
  - Environments: 2048x2048
```

---

### Texture & Material Workflow

**Texture Creation:**
- Software: Substance Painter / Photoshop
- Format: PNG (diffuse/emissive), TGA (normal)
- Compression:
  - Diffuse: RGB Compressed DXT1
  - Normal: RGB Compressed DXT5
  - Emissive: RGB Compressed DXT1

**Material Naming Convention:**
```
MAT_CharacterName_PartName_Variant
Example: MAT_Gelatinous_Body_Blue
```

---

### Animation Workflow

**Animation Creation:**
- Software: Blender (using character rig)
- Frame rate: 60 FPS
- Export: FBX with only animation data
- Naming: ANIM_Action_Variant

**Animation Import Settings:**
```
Rig: Humanoid (for generic anims) or Generic (for specific rigs)
Avatar Definition: Create from This Model
Optimize Game Objects: Yes
Import Constraints: No
Import Animation: Yes
Bake Animations: Yes
Resample Curves: Yes
Anim Compression: Keyframe Reduction
```

---

### Audio Workflow

**Audio Specifications:**
```
Music:
  - Format: .OGG (compressed)
  - Sample Rate: 44.1 kHz
  - Bitrate: 192 kbps
  - Channels: Stereo

SFX:
  - Format: .WAV (short) / .OGG (long)
  - Sample Rate: 44.1 kHz
  - Bit Depth: 16-bit
  - Channels: Mono (positional) / Stereo (UI)

Load Type:
  - Music: Streaming
  - Short SFX (<1s): Decompress on Load
  - Long SFX: Compressed in Memory
```

---

## PERFORMANCE OPTIMIZATION

### Target Performance Metrics

```
Target Frame Rate: 60 FPS (stable)
Maximum Frame Time: 16.6ms

Breakdown:
- Physics: <4ms
- Rendering: <8ms
- Scripting: <3ms
- Other: <1.6ms

Player Count: 8
Physics Objects: ~120 rigidbodies active
Particle Systems: ~20 active
```

---

### Optimization Strategies

**1. Physics Optimization**
- Use fixed timestep: 0.02s (50Hz physics)
- Limit rigidbody count via smart sync
- Sleep inactive rigidbodies
- Simplify collision meshes (use primitives where possible)
- Physics layer matrix optimization

**2. Rendering Optimization**
- Occlusion culling on environment
- LOD groups on characters (3 levels)
- Texture atlasing for UI
- Batching: Static batching for environments, GPU instancing for particles
- Shadow cascade optimization

**3. Scripting Optimization**
- Object pooling for VFX, projectiles
- Avoid GetComponent in Update() (cache references)
- Use Unity Jobs System for complex calculations
- Burst compile math-heavy code

**4. Network Optimization**
- Interest management (distance-based)
- Variable update rates (important objects 50Hz, others 20Hz)
- Delta compression for networked variables
- RPCs only when necessary (prefer NetworkVariables)

---

## MODULAR SYSTEM DESIGN

### Core Systems Architecture

**Dependency Graph:**

```
GameManager (Singleton)
├── MatchManager
│   ├── RoundManager
│   ├── ScoreManager
│   └── ArenaEvolutionManager
├── NetworkManager
│   ├── LobbyManager
│   └── MatchmakingManager
├── AudioManager
│   ├── MusicManager
│   └── SFXManager
└── UIManager
    ├── HUDManager
    └── MenuManager

PlayerController (per player)
├── PlayerInput
├── RagdollController
│   └── SpeciesPhysics (polymorphic)
├── GrabSystem
├── CombatSystem
├── AbilitySystem
└── PlayerAnimator
```

---

### Design Patterns Used

**1. Singleton Pattern**
- GameManager, AudioManager, NetworkManager
- Ensures single instance, global access

**2. Observer Pattern**
- Event system for game state changes
- Example: OnRoundStart, OnPlayerKO, OnMatchEnd

**3. Component Pattern**
- PlayerController composed of modular systems
- Easy to add/remove features

**4. Object Pool Pattern**
- VFX particles
- Projectiles
- UI elements (score popups)

**5. State Pattern**
- MatchState (Lobby → Playing → RoundEnd → MatchEnd)
- PlayerState (Idle → Moving → Jumping → Ragdoll → Recovery)

**6. Strategy Pattern**
- SpeciesPhysics (different physics per species)
- BotDifficulty (easy/medium/hard AI)

---

### Scriptable Objects for Data

**GameData ScriptableObjects:**

```csharp
// SpeciesData.asset
[CreateAssetMenu(fileName = "Species", menuName = "TumbleRumble/SpeciesData")]
public class SpeciesData : ScriptableObject
{
    public string speciesName;
    public GameObject characterPrefab;
    public float massMultiplier;
    public float speedMultiplier;
    public float jumpMultiplier;
    public AudioClip[] movementSounds;
    public Material[] availableMaterials;
}

// ArenaData.asset
[CreateAssetMenu(fileName = "Arena", menuName = "TumbleRumble/ArenaData")]
public class ArenaData : ScriptableObject
{
    public string arenaName;
    public string sceneRef;
    public Sprite previewImage;
    public ArenaEvolutionData[] evolutionStages;
    public AudioClip ambientMusic;
}

// AbilityData.asset
[CreateAssetMenu(fileName = "Ability", menuName = "TumbleRumble/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string abilityName;
    public int energyCost;
    public float cooldown;
    public GameObject vfxPrefab;
    public AudioClip activationSound;
}
```

---

## TESTING INFRASTRUCTURE

### Test Scenes

**1. PhysicsTest.unity**
- Single arena
- 8 player spawn points
- Debug UI for force values, joint states
- Adjustable physics parameters via UI

**2. NetworkingTest.unity**
- Local network simulation
- Latency simulator (add artificial lag)
- Packet loss simulator
- 2-4 player test

**3. PerformanceTest.unity**
- Full 8 players
- All VFX active
- FPS counter
- Profiler markers

---

### Unit Tests

**Unity Test Framework:**

```csharp
// Example: ScoreManagerTests.cs
public class ScoreManagerTests
{
    [Test]
    public void AddScore_StandardKO_Adds1Point()
    {
        var scoreManager = new ScoreManager();
        scoreManager.AddScore(playerId: 1, KOType.Standard);
        Assert.AreEqual(1, scoreManager.GetScore(1));
    }

    [Test]
    public void AddScore_AerialKO_Adds3Points()
    {
        var scoreManager = new ScoreManager();
        scoreManager.AddScore(playerId: 1, KOType.Aerial);
        Assert.AreEqual(3, scoreManager.GetScore(1));
    }
}
```

---

## BUILD CONFIGURATION

### Build Targets

**1. Windows (x64)**
```
Scripting Backend: IL2CPP
API Compatibility: .NET Standard 2.1
Compression: LZ4 (faster builds)
```

**2. macOS (Universal)**
```
Scripting Backend: IL2CPP
Target: macOS (Intel + Apple Silicon)
```

**3. Linux (x64)**
```
Scripting Backend: Mono (better compatibility)
API Compatibility: .NET Standard 2.1
```

---

### Build Pipeline

**Build Process:**
1. Increment version number (semantic versioning)
2. Run unit tests
3. Build player (with debugging symbols for alpha/beta)
4. Package with required data files
5. Generate changelog
6. Create installer (optional: Steam upload)

---

## SUMMARY

This completes **PHASE 2: Technical Architecture Blueprint**.

**What We've Defined:**
✓ Engine selection (Unity 2022.3 LTS with URP)
✓ Complete project folder structure
✓ Physics configuration (joints, constraints, materials)
✓ Ragdoll system with 15 rigidbodies per character
✓ Animation system (blend trees, state machines)
✓ Rendering pipeline (URP, shaders, VFX)
✓ Networking architecture (Unity Netcode, client-server)
✓ Input system (New Input System, 8-player support)
✓ Asset pipeline (3D, textures, audio workflows)
✓ Performance targets and optimization strategies
✓ Modular system design with design patterns
✓ Testing infrastructure

**Next Phase:** Full Working Code Generation

---

*Document Version: 1.0*
*Last Updated: 2025-11-15*
*Status: PHASE 2 COMPLETE ✓*
