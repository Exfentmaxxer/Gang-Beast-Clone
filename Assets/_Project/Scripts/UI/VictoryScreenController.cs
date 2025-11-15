using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using TumbleRumble.Core;

namespace TumbleRumble.UI
{
    /// <summary>
    /// Victory screen controller - displays match results and options
    /// </summary>
    public class VictoryScreenController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private TextMeshProUGUI winnerText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button rematchButton;
        [SerializeField] private Button mainMenuButton;

        private void Start()
        {
            // Initially hide the victory screen
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(false);
            }

            // Hook up button listeners
            if (rematchButton != null)
            {
                rematchButton.onClick.AddListener(OnRematchClicked);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            }

            // Subscribe to match end event
            MatchManager matchManager = FindObjectOfType<MatchManager>();
            if (matchManager != null)
            {
                // We'll need to add an event system to MatchManager
                // For now, we'll poll in Update
            }
        }

        private void Update()
        {
            // Check if match has ended
            MatchManager matchManager = FindObjectOfType<MatchManager>();
            if (matchManager != null && matchManager.MatchEnded && victoryPanel != null && !victoryPanel.activeSelf)
            {
                ShowVictoryScreen(matchManager.WinnerId);
            }
        }

        /// <summary>
        /// Display the victory screen with match results
        /// </summary>
        public void ShowVictoryScreen(int winnerId)
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            // Display winner
            if (winnerText != null)
            {
                if (winnerId >= 0)
                {
                    winnerText.text = $"Player {winnerId + 1} Wins!";
                    winnerText.color = GetPlayerColor(winnerId);
                }
                else
                {
                    winnerText.text = "Draw!";
                    winnerText.color = Color.white;
                }
            }

            // Display final scores
            if (finalScoreText != null)
            {
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    string scoreText = "Final Scores:\n";
                    for (int i = 0; i < 2; i++)
                    {
                        int score = scoreManager.GetScore(i);
                        scoreText += $"Player {i + 1}: {score}\n";
                    }
                    finalScoreText.text = scoreText;
                }
            }

            // Pause the game
            Time.timeScale = 0f;

            Debug.Log($"[VictoryScreenController] Victory screen shown for winner: {winnerId}");
        }

        private void OnRematchClicked()
        {
            Debug.Log("[VictoryScreenController] Rematch button clicked - Reloading scene...");

            // Resume time
            Time.timeScale = 1f;

            // Reload current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnMainMenuClicked()
        {
            Debug.Log("[VictoryScreenController] Main menu button clicked - Returning to menu...");

            // Resume time
            Time.timeScale = 1f;

            // Load main menu
            SceneManager.LoadScene("MainMenu");
        }

        private Color GetPlayerColor(int playerId)
        {
            // Return color based on player ID
            switch (playerId)
            {
                case 0: return Color.cyan;
                case 1: return new Color(1f, 0.4f, 0.8f); // Pink
                case 2: return Color.green;
                case 3: return Color.yellow;
                case 4: return new Color(1f, 0.5f, 0f); // Orange
                case 5: return new Color(0.5f, 0f, 1f); // Purple
                case 6: return Color.red;
                case 7: return Color.blue;
                default: return Color.white;
            }
        }

        private void OnDestroy()
        {
            // Clean up listeners
            if (rematchButton != null)
            {
                rematchButton.onClick.RemoveListener(OnRematchClicked);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            }

            // Ensure time scale is restored
            Time.timeScale = 1f;
        }
    }
}
