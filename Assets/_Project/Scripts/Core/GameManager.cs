using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Central game manager - Singleton pattern
    /// Manages game-wide state, scene transitions, and system initialization
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Events
        public static event Action OnGameInitialized;
        public static event Action<string> OnSceneTransition;
        public static event Action OnGamePaused;
        public static event Action OnGameResumed;
        #endregion

        #region Inspector Fields
        [Header("Game Settings")]
        [SerializeField] private bool startInMainMenu = true;
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private bool debugMode = false;

        [Header("References")]
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private Audio.AudioManager audioManager;
        #endregion

        #region Private Fields
        private bool _isInitialized = false;
        private bool _isPaused = false;
        private float _timeScale = 1f;
        #endregion

        #region Properties
        public bool IsInitialized => _isInitialized;
        public bool IsPaused => _isPaused;
        public bool DebugMode => debugMode;
        public MatchManager Match => matchManager;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // Singleton enforcement
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeGame();
        }

        private void Start()
        {
            if (startInMainMenu)
            {
                LoadScene(mainMenuSceneName);
            }
        }

        private void Update()
        {
            // Global pause toggle (for debugging)
            if (debugMode && Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
        }

        private void OnApplicationQuit()
        {
            SavePlayerPreferences();
        }
        #endregion

        #region Initialization
        private void InitializeGame()
        {
            if (_isInitialized) return;

            Debug.Log("[GameManager] Initializing game systems...");

            // Set target frame rate
            Application.targetFrameRate = 60;

            // Set quality settings
            QualitySettings.vSyncCount = 0; // Disable VSync for consistent frame rate

            // Initialize physics settings
            Physics.defaultSolverIterations = 10;
            Physics.defaultSolverVelocityIterations = 8;

            // Find or create managers
            if (matchManager == null)
            {
                matchManager = FindObjectOfType<MatchManager>();
            }

            if (audioManager == null)
            {
                audioManager = FindObjectOfType<Audio.AudioManager>();
            }

            _isInitialized = true;
            OnGameInitialized?.Invoke();

            Debug.Log("[GameManager] Game initialized successfully");
        }
        #endregion

        #region Scene Management
        public void LoadScene(string sceneName)
        {
            Debug.Log($"[GameManager] Loading scene: {sceneName}");
            OnSceneTransition?.Invoke(sceneName);
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName, Action onComplete = null)
        {
            OnSceneTransition?.Invoke(sceneName);
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onComplete));
        }

        private System.Collections.IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action onComplete)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                // Could update loading bar here
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }

            onComplete?.Invoke();
        }

        public void ReloadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMainMenu()
        {
            LoadScene(mainMenuSceneName);
        }
        #endregion

        #region Pause System
        public void TogglePause()
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            if (_isPaused) return;

            _timeScale = Time.timeScale;
            Time.timeScale = 0f;
            _isPaused = true;

            OnGamePaused?.Invoke();
            Debug.Log("[GameManager] Game paused");
        }

        public void ResumeGame()
        {
            if (!_isPaused) return;

            Time.timeScale = _timeScale;
            _isPaused = false;

            OnGameResumed?.Invoke();
            Debug.Log("[GameManager] Game resumed");
        }
        #endregion

        #region Player Preferences
        private void SavePlayerPreferences()
        {
            // Save any persistent data
            PlayerPrefs.Save();
        }

        public void SetMasterVolume(float volume)
        {
            PlayerPrefs.SetFloat("MasterVolume", volume);
            if (audioManager != null)
            {
                audioManager.SetMasterVolume(volume);
            }
        }

        public float GetMasterVolume()
        {
            return PlayerPrefs.GetFloat("MasterVolume", 1f);
        }
        #endregion

        #region Utility Methods
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");
            SavePlayerPreferences();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        public string GetVersionString()
        {
            return Application.version;
        }
        #endregion
    }
}
