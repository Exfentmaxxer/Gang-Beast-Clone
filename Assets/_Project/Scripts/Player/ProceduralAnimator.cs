using UnityEngine;
using System.Collections.Generic;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Procedural animation system - drives ragdoll joints to create realistic movement
    /// Uses spring-based physics to animate character without traditional animation files
    /// </summary>
    public class ProceduralAnimator : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float walkCycleSpeed = 2f;
        [SerializeField] private float walkStepHeight = 0.3f;
        [SerializeField] private float walkStepLength = 0.5f;
        [SerializeField] private float armSwingAmount = 30f;
        [SerializeField] private float bodyBobAmount = 0.1f;

        [Header("Joint Targets")]
        [SerializeField] private Transform leftArmTarget;
        [SerializeField] private Transform rightArmTarget;
        [SerializeField] private Transform leftLegTarget;
        [SerializeField] private Transform rightLegTarget;
        [SerializeField] private Transform bodyTarget;

        [Header("References")]
        [SerializeField] private Rigidbody bodyRigidbody;
        [SerializeField] private ConfigurableJoint leftArmJoint;
        [SerializeField] private ConfigurableJoint rightArmJoint;
        [SerializeField] private ConfigurableJoint leftLegJoint;
        [SerializeField] private ConfigurableJoint rightLegJoint;

        private float walkCycle = 0f;
        private Vector3 moveDirection;
        private bool isMoving = false;
        private bool isJumping = false;
        private bool isRagdolled = false;

        private Vector3 leftLegRestPosition;
        private Vector3 rightLegRestPosition;
        private Vector3 leftArmRestPosition;
        private Vector3 rightArmRestPosition;

        private void Start()
        {
            // Store rest positions
            if (leftLegTarget != null) leftLegRestPosition = leftLegTarget.localPosition;
            if (rightLegTarget != null) rightLegRestPosition = rightLegTarget.localPosition;
            if (leftArmTarget != null) leftArmRestPosition = leftArmTarget.localPosition;
            if (rightArmTarget != null) rightArmRestPosition = rightArmTarget.localPosition;
        }

        private void FixedUpdate()
        {
            if (isRagdolled) return;

            // Update walk cycle
            if (isMoving)
            {
                walkCycle += Time.fixedDeltaTime * walkCycleSpeed;
                AnimateWalking();
            }
            else
            {
                // Return to idle pose
                AnimateIdle();
            }

            // Apply body bob
            AnimateBodyBob();
        }

        /// <summary>
        /// Set movement direction for procedural walking animation
        /// </summary>
        public void SetMovement(Vector3 direction)
        {
            moveDirection = direction.normalized;
            isMoving = direction.magnitude > 0.1f;
        }

        /// <summary>
        /// Trigger jump animation
        /// </summary>
        public void TriggerJump()
        {
            isJumping = true;
            AnimateJump();
            Invoke(nameof(ResetJump), 0.5f);
        }

        /// <summary>
        /// Enable/disable ragdoll mode
        /// </summary>
        public void SetRagdoll(bool enabled)
        {
            isRagdolled = enabled;
        }

        /// <summary>
        /// Animate punching motion
        /// </summary>
        public void TriggerPunch(bool useRightArm = true)
        {
            if (useRightArm && rightArmTarget != null)
            {
                StartCoroutine(PunchAnimation(rightArmTarget, rightArmRestPosition));
            }
            else if (leftArmTarget != null)
            {
                StartCoroutine(PunchAnimation(leftArmTarget, leftArmRestPosition));
            }
        }

        private void AnimateWalking()
        {
            // Procedural walking using sine wave for leg movement
            float leftLegPhase = Mathf.Sin(walkCycle);
            float rightLegPhase = Mathf.Sin(walkCycle + Mathf.PI); // Opposite phase

            // Animate legs
            if (leftLegTarget != null)
            {
                Vector3 leftLegPos = leftLegRestPosition;
                leftLegPos.y += Mathf.Max(0, leftLegPhase) * walkStepHeight;
                leftLegPos.z += leftLegPhase * walkStepLength;
                leftLegTarget.localPosition = Vector3.Lerp(leftLegTarget.localPosition, leftLegPos, Time.fixedDeltaTime * 10f);
            }

            if (rightLegTarget != null)
            {
                Vector3 rightLegPos = rightLegRestPosition;
                rightLegPos.y += Mathf.Max(0, rightLegPhase) * walkStepHeight;
                rightLegPos.z += rightLegPhase * walkStepLength;
                rightLegTarget.localPosition = Vector3.Lerp(rightLegTarget.localPosition, rightLegPos, Time.fixedDeltaTime * 10f);
            }

            // Animate arms (swing opposite to legs)
            if (leftArmTarget != null)
            {
                Vector3 leftArmPos = leftArmRestPosition;
                leftArmPos.z += rightLegPhase * (walkStepLength * 0.5f);
                Quaternion leftArmRot = Quaternion.Euler(rightLegPhase * armSwingAmount, 0, 0);
                leftArmTarget.localPosition = Vector3.Lerp(leftArmTarget.localPosition, leftArmPos, Time.fixedDeltaTime * 8f);
                leftArmTarget.localRotation = Quaternion.Slerp(leftArmTarget.localRotation, leftArmRot, Time.fixedDeltaTime * 8f);
            }

            if (rightArmTarget != null)
            {
                Vector3 rightArmPos = rightArmRestPosition;
                rightArmPos.z += leftLegPhase * (walkStepLength * 0.5f);
                Quaternion rightArmRot = Quaternion.Euler(leftLegPhase * armSwingAmount, 0, 0);
                rightArmTarget.localPosition = Vector3.Lerp(rightArmTarget.localPosition, rightArmPos, Time.fixedDeltaTime * 8f);
                rightArmTarget.localRotation = Quaternion.Slerp(rightArmTarget.localRotation, rightArmRot, Time.fixedDeltaTime * 8f);
            }
        }

        private void AnimateIdle()
        {
            // Smoothly return to rest positions
            if (leftLegTarget != null)
            {
                leftLegTarget.localPosition = Vector3.Lerp(leftLegTarget.localPosition, leftLegRestPosition, Time.fixedDeltaTime * 5f);
            }

            if (rightLegTarget != null)
            {
                rightLegTarget.localPosition = Vector3.Lerp(rightLegTarget.localPosition, rightLegRestPosition, Time.fixedDeltaTime * 5f);
            }

            if (leftArmTarget != null)
            {
                leftArmTarget.localPosition = Vector3.Lerp(leftArmTarget.localPosition, leftArmRestPosition, Time.fixedDeltaTime * 5f);
                leftArmTarget.localRotation = Quaternion.Slerp(leftArmTarget.localRotation, Quaternion.identity, Time.fixedDeltaTime * 5f);
            }

            if (rightArmTarget != null)
            {
                rightArmTarget.localPosition = Vector3.Lerp(rightArmTarget.localPosition, rightArmRestPosition, Time.fixedDeltaTime * 5f);
                rightArmTarget.localRotation = Quaternion.Slerp(rightArmTarget.localRotation, Quaternion.identity, Time.fixedDeltaTime * 5f);
            }
        }

        private void AnimateBodyBob()
        {
            if (bodyTarget != null && isMoving)
            {
                // Subtle body bob during walking
                float bobAmount = Mathf.Sin(walkCycle * 2f) * bodyBobAmount;
                Vector3 targetPos = bodyTarget.localPosition;
                targetPos.y = bobAmount;
                bodyTarget.localPosition = Vector3.Lerp(bodyTarget.localPosition, targetPos, Time.fixedDeltaTime * 10f);
            }
        }

        private void AnimateJump()
        {
            // Pull legs up during jump
            if (leftLegTarget != null)
            {
                Vector3 jumpPos = leftLegRestPosition;
                jumpPos.y += 0.5f;
                leftLegTarget.localPosition = jumpPos;
            }

            if (rightLegTarget != null)
            {
                Vector3 jumpPos = rightLegRestPosition;
                jumpPos.y += 0.5f;
                rightLegTarget.localPosition = jumpPos;
            }

            // Raise arms
            if (leftArmTarget != null)
            {
                leftArmTarget.localRotation = Quaternion.Euler(-45f, 0, 0);
            }

            if (rightArmTarget != null)
            {
                rightArmTarget.localRotation = Quaternion.Euler(-45f, 0, 0);
            }
        }

        private void ResetJump()
        {
            isJumping = false;
        }

        private System.Collections.IEnumerator PunchAnimation(Transform armTarget, Vector3 restPosition)
        {
            // Punch forward
            float punchTime = 0f;
            float punchDuration = 0.2f;
            Vector3 punchPosition = restPosition + Vector3.forward * 0.5f;

            while (punchTime < punchDuration)
            {
                punchTime += Time.deltaTime;
                float t = punchTime / punchDuration;
                armTarget.localPosition = Vector3.Lerp(restPosition, punchPosition, t);
                yield return null;
            }

            // Return to rest
            float returnTime = 0f;
            float returnDuration = 0.3f;

            while (returnTime < returnDuration)
            {
                returnTime += Time.deltaTime;
                float t = returnTime / returnDuration;
                armTarget.localPosition = Vector3.Lerp(punchPosition, restPosition, t);
                yield return null;
            }

            armTarget.localPosition = restPosition;
        }

        /// <summary>
        /// Auto-setup: find all necessary joints and create targets
        /// </summary>
        public void AutoSetup()
        {
            // Find body rigidbody
            bodyRigidbody = GetComponent<Rigidbody>();

            // Create target parent
            GameObject targetsParent = new GameObject("AnimationTargets");
            targetsParent.transform.SetParent(transform);
            targetsParent.transform.localPosition = Vector3.zero;

            // Create targets for each limb
            leftLegTarget = CreateTarget(targetsParent.transform, "LeftLegTarget", new Vector3(-0.3f, 0, 0));
            rightLegTarget = CreateTarget(targetsParent.transform, "RightLegTarget", new Vector3(0.3f, 0, 0));
            leftArmTarget = CreateTarget(targetsParent.transform, "LeftArmTarget", new Vector3(-0.5f, 1.2f, 0));
            rightArmTarget = CreateTarget(targetsParent.transform, "RightArmTarget", new Vector3(0.5f, 1.2f, 0));
            bodyTarget = CreateTarget(targetsParent.transform, "BodyTarget", Vector3.zero);

            Debug.Log("[ProceduralAnimator] Auto-setup complete - animation targets created");
        }

        private Transform CreateTarget(Transform parent, string name, Vector3 localPosition)
        {
            GameObject target = new GameObject(name);
            target.transform.SetParent(parent);
            target.transform.localPosition = localPosition;
            target.transform.localRotation = Quaternion.identity;
            return target.transform;
        }
    }
}
