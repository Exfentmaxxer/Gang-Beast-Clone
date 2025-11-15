using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace TumbleRumble.Editor
{
    /// <summary>
    /// Automated project setup - creates all necessary assets, scenes, and prefabs
    /// Run this once when first opening the project
    /// </summary>
    public class ProjectSetup : EditorWindow
    {
        [MenuItem("Tumble Rumble/Setup Project")]
        public static void ShowWindow()
        {
            GetWindow<ProjectSetup>("Project Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Tumble Rumble - Project Setup", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.Label("This will create all necessary assets and scenes for the demo.");
            GUILayout.Space(10);

            if (GUILayout.Button("Create All Assets & Scenes", GUILayout.Height(40)))
            {
                CreateProjectStructure();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Create Test Scene Only", GUILayout.Height(30)))
            {
                CreateTestScene();
            }
        }

        private static void CreateProjectStructure()
        {
            Debug.Log("[ProjectSetup] Starting project setup...");

            // Create folders
            CreateFolders();

            // Create materials
            CreateMaterials();

            // Create physics materials
            CreatePhysicsMaterials();

            // Create player prefab
            CreatePlayerPrefab();

            // Create test scene
            CreateTestScene();

            // Create main menu scene
            CreateMainMenuScene();

            // Refresh asset database
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[ProjectSetup] Setup complete! Open Scenes/TestArena.unity to play.");
            EditorUtility.DisplayDialog("Setup Complete", "All assets and scenes created. Open Scenes/TestArena.unity to test the game!", "OK");
        }

        private static void CreateFolders()
        {
            string[] folders = new string[]
            {
                "Assets/_Project/Prefabs/Characters",
                "Assets/_Project/Prefabs/Environment",
                "Assets/_Project/Materials",
                "Assets/_Project/PhysicsMaterials",
                "Assets/_Project/Scenes"
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                    Debug.Log($"Created folder: {folder}");
                }
            }
        }

        private static void CreateMaterials()
        {
            // Player material (Cyan)
            Material playerMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            playerMat.color = new Color(0f, 1f, 1f, 1f); // Cyan
            playerMat.SetFloat("_Metallic", 0f);
            playerMat.SetFloat("_Smoothness", 0.5f);
            playerMat.EnableKeyword("_EMISSION");
            playerMat.SetColor("_EmissionColor", new Color(0f, 1f, 1f, 1f) * 0.5f);
            AssetDatabase.CreateAsset(playerMat, "Assets/_Project/Materials/MAT_Player_Cyan.mat");

            // Player 2 material (Pink)
            Material player2Mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            player2Mat.color = new Color(1f, 0.4f, 0.8f, 1f); // Pink
            player2Mat.SetFloat("_Metallic", 0f);
            player2Mat.SetFloat("_Smoothness", 0.5f);
            player2Mat.EnableKeyword("_EMISSION");
            player2Mat.SetColor("_EmissionColor", new Color(1f, 0.4f, 0.8f, 1f) * 0.5f);
            AssetDatabase.CreateAsset(player2Mat, "Assets/_Project/Materials/MAT_Player_Pink.mat");

            // Platform material
            Material platformMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            platformMat.color = new Color(0.25f, 0.25f, 0.25f, 1f); // Dark gray
            platformMat.SetFloat("_Metallic", 0.3f);
            platformMat.SetFloat("_Smoothness", 0.6f);
            AssetDatabase.CreateAsset(platformMat, "Assets/_Project/Materials/MAT_Platform.mat");

            Debug.Log("[ProjectSetup] Materials created");
        }

        private static void CreatePhysicsMaterials()
        {
            // Player physics material
            PhysicMaterial playerPhys = new PhysicMaterial("PlayerPhysicsMaterial");
            playerPhys.dynamicFriction = 0.6f;
            playerPhys.staticFriction = 0.6f;
            playerPhys.bounciness = 0.1f;
            playerPhys.frictionCombine = PhysicMaterialCombine.Average;
            playerPhys.bounceCombine = PhysicMaterialCombine.Average;
            AssetDatabase.CreateAsset(playerPhys, "Assets/_Project/PhysicsMaterials/PhysicsMaterial_Player.physicMaterial");

            // Environment physics material
            PhysicMaterial envPhys = new PhysicMaterial("EnvironmentPhysicsMaterial");
            envPhys.dynamicFriction = 0.7f;
            envPhys.staticFriction = 0.8f;
            envPhys.bounciness = 0.0f;
            AssetDatabase.CreateAsset(envPhys, "Assets/_Project/PhysicsMaterials/PhysicsMaterial_Environment.physicMaterial");

            Debug.Log("[ProjectSetup] Physics materials created");
        }

        private static void CreatePlayerPrefab()
        {
            // Create player GameObject
            GameObject player = new GameObject("Player");

            // Add Rigidbody (Pelvis)
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.mass = 40f;
            rb.drag = 0.5f;
            rb.angularDrag = 5f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // Add Capsule Collider
            CapsuleCollider col = player.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.5f;
            col.center = new Vector3(0f, 1f, 0f);

            // Add visual (cube for now)
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visual.name = "Visual";
            visual.transform.SetParent(player.transform);
            visual.transform.localPosition = new Vector3(0f, 1f, 0f);
            visual.transform.localScale = new Vector3(0.8f, 1.5f, 0.8f);
            DestroyImmediate(visual.GetComponent<Collider>()); // Remove duplicate collider

            // Apply material
            Material playerMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/MAT_Player_Cyan.mat");
            if (playerMat != null)
            {
                visual.GetComponent<Renderer>().material = playerMat;
            }

            // Add scripts
            player.AddComponent<TumbleRumble.Player.PlayerController>();
            player.AddComponent<TumbleRumble.Player.RagdollController>();
            player.AddComponent<TumbleRumble.Player.GrabSystem>();
            player.AddComponent<TumbleRumble.Player.CombatSystem>();
            player.AddComponent<TumbleRumble.Player.AbilitySystem>();

            // Create prefab
            PrefabUtility.SaveAsPrefabAsset(player, "Assets/_Project/Prefabs/Characters/Player.prefab");
            DestroyImmediate(player);

            Debug.Log("[ProjectSetup] Player prefab created");
        }

        private static void CreateTestScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Create ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(5f, 1f, 5f); // 50m x 50m
            ground.layer = LayerMask.NameToLayer("Default");

            // Apply platform material
            Material platformMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/MAT_Platform.mat");
            if (platformMat != null)
            {
                ground.GetComponent<Renderer>().material = platformMat;
            }

            // Create main platform
            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            platform.name = "MainPlatform";
            platform.transform.position = new Vector3(0f, 0.5f, 0f);
            platform.transform.localScale = new Vector3(15f, 0.5f, 15f);

            if (platformMat != null)
            {
                platform.GetComponent<Renderer>().material = platformMat;
            }

            // Create void zone
            GameObject voidZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            voidZone.name = "VoidZone";
            voidZone.transform.position = new Vector3(0f, -10f, 0f);
            voidZone.transform.localScale = new Vector3(100f, 1f, 100f);
            DestroyImmediate(voidZone.GetComponent<Renderer>()); // Make invisible
            voidZone.GetComponent<Collider>().isTrigger = true;
            voidZone.AddComponent<TumbleRumble.Environment.VoidZone>();

            // Spawn 2 players for testing
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Prefabs/Characters/Player.prefab");
            if (playerPrefab != null)
            {
                GameObject player1 = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
                player1.name = "Player1";
                player1.transform.position = new Vector3(-3f, 2f, 0f);

                GameObject player2 = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
                player2.name = "Player2";
                player2.transform.position = new Vector3(3f, 2f, 0f);

                // Change Player 2 color
                Material pinkMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/MAT_Player_Pink.mat");
                if (pinkMat != null)
                {
                    player2.transform.Find("Visual").GetComponent<Renderer>().material = pinkMat;
                }
            }

            // Setup lighting
            Light sun = FindObjectOfType<Light>();
            if (sun != null)
            {
                sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
                sun.intensity = 1f;
                sun.color = Color.white;
            }

            // Setup camera
            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.transform.position = new Vector3(0f, 15f, -10f);
                cam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
                cam.gameObject.AddComponent<TumbleRumble.CameraSystem.DynamicCamera>();
            }

            // Create game managers
            GameObject managers = new GameObject("GameManagers");
            GameObject gmObj = new GameObject("GameManager");
            gmObj.transform.SetParent(managers.transform);
            gmObj.AddComponent<TumbleRumble.Core.GameManager>();

            GameObject matchObj = new GameObject("MatchManager");
            matchObj.transform.SetParent(managers.transform);
            matchObj.AddComponent<TumbleRumble.Core.MatchManager>();
            matchObj.AddComponent<TumbleRumble.Core.RoundManager>();
            matchObj.AddComponent<TumbleRumble.Core.ScoreManager>();

            // Create audio manager
            GameObject audioObj = new GameObject("AudioManager");
            audioObj.AddComponent<TumbleRumble.Audio.AudioManager>();

            // Save scene
            EditorSceneManager.SaveScene(scene, "Assets/_Project/Scenes/TestArena.unity");

            Debug.Log("[ProjectSetup] Test scene created: TestArena.unity");
        }

        private static void CreateMainMenuScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Remove default directional light (not needed for UI-only scene)
            Light light = FindObjectOfType<Light>();
            if (light != null) DestroyImmediate(light.gameObject);

            // Create Canvas
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();

            // Create Event System
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            // Create background
            GameObject bg = new GameObject("Background");
            bg.transform.SetParent(canvasObj.transform);
            UnityEngine.UI.Image bgImage = bg.AddComponent<UnityEngine.UI.Image>();
            bgImage.color = new Color(0.04f, 0.0f, 0.2f, 1f); // Dark blue

            RectTransform bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;

            // Create title text
            GameObject title = new GameObject("TitleText");
            title.transform.SetParent(canvasObj.transform);
            UnityEngine.UI.Text titleText = title.AddComponent<UnityEngine.UI.Text>();
            titleText.text = "TUMBLE RUMBLE";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 72;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.cyan;

            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.7f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.sizeDelta = new Vector2(800f, 200f);
            titleRect.anchoredPosition = Vector2.zero;

            // Create Play button
            GameObject playBtn = CreateButton(canvasObj.transform, "PlayButton", "PLAY", new Vector2(0.5f, 0.5f));
            playBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);

            // Create Quit button
            GameObject quitBtn = CreateButton(canvasObj.transform, "QuitButton", "QUIT", new Vector2(0.5f, 0.5f));
            quitBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);

            // Save scene
            EditorSceneManager.SaveScene(scene, "Assets/_Project/Scenes/MainMenu.unity");

            Debug.Log("[ProjectSetup] Main menu scene created: MainMenu.unity");
        }

        private static GameObject CreateButton(Transform parent, string name, string text, Vector2 anchor)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent);

            UnityEngine.UI.Image btnImage = btnObj.AddComponent<UnityEngine.UI.Image>();
            btnImage.color = new Color(0.2f, 0.6f, 0.8f, 1f);

            UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();

            RectTransform btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = anchor;
            btnRect.anchorMax = anchor;
            btnRect.sizeDelta = new Vector2(300f, 80f);

            // Button text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform);

            UnityEngine.UI.Text btnText = textObj.AddComponent<UnityEngine.UI.Text>();
            btnText.text = text;
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.fontSize = 32;
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.color = Color.white;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            return btnObj;
        }
    }
}
