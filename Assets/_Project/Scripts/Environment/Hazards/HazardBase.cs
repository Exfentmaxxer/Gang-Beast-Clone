using UnityEngine;
using TumbleRumble.Player;

namespace TumbleRumble.Environment
{
    /// <summary>
    /// Base class for all environmental hazards
    /// Provides common functionality for damage, knockback, and elimination
    /// </summary>
    public abstract class HazardBase : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Hazard Settings")]
        [SerializeField] protected float damageAmount = 10f;
        [SerializeField] protected float knockbackForce = 5f;
        [SerializeField] protected bool instantKill = false;
        [SerializeField] protected LayerMask affectedLayers;

        [Header("Visual Feedback")]
        [SerializeField] protected GameObject warningVFX;
        [SerializeField] protected GameObject activeVFX;
        [SerializeField] protected AudioClip activationSound;
        #endregion

        #region Protected Fields
        protected bool _isActive = true;
        protected AudioSource _audioSource;
        #endregion

        #region Unity Lifecycle
        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        protected virtual void Start()
        {
            Initialize();
        }
        #endregion

        #region Initialization
        protected virtual void Initialize()
        {
            // Override in derived classes
        }
        #endregion

        #region Hazard Activation
        public virtual void Activate()
        {
            _isActive = true;

            if (activeVFX != null)
            {
                activeVFX.SetActive(true);
            }

            if (activationSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(activationSound);
            }
        }

        public virtual void Deactivate()
        {
            _isActive = false;

            if (activeVFX != null)
            {
                activeVFX.SetActive(false);
            }
        }
        #endregion

        #region Player Interaction
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_isActive) return;
            if (((1 << other.gameObject.layer) & affectedLayers) == 0) return;

            HandlePlayerContact(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (!_isActive) return;
            if (((1 << other.gameObject.layer) & affectedLayers) == 0) return;

            HandlePlayerStay(other);
        }

        protected virtual void HandlePlayerContact(Collider playerCollider)
        {
            PlayerController player = playerCollider.GetComponentInParent<PlayerController>();
            if (player == null) return;

            if (instantKill)
            {
                EliminatePlayer(player);
            }
            else
            {
                ApplyKnockback(player, playerCollider);
            }
        }

        protected virtual void HandlePlayerStay(Collider playerCollider)
        {
            // Override for continuous damage hazards
        }
        #endregion

        #region Effects
        protected virtual void ApplyKnockback(PlayerController player, Collider playerCollider)
        {
            Rigidbody rb = playerCollider.GetComponent<Rigidbody>();
            if (rb == null) return;

            Vector3 knockbackDirection = (playerCollider.transform.position - transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            Debug.Log($"[HazardBase] Knocked back player {player.PlayerId}");
        }

        protected virtual void EliminatePlayer(PlayerController player)
        {
            player.Eliminate();
            Debug.Log($"[HazardBase] Eliminated player {player.PlayerId}");
        }
        #endregion

        #region Warning System
        protected void ShowWarning(float duration)
        {
            if (warningVFX != null)
            {
                warningVFX.SetActive(true);
                Invoke(nameof(HideWarning), duration);
            }
        }

        private void HideWarning()
        {
            if (warningVFX != null)
            {
                warningVFX.SetActive(false);
            }
        }
        #endregion
    }
}
