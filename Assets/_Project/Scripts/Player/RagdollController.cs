using UnityEngine;
using System.Collections.Generic;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Manages ragdoll physics state transitions
    /// Controls joint stiffness and collision layers
    /// </summary>
    public class RagdollController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Ragdoll Settings")]
        [SerializeField] private bool startAsRagdoll = false;
        [SerializeField] private float activeSpring = 200f;
        [SerializeField] private float activeDamper = 10f;
        [SerializeField] private float ragdollSpring = 10f;
        [SerializeField] private float ragdollDamper = 1f;

        [Header("Rigidbody References")]
        [SerializeField] private Rigidbody pelvisRb;
        [SerializeField] private List<Rigidbody> allRigidbodies = new List<Rigidbody>();
        [SerializeField] private List<CharacterJoint> allJoints = new List<CharacterJoint>();
        #endregion

        #region Private Fields
        private bool _isRagdoll = false;
        private Dictionary<Rigidbody, bool> _originalKinematicStates = new Dictionary<Rigidbody, bool>();
        #endregion

        #region Properties
        public bool IsRagdoll => _isRagdoll;
        public Rigidbody PelvisRb => pelvisRb;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            CollectRigidbodies();
            CollectJoints();
            StoreOriginalStates();

            if (startAsRagdoll)
            {
                EnableRagdoll();
            }
            else
            {
                DisableRagdoll();
            }
        }
        #endregion

        #region Initialization
        private void CollectRigidbodies()
        {
            if (allRigidbodies.Count == 0)
            {
                allRigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
            }

            if (pelvisRb == null && allRigidbodies.Count > 0)
            {
                // Assume first rigidbody is pelvis (root)
                pelvisRb = allRigidbodies[0];
            }

            Debug.Log($"[RagdollController] Collected {allRigidbodies.Count} rigidbodies");
        }

        private void CollectJoints()
        {
            if (allJoints.Count == 0)
            {
                allJoints.AddRange(GetComponentsInChildren<CharacterJoint>());
            }

            Debug.Log($"[RagdollController] Collected {allJoints.Count} character joints");
        }

        private void StoreOriginalStates()
        {
            foreach (var rb in allRigidbodies)
            {
                _originalKinematicStates[rb] = rb.isKinematic;
            }
        }
        #endregion

        #region Ragdoll Control
        public void EnableRagdoll()
        {
            if (_isRagdoll) return;

            _isRagdoll = true;

            // Set all rigidbodies to non-kinematic
            foreach (var rb in allRigidbodies)
            {
                rb.isKinematic = false;
            }

            // Loosen joints
            SetJointStiffness(ragdollSpring, ragdollDamper);

            // Change collision layer to PlayerRagdoll
            foreach (var rb in allRigidbodies)
            {
                rb.gameObject.layer = LayerMask.NameToLayer("PlayerRagdoll");
            }

            Debug.Log("[RagdollController] Ragdoll enabled");
        }

        public void DisableRagdoll()
        {
            if (!_isRagdoll && !startAsRagdoll) return;

            _isRagdoll = false;

            // Stiffen joints for active control
            SetJointStiffness(activeSpring, activeDamper);

            // Change collision layer back to Player
            foreach (var rb in allRigidbodies)
            {
                rb.gameObject.layer = LayerMask.NameToLayer("Player");
            }

            Debug.Log("[RagdollController] Ragdoll disabled (active control)");
        }

        private void SetJointStiffness(float spring, float damper)
        {
            foreach (var joint in allJoints)
            {
                if (joint == null) continue;

                var drive = joint.slerpDrive;
                drive.positionSpring = spring;
                drive.positionDamper = damper;
                joint.slerpDrive = drive;
            }
        }
        #endregion

        #region Force Application
        public void AddForceToAll(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            foreach (var rb in allRigidbodies)
            {
                rb.AddForce(force, mode);
            }
        }

        public void AddExplosionForce(float force, Vector3 position, float radius)
        {
            foreach (var rb in allRigidbodies)
            {
                rb.AddExplosionForce(force, position, radius);
            }
        }
        #endregion

        #region Utility
        public void ResetPose()
        {
            // Reset all rigidbodies to zero velocity
            foreach (var rb in allRigidbodies)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public Vector3 GetCenterOfMass()
        {
            Vector3 totalPosition = Vector3.zero;
            int count = 0;

            foreach (var rb in allRigidbodies)
            {
                totalPosition += rb.position;
                count++;
            }

            return count > 0 ? totalPosition / count : Vector3.zero;
        }
        #endregion
    }
}
