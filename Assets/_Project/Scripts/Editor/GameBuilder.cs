using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;
using System;

namespace TumbleRumble.Editor
{
    /// <summary>
    /// Automated game builder - creates standalone executables for all platforms
    /// Provides one-click build solution for distributable game
    /// </summary>
    public class GameBuilder : EditorWindow
    {
        private static string companyName = "Indie Game Studio";
        private static string productName = "Tumble Rumble";
        private static string version = "1.0.0";

        private bool buildWindows = true;
        private bool buildMac = true;
        private bool buildLinux = true;
        private string buildPath = "Builds";

        [MenuItem("Tumble Rumble/Build Game")]
        public static void ShowWindow()
        {
            var window = GetWindow<GameBuilder>("Game Builder");
            window.minSize = new Vector2(400, 300);
        }

        private void OnGUI()
        {
            GUILayout.Label("Tumble Rumble - Game Builder", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Label("This will build standalone executables for distribution.");
            GUILayout.Space(10);

            // Platform selection
            GUILayout.Label("Target Platforms:", EditorStyles.boldLabel);
            buildWindows = EditorGUILayout.Toggle("Build for Windows", buildWindows);
            buildMac = EditorGUILayout.Toggle("Build for macOS", buildMac);
            buildLinux = EditorGUILayout.Toggle("Build for Linux", buildLinux);

            GUILayout.Space(10);

            // Build path
            GUILayout.BeginHorizontal();
            GUILayout.Label("Build Output Path:", GUILayout.Width(120));
            buildPath = GUILayout.TextField(buildPath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Build Folder", buildPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    buildPath = selectedPath;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            // Build button
            if (GUILayout.Button("Build All Selected Platforms", GUILayout.Height(40)))
            {
                BuildAllPlatforms();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Quick Build (Windows Only)", GUILayout.Height(30)))
            {
                QuickBuildWindows();
            }
        }

        private void BuildAllPlatforms()
        {
            Debug.Log("[GameBuilder] Starting multi-platform build...");

            // Configure player settings
            ConfigurePlayerSettings();

            int successCount = 0;
            int totalCount = 0;

            if (buildWindows)
            {
                totalCount++;
                if (BuildForWindows())
                    successCount++;
            }

            if (buildMac)
            {
                totalCount++;
                if (BuildForMac())
                    successCount++;
            }

            if (buildLinux)
            {
                totalCount++;
                if (BuildForLinux())
                    successCount++;
            }

            EditorUtility.DisplayDialog("Build Complete",
                $"Built {successCount} out of {totalCount} platforms successfully!\n\n" +
                $"Builds location: {Path.GetFullPath(buildPath)}",
                "OK");

            Debug.Log($"[GameBuilder] Build process complete: {successCount}/{totalCount} successful");
        }

        private void QuickBuildWindows()
        {
            Debug.Log("[GameBuilder] Quick build for Windows...");

            ConfigurePlayerSettings();

            if (BuildForWindows())
            {
                EditorUtility.DisplayDialog("Build Complete",
                    $"Windows build created successfully!\n\n" +
                    $"Build location: {Path.GetFullPath(buildPath)}/Windows",
                    "OK");
            }
        }

        private void ConfigurePlayerSettings()
        {
            Debug.Log("[GameBuilder] Configuring player settings...");

            // Set product name and company
            PlayerSettings.companyName = companyName;
            PlayerSettings.productName = productName;
            PlayerSettings.bundleVersion = version;

            // Set icon (if available)
            // PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Standalone, icons);

            // Set standalone player options
            PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.runInBackground = false;
            PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Enabled;

            // Set graphics
            PlayerSettings.colorSpace = ColorSpace.Linear;

            Debug.Log("[GameBuilder] Player settings configured");
        }

        private bool BuildForWindows()
        {
            Debug.Log("[GameBuilder] Building for Windows...");

            string outputPath = Path.Combine(buildPath, "Windows", $"{productName}.exe");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = outputPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[GameBuilder] Windows build succeeded: {summary.totalSize} bytes");
                return true;
            }
            else
            {
                Debug.LogError($"[GameBuilder] Windows build failed: {summary.result}");
                return false;
            }
        }

        private bool BuildForMac()
        {
            Debug.Log("[GameBuilder] Building for macOS...");

            string outputPath = Path.Combine(buildPath, "macOS", $"{productName}.app");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = outputPath,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[GameBuilder] macOS build succeeded: {summary.totalSize} bytes");
                return true;
            }
            else
            {
                Debug.LogError($"[GameBuilder] macOS build failed: {summary.result}");
                return false;
            }
        }

        private bool BuildForLinux()
        {
            Debug.Log("[GameBuilder] Building for Linux...");

            string outputPath = Path.Combine(buildPath, "Linux", $"{productName}.x86_64");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = outputPath,
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[GameBuilder] Linux build succeeded: {summary.totalSize} bytes");
                return true;
            }
            else
            {
                Debug.LogError($"[GameBuilder] Linux build failed: {summary.result}");
                return false;
            }
        }

        private string[] GetScenePaths()
        {
            // Get all scenes in build settings
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }

            // If no scenes in build settings, try to find them
            if (scenes.Length == 0)
            {
                Debug.LogWarning("[GameBuilder] No scenes in build settings, searching for scenes...");

                string[] foundScenes = System.IO.Directory.GetFiles("Assets/_Project/Scenes", "*.unity", SearchOption.AllDirectories);
                scenes = foundScenes;

                Debug.Log($"[GameBuilder] Found {scenes.Length} scenes");
            }

            return scenes;
        }

        [MenuItem("Tumble Rumble/Configure Build Settings")]
        public static void ConfigureBuildSettings()
        {
            Debug.Log("[GameBuilder] Configuring build settings...");

            // Find all scenes
            string[] sceneFiles = Directory.GetFiles("Assets/_Project/Scenes", "*.unity", SearchOption.AllDirectories);

            // Create EditorBuildSettingsScene array
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[sceneFiles.Length];

            // MainMenu should be first
            int mainMenuIndex = Array.FindIndex(sceneFiles, s => s.Contains("MainMenu"));
            if (mainMenuIndex >= 0)
            {
                scenes[0] = new EditorBuildSettingsScene(sceneFiles[mainMenuIndex], true);

                // Add other scenes
                int sceneIndex = 1;
                for (int i = 0; i < sceneFiles.Length; i++)
                {
                    if (i != mainMenuIndex)
                    {
                        scenes[sceneIndex] = new EditorBuildSettingsScene(sceneFiles[i], true);
                        sceneIndex++;
                    }
                }
            }
            else
            {
                // No main menu, just add all scenes
                for (int i = 0; i < sceneFiles.Length; i++)
                {
                    scenes[i] = new EditorBuildSettingsScene(sceneFiles[i], true);
                }
            }

            // Apply to build settings
            EditorBuildSettings.scenes = scenes;

            Debug.Log($"[GameBuilder] Added {scenes.Length} scenes to build settings");
            EditorUtility.DisplayDialog("Build Settings Configured",
                $"Added {scenes.Length} scenes to build settings.\n\n" +
                "You can now build the game!",
                "OK");
        }
    }
}
