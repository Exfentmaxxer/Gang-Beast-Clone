using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Manages individual round lifecycle: countdown, gameplay, elimination tracking
    /// </summary>
    public class RoundManager : MonoBehaviour
    {
        #region Events
        public static event Action<int> OnRoundStart; // Parameter: round number
        public static event Action<int> OnRoundEnd;   // Parameter: winner player ID
        public static event Action<float> OnRoundTimeUpdate; // Parameter: remaining time
        public static event Action<int> OnPlayerEliminated; // Parameter: eliminated player ID
        #endregion

        #region Inspector Fields
        [Header("Round Settings")]
        [SerializeField] private float roundDuration = 120f; // 2 minutes
        [SerializeField] private float countdownDuration = 3f;
        [SerializeField] private bool useTimeLimit = true;

        [Header("Elimination Settings")]
        [SerializeField] private bool lastPlayerStanding = true;
        #endregion

        #region Private Fields
        private bool _roundActive = false;
        private int _currentRoundNumber = 0;
        private int _lastRoundWinner = -1;
        private float _remainingTime = 0f;
        private HashSet<int> _activePlayers = new HashSet<int>();
        private HashSet<int> _eliminatedPlayers = new HashSet<int>();
        #endregion

        #region Properties
        public bool IsRoundActive => _roundActive;
        public bool RoundActive => _roundActive; // Alias for GameFlowManager
        public int CurrentRoundNumber => _currentRoundNumber;
        public int RoundWinner => _lastRoundWinner;
        public float RemainingTime => _remainingTime;
        public float RoundTimeRemaining => _remainingTime; // Alias for GameFlowManager
        public int ActivePlayerCount => _activePlayers.Count;
        public float RoundDuration
        {
            get => roundDuration;
            set => roundDuration = value;
        }
        #endregion

        #region Round Control
        public void StartRound()
        {
            if (_roundActive)
            {
                Debug.LogWarning("[RoundManager] Round already active!");
                return;
            }

            _currentRoundNumber++;
            Debug.Log($"[RoundManager] Starting round {_currentRoundNumber}");

            StartCoroutine(RoundSequence());
        }

        private IEnumerator RoundSequence()
        {
            // Countdown
            yield return StartCoroutine(CountdownSequence());

            // Start round
            _roundActive = true;
            _remainingTime = roundDuration;
            _eliminatedPlayers.Clear();

            OnRoundStart?.Invoke(_currentRoundNumber);

            // Round timer
            if (useTimeLimit)
            {
                while (_roundActive && _remainingTime > 0f)
                {
                    _remainingTime -= Time.deltaTime;
                    OnRoundTimeUpdate?.Invoke(_remainingTime);
                    yield return null;

                    // Check for round end condition
                    if (CheckRoundEndCondition())
                    {
                        break;
                    }
                }

                // Time ran out
                if (_remainingTime <= 0f && _roundActive)
                {
                    HandleTimeExpired();
                }
            }
            else
            {
                // No time limit - wait for elimination
                while (_roundActive)
                {
                    yield return null;
                    if (CheckRoundEndCondition())
                    {
                        break;
                    }
                }
            }
        }

        private IEnumerator CountdownSequence()
        {
            float countdown = countdownDuration;
            while (countdown > 0f)
            {
                Debug.Log($"[RoundManager] Round starting in: {Mathf.CeilToInt(countdown)}");
                // Could trigger UI countdown here
                countdown -= Time.deltaTime;
                yield return null;
            }

            Debug.Log("[RoundManager] GO!");
        }

        public void EndRound()
        {
            if (!_roundActive) return;

            int winnerId = DetermineRoundWinner();
            _lastRoundWinner = winnerId;
            Debug.Log($"[RoundManager] Round {_currentRoundNumber} ended. Winner: Player {winnerId}");

            _roundActive = false;
            OnRoundEnd?.Invoke(winnerId);
        }

        private void HandleTimeExpired()
        {
            Debug.Log("[RoundManager] Time expired!");

            // Determine winner by survival or score
            EndRound();
        }
        #endregion

        #region Player Management
        public void RegisterPlayer(int playerId)
        {
            _activePlayers.Add(playerId);
            Debug.Log($"[RoundManager] Player {playerId} registered for round");
        }

        public void UnregisterPlayer(int playerId)
        {
            _activePlayers.Remove(playerId);
        }

        public void EliminatePlayer(int playerId)
        {
            if (!_activePlayers.Contains(playerId)) return;
            if (_eliminatedPlayers.Contains(playerId)) return;

            _eliminatedPlayers.Add(playerId);
            Debug.Log($"[RoundManager] Player {playerId} eliminated! Remaining: {_activePlayers.Count - _eliminatedPlayers.Count}");

            OnPlayerEliminated?.Invoke(playerId);

            // Check if round should end
            if (CheckRoundEndCondition())
            {
                EndRound();
            }
        }

        public bool IsPlayerEliminated(int playerId)
        {
            return _eliminatedPlayers.Contains(playerId);
        }
        #endregion

        #region Win Conditions
        private bool CheckRoundEndCondition()
        {
            if (!lastPlayerStanding) return false;

            int remainingPlayers = _activePlayers.Count - _eliminatedPlayers.Count;
            return remainingPlayers <= 1;
        }

        private int DetermineRoundWinner()
        {
            // If one player remains, they win
            if (lastPlayerStanding)
            {
                var remaining = _activePlayers.Except(_eliminatedPlayers).ToList();
                if (remaining.Count == 1)
                {
                    return remaining[0];
                }
            }

            // If multiple players remain (time expired), determine by score
            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                int highestScore = int.MinValue;
                int winnerId = -1;

                foreach (int playerId in _activePlayers)
                {
                    if (_eliminatedPlayers.Contains(playerId)) continue;

                    int score = scoreManager.GetScore(playerId);
                    if (score > highestScore)
                    {
                        highestScore = score;
                        winnerId = playerId;
                    }
                }

                return winnerId;
            }

            // Fallback: first active non-eliminated player
            return _activePlayers.Except(_eliminatedPlayers).FirstOrDefault();
        }
        #endregion
    }
}
