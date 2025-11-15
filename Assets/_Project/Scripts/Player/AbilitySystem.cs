using UnityEngine;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Manages cosmic energy and special abilities
    /// Handles energy acquisition, storage, and ability activation
    /// </summary>
    public class AbilitySystem : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Energy Settings")]
        [SerializeField] private int maxEnergy = 100;
        [SerializeField] private int currentEnergy = 0;

        [Header("Ability Costs")]
        [SerializeField] private int energyBurstCost = 25;
        [SerializeField] private int gravityShiftCost = 30;
        [SerializeField] private int tentacleExtensionCost = 20;
        [SerializeField] private int shieldBubbleCost = 40;
        [SerializeField] private int speedBoostCost = 15;

        [Header("Ability Settings")]
        [SerializeField] private float energyBurstRadius = 5f;
        [SerializeField] private float energyBurstForce = 500f;
        [SerializeField] private float abilityCooldown = 2f;

        [Header("References")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Rigidbody playerRb;
        #endregion

        #region Private Fields
        private float _lastAbilityTime = 0f;
        private bool _canUseAbility = true;
        private bool _shieldActive = false;
        private bool _speedBoostActive = false;
        #endregion

        #region Properties
        public int CurrentEnergy => currentEnergy;
        public int MaxEnergy => maxEnergy;
        public float EnergyPercentage => (float)currentEnergy / maxEnergy;
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
        }

        private void Update()
        {
            // Cooldown management
            if (!_canUseAbility && Time.time >= _lastAbilityTime + abilityCooldown)
            {
                _canUseAbility = true;
            }
        }
        #endregion

        #region Energy Management
        public void AddEnergy(int amount)
        {
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
            Debug.Log($"[AbilitySystem] Energy added: +{amount}. Total: {currentEnergy}/{maxEnergy}");
        }

        public bool SpendEnergy(int amount)
        {
            if (currentEnergy >= amount)
            {
                currentEnergy -= amount;
                return true;
            }
            return false;
        }

        public void ResetEnergy()
        {
            currentEnergy = 0;
        }
        #endregion

        #region Ability Usage
        public void UseAbility()
        {
            if (!_canUseAbility) return;

            // Default ability: Energy Burst
            UseEnergyBurst();
        }

        public void UseEnergyBurst()
        {
            if (!SpendEnergy(energyBurstCost)) return;

            _canUseAbility = false;
            _lastAbilityTime = Time.time;

            // Create radial knockback wave
            Collider[] hits = Physics.OverlapSphere(transform.position, energyBurstRadius);

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue;

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(direction * energyBurstForce, ForceMode.Impulse);
                }
            }

            Debug.Log("[AbilitySystem] Energy Burst activated!");

            // Could spawn VFX here
        }

        public void UseGravityShift(Vector3 newGravityDirection)
        {
            if (!SpendEnergy(gravityShiftCost)) return;

            _canUseAbility = false;
            _lastAbilityTime = Time.time;

            // Apply custom gravity for duration
            StartCoroutine(GravityShiftCoroutine(newGravityDirection, 3f));

            Debug.Log("[AbilitySystem] Gravity Shift activated!");
        }

        private System.Collections.IEnumerator GravityShiftCoroutine(Vector3 direction, float duration)
        {
            float elapsed = 0f;
            float customGravity = 20f;

            while (elapsed < duration)
            {
                playerRb.AddForce(direction.normalized * customGravity, ForceMode.Acceleration);
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            Debug.Log("[AbilitySystem] Gravity Shift ended");
        }

        public void UseTentacleExtension()
        {
            if (!SpendEnergy(tentacleExtensionCost)) return;

            _canUseAbility = false;
            _lastAbilityTime = Time.time;

            // Increase grab range temporarily
            var grabSystem = GetComponent<GrabSystem>();
            if (grabSystem != null)
            {
                // This would extend grab range
                Debug.Log("[AbilitySystem] Tentacle Extension activated!");
            }
        }

        public void UseShieldBubble()
        {
            if (!SpendEnergy(shieldBubbleCost)) return;

            _canUseAbility = false;
            _lastAbilityTime = Time.time;

            _shieldActive = true;

            // Invulnerability for 1.5 seconds
            Invoke(nameof(DeactivateShield), 1.5f);

            Debug.Log("[AbilitySystem] Shield Bubble activated!");
        }

        private void DeactivateShield()
        {
            _shieldActive = false;
            Debug.Log("[AbilitySystem] Shield Bubble deactivated");
        }

        public void UseSpeedBoost()
        {
            if (!SpendEnergy(speedBoostCost)) return;

            _canUseAbility = false;
            _lastAbilityTime = Time.time;

            _speedBoostActive = true;

            // Speed boost for 4 seconds
            Invoke(nameof(DeactivateSpeedBoost), 4f);

            Debug.Log("[AbilitySystem] Speed Boost activated!");
        }

        private void DeactivateSpeedBoost()
        {
            _speedBoostActive = false;
            Debug.Log("[AbilitySystem] Speed Boost deactivated");
        }
        #endregion

        #region Energy Gain Events
        // Called by other systems when player performs actions
        public void OnHitLanded()
        {
            AddEnergy(5);
        }

        public void OnGrabSuccessful()
        {
            AddEnergy(8);
        }

        public void OnThrowExecuted()
        {
            AddEnergy(10);
        }

        public void OnHazardSurvived()
        {
            AddEnergy(15);
        }

        public void OnKOAchieved()
        {
            AddEnergy(30);
        }
        #endregion

        #region Query Methods
        public bool IsShieldActive()
        {
            return _shieldActive;
        }

        public bool IsSpeedBoostActive()
        {
            return _speedBoostActive;
        }

        public float GetSpeedMultiplier()
        {
            return _speedBoostActive ? 1.5f : 1f;
        }
        #endregion
    }
}
