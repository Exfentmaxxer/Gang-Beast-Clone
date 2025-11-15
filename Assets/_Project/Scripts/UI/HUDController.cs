using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TumbleRumble.Core;

namespace TumbleRumble.UI
{
    /// <summary>
    /// HUD controller - displays match info, scores, timer, player status
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI player1ScoreText;
        [SerializeField] private TextMeshProUGUI player2ScoreText;
        [SerializeField] private TextMeshProUGUI statusText;

        private MatchManager matchManager;
        private RoundManager roundManager;
        private ScoreManager scoreManager;

        private void Start()
        {
            // Find managers
            matchManager = FindObjectOfType<MatchManager>();
            roundManager = FindObjectOfType<RoundManager>();
            scoreManager = FindObjectOfType<ScoreManager>();

            if (matchManager == null)
            {
                Debug.LogWarning("[HUDController] MatchManager not found in scene");
            }

            if (roundManager == null)
            {
                Debug.LogWarning("[HUDController] RoundManager not found in scene");
            }

            if (scoreManager == null)
            {
                Debug.LogWarning("[HUDController] ScoreManager not found in scene");
            }

            // Initial update
            UpdateHUD();
        }

        private void Update()
        {
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            // Update round info
            if (roundText != null && matchManager != null)
            {
                roundText.text = $"Round {matchManager.CurrentRound} / {matchManager.RoundsToWin}";
            }

            // Update timer
            if (timerText != null && roundManager != null)
            {
                float timeRemaining = roundManager.RoundTimeRemaining;
                int minutes = Mathf.FloorToInt(timeRemaining / 60f);
                int seconds = Mathf.FloorToInt(timeRemaining % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";

                // Change color if time is running out
                if (timeRemaining < 10f)
                {
                    timerText.color = Color.red;
                }
                else if (timeRemaining < 30f)
                {
                    timerText.color = Color.yellow;
                }
                else
                {
                    timerText.color = Color.white;
                }
            }

            // Update scores
            if (scoreManager != null)
            {
                if (player1ScoreText != null)
                {
                    int score = scoreManager.GetScore(0);
                    player1ScoreText.text = $"P1: {score}";
                }

                if (player2ScoreText != null)
                {
                    int score = scoreManager.GetScore(1);
                    player2ScoreText.text = $"P2: {score}";
                }
            }

            // Update status text based on game state
            if (statusText != null && roundManager != null)
            {
                if (!roundManager.RoundActive)
                {
                    statusText.text = "Get Ready!";
                    statusText.gameObject.SetActive(true);
                }
                else
                {
                    statusText.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Show a temporary message on the HUD
        /// </summary>
        public void ShowMessage(string message, float duration = 2f)
        {
            if (statusText != null)
            {
                statusText.text = message;
                statusText.gameObject.SetActive(true);
                CancelInvoke(nameof(HideMessage));
                Invoke(nameof(HideMessage), duration);
            }
        }

        private void HideMessage()
        {
            if (statusText != null)
            {
                statusText.gameObject.SetActive(false);
            }
        }
    }
}
