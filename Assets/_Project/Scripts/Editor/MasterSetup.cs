using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace TumbleRumble.Editor
{
    /// <summary>
    /// Master setup - Complete one-click game creation and deployment
    /// This is the ULTIMATE setup script that creates a fully playable game
    /// </summary>
    public class MasterSetup : EditorWindow
    {
        private bool setupComplete = false;
        private string statusMessage = "";

        [MenuItem("Tumble Rumble/MASTER SETUP - Complete Game", priority = 0)]
        public static void ShowWindow()
        {
            var window = GetWindow<MasterSetup>("Master Setup");
            window.minSize = new Vector2(500, 400);
        }

        private void OnGUI()
        {
            GUILayout.Space(20);

            // Title
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 20;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("TUMBLE RUMBLE", titleStyle);
            GUILayout.Label("Complete Game Setup System", EditorStyles.centeredGreyMiniLabel);

            GUILayout.Space(30);

            // Description
            EditorGUILayout.HelpBox(
                "This is the MASTER SETUP that will create a completely functional game:\n\n" +
                "✓ Generate all 8 arenas with unique layouts\n" +
                "✓ Create 8 playable characters with procedural models\n" +
                "✓ Build complete UI system (menus, HUD, victory screens)\n" +
                "✓ Generate all audio and visual effects\n" +
                "✓ Configure game managers and flow\n" +
                "✓ Set up build settings\n" +
                "✓ Create a fully playable demo\n\n" +
                "After clicking the button below, you can immediately press Play to test the game!",
                MessageType.Info
            );

            GUILayout.Space(20);

            // Setup status
            if (!string.IsNullOrEmpty(statusMessage))
            {
                EditorGUILayout.HelpBox(statusMessage, setupComplete ? MessageType.Info : MessageType.Warning);
                GUILayout.Space(10);
            }

            // Main setup button
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("CREATE COMPLETE GAME NOW", GUILayout.Height(50)))
            {
                RunMasterSetup();
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Space(20);

            // Additional options
            GUILayout.Label("Additional Options:", EditorStyles.boldLabel);

            if (GUILayout.Button("Configure Build Settings Only", GUILayout.Height(30)))
            {
                GameBuilder.ConfigureBuildSettings();
            }

            if (GUILayout.Button("Open Build System", GUILayout.Height(30)))
            {
                GameBuilder.ShowWindow();
            }

            GUILayout.Space(20);

            // Quick start guide
            if (setupComplete)
            {
                EditorGUILayout.HelpBox(
                    "🎮 SETUP COMPLETE! 🎮\n\n" +
                    "Quick Start:\n" +
                    "1. Open Scenes/MainMenu.unity\n" +
                    "2. Press the Play button in Unity\n" +
                    "3. Click 'PLAY' in the main menu\n" +
                    "4. Enjoy the game!\n\n" +
                    "Controls:\n" +
                    "• WASD - Move\n" +
                    "• Space - Jump\n" +
                    "• E - Grab\n" +
                    "• Left Mouse - Punch\n" +
                    "• Q - Special Ability",
                    MessageType.Info
                );
            }
        }

        private void RunMasterSetup()
        {
            Debug.Log("========================================");
            Debug.Log("MASTER SETUP - Starting complete game creation");
            Debug.Log("========================================");

            setupComplete = false;
            statusMessage = "Running master setup... Please wait...";
            Repaint();

            try
            {
                // Step 1: Run enhanced project setup
                Debug.Log("[MasterSetup] Step 1: Creating complete game structure...");
                EditorUtility.DisplayProgressBar("Master Setup", "Creating game structure...", 0.2f);

                // Call the enhanced project setup method directly
                var enhancedSetupType = System.Type.GetType("TumbleRumble.Editor.EnhancedProjectSetup, Assembly-CSharp-Editor");
                if (enhancedSetupType != null)
                {
                    var method = enhancedSetupType.GetMethod("CreateCompleteGameStatic",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    if (method != null)
                    {
                        method.Invoke(null, null);
                    }
                    else
                    {
                        Debug.LogWarning("[MasterSetup] CreateCompleteGameStatic method not found, trying alternative...");
                        // Alternative: just run the window
                        EnhancedProjectSetup.CreateAllAssets();
                    }
                }

                // Step 2: Configure build settings
                Debug.Log("[MasterSetup] Step 2: Configuring build settings...");
                EditorUtility.DisplayProgressBar("Master Setup", "Configuring build settings...", 0.6f);
                GameBuilder.ConfigureBuildSettings();

                // Step 3: Create readme files
                Debug.Log("[MasterSetup] Step 3: Creating documentation...");
                EditorUtility.DisplayProgressBar("Master Setup", "Creating documentation...", 0.8f);
                CreateQuickStartGuide();

                // Step 4: Final verification
                Debug.Log("[MasterSetup] Step 4: Finalizing setup...");
                EditorUtility.DisplayProgressBar("Master Setup", "Finalizing...", 0.9f);
                VerifySetup();

                // Complete!
                EditorUtility.ClearProgressBar();

                setupComplete = true;
                statusMessage = "✓ SETUP COMPLETE! Game is ready to play!";

                Debug.Log("========================================");
                Debug.Log("MASTER SETUP - Complete!");
                Debug.Log("========================================");
                Debug.Log("Next steps:");
                Debug.Log("1. Open Scenes/MainMenu.unity");
                Debug.Log("2. Press Play in Unity");
                Debug.Log("3. Enjoy the game!");

                // Open main menu scene
                EditorSceneManager.OpenScene("Assets/_Project/Scenes/MainMenu.unity");

                // Show success dialog
                EditorUtility.DisplayDialog(
                    "🎮 Setup Complete! 🎮",
                    "Tumble Rumble is now fully set up and ready to play!\n\n" +
                    "The MainMenu scene has been opened for you.\n\n" +
                    "Press the Play button in Unity to start the game!\n\n" +
                    "To build a standalone executable:\n" +
                    "Go to: Tumble Rumble → Build Game",
                    "Let's Play!"
                );

                Repaint();
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                setupComplete = false;
                statusMessage = "✗ Setup failed: " + e.Message;
                Debug.LogError($"[MasterSetup] Setup failed: {e.Message}");
                Debug.LogException(e);

                EditorUtility.DisplayDialog(
                    "Setup Failed",
                    $"An error occurred during setup:\n\n{e.Message}\n\nCheck the console for details.",
                    "OK"
                );

                Repaint();
            }
        }

        private void CreateQuickStartGuide()
        {
            string guidePath = "Assets/QUICK_START.txt";
            string content = @"
TUMBLE RUMBLE - Quick Start Guide
==================================

🎮 HOW TO PLAY (In Unity Editor):
1. Open 'Scenes/MainMenu.unity'
2. Press the Play button (▶) in Unity
3. Click 'PLAY' in the main menu
4. Battle in the cosmic arena!

⌨️ CONTROLS:
Movement: WASD
Jump: Space
Grab: E
Punch: Left Mouse Button
Special Ability: Q
Crouch: Ctrl
Pause: Esc

🎯 OBJECTIVE:
Knock opponents off the platform into the void!
Win 3 rounds to claim victory!

🏗️ TO BUILD EXECUTABLE:
Go to: Tumble Rumble → Build Game
Choose your platform and build!

📚 MORE INFO:
See README.md and BUILD_README.md for complete details.

Enjoy the cosmic chaos!
";

            File.WriteAllText(guidePath, content);
            AssetDatabase.Refresh();
            Debug.Log("[MasterSetup] Created quick start guide");
        }

        private void VerifySetup()
        {
            int sceneCount = EditorBuildSettings.scenes.Length;
            bool mainMenuExists = File.Exists("Assets/_Project/Scenes/MainMenu.unity");
            bool prefabsExist = Directory.Exists("Assets/_Project/Prefabs/Characters");

            Debug.Log($"[MasterSetup] Verification:");
            Debug.Log($"  - Scenes in build: {sceneCount}");
            Debug.Log($"  - Main menu exists: {mainMenuExists}");
            Debug.Log($"  - Prefabs folder exists: {prefabsExist}");

            if (sceneCount == 0)
            {
                Debug.LogWarning("[MasterSetup] No scenes in build settings!");
            }
            if (!mainMenuExists)
            {
                Debug.LogWarning("[MasterSetup] Main menu scene not found!");
            }
        }

        [MenuItem("Tumble Rumble/Open Quick Start Guide")]
        public static void OpenQuickStart()
        {
            string guidePath = "Assets/QUICK_START.txt";
            if (File.Exists(guidePath))
            {
                System.Diagnostics.Process.Start(guidePath);
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Quick Start Not Found",
                    "Run the Master Setup first to create the quick start guide.",
                    "OK"
                );
            }
        }
    }
}
