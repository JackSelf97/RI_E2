using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private float gravityValue = -9.81f;
    [SerializeField]
    private float moveSpeed = 10f;
    private float throwForce = 1500f;
    public bool isActive;
    

    // Jumping
    [SerializeField] private Transform groundCheck;
    [SerializeField] [Range(1, 15)] private float jumpVelocity;
    [SerializeField] private float groundDistance = 0.4f, fallMultiplier = 2.5f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool isGrounded;

    // Object Selection
    [SerializeField] private GameObject itemPickedUp;
    public GameObject destination;
    private float rayDistance = 10f;
    public Text capTxt;

    // Constants
    private const float zero = 0f;

    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 144;
    }

    private void Start()
    {
        myRb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        isActive = false;
    }

    void Update()
    {
        if (isActive)
        {
            PlayerGroundCheck();
            PlayerRayCast();
            MoveInput();
            JumpInput();

            if (itemPickedUp != null)
            {
                Throw();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
            RigidMovement();
    }

    #region Inputs & Camera

    void MoveInput()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");

        direction = new Vector3(moveX, zero, moveY).normalized;
    }

    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myRb.velocity = Vector3.up * jumpVelocity;
        }
    }

    void PickUp(GameObject item)
    {
        // Pick up objects
        if (itemPickedUp == null)
        {
            Debug.Log("Item picked up " + item.name);
            itemPickedUp = item;

            if (itemPickedUp.layer == 9)
            {
                itemPickedUp.GetComponent<Container_Script>().onHand = true;
                capTxt.enabled = true;
            }
            if (itemPickedUp.layer == 10)
            {
                itemPickedUp.GetComponent<Trash_Script>().pickedUp = true;
                itemPickedUp.GetComponent<Trash_Script>().lifeTime = 30;
            }


            itemPickedUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            itemPickedUp.GetComponent<Rigidbody>().useGravity = false;
            itemPickedUp.transform.position = destination.transform.position;
            itemPickedUp.transform.parent = destination.transform;
            itemPickedUp.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }
    }

    void Throw()
    {
        if (Input.GetMouseButton(1))
        {
            if (itemPickedUp.layer == 9)
            {
                capTxt.enabled = false;
                itemPickedUp.GetComponent<Container_Script>().thrown = true;
                itemPickedUp.GetComponent<Container_Script>().onHand = false;
            }
            if (itemPickedUp.layer == 10)
            {
                itemPickedUp.GetComponent<Trash_Script>().pickedUp = false;
            }

            itemPickedUp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            itemPickedUp.GetComponent<Rigidbody>().useGravity = true;
            itemPickedUp.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * throwForce);
            itemPickedUp.transform.parent = null;
            itemPickedUp = null;
        }
    }

    #endregion

    void PlayerRayCast()
    {
        #region Raycast Properties

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        #endregion

        // ----------------------- Start of Raycasting -------------------------------

        if (Physics.Raycast(ray, out hit, rayDistance)) // Number identifies length of raycast
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red); // can be removed
            var selection = hit.collider;
            string selectableTag = "Interactable";

            // Basic Interactable Object
            if (selection.CompareTag(selectableTag))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject item = hit.collider.gameObject;
                    PickUp(item);
                }
            }
        }
    }

    void RigidMovement()
    {
        float yawCam = cam.transform.rotation.eulerAngles.y; // temp float used for turning the camera left/right
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(zero, yawCam, zero), camTurnSpeed * Time.fixedDeltaTime); // turns the character left/right in sync with camera
        if (direction.magnitude >= 0.1f) // if we are getting a movement
        {
            myRb.MovePosition(transform.position + (transform.right * direction.x + transform.forward * direction.z).normalized * Time.fixedDeltaTime * moveSpeed);
        }
    }

    public void PlayerGroundCheck()
    {
        // Check if player is grounded (stops infinite jumps)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

}