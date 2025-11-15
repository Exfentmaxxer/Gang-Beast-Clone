using UnityEngine;
using TumbleRumble.Core;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Handles combat actions: punching, kicking, damage application
    /// Detects hits and awards points for KOs
    /// </summary>
    public class CombatSystem : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Combat Settings")]
        [SerializeField] private float punchForce = 15f;
        [SerializeField] private float punchRange = 1.5f;
        [SerializeField] private float punchCooldown = 0.5f;
        [SerializeField] private LayerMask hitMask;

        [Header("References")]
        [SerializeField] private Transform punchPoint;
        [SerializeField] private Rigidbody playerRb;
        [SerializeField] private PlayerController playerController;
        #endregion

        #region Private Fields
        private float _lastPunchTime = 0f;
        private bool _canPunch = true;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (playerController == null)
            {
                playerController = GetComponent<PlayerController>();
            }

            if (playerRb == null)
            {
                playerRb = GetComponent<RagdollController>().PelvisRb;
            }

            if (punchPoint == null)
            {
                GameObject punchPointObj = new GameObject("PunchPoint");
                punchPointObj.transform.SetParent(playerRb.transform);
                punchPointObj.transform.localPosition = Vector3.forward * 0.8f;
                punchPoint = punchPointObj.transform;
            }
        }

        private void Update()
        {
            // Cooldown management
            if (!_canPunch && Time.time >= _lastPunchTime + punchCooldown)
            {
                _canPunch = true;
            }
        }
        #endregion

        #region Combat Actions
        public void Punch()
        {
            if (!_canPunch) return;

            _canPunch = false;
            _lastPunchTime = Time.time;

            // Detect targets in range
            Collider[] hits = Physics.OverlapSphere(punchPoint.position, punchRange, hitMask);

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue; // Don't hit self

                // Apply force
                Rigidbody targetRb = hit.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    Vector3 direction = (hit.transform.position - transform.position).normalized;
                    targetRb.AddForce(direction * punchForce, ForceMode.Impulse);

                    // Check if target is a player
                    PlayerController targetPlayer = hit.GetComponent<PlayerController>();
                    if (targetPlayer != null)
                    {
                        OnHitPlayer(targetPlayer);
                    }
                }
            }

            Debug.Log("[CombatSystem] Punch executed");

            // Could trigger animation here
        }

        public void Kick()
        {
            // Similar to punch but different force/range
            Punch(); // Simplified for now
        }
        #endregion

        #region Hit Detection
        private void OnHitPlayer(PlayerController target)
        {
            Debug.Log($"[CombatSystem] Player {playerController.PlayerId} hit Player {target.PlayerId}");

            // Award points to attacker
            var scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                // Standard hit doesn't award points, only KOs do
                // But we could add energy here via AbilitySystem
            }

            // Could trigger effects on target
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Detect if player was knocked off by another player
            if (collision.relativeVelocity.magnitude > 12f)
            {
                // High-impact collision - possible KO
                CheckForKO(collision);
            }
        }

        private void CheckForKO(Collision collision)
        {
            // This would be called when player falls off map or is knocked out
            // For now, just log
            Debug.Log($"[CombatSystem] High impact collision: {collision.relativeVelocity.magnitude}");
        }
        #endregion

        #region Debug
        private void OnDrawGizmosSelected()
        {
            if (punchPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(punchPoint.position, punchRange);
            }
        }
        #endregion
    }
}
