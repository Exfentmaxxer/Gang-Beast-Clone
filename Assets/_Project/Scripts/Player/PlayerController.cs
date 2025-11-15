using UnityEngine;
using TumbleRumble.Core;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Main player controller - coordinates all player subsystems
    /// Handles movement, state management, and component coordination
    /// </summary>
    [RequireComponent(typeof(RagdollController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Player Info")]
        [SerializeField] private int playerId = 0;
        [SerializeField] private string playerName = "Player";
        [SerializeField] private Color playerColor = Color.cyan;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float maxGroundSlope = 45f;

        [Header("References")]
        [SerializeField] private Rigidbody pelvisRigidbody;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        [Header("Components")]
        private RagdollController ragdollController;
        private PlayerInput playerInput;
        private GrabSystem grabSystem;
        private CombatSystem combatSystem;
        private AbilitySystem abilitySystem;
        #endregion

        #region Private Fields
        private bool _isGrounded = false;
        private bool _canMove = true;
        private bool _isKnockedOut = false;
        private Vector3 _moveDirection = Vector3.zero;
        private float _groundCheckDistance = 0.2f;
        #endregion

        #region Properties
        public int PlayerId => playerId;
        public string PlayerName => playerName;
        public Color PlayerColor => playerColor;
        public bool IsGrounded => _isGrounded;
        public bool IsKnockedOut => _isKnockedOut;
        public bool CanMove => _canMove && !_isKnockedOut;
        public Rigidbody PelvisRb => pelvisRigidbody;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            InitializeComponents();
        }

        private void FixedUpdate()
        {
            if (!CanMove) return;

            CheckGroundStatus();
            ApplyMovement();
        }

        private void Update()
        {
            if (!CanMove) return;

            HandleInput();
        }
        #endregion

        #region Initialization
        private void InitializeComponents()
        {
            ragdollController = GetComponent<RagdollController>();
            playerInput = GetComponent<PlayerInput>();
            grabSystem = GetComponent<GrabSystem>();
            combatSystem = GetComponent<CombatSystem>();
            abilitySystem = GetComponent<AbilitySystem>();

            if (pelvisRigidbody == null)
            {
                pelvisRigidbody = GetComponentInChildren<Rigidbody>();
                if (pelvisRigidbody == null)
                {
                    Debug.LogError("[PlayerController] No pelvis rigidbody found!");
                }
            }

            if (groundCheck == null)
            {
                // Create ground check point
                GameObject groundCheckObj = new GameObject("GroundCheck");
                groundCheckObj.transform.SetParent(pelvisRigidbody.transform);
                groundCheckObj.transform.localPosition = Vector3.down * 0.5f;
                groundCheck = groundCheckObj.transform;
            }
        }

        public void Initialize(int id, string name, Color color)
        {
            playerId = id;
            playerName = name;
            playerColor = color;

            Debug.Log($"[PlayerController] Player {playerId} ({playerName}) initialized");
        }
        #endregion

        #region Input Handling
        private void HandleInput()
        {
            if (playerInput == null) return;

            // Get movement input
            Vector2 moveInput = playerInput.GetMovementInput();
            _moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

            // Jump
            if (playerInput.GetJumpDown() && _isGrounded)
            {
                Jump();
            }

            // Grab
            if (playerInput.GetGrabDown() && grabSystem != null)
            {
                grabSystem.TryGrab();
            }
            else if (playerInput.GetGrabUp() && grabSystem != null)
            {
                grabSystem.ReleaseGrab();
            }

            // Attack
            if (playerInput.GetPunchDown() && combatSystem != null)
            {
                combatSystem.Punch();
            }

            // Ability
            if (playerInput.GetAbilityDown() && abilitySystem != null)
            {
                abilitySystem.UseAbility();
            }
        }
        #endregion

        #region Movement
        private void ApplyMovement()
        {
            if (pelvisRigidbody == null) return;

            // Calculate desired velocity
            Vector3 desiredVelocity = _moveDirection.normalized * moveSpeed;

            // Only modify horizontal velocity, preserve vertical
            Vector3 currentVelocity = pelvisRigidbody.velocity;
            Vector3 velocityDelta = desiredVelocity - new Vector3(currentVelocity.x, 0f, currentVelocity.z);

            // Apply force to reach desired velocity
            float forceMultiplier = 20f;
            Vector3 force = velocityDelta * forceMultiplier;

            pelvisRigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private void Jump()
        {
            if (pelvisRigidbody == null) return;

            pelvisRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log($"[PlayerController] Player {playerId} jumped");
        }

        private void CheckGroundStatus()
        {
            if (groundCheck == null) return;

            _isGrounded = Physics.CheckSphere(
                groundCheck.position,
                _groundCheckDistance,
                groundLayer
            );
        }
        #endregion

        #region State Management
        public void Knockout(float duration = 3f)
        {
            if (_isKnockedOut) return;

            _isKnockedOut = true;
            _canMove = false;

            if (ragdollController != null)
            {
                ragdollController.EnableRagdoll();
            }

            Debug.Log($"[PlayerController] Player {playerId} knocked out for {duration}s");

            Invoke(nameof(Recover), duration);
        }

        public void Recover()
        {
            _isKnockedOut = false;
            _canMove = true;

            if (ragdollController != null)
            {
                ragdollController.DisableRagdoll();
            }

            Debug.Log($"[PlayerController] Player {playerId} recovered");
        }

        public void Eliminate()
        {
            Debug.Log($"[PlayerController] Player {playerId} eliminated!");

            // Notify round manager
            var roundManager = FindObjectOfType<RoundManager>();
            if (roundManager != null)
            {
                roundManager.EliminatePlayer(playerId);
            }

            // Disable player
            _canMove = false;
            gameObject.SetActive(false);
        }
        #endregion

        #region Physics Events
        private void OnCollisionEnter(Collision collision)
        {
            // High-velocity impact detection
            float impactForce = collision.relativeVelocity.magnitude;
            if (impactForce > 10f)
            {
                // Could trigger knockout or damage
                Debug.Log($"[PlayerController] Player {playerId} high impact: {impactForce}");
            }
        }
        #endregion

        #region Debug
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = _isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, _groundCheckDistance);
            }
        }
        #endregion
    }
}
