using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement and layout switching when exiting a doorway.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.1f;
    public float acceleration = 10f;
    public float sprintAmount = 1.8f;

    [Header("References")]
    public Transform cameraHolder;

    private CharacterController controller;
    private InputSystem controls;
    private Vector2 moveInput;
    private Vector2 currentInput;
    private Vector2 lookInput;
    private bool isSprinting;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new InputSystem();

        // Input bindings
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Player.Sprint.performed += ctx => isSprinting = true;
        controls.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        Move();
        // Look(); // Mouse look control (currently disabled)
    }

    /// <summary>
    /// Handles character movement with optional sprinting and smoothing.
    /// </summary>
    void Move()
    {
        float targetSpeed = isSprinting ? moveSpeed * sprintAmount : moveSpeed;
        currentInput = Vector2.Lerp(currentInput, moveInput, acceleration * Time.deltaTime);
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(targetSpeed * Time.deltaTime * direction);
    }

    /// <summary>
    /// Handles first-person camera rotation.
    /// </summary>
    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    /// <summary>
    /// When player leaves a room trigger, switch to the next layout.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DoorTrigger"))
        {
            LayoutManager layoutManager = FindFirstObjectByType<LayoutManager>();
            if (layoutManager != null)
            {
                layoutManager.NextLayout(); // Switch to next layout
            }

            // other.enabled = false; // Disable trigger to prevent re-triggering
        }
    }
}
