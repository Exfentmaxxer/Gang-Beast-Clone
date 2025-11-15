using UnityEngine;
using UnityEngine.InputSystem;

namespace TumbleRumble.Player
{
    /// <summary>
    /// Handles player input using Unity's new Input System
    /// Supports keyboard, gamepad, and multiple local players
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInput : MonoBehaviour
    {
        #region Input Actions
        private PlayerInput _playerInputComponent;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _grabAction;
        private InputAction _punchAction;
        private InputAction _abilityAction;
        private InputAction _crouchAction;
        #endregion

        #region Input Values
        private Vector2 _movementInput = Vector2.zero;
        private bool _jumpPressed = false;
        private bool _jumpDown = false;
        private bool _grabPressed = false;
        private bool _grabDown = false;
        private bool _grabUp = false;
        private bool _punchDown = false;
        private bool _abilityDown = false;
        private bool _crouchPressed = false;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _playerInputComponent = GetComponent<PlayerInput>();
            SetupInputActions();
        }

        private void OnEnable()
        {
            EnableInputActions();
        }

        private void OnDisable()
        {
            DisableInputActions();
        }

        private void Update()
        {
            // Reset one-frame inputs
            _jumpDown = false;
            _grabDown = false;
            _grabUp = false;
            _punchDown = false;
            _abilityDown = false;
        }
        #endregion

        #region Setup
        private void SetupInputActions()
        {
            var actionMap = _playerInputComponent.currentActionMap;

            _moveAction = actionMap.FindAction("Move");
            _jumpAction = actionMap.FindAction("Jump");
            _grabAction = actionMap.FindAction("Grab");
            _punchAction = actionMap.FindAction("Punch");
            _abilityAction = actionMap.FindAction("Ability");
            _crouchAction = actionMap.FindAction("Crouch");

            // Subscribe to events
            if (_moveAction != null)
            {
                _moveAction.performed += OnMove;
                _moveAction.canceled += OnMove;
            }

            if (_jumpAction != null)
            {
                _jumpAction.performed += OnJump;
                _jumpAction.canceled += OnJumpCanceled;
            }

            if (_grabAction != null)
            {
                _grabAction.performed += OnGrab;
                _grabAction.canceled += OnGrabCanceled;
            }

            if (_punchAction != null)
            {
                _punchAction.performed += OnPunch;
            }

            if (_abilityAction != null)
            {
                _abilityAction.performed += OnAbility;
            }

            if (_crouchAction != null)
            {
                _crouchAction.performed += OnCrouch;
                _crouchAction.canceled += OnCrouchCanceled;
            }
        }

        private void EnableInputActions()
        {
            _moveAction?.Enable();
            _jumpAction?.Enable();
            _grabAction?.Enable();
            _punchAction?.Enable();
            _abilityAction?.Enable();
            _crouchAction?.Enable();
        }

        private void DisableInputActions()
        {
            _moveAction?.Disable();
            _jumpAction?.Disable();
            _grabAction?.Disable();
            _punchAction?.Disable();
            _abilityAction?.Disable();
            _crouchAction?.Disable();
        }
        #endregion

        #region Input Callbacks
        private void OnMove(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            _jumpPressed = true;
            _jumpDown = true;
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            _jumpPressed = false;
        }

        private void OnGrab(InputAction.CallbackContext context)
        {
            if (!_grabPressed)
            {
                _grabDown = true;
            }
            _grabPressed = true;
        }

        private void OnGrabCanceled(InputAction.CallbackContext context)
        {
            _grabUp = true;
            _grabPressed = false;
        }

        private void OnPunch(InputAction.CallbackContext context)
        {
            _punchDown = true;
        }

        private void OnAbility(InputAction.CallbackContext context)
        {
            _abilityDown = true;
        }

        private void OnCrouch(InputAction.CallbackContext context)
        {
            _crouchPressed = true;
        }

        private void OnCrouchCanceled(InputAction.CallbackContext context)
        {
            _crouchPressed = false;
        }
        #endregion

        #region Public Getters
        public Vector2 GetMovementInput()
        {
            return _movementInput;
        }

        public bool GetJump()
        {
            return _jumpPressed;
        }

        public bool GetJumpDown()
        {
            return _jumpDown;
        }

        public bool GetGrab()
        {
            return _grabPressed;
        }

        public bool GetGrabDown()
        {
            return _grabDown;
        }

        public bool GetGrabUp()
        {
            return _grabUp;
        }

        public bool GetPunchDown()
        {
            return _punchDown;
        }

        public bool GetAbilityDown()
        {
            return _abilityDown;
        }

        public bool GetCrouch()
        {
            return _crouchPressed;
        }
        #endregion
    }
}
