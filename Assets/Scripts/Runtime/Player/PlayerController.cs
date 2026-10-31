using System;
using UnityEngine;
using UnityEngine.InputSystem;
using LastShift.Matchstick;

namespace LastShift.Player
{
    /// <summary>
    /// First-person player controller implementing PLR-01 through PLR-06.
    /// Manages walking, crouching in vents, conditional sprint, camera look, and raycast interaction.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings (PLR-01, PLR-02)")]
        [SerializeField] private float _walkSpeed = 2.8f;
        [SerializeField] private float _crouchSpeed = 1.4f;
        [SerializeField] private float _sprintSpeed = 4.6f;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Crouch Heights")]
        [SerializeField] private float _standingHeight = 1.8f;
        [SerializeField] private float _crouchingHeight = 0.9f;
        [SerializeField] private float _crouchTransitionSpeed = 10f;

        [Header("Look & Camera Settings (PLR-05)")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _mouseSensitivity = 2.0f;
        [SerializeField] private float _minPitch = -85f;
        [SerializeField] private float _maxPitch = 85f;
        [SerializeField] private float _headBobSpeed = 8f;
        [SerializeField] private float _headBobAmount = 0.04f;

        [Header("Interaction (PLR-03)")]
        [SerializeField] private float _interactionDistance = 2.5f;
        [SerializeField] private LayerMask _interactionMask = ~0;

        [Header("References")]
        [SerializeField] private MatchstickController _matchstickController;

        private CharacterController _characterController;
        private float _cameraPitch = 0f;
        private Vector3 _velocity;
        private bool _isInputLocked = false;
        private bool _isSprintAllowed = false;
        private bool _isCrouching = false;
        private float _cameraOriginalY;
        private float _bobTimer = 0f;
        private IInteractable _currentInteractable;

        public event Action<string> OnInteractionPromptChanged;
        public event Action<bool> OnCrouchStateChanged;

        public Transform CameraTransform => _playerCamera != null ? _playerCamera.transform : transform;

        public bool IsInputLocked => _isInputLocked;
        public bool IsCrouching => _isCrouching;
        public Camera PlayerCamera => _playerCamera;
        public MatchstickController Matchstick => _matchstickController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (_playerCamera == null)
            {
                _playerCamera = GetComponentInChildren<Camera>();
            }

            if (_playerCamera != null)
            {
                _cameraOriginalY = _playerCamera.transform.localPosition.y;
            }

            if (_matchstickController == null)
            {
                _matchstickController = GetComponentInChildren<MatchstickController>();
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_isInputLocked || Time.timeScale <= 0f)
            {
                return;
            }

            HandleLook();
            HandleMovement();
            HandleCrouch();
            HandleInteraction();
            HandleMatchstickInput();
        }

        public void SetInputLocked(bool locked)
        {
            _isInputLocked = locked;
            if (locked)
            {
                _velocity = Vector3.zero;
                _currentInteractable = null;
                OnInteractionPromptChanged?.Invoke(string.Empty);
            }
        }

        public void SetSprintAllowed(bool allowed)
        {
            _isSprintAllowed = allowed;
        }

        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            _characterController.enabled = false;
            transform.position = position;
            transform.rotation = rotation;
            _cameraPitch = rotation.eulerAngles.x;
            _characterController.enabled = true;
        }

        private void HandleLook()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 mouseDelta = mouse.delta.ReadValue();
            float mouseX = mouseDelta.x * _mouseSensitivity * 0.1f;
            float mouseY = mouseDelta.y * _mouseSensitivity * 0.1f;

            _cameraPitch -= mouseY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, _minPitch, _maxPitch);

            if (_playerCamera != null)
            {
                _playerCamera.transform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
            }

            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            bool isGrounded = _characterController.isGrounded;
            if (isGrounded && _velocity.y < 0f)
            {
                _velocity.y = -2f;
            }

            var keyboard = Keyboard.current;
            float moveX = 0f;
            float moveZ = 0f;
            bool wantsSprint = false;

            if (keyboard != null)
            {
                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) moveX += 1f;
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) moveX -= 1f;
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) moveZ += 1f;
                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) moveZ -= 1f;

                wantsSprint = _isSprintAllowed && (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
            }

            Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

            float currentSpeed = _walkSpeed;
            if (_isCrouching)
            {
                currentSpeed = _crouchSpeed;
            }
            else if (wantsSprint)
            {
                currentSpeed = _sprintSpeed;
            }

            _characterController.Move(moveDirection * (currentSpeed * Time.deltaTime));

            // Apply gravity
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            // Subtle headbob
            if (moveDirection.sqrMagnitude > 0.01f && isGrounded && _playerCamera != null)
            {
                _bobTimer += Time.deltaTime * _headBobSpeed * (currentSpeed / _walkSpeed);
                float newY = _cameraOriginalY + Mathf.Sin(_bobTimer) * _headBobAmount;
                var camPos = _playerCamera.transform.localPosition;
                camPos.y = newY;
                _playerCamera.transform.localPosition = camPos;
            }
            else if (_playerCamera != null)
            {
                _bobTimer = 0f;
                var camPos = _playerCamera.transform.localPosition;
                camPos.y = Mathf.Lerp(camPos.y, _cameraOriginalY, Time.deltaTime * 5f);
                _playerCamera.transform.localPosition = camPos;
            }
        }

        private void HandleCrouch()
        {
            var keyboard = Keyboard.current;
            bool wantsCrouch = keyboard != null && (keyboard.cKey.isPressed || keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed);

            if (wantsCrouch != _isCrouching)
            {
                _isCrouching = wantsCrouch;
                OnCrouchStateChanged?.Invoke(_isCrouching);
            }

            float targetHeight = _isCrouching ? _crouchingHeight : _standingHeight;
            _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, Time.deltaTime * _crouchTransitionSpeed);
            _characterController.center = new Vector3(0f, _characterController.height / 2f, 0f);

            if (_playerCamera != null)
            {
                float targetCamY = _isCrouching ? (_crouchingHeight - 0.15f) : (_standingHeight - 0.2f);
                _cameraOriginalY = targetCamY;
            }
        }

        private void HandleInteraction()
        {
            if (_playerCamera == null)
            {
                return;
            }

            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _interactionMask, QueryTriggerInteraction.Ignore))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    if (interactable != _currentInteractable)
                    {
                        _currentInteractable = interactable;
                        OnInteractionPromptChanged?.Invoke(interactable.InteractionPrompt);
                    }

                    var keyboard = Keyboard.current;
                    if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
                    {
                        interactable.Interact(this);
                    }
                    return;
                }
            }

            if (_currentInteractable != null)
            {
                _currentInteractable = null;
                OnInteractionPromptChanged?.Invoke(string.Empty);
            }
        }

        private void HandleMatchstickInput()
        {
            if (_matchstickController == null)
            {
                return;
            }

            var mouse = Mouse.current;
            var keyboard = Keyboard.current;

            bool strikePressed = (mouse != null && mouse.leftButton.wasPressedThisFrame) ||
                                 (keyboard != null && keyboard.fKey.wasPressedThisFrame);

            if (strikePressed)
            {
                _matchstickController.TryIgnite();
            }
        }
    }
}
