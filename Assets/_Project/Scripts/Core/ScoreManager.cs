using UnityEngine;
using System;
using System.Collections.Generic;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Manages scoring system with various KO types
    /// Tracks points per player and triggers score events
    /// </summary>
    public enum KOType
    {
        Standard = 1,       // 1 point
        Hazard = 2,         // 2 points
        Aerial = 3,         // 3 points
        Combo = 4,          // 4 points
        Environmental = 3,  // 3 points
        SelfPreservation = 2, // Bonus for surviving round
        HatTrick = 5,       // 5 points for eliminating 3+ players in one round
        Comeback = 2        // 2 points for KO while in last place
    }

    public class ScoreManager : MonoBehaviour
    {
        #region Events
        public static event Action<int, KOType, int> OnScoreAdded; // playerId, koType, newTotal
        public static event Action OnScoresReset;
        #endregion

        #region Inspector Fields
        [Header("Score Settings")]
        [SerializeField] private bool persistScoresAcrossRounds = false;
        #endregion

        #region Private Fields
        private Dictionary<int, int> _playerScores = new Dictionary<int, int>();
        private Dictionary<int, int> _roundKOCounts = new Dictionary<int, int>(); // Track KOs per round for Hat Trick
        private Dictionary<int, List<KOType>> _roundKOHistory = new Dictionary<int, List<KOType>>();
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            RoundManager.OnRoundStart += HandleRoundStart;
            RoundManager.OnRoundEnd += HandleRoundEnd;
        }

        private void OnDisable()
        {
            RoundManager.OnRoundStart -= HandleRoundStart;
            RoundManager.OnRoundEnd -= HandleRoundEnd;
        }
        #endregion

        #region Score Management
        public void AddScore(int playerId, KOType koType)
        {
            if (!_playerScores.ContainsKey(playerId))
            {
                _playerScores[playerId] = 0;
            }

            int points = (int)koType;
            _playerScores[playerId] += points;

            // Track KO count
            if (!_roundKOCounts.ContainsKey(playerId))
            {
                _roundKOCounts[playerId] = 0;
            }
            _roundKOCounts[playerId]++;

            // Track KO history
            if (!_roundKOHistory.ContainsKey(playerId))
            {
                _roundKOHistory[playerId] = new List<KOType>();
            }
            _roundKOHistory[playerId].Add(koType);

            Debug.Log($"[ScoreManager] Player {playerId} scored {points} points ({koType}). Total: {_playerScores[playerId]}");

            OnScoreAdded?.Invoke(playerId, koType, _playerScores[playerId]);

            // Check for Hat Trick
            if (_roundKOCounts[playerId] >= 3)
            {
                CheckForHatTrick(playerId);
            }
        }

        public void AddComboScore(List<int> playerIds, int targetPlayerId)
        {
            // Award combo points to all players involved in the combo KO
            foreach (int playerId in playerIds)
            {
                AddScore(playerId, KOType.Combo);
            }

            Debug.Log($"[ScoreManager] Combo KO! Players {string.Join(", ", playerIds)} eliminated Player {targetPlayerId}");
        }

        public int GetScore(int playerId)
        {
            return _playerScores.ContainsKey(playerId) ? _playerScores[playerId] : 0;
        }

        public Dictionary<int, int> GetAllScores()
        {
            return new Dictionary<int, int>(_playerScores);
        }

        public void ResetScores()
        {
            _playerScores.Clear();
            _roundKOCounts.Clear();
            _roundKOHistory.Clear();

            Debug.Log("[ScoreManager] All scores reset");
            OnScoresReset?.Invoke();
        }

        public void ResetPlayerScore(int playerId)
        {
            if (_playerScores.ContainsKey(playerId))
            {
                _playerScores[playerId] = 0;
            }
        }
        #endregion

        #region Round Events
        private void HandleRoundStart(int roundNumber)
        {
            // Reset round-specific tracking
            _roundKOCounts.Clear();
            _roundKOHistory.Clear();

            if (!persistScoresAcrossRounds)
            {
                ResetScores();
            }

            Debug.Log($"[ScoreManager] Round {roundNumber} started. Scores reset: {!persistScoresAcrossRounds}");
        }

        private void HandleRoundEnd(int winnerId)
        {
            // Award self-preservation bonus to all surviving players
            var roundManager = FindObjectOfType<RoundManager>();
            if (roundManager != null)
            {
                foreach (var kvp in _playerScores)
                {
                    int playerId = kvp.Key;
                    if (!roundManager.IsPlayerEliminated(playerId))
                    {
                        AddScore(playerId, KOType.SelfPreservation);
                    }
                }
            }
        }
        #endregion

        #region Special Achievements
        private void CheckForHatTrick(int playerId)
        {
            if (_roundKOCounts[playerId] == 3)
            {
                // First time hitting 3 KOs this round - award Hat Trick
                AddScore(playerId, KOType.HatTrick);
                Debug.Log($"[ScoreManager] HAT TRICK! Player {playerId} eliminated 3 opponents!");
            }
        }

        public void AwardComebackBonus(int playerId)
        {
            // Called externally when a player in last place gets a KO
            if (IsPlayerInLastPlace(playerId))
            {
                AddScore(playerId, KOType.Comeback);
                Debug.Log($"[ScoreManager] COMEBACK! Player {playerId} scored while in last place!");
            }
        }

        private bool IsPlayerInLastPlace(int playerId)
        {
            if (!_playerScores.ContainsKey(playerId)) return false;

            int playerScore = _playerScores[playerId];
            int lowestScore = int.MaxValue;

            foreach (var score in _playerScores.Values)
            {
                if (score < lowestScore)
                {
                    lowestScore = score;
                }
            }

            return playerScore == lowestScore;
        }
        #endregion

        #region Statistics
        public int GetPlayerKOCount(int playerId)
        {
            return _roundKOCounts.ContainsKey(playerId) ? _roundKOCounts[playerId] : 0;
        }

        public List<KOType> GetPlayerKOHistory(int playerId)
        {
            return _roundKOHistory.ContainsKey(playerId) ? _roundKOHistory[playerId] : new List<KOType>();
        }

        public int GetHighestScore()
        {
            int highest = 0;
            foreach (var score in _playerScores.Values)
            {
                if (score > highest)
                {
                    highest = score;
                }
            }
            return highest;
        }

        public int GetLeadingPlayer()
        {
            int leaderId = -1;
            int highestScore = int.MinValue;

            foreach (var kvp in _playerScores)
            {
                if (kvp.Value > highestScore)
                {
                    highestScore = kvp.Value;
                    leaderId = kvp.Key;
                }
            }

            return leaderId;
        }
        #endregion
    }
}
