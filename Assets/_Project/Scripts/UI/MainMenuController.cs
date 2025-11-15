using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TumbleRumble.UI
{
    /// <summary>
    /// Main menu controller - handles menu button interactions
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            // Hook up button listeners
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayClicked);
            }
            else
            {
                Debug.LogError("[MainMenuController] Play button not assigned!");
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitClicked);
            }
            else
            {
                Debug.LogError("[MainMenuController] Quit button not assigned!");
            }
        }

        private void OnPlayClicked()
        {
            Debug.Log("[MainMenuController] Play button clicked - Loading first arena...");

            // Load the first arena (Gravity Well)
            SceneManager.LoadScene("Arena_GravityWell");
        }

        private void OnQuitClicked()
        {
            Debug.Log("[MainMenuController] Quit button clicked - Exiting game...");

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void OnDestroy()
        {
            // Clean up listeners
            if (playButton != null)
            {
                playButton.onClick.RemoveListener(OnPlayClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.RemoveListener(OnQuitClicked);
            }
        }
    }
}
