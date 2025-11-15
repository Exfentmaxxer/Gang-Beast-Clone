using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TumbleRumble.Core
{
    /// <summary>
    /// Manages match flow: rounds, scoring, win conditions
    /// Coordinates between RoundManager and ScoreManager
    /// </summary>
    public class MatchManager : MonoBehaviour
    {
        #region Events
        public static event Action OnMatchStart;
        public static event Action<int> OnMatchEnd; // Parameter: winner player ID
        public static event Action<List<PlayerScore>> OnScoresUpdated;
        #endregion

        #region Inspector Fields
        [Header("Match Settings")]
        [SerializeField] private int roundsToWin = 3;
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private int maxPlayers = 8;

        [Header("References")]
        [SerializeField] private RoundManager roundManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private Transform[] playerSpawnPoints;
        #endregion

        #region Private Fields
        private bool _matchActive = false;
        private List<int> _activePlayers = new List<int>();
        private Dictionary<int, int> _roundWins = new Dictionary<int, int>();
        #endregion

        #region Properties
        public bool IsMatchActive => _matchActive;
        public int RoundsToWin => roundsToWin;
        public List<int> ActivePlayers => _activePlayers;
        public int CurrentRound => roundManager != null ? roundManager.CurrentRoundNumber : 0;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // Find managers if not assigned
            if (roundManager == null)
                roundManager = GetComponent<RoundManager>();
            if (scoreManager == null)
                scoreManager = GetComponent<ScoreManager>();
        }

        private void OnEnable()
        {
            RoundManager.OnRoundEnd += HandleRoundEnd;
        }

        private void OnDisable()
        {
            RoundManager.OnRoundEnd -= HandleRoundEnd;
        }
        #endregion

        #region Match Control
        public void StartMatch(List<int> playerIds)
        {
            if (_matchActive)
            {
                Debug.LogWarning("[MatchManager] Match already active!");
                return;
            }

            if (playerIds.Count < minPlayers)
            {
                Debug.LogError($"[MatchManager] Not enough players! Need at least {minPlayers}");
                return;
            }

            Debug.Log($"[MatchManager] Starting match with {playerIds.Count} players");

            _activePlayers = new List<int>(playerIds);
            _roundWins.Clear();

            foreach (int playerId in _activePlayers)
            {
                _roundWins[playerId] = 0;
            }

            _matchActive = true;
            OnMatchStart?.Invoke();

            // Start first round
            if (roundManager != null)
            {
                roundManager.StartRound();
            }
        }

        public void EndMatch(int winnerId)
        {
            if (!_matchActive) return;

            Debug.Log($"[MatchManager] Match ended! Winner: Player {winnerId}");

            _matchActive = false;
            OnMatchEnd?.Invoke(winnerId);

            // Could transition to victory screen here
        }

        public void AbortMatch()
        {
            if (!_matchActive) return;

            Debug.Log("[MatchManager] Match aborted");

            _matchActive = false;
            _activePlayers.Clear();
            _roundWins.Clear();

            if (roundManager != null)
            {
                roundManager.EndRound();
            }
        }
        #endregion

        #region Round Management
        private void HandleRoundEnd(int roundWinnerId)
        {
            if (!_matchActive) return;

            // Award round win
            if (_roundWins.ContainsKey(roundWinnerId))
            {
                _roundWins[roundWinnerId]++;
                Debug.Log($"[MatchManager] Player {roundWinnerId} wins round! Total rounds won: {_roundWins[roundWinnerId]}");
            }

            // Check for match winner
            int matchWinner = CheckForMatchWinner();
            if (matchWinner != -1)
            {
                EndMatch(matchWinner);
                return;
            }

            // Continue to next round
            StartCoroutine(TransitionToNextRound());
        }

        private int CheckForMatchWinner()
        {
            foreach (var kvp in _roundWins)
            {
                if (kvp.Value >= roundsToWin)
                {
                    return kvp.Key; // This player has won enough rounds
                }
            }
            return -1; // No winner yet
        }

        private IEnumerator TransitionToNextRound()
        {
            // Wait for round transition UI
            yield return new WaitForSeconds(3f);

            if (roundManager != null)
            {
                roundManager.StartRound();
            }
        }
        #endregion

        #region Player Management
        public void RegisterPlayer(int playerId)
        {
            if (!_activePlayers.Contains(playerId))
            {
                _activePlayers.Add(playerId);
                _roundWins[playerId] = 0;
                Debug.Log($"[MatchManager] Player {playerId} registered");
            }
        }

        public void UnregisterPlayer(int playerId)
        {
            if (_activePlayers.Contains(playerId))
            {
                _activePlayers.Remove(playerId);
                _roundWins.Remove(playerId);
                Debug.Log($"[MatchManager] Player {playerId} unregistered");

                // Check if too few players remain
                if (_matchActive && _activePlayers.Count < minPlayers)
                {
                    Debug.Log("[MatchManager] Not enough players remaining, ending match");
                    AbortMatch();
                }
            }
        }

        public int GetPlayerRoundWins(int playerId)
        {
            return _roundWins.ContainsKey(playerId) ? _roundWins[playerId] : 0;
        }

        public Vector3 GetSpawnPoint(int playerIndex)
        {
            if (playerSpawnPoints == null || playerSpawnPoints.Length == 0)
            {
                Debug.LogWarning("[MatchManager] No spawn points assigned!");
                return Vector3.zero;
            }

            int index = playerIndex % playerSpawnPoints.Length;
            return playerSpawnPoints[index].position;
        }
        #endregion

        #region Query Methods
        public List<PlayerScore> GetCurrentStandings()
        {
            var standings = new List<PlayerScore>();

            foreach (var playerId in _activePlayers)
            {
                int roundWins = GetPlayerRoundWins(playerId);
                int score = scoreManager != null ? scoreManager.GetScore(playerId) : 0;

                standings.Add(new PlayerScore
                {
                    playerId = playerId,
                    score = score,
                    roundWins = roundWins
                });
            }

            // Sort by round wins, then by score
            standings = standings.OrderByDescending(p => p.roundWins)
                                 .ThenByDescending(p => p.score)
                                 .ToList();

            return standings;
        }
        #endregion
    }

    #region Data Structures
    [System.Serializable]
    public struct PlayerScore
    {
        public int playerId;
        public int score;
        public int roundWins;
    }
    #endregion
}
