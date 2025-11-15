using System;
using UnityEngine;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Manages game state transitions
    /// </summary>
    public enum GameState
    {
        MainMenu,
        CharacterCustomization,
        Lobby,
        LoadingMatch,
        InMatch,
        RoundTransition,
        MatchEnd,
        Paused
    }

    public class GameStateManager : MonoBehaviour
    {
        #region Events
        public static event Action<GameState, GameState> OnStateChanged;
        #endregion

        #region Private Fields
        private GameState _currentState = GameState.MainMenu;
        private GameState _previousState = GameState.MainMenu;
        #endregion

        #region Properties
        public GameState CurrentState => _currentState;
        public GameState PreviousState => _previousState;
        #endregion

        #region State Management
        public void ChangeState(GameState newState)
        {
            if (_currentState == newState) return;

            Debug.Log($"[GameStateManager] State transition: {_currentState} → {newState}");

            _previousState = _currentState;
            _currentState = newState;

            OnStateExit(_previousState);
            OnStateChanged?.Invoke(_previousState, _currentState);
            OnStateEnter(_currentState);
        }

        private void OnStateEnter(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;

                case GameState.InMatch:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    // Don't modify time scale here - GameManager handles it
                    break;

                case GameState.MatchEnd:
                    // Could trigger victory music, etc.
                    break;
            }
        }

        private void OnStateExit(GameState state)
        {
            switch (state)
            {
                case GameState.InMatch:
                    // Cleanup match-specific data
                    break;

                case GameState.Paused:
                    // Resume if needed
                    break;
            }
        }
        #endregion

        #region Query Methods
        public bool IsInGameplay()
        {
            return _currentState == GameState.InMatch;
        }

        public bool IsInMenu()
        {
            return _currentState == GameState.MainMenu ||
                   _currentState == GameState.CharacterCustomization ||
                   _currentState == GameState.Lobby;
        }

        public bool CanPause()
        {
            return _currentState == GameState.InMatch;
        }
        #endregion
    }
}
