using UnityEngine;
using System.Collections.Generic;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Handles grab mechanics: grabbing players, objects, and ledges
    /// Supports multi-limb grabbing for Tentacled species
    /// </summary>
    public class GrabSystem : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Grab Settings")]
        [SerializeField] private float grabRange = 2f;
        [SerializeField] private float grabForce = 500f;
        [SerializeField] private LayerMask grabMask;
        [SerializeField] private int maxSimultaneousGrabs = 1; // Increased for Tentacled species

        [Header("References")]
        [SerializeField] private Transform grabPoint;
        [SerializeField] private Rigidbody playerRb;
        #endregion

        #region Private Fields
        private List<GameObject> _grabbedObjects = new List<GameObject>();
        private List<FixedJoint> _grabJoints = new List<FixedJoint>();
        private bool _isGrabbing = false;
        #endregion

        #region Properties
        public bool IsGrabbing => _isGrabbing && _grabbedObjects.Count > 0;
        public int GrabbedCount => _grabbedObjects.Count;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (playerRb == null)
            {
                playerRb = GetComponent<RagdollController>().PelvisRb;
            }

            if (grabPoint == null)
            {
                // Create grab point in front of player
                GameObject grabPointObj = new GameObject("GrabPoint");
                grabPointObj.transform.SetParent(playerRb.transform);
                grabPointObj.transform.localPosition = Vector3.forward * 0.5f;
                grabPoint = grabPointObj.transform;
            }
        }
        #endregion

        #region Grab Actions
        public void TryGrab()
        {
            if (_grabbedObjects.Count >= maxSimultaneousGrabs)
            {
                Debug.Log("[GrabSystem] Maximum grab limit reached");
                return;
            }

            // Raycast/SphereCast to find grabbable objects
            Collider[] hits = Physics.OverlapSphere(grabPoint.position, grabRange, grabMask);

            foreach (var hit in hits)
            {
                if (hit.gameObject == gameObject) continue; // Don't grab self
                if (_grabbedObjects.Contains(hit.gameObject)) continue; // Already grabbed

                // Try to grab
                if (CanGrab(hit.gameObject))
                {
                    GrabObject(hit.gameObject);
                    break; // Grab one object per input (unless multi-limb)
                }
            }
        }

        private bool CanGrab(GameObject target)
        {
            // Check if target is a player
            PlayerController targetPlayer = target.GetComponent<PlayerController>();
            if (targetPlayer != null && targetPlayer.IsKnockedOut)
            {
                return true;
            }

            // Check if target is a grabbable object
            if (target.GetComponent<Rigidbody>() != null)
            {
                return true;
            }

            return false;
        }

        private void GrabObject(GameObject target)
        {
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb == null)
            {
                // Try to get rigidbody from parent
                targetRb = target.GetComponentInParent<Rigidbody>();
            }

            if (targetRb == null)
            {
                Debug.LogWarning($"[GrabSystem] Cannot grab {target.name} - no rigidbody");
                return;
            }

            // Create fixed joint to hold object
            FixedJoint joint = playerRb.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = targetRb;
            joint.breakForce = grabForce;
            joint.breakTorque = grabForce;
            joint.enableCollision = false;
            joint.enablePreprocessing = false; // Better performance

            _grabbedObjects.Add(target);
            _grabJoints.Add(joint);
            _isGrabbing = true;

            Debug.Log($"[GrabSystem] Grabbed {target.name}");

            // Notify target if it's a player
            PlayerController targetPlayer = target.GetComponent<PlayerController>();
            if (targetPlayer != null)
            {
                OnGrabPlayer(targetPlayer);
            }
        }

        public void ReleaseGrab()
        {
            if (_grabbedObjects.Count == 0) return;

            // Release all grabbed objects
            for (int i = _grabJoints.Count - 1; i >= 0; i--)
            {
                if (_grabJoints[i] != null)
                {
                    Destroy(_grabJoints[i]);
                }
            }

            Debug.Log($"[GrabSystem] Released {_grabbedObjects.Count} grabs");

            _grabbedObjects.Clear();
            _grabJoints.Clear();
            _isGrabbing = false;
        }

        public void ReleaseGrab(int index)
        {
            if (index < 0 || index >= _grabbedObjects.Count) return;

            if (_grabJoints[index] != null)
            {
                Destroy(_grabJoints[index]);
            }

            _grabbedObjects.RemoveAt(index);
            _grabJoints.RemoveAt(index);

            if (_grabbedObjects.Count == 0)
            {
                _isGrabbing = false;
            }
        }

        public void ThrowGrabbed(Vector3 direction, float throwForce = 10f)
        {
            if (_grabbedObjects.Count == 0) return;

            foreach (var grabbed in _grabbedObjects)
            {
                Rigidbody targetRb = grabbed.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    targetRb.AddForce(direction.normalized * throwForce, ForceMode.Impulse);
                }
            }

            ReleaseGrab();
        }
        #endregion

        #region Player-Specific Grabs
        private void OnGrabPlayer(PlayerController target)
        {
            // Could apply effects to grabbed player
            Debug.Log($"[GrabSystem] Grabbed player {target.PlayerId}");
        }
        #endregion

        #region Multi-Limb Support
        public void SetMaxGrabs(int count)
        {
            maxSimultaneousGrabs = Mathf.Max(1, count);
        }
        #endregion

        #region Joint Break Handling
        private void OnJointBreak(float breakForce)
        {
            // Clean up broken joints
            for (int i = _grabJoints.Count - 1; i >= 0; i--)
            {
                if (_grabJoints[i] == null)
                {
                    _grabbedObjects.RemoveAt(i);
                    _grabJoints.RemoveAt(i);
                }
            }

            if (_grabbedObjects.Count == 0)
            {
                _isGrabbing = false;
            }
        }
        #endregion

        #region Debug
        private void OnDrawGizmosSelected()
        {
            if (grabPoint != null)
            {
                Gizmos.color = _isGrabbing ? Color.green : Color.yellow;
                Gizmos.DrawWireSphere(grabPoint.position, grabRange);
            }
        }
        #endregion
    }
}
