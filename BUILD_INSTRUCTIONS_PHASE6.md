# TUMBLE RUMBLE: COMPLETE BUILD INSTRUCTIONS
## Phase 6: Installation, Setup, Building & Deployment

Exhaustive step-by-step instructions to build and run the game from scratch.

---

## TABLE OF CONTENTS

1. Prerequisites & Installation
2. Unity Project Setup
3. Package Installation & Configuration
4. Asset Import & Organization
5. Scene Setup
6. Testing & Debugging
7. Building for Platforms (Windows/Mac/Linux)
8. Multiplayer Configuration & Packaging
9. Troubleshooting Guide

---

# 1. PREREQUISITES & INSTALLATION

## 1.1 System Requirements

### Minimum Development Machine:
```
CPU: Intel i5 / AMD Ryzen 5 (4 cores)
RAM: 16 GB
GPU: NVIDIA GTX 1050 / AMD RX 560 (2GB VRAM)
Storage: 20 GB free space (SSD recommended)
OS: Windows 10/11, macOS 10.15+, or Ubuntu 20.04+
```

### Recommended Development Machine:
```
CPU: Intel i7 / AMD Ryzen 7 (8 cores)
RAM: 32 GB
GPU: NVIDIA RTX 3060 / AMD RX 6700 XT (8GB VRAM)
Storage: 50 GB free space (NVMe SSD)
OS: Windows 11, macOS 12+, or Ubuntu 22.04+
```

---

## 1.2 Software Installation

### Step 1: Install Unity Hub
```
1. Download Unity Hub from: https://unity.com/download
2. Run installer
3. Complete installation wizard
4. Launch Unity Hub
5. Sign in with Unity account (or create free account)
```

### Step 2: Install Unity 2022.3 LTS
```
1. Open Unity Hub
2. Click "Installs" tab
3. Click "Install Editor"
4. Select version: 2022.3.x LTS (latest patch)
5. Click "Next"
6. Select modules:
   ☑ Windows Build Support (IL2CPP)
   ☑ Mac Build Support (Mono)
   ☑ Linux Build Support (Mono)
   ☑ Documentation
   ☑ Language Pack (optional)
7. Click "Install"
8. Wait for installation (15-30 minutes)
```

### Step 3: Install Visual Studio / Visual Studio Code
```
Option A: Visual Studio 2022 (Windows, recommended for debugging)
- Download from: https://visualstudio.microsoft.com/
- Install workloads:
  ☑ Game development with Unity
  ☑ .NET desktop development

Option B: Visual Studio Code (Cross-platform, lightweight)
- Download from: https://code.visualstudio.com/
- Install extensions:
  - C# (Microsoft)
  - Unity Code Snippets
  - Debugger for Unity
```

### Step 4: Install Git (Version Control)
```
1. Download from: https://git-scm.com/downloads
2. Run installer with default settings
3. Verify installation:
   - Open terminal/command prompt
   - Run: git --version
   - Should display: git version 2.x.x
```

---

# 2. UNITY PROJECT SETUP

## 2.1 Create New Unity Project

### Method A: Unity Hub
```
1. Open Unity Hub
2. Click "Projects" tab
3. Click "New Project"
4. Template: 3D (URP) - Universal Render Pipeline
5. Project Name: TumbleRumble
6. Location: Choose directory (e.g., C:\Unity\Projects\)
7. Click "Create Project"
8. Wait for project initialization (2-5 minutes)
```

### Method B: Clone from Repository (if you've already set up Git)
```bash
# Open terminal/command prompt
cd /path/to/your/projects/
git clone <your-repo-url> TumbleRumble
cd TumbleRumble

# Open in Unity Hub
# Click "Open" → Navigate to TumbleRumble folder → Select
```

---

## 2.2 Project Structure Setup

### Create Folder Hierarchy
```
In Unity Project window, create this structure:

Assets/
├── _Project/
│   ├── Art/
│   │   ├── Characters/
│   │   ├── Environments/
│   │   ├── UI/
│   │   ├── VFX/
│   │   └── Shaders/
│   ├── Audio/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── Mixers/
│   ├── Scripts/              [Copy scripts from Phase 3 here]
│   │   ├── Core/
│   │   ├── Player/
│   │   ├── Camera/
│   │   ├── Environment/
│   │   ├── UI/
│   │   ├── Networking/
│   │   ├── AI/
│   │   ├── Audio/
│   │   ├── Utilities/
│   │   └── SelfHealing/
│   ├── Prefabs/
│   │   ├── Characters/
│   │   ├── Arenas/
│   │   ├── Hazards/
│   │   ├── VFX/
│   │   └── UI/
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── Lobby.unity
│   │   ├── Arenas/
│   │   └── TestScenes/
│   ├── Resources/
│   └── Settings/
└── Plugins/
```

---

# 3. PACKAGE INSTALLATION & CONFIGURATION

## 3.1 Install Required Packages

### Open Package Manager
```
Unity Editor → Window → Package Manager
```

### Install Core Packages
```
1. Universal RP (URP)
   - Should be pre-installed with template
   - If not: Search "Universal RP" → Install
   - Version: 14.x or higher

2. Input System
   - Search "Input System"
   - Click "Install"
   - Click "Yes" when prompted to restart

3. Cinemachine (for camera system)
   - Search "Cinemachine"
   - Install

4. TextMeshPro
   - Search "TextMeshPro"
   - Install
   - Import TMP Essentials when prompted

5. ProBuilder (for level prototyping)
   - Search "ProBuilder"
   - Install
```

### Install Networking Package

**Option A: Unity Netcode for GameObjects (Recommended)**
```
1. Package Manager → "+" → "Add package from git URL"
2. Enter: com.unity.netcode.gameobjects
3. Click "Add"
4. Wait for installation

Alternative: Manual installation
- Download from: https://github.com/Unity-Technologies/com.unity.netcode.gameobjects
- Package Manager → "+" → "Add package from disk"
- Select package.json
```

**Option B: Photon Fusion (Alternative)**
```
1. Asset Store window (Window → Asset Store)
2. Search "Photon Fusion"
3. Download and Import
```

---

## 3.2 Configure Unity Settings

### Project Settings (Edit → Project Settings)

#### **Player Settings**
```
Company Name: [Your Studio Name]
Product Name: Tumble Rumble
Version: 0.1.0
Default Icon: [Import and assign game icon]

Other Settings:
- Color Space: Linear
- Auto Graphics API: Disabled
  - Windows: DirectX11, DirectX12
  - macOS: Metal
  - Linux: Vulkan, OpenGL
- Scripting Backend: IL2CPP (for better performance)
- API Compatibility Level: .NET Standard 2.1
```

#### **Physics Settings**
```
Gravity: (0, -20, 0)
Default Solver Iterations: 10
Default Solver Velocity Iterations: 8
Bounce Threshold: 2
Sleep Threshold: 0.005
Default Contact Offset: 0.01
Queries Hit Triggers: False
Enable Adaptive Force: True
Enable Enhanced Determinism: True

Layer Collision Matrix:
[Set according to Phase 2 specifications]
```

#### **Time Settings**
```
Fixed Timestep: 0.02 (50 Hz physics)
Maximum Allowed Timestep: 0.333333 (avoid physics spiral)
Time Scale: 1
```

#### **Quality Settings**
```
Create 3 quality levels:

1. Low (for older hardware):
   - Shadow Resolution: 1024
   - Shadow Distance: 30
   - VSync: Off
   - Anti-Aliasing: Disabled
   - Texture Quality: Half Res

2. Medium (default):
   - Shadow Resolution: 2048
   - Shadow Distance: 50
   - VSync: Off
   - Anti-Aliasing: 2x MSAA
   - Texture Quality: Full Res

3. High (for modern hardware):
   - Shadow Resolution: 4096
   - Shadow Distance: 75
   - VSync: Off
   - Anti-Aliasing: 4x MSAA
   - Texture Quality: Full Res
```

#### **Input System**
```
1. Active Input Handling: Input System Package (New)
2. Click "Apply" (Unity will restart)
3. Create Input Actions asset:
   - Right-click in Project → Create → Input Actions
   - Name: "InputActions"
   - Location: Assets/_Project/Settings/

4. Configure Input Actions (double-click InputActions asset):

   Action Maps:

   [Gameplay]
   - Move: Value, Vector2, Gamepad Left Stick / WASD
   - Jump: Button, Gamepad South / Space
   - Grab: Button, Gamepad West / E
   - Punch: Button, Gamepad East / LMB
   - Ability: Button, Gamepad North / Q
   - Crouch: Button, Gamepad L1 / Ctrl

   [UI]
   - Navigate: Value, Vector2, Gamepad DPad / Arrow Keys
   - Submit: Button, Gamepad South / Enter
   - Cancel: Button, Gamepad East / Escape

   [Pause]
   - Pause: Button, Gamepad Start / Escape

5. Click "Save Asset"
6. Enable "Generate C# Class"
7. Click "Apply"
```

#### **Tags & Layers**
```
Create Tags:
- Player
- Enemy
- Hazard
- Grabbable
- Projectile

Create Layers:
- Layer 8: Player
- Layer 9: PlayerRagdoll
- Layer 10: Environment
- Layer 11: Hazard
- Layer 12: Projectile
- Layer 13: Trigger
- Layer 14: GrabTarget
- Layer 15: IgnoreDuringRagdoll
```

---

# 4. ASSET IMPORT & ORGANIZATION

## 4.1 Import Scripts

### Copy All Scripts from Phase 3
```
1. Copy all .cs files from this repository's Scripts folder
2. Paste into Assets/_Project/Scripts/
3. Wait for Unity to compile (check bottom-right progress bar)
4. Check for compilation errors in Console (Ctrl+Shift+C)
5. Fix any namespace or missing reference errors
```

### Verify Script Compilation
```
If errors appear:
1. Double-check all scripts are in correct folders
2. Verify Unity version matches (2022.3 LTS)
3. Ensure all required packages are installed
4. Check for typos in class names
```

---

## 4.2 Create Basic Assets

### Create Physics Materials

**Player Physics Material:**
```
1. Right-click in Project → Create → Physic Material
2. Name: "PhysicsMaterial_Player"
3. Settings:
   - Dynamic Friction: 0.6
   - Static Friction: 0.6
   - Bounciness: 0.1
   - Friction Combine: Average
   - Bounce Combine: Average
```

**Environment Physics Material:**
```
1. Create → Physic Material
2. Name: "PhysicsMaterial_Environment"
3. Settings:
   - Dynamic Friction: 0.7
   - Static Friction: 0.8
   - Bounciness: 0.0
```

---

### Create Placeholder Materials

**Character Material (Cel-Shaded):**
```
1. Right-click → Create → Material
2. Name: "MAT_Character_Cyan"
3. Shader: Select custom shader when created (or URP/Lit for now)
4. Base Color: Cyan (#00FFFF)
5. Metallic: 0
6. Smoothness: 0.5
7. Emission: Enabled, Color: Cyan, Intensity: 1.0
```

**Platform Material:**
```
1. Create → Material
2. Name: "MAT_Platform"
3. Shader: URP/Lit
4. Base Color: Dark Gray (#404040)
5. Metallic: 0.3
6. Smoothness: 0.6
```

---

## 4.3 Create Character Prefab (Prototype)

### Step-by-Step Character Creation
```
1. Create Empty GameObject: "Player_Gelatinous"

2. Add Main Rigidbody (Pelvis):
   - Add Component → Rigidbody
   - Mass: 40
   - Drag: 0.5
   - Angular Drag: 5
   - Use Gravity: True
   - Is Kinematic: False
   - Interpolate: Interpolate
   - Collision Detection: Continuous Dynamic

3. Add Capsule Collider:
   - Height: 2
   - Radius: 0.5
   - Center: (0, 1, 0)

4. Add Visual (Temporary):
   - Create child: Cube (scaled 0.8x1.5x0.8)
   - Apply MAT_Character_Cyan

5. Add Scripts:
   - PlayerController
   - RagdollController
   - PlayerInput
   - GrabSystem
   - CombatSystem
   - AbilitySystem

6. Configure PlayerController:
   - Pelvis Rigidbody: Assign main Rigidbody
   - Move Speed: 5
   - Jump Force: 8
   - Ground Layer: Everything (for now)

7. Save as Prefab:
   - Drag to Assets/_Project/Prefabs/Characters/
```

---

# 5. SCENE SETUP

## 5.1 Create Main Menu Scene

```
1. File → New Scene
2. Create UI Canvas:
   - GameObject → UI → Canvas
   - Render Mode: Screen Space - Overlay
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080

3. Add Background:
   - Right-click Canvas → UI → Image
   - Name: "Background"
   - Color: Dark blue (#0a0033)
   - Stretch to fill

4. Add Title Text:
   - Right-click Canvas → UI → Text - TextMeshPro
   - Name: "TitleText"
   - Text: "TUMBLE RUMBLE"
   - Font Size: 120
   - Alignment: Center
   - Position: Top-center

5. Add Buttons:
   - UI → Button - TextMeshPro (x4)
   - Names: "PlayButton", "CustomizeButton", "SettingsButton", "QuitButton"
   - Arrange vertically
   - Text: "PLAY", "CUSTOMIZE", "SETTINGS", "QUIT"

6. Add MainMenu Script:
   - Create Empty GameObject: "MenuManager"
   - Add Component: MainMenu (from Phase 3)
   - Assign button references in Inspector

7. Save Scene:
   - File → Save As
   - Name: "MainMenu"
   - Location: Assets/_Project/Scenes/
```

---

## 5.2 Create Test Arena Scene

```
1. File → New Scene
2. Save As: "Arena_TestPhysics"

3. Create Ground:
   - GameObject → 3D Object → Plane
   - Name: "Ground"
   - Scale: (5, 1, 5) for 50m x 50m
   - Material: MAT_Platform
   - Layer: Environment
   - Add Component: Box Collider (if needed)

4. Create Platform:
   - GameObject → 3D Object → Cylinder
   - Name: "MainPlatform"
   - Scale: (15, 0.5, 15) for 30m diameter
   - Position: (0, 0.5, 0)
   - Material: MAT_Platform
   - Layer: Environment

5. Create Void Zone:
   - GameObject → 3D Object → Cube
   - Name: "VoidZone"
   - Position: (0, -10, 0)
   - Scale: (100, 1, 100)
   - Remove Mesh Renderer
   - Mesh Collider → Is Trigger: True
   - Add Component: VoidZone script
   - Layer: Hazard

6. Add Lighting:
   - Directional Light (default should exist)
   - Rotation: (50, -30, 0)
   - Intensity: 1
   - Color: White

7. Add Player Spawn Points:
   - Create Empty GameObjects (x8)
   - Names: "SpawnPoint_1" through "SpawnPoint_8"
   - Positions: Arrange in circle around platform
   - Tag: Respawn (create tag if needed)

8. Add Camera:
   - GameObject → Camera
   - Name: "DynamicCamera"
   - Position: (0, 15, -10)
   - Rotation: (45, 0, 0)
   - Add Component: DynamicCamera script

9. Add Game Managers:
   - Create Empty: "GameManagers"
   - Add child: "GameManager" with GameManager script
   - Add child: "MatchManager" with MatchManager script
   - Add child: "RoundManager" with RoundManager script
   - Add child: "ScoreManager" with ScoreManager script

10. Add Audio Manager:
    - Create Empty: "AudioManager"
    - Add AudioManager script

11. Spawn Test Players:
    - Drag Player_Gelatinous prefab into scene (x2 for testing)
    - Position at spawn points 1 and 2

12. Save Scene
```

---

# 6. TESTING & DEBUGGING

## 6.1 Initial Play Test

```
1. Open Arena_TestPhysics scene
2. Click Play button (Ctrl+P)
3. Verify:
   ☑ Players spawn correctly
   ☑ Gravity affects players
   ☑ Camera tracks players
   ☑ Input works (WASD to move, Space to jump)
   ☑ Console has no errors
4. Click Play again to stop (Ctrl+P)
```

## 6.2 Debug Controls

### Enable Debug Mode in GameManager:
```
1. Select GameManager in Hierarchy
2. In Inspector, enable "Debug Mode"
3. In Play Mode:
   - P key: Pause/unpause
   - Additional debug shortcuts as needed
```

### Console Filtering:
```
- Click Console tab (Ctrl+Shift+C)
- Enable "Error Pause" (pause on errors)
- Filter by: Error, Warning, Log
- Clear on Play: Enabled
```

---

## 6.3 Performance Testing

### Unity Profiler:
```
1. Window → Analysis → Profiler
2. Click "Record"
3. Enter Play Mode
4. Monitor:
   - CPU Usage (should be < 16ms for 60 FPS)
   - Rendering (draw calls, batches)
   - Physics (physics time)
   - Memory (total allocations)
5. Look for spikes and optimize accordingly
```

### Frame Debugger:
```
1. Window → Analysis → Frame Debugger
2. Click "Enable"
3. Step through render calls
4. Identify expensive draws
```

---

# 7. BUILDING FOR PLATFORMS

## 7.1 Build Settings Configuration

### Open Build Settings
```
File → Build Settings (Ctrl+Shift+B)
```

### Add Scenes to Build
```
1. Click "Add Open Scenes" (for current scene)
2. Or drag scenes from Project window:
   - MainMenu
   - Lobby
   - All Arena scenes

Order:
[0] MainMenu
[1] Lobby
[2] Arena_GravityWell
[3] Arena_CrystalCavern
... (all arenas)
```

---

## 7.2 Windows Build (x64)

```
1. Build Settings → Platform → PC, Mac & Linux Standalone
2. Click "Switch Platform" (if not already selected)
3. Target Platform: Windows
4. Architecture: x86_64

5. Player Settings:
   - Icon: Assign game icon
   - Splash Screen: Customize (optional)
   - Resolution:
     ☑ Fullscreen Mode: Fullscreen Window
     ☑ Default Screen Width: 1920
     ☑ Default Screen Height: 1080
     ☑ Resizable Window: True

   - Other Settings:
     - Scripting Backend: IL2CPP
     - API Compatibility: .NET Standard 2.1
     - Allow 'unsafe' Code: False
     - Active Input Handling: Input System Package

6. Click "Build"
7. Choose build folder: "Builds/Windows/"
8. Name: "TumbleRumble_Windows"
9. Click "Select Folder"
10. Wait for build (5-20 minutes)

Build Output:
Builds/Windows/
├── TumbleRumble_Windows.exe
├── TumbleRumble_Windows_Data/
├── MonoBleedingEdge/ (if Mono backend)
└── UnityCrashHandler64.exe
```

---

## 7.3 macOS Build (Universal)

```
1. Build Settings → Platform → PC, Mac & Linux Standalone
2. Click "Switch Platform"
3. Target Platform: macOS
4. Architecture: Apple Silicon + Intel (Universal)

5. Player Settings (same as Windows, plus):
   - Bundle Identifier: com.yourstudio.tumblerumble
   - Version: 0.1.0
   - Build Number: 1
   - Camera Usage Description: (if using camera)
   - Microphone Usage Description: (if using mic)
   - Target minimum macOS Version: 10.15

6. Click "Build"
7. Choose: "Builds/macOS/"
8. Name: "TumbleRumble_macOS"
9. Build

Build Output:
Builds/macOS/
└── TumbleRumble_macOS.app (application bundle)

Post-Build (for distribution):
- Code sign the app (requires Apple Developer account)
- Notarize for macOS 10.15+ (Apple requirement)
```

---

## 7.4 Linux Build (x64)

```
1. Build Settings → Platform → PC, Mac & Linux Standalone
2. Target Platform: Linux
3. Architecture: x86_64

4. Player Settings (similar to Windows)

5. Click "Build"
6. Choose: "Builds/Linux/"
7. Name: "TumbleRumble_Linux"
8. Build

Build Output:
Builds/Linux/
├── TumbleRumble_Linux.x86_64 (executable)
└── TumbleRumble_Linux_Data/
```

---

## 7.5 Build Optimization

### Reduce Build Size
```
Player Settings → Other Settings:
- Managed Stripping Level: High
- Strip Engine Code: True (requires testing)
- Vertex Compression: Everything
- Optimize Mesh Data: True

Publishing Settings:
- Compression Method: LZ4 (faster) or LZ4HC (smaller)
```

### Build Automation Script
```csharp
// Place in Editor folder
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build All Platforms")]
    public static void BuildAll()
    {
        BuildWindows();
        BuildMacOS();
        BuildLinux();
    }

    static void BuildWindows()
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = "Builds/Windows/TumbleRumble.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        LogBuildResult(report, "Windows");
    }

    static void BuildMacOS()
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = "Builds/macOS/TumbleRumble.app",
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        LogBuildResult(report, "macOS");
    }

    static void BuildLinux()
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = "Builds/Linux/TumbleRumble.x86_64",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        LogBuildResult(report, "Linux");
    }

    static string[] GetScenes()
    {
        return new string[]
        {
            "Assets/_Project/Scenes/MainMenu.unity",
            "Assets/_Project/Scenes/Lobby.unity",
            // Add all arena scenes
        };
    }

    static void LogBuildResult(BuildReport report, string platform)
    {
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"{platform} build succeeded: {report.summary.totalSize} bytes");
        }
        else
        {
            Debug.LogError($"{platform} build failed");
        }
    }
}
```

---

# 8. MULTIPLAYER CONFIGURATION & PACKAGING

## 8.1 Unity Netcode Setup

### Install Transport
```
Package Manager → Add package from git URL:
com.unity.transport
```

### Configure Network Manager
```
1. In Lobby scene, create: "NetworkManager"
2. Add Component: Unity.Netcode.NetworkManager
3. Settings:
   - Network Transport: Unity Transport
   - Player Prefab: Drag Player_Gelatinous prefab
   - Default Connection Approval: False (for testing)
   - Tick Rate: 60 (matches physics)

4. Unity Transport Component:
   - Connection Data:
     - Address: 127.0.0.1 (localhost for testing)
     - Port: 7777
     - Max Connections: 8
```

### Make Player Prefab Network-Ready
```
1. Open Player_Gelatinous prefab
2. Add Component: NetworkObject
3. Settings:
   - Synchronize Transform: True
   - Is Player Object: True
   - Destroy With Scene: False

4. Save Prefab
```

### Test Multiplayer Locally
```
1. Build the game first (File → Build)
2. Run the built game (Host)
3. In Unity Editor, click Play (Client)
4. In Host game: Click "Start Host"
5. In Editor game: Click "Start Client"
6. Verify connection in Console
```

---

## 8.2 Unity Relay Service (For Internet Play)

### Setup Unity Services
```
1. Window → Services
2. Create Unity Project ID
3. Enable "Relay" service
4. Install package: com.unity.services.relay
```

### Relay Integration Code
```csharp
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public async void StartHostWithRelay()
{
    await UnityServices.InitializeAsync();

    Allocation allocation = await RelayService.Instance.CreateAllocationAsync(8);
    string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

    // Display joinCode to host
    Debug.Log($"Join Code: {joinCode}");

    NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
        allocation.RelayServer.IpV4,
        (ushort)allocation.RelayServer.Port,
        allocation.AllocationIdBytes,
        allocation.Key,
        allocation.ConnectionData
    );

    NetworkManager.Singleton.StartHost();
}

public async void JoinWithCode(string joinCode)
{
    await UnityServices.InitializeAsync();

    JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

    NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
        joinAllocation.RelayServer.IpV4,
        (ushort)joinAllocation.RelayServer.Port,
        joinAllocation.AllocationIdBytes,
        joinAllocation.Key,
        joinAllocation.ConnectionData,
        joinAllocation.HostConnectionData
    );

    NetworkManager.Singleton.StartClient();
}
```

---

## 8.3 Steam Integration (Optional)

### Steamworks.NET Setup
```
1. Download Steamworks.NET from GitHub
2. Import into Unity
3. Create steam_appid.txt in project root
4. Add your Steam App ID

5. Initialize Steamworks:
```csharp
using Steamworks;

void Start()
{
    if (!SteamAPI.Init())
    {
        Debug.LogError("Steam initialization failed!");
        return;
    }
    Debug.Log("Steam initialized successfully");
}

void OnDestroy()
{
    SteamAPI.Shutdown();
}
```

### Steam Build Upload
```
1. Build game normally
2. Use Steamworks SDK's ContentBuilder
3. Configure depot manifest
4. Upload to Steam (requires Steamworks partner account)
```

---

# 9. TROUBLESHOOTING GUIDE

## Common Issues & Solutions

### Issue: Scripts don't compile
```
Solution:
1. Check Unity version (must be 2022.3 LTS)
2. Verify all packages installed
3. Delete Library folder, restart Unity
4. Reimport all scripts
```

### Issue: Physics acts strangely
```
Solution:
1. Verify Time → Fixed Timestep = 0.02
2. Check Physics.gravity = (0, -20, 0)
3. Ensure layer collision matrix is correct
4. Check rigidbody masses > 0
```

### Issue: Input doesn't work
```
Solution:
1. Verify Input System package installed
2. Check Active Input Handling = Input System Package
3. Ensure InputActions asset exists and is enabled
4. Verify PlayerInput component references correct action map
```

### Issue: Multiplayer doesn't connect
```
Solution:
1. Check firewall settings (allow port 7777)
2. Verify NetworkManager settings
3. Test with localhost first
4. Check Console for network errors
```

### Issue: Build crashes on startup
```
Solution:
1. Check for missing scenes in Build Settings
2. Verify all assets are included (not editor-only)
3. Test in development build with profiler attached
4. Check crash logs (Unity Player.log location varies by OS)
```

### Issue: Poor performance in build
```
Solution:
1. Enable IL2CPP instead of Mono
2. Set Managed Stripping Level to High
3. Reduce quality settings
4. Profile in build mode, not editor
5. Enable auto-optimization in PerformanceProfiler
```

---

# 10. FINAL CHECKLIST BEFORE RELEASE

```
☐ All scenes added to Build Settings
☐ All critical bugs fixed
☐ Performance targets met (60 FPS stable)
☐ Tested on all target platforms
☐ Multiplayer tested with 8 players
☐ Audio levels balanced
☐ UI tested at different resolutions
☐ Input tested with keyboard and gamepad
☐ Credits screen created
☐ Version number updated
☐ README.md written
☐ License file included
☐ Build tested from clean install
```

---

# PHASE 6 COMPLETION SUMMARY

This document provides complete build instructions:

✓ **Prerequisites & Installation** - All required software
✓ **Unity Project Setup** - From scratch project creation
✓ **Package Installation** - All dependencies configured
✓ **Asset Import** - Scripts and assets organized
✓ **Scene Setup** - Main menu and test arena
✓ **Testing** - Debug tools and profiling
✓ **Platform Builds** - Windows, macOS, Linux instructions
✓ **Multiplayer Configuration** - Netcode and Relay setup
✓ **Troubleshooting** - Common issues and solutions

**Time Estimates:**
- Initial Setup: 1-2 hours
- Script Import & Configuration: 1-3 hours
- Asset Creation & Scene Building: 4-8 hours
- Testing & Debugging: 2-4 hours
- Building & Packaging: 1-2 hours
**Total: ~10-20 hours to full working prototype**

---

*Document Version: 1.0*
*Last Updated: 2025-11-15*
*Status: PHASE 6 COMPLETE ✓*

*ALL PHASES (1-6) COMPLETE ✓*
*PROJECT READY FOR IMPLEMENTATION*
