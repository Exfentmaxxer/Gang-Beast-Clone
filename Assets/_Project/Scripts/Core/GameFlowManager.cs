using UnityEngine;
using UnityEngine.SceneManagement;
using TumbleRumble.UI;
using System.Collections;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Game flow manager - orchestrates complete game loop
    /// Handles: Main Menu → Match Start → Rounds → Victory → Return to Menu
    /// </summary>
    public class GameFlowManager : MonoBehaviour
    {
        private static GameFlowManager _instance;
        public static GameFlowManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameFlowManager");
                    _instance = go.AddComponent<GameFlowManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [Header("Game Settings")]
        [SerializeField] private int roundsToWin = 3;
        [SerializeField] private float roundDuration = 120f;
        [SerializeField] private float roundTransitionDelay = 3f;
        [SerializeField] private float matchEndDelay = 2f;

        [Header("Current Game State")]
        [SerializeField] private bool matchInProgress = false;
        [SerializeField] private string currentArena = "";

        private MatchManager matchManager;
        private RoundManager roundManager;
        private ScoreManager scoreManager;
        private HUDController hudController;
        private VictoryScreenController victoryScreenController;

        public bool MatchInProgress => matchInProgress;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[GameFlowManager] Scene loaded: {scene.name}");

            // Check if this is an arena scene
            if (scene.name.StartsWith("Arena_"))
            {
                currentArena = scene.name;
                StartCoroutine(InitializeMatch());
            }
            else if (scene.name == "MainMenu")
            {
                currentArena = "";
                matchInProgress = false;
            }
        }

        private IEnumerator InitializeMatch()
        {
            // Wait for scene to fully load
            yield return new WaitForSeconds(0.5f);

            // Find all managers
            matchManager = FindObjectOfType<MatchManager>();
            roundManager = FindObjectOfType<RoundManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            hudController = FindObjectOfType<HUDController>();
            victoryScreenController = FindObjectOfType<VictoryScreenController>();

            if (matchManager == null)
            {
                Debug.LogError("[GameFlowManager] MatchManager not found in scene!");
                yield break;
            }

            if (roundManager == null)
            {
                Debug.LogError("[GameFlowManager] RoundManager not found in scene!");
                yield break;
            }

            if (scoreManager == null)
            {
                Debug.LogError("[GameFlowManager] ScoreManager not found in scene!");
                yield break;
            }

            // Initialize match
            matchInProgress = true;

            // Configure match settings
            matchManager.RoundsToWin = roundsToWin;
            roundManager.RoundDuration = roundDuration;

            // Show start message
            if (hudController != null)
            {
                hudController.ShowMessage("GET READY!", 3f);
            }

            // Wait for ready period
            yield return new WaitForSeconds(3f);

            // Start first round
            StartRound();
        }

        public void StartRound()
        {
            if (roundManager != null)
            {
                Debug.Log($"[GameFlowManager] Starting round {matchManager.CurrentRound}");

                // Show round start message
                if (hudController != null)
                {
                    hudController.ShowMessage($"ROUND {matchManager.CurrentRound}", 2f);
                }

                // Start round after brief delay
                StartCoroutine(DelayedRoundStart());
            }
        }

        private IEnumerator DelayedRoundStart()
        {
            yield return new WaitForSeconds(2f);

            if (roundManager != null)
            {
                roundManager.StartRound();
            }

            // Monitor round progress
            StartCoroutine(MonitorRound());
        }

        private IEnumerator MonitorRound()
        {
            // Wait for round to complete
            while (roundManager != null && roundManager.RoundActive)
            {
                yield return null;
            }

            Debug.Log("[GameFlowManager] Round ended");

            // Show round winner
            if (roundManager != null && hudController != null)
            {
                int roundWinner = roundManager.RoundWinner;
                if (roundWinner >= 0)
                {
                    hudController.ShowMessage($"Player {roundWinner + 1} wins the round!", roundTransitionDelay);
                }
                else
                {
                    hudController.ShowMessage("Round draw!", roundTransitionDelay);
                }
            }

            // Wait for transition
            yield return new WaitForSeconds(roundTransitionDelay);

            // Check if match is over
            if (matchManager != null && matchManager.IsMatchOver())
            {
                EndMatch();
            }
            else
            {
                // Start next round
                if (matchManager != null)
                {
                    matchManager.NextRound();
                }
                StartRound();
            }
        }

        private void EndMatch()
        {
            Debug.Log("[GameFlowManager] Match ended");

            matchInProgress = false;

            // Determine winner
            int winnerId = -1;
            if (matchManager != null)
            {
                winnerId = matchManager.GetMatchWinner();
            }

            // Show victory screen
            if (victoryScreenController != null)
            {
                StartCoroutine(DelayedVictoryScreen(winnerId));
            }
            else
            {
                Debug.LogWarning("[GameFlowManager] VictoryScreenController not found!");
            }
        }

        private IEnumerator DelayedVictoryScreen(int winnerId)
        {
            yield return new WaitForSeconds(matchEndDelay);

            if (victoryScreenController != null)
            {
                victoryScreenController.ShowVictoryScreen(winnerId);
            }
        }

        /// <summary>
        /// Start a new match in specified arena
        /// </summary>
        public void StartMatch(string arenaName)
        {
            Debug.Log($"[GameFlowManager] Starting match in {arenaName}");
            SceneManager.LoadScene(arenaName);
        }

        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            Debug.Log("[GameFlowManager] Returning to main menu");
            matchInProgress = false;
            Time.timeScale = 1f; // Ensure time is running
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Restart current match
        /// </summary>
        public void RestartMatch()
        {
            if (!string.IsNullOrEmpty(currentArena))
            {
                Debug.Log($"[GameFlowManager] Restarting match in {currentArena}");
                Time.timeScale = 1f; // Ensure time is running
                SceneManager.LoadScene(currentArena);
            }
            else
            {
                Debug.LogWarning("[GameFlowManager] No current arena to restart!");
            }
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void PauseGame()
        {
            Time.timeScale = 0f;
            Debug.Log("[GameFlowManager] Game paused");
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            Debug.Log("[GameFlowManager] Game resumed");
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameFlowManager] Quitting game");

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
