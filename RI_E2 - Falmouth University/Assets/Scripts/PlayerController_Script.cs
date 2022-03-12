using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController_Script : MonoBehaviour
{
    // Camera
    public Camera cam;
    private float turnSpeed = 0.1f, camTurnSpeed = 15f;
    private Transform cameraTransform;
    public Camera fpsCam;

    // Player properties
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 direction;
    private Rigidbody myRb;
    [SerializeField]
    private float playerSpeed = 12.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float moveSpeed = 10f;

    // Jumping
    [SerializeField] private Transform groundCheck;
    [SerializeField] [Range(1, 15)] private float jumpVelocity;
    [SerializeField] private float groundDistance = 0.4f, fallMultiplier = 2.5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool grounded;

    #region Input variables

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction dropAction;

    #endregion

    // Constants
    private const float zero = 0f;

    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 144;
    }

    private void OnEnable()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        interactAction = playerInput.actions["Interact"];
        dropAction = playerInput.actions["Drop"];
        moveAction.performed += ctx => MoveInput(ctx.ReadValue<Vector2>());
        moveAction.canceled += ctx => MoveInput(ctx.ReadValue<Vector2>());
        jumpAction.performed += _ => JumpInput();
        interactAction.performed += _ => InteractInput();
        dropAction.performed += _ => DropInput();
    }

    private void Start()
    {
        myRb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerGroundCheck();
    }

    private void FixedUpdate()
    {
        RigidMovement();
    }

    #region Inputs & Camera

    void MoveInput(Vector2 input)
    {
        direction = new Vector3(input.x, zero, input.y).normalized;
    }

    void JumpInput()
    {
        if (grounded)
        {
            myRb.velocity = Vector3.up * jumpVelocity;
        }
    }

    void InteractInput()
    {

    }

    void DropInput()
    {

    }

    #endregion

    void RigidMovement()
    {
        float yawCam = cam.transform.rotation.eulerAngles.y; // temp float used for turning the camera left/right
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(zero, yawCam, zero), camTurnSpeed * Time.fixedDeltaTime); // turns the character left/right in sync with camera
        if (direction.magnitude >= 0.1f) // if we are getting a movement
        {
            myRb.MovePosition(transform.position + (transform.right * direction.x + transform.forward * direction.z) * Time.fixedDeltaTime * moveSpeed);
        }
    }

    public void PlayerGroundCheck()
    {
        // Check if player is grounded (stops infinite jumps)
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
}