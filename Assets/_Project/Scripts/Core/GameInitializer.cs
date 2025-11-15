using UnityEngine;
using TumbleRumble.VFX;
using TumbleRumble.Audio;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Game initializer - ensures all singleton systems are created and initialized
    /// Add this to the first scene (Main Menu) to bootstrap the game
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        [Header("Initialization Settings")]
        [SerializeField] private bool initializeOnAwake = true;
        [SerializeField] private bool showDebugLogs = true;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                InitializeGame();
            }
        }

        public void InitializeGame()
        {
            if (showDebugLogs)
            {
                Debug.Log("[GameInitializer] Initializing game systems...");
            }

            // Initialize VFX system
            var vfxSystem = ProceduralVFXGenerator.Instance;
            if (vfxSystem != null && showDebugLogs)
            {
                Debug.Log("[GameInitializer] VFX system initialized");
            }

            // Initialize game flow manager
            var flowManager = GameFlowManager.Instance;
            if (flowManager != null && showDebugLogs)
            {
                Debug.Log("[GameInitializer] Game flow manager initialized");
            }

            // Initialize audio system (will be auto-created when first accessed)
            var audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null && showDebugLogs)
            {
                Debug.Log("[GameInitializer] Audio manager found");
            }

            // Generate all procedural audio clips
            GenerateAudioLibrary();

            if (showDebugLogs)
            {
                Debug.Log("[GameInitializer] Game systems initialized successfully!");
            }
        }

        private void GenerateAudioLibrary()
        {
            if (showDebugLogs)
            {
                Debug.Log("[GameInitializer] Generating audio library...");
            }

            // Pre-generate common audio clips
            var impactSound = ProceduralAudioGenerator.GenerateImpactSound();
            var jumpSound = ProceduralAudioGenerator.GenerateJumpSound();
            var uiClick = ProceduralAudioGenerator.GenerateUIClick();
            var grabSound = ProceduralAudioGenerator.GenerateGrabSound();
            var punchSound = ProceduralAudioGenerator.GeneratePunchSound();

            if (showDebugLogs)
            {
                Debug.Log($"[GameInitializer] Generated {5} audio clips");
            }
        }
    }
}
