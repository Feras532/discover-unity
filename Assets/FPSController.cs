using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 1000f;
    public Transform playerBody;
    public Animator anim;

    private bool isJumping = false;

    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private float currentYaw = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerBody = transform.Find("Capsule");

        if (anim == null)
        {
            Debug.LogWarning("Animator reference not set in inspector.");
        }
        else
        {
            Debug.Log("Animator assigned to: " + anim.gameObject.name);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (anim == null)
        {
            Debug.LogWarning("Animator is NULL! Aborting Update.");
            return;
        }

        isGrounded = controller.isGrounded;


        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentYaw += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Jumping
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            if (isJumping)
            {
                isJumping = false;
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsGrounded", true);
            }
        }

        if (!isJumping && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            anim.SetBool("IsJumping", true);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        Vector3 horizontal = playerBody.TransformDirection(inputDirection) * speed;
        Vector3 vertical = new Vector3(0, velocity.y, 0);

        controller.Move((horizontal + vertical) * Time.deltaTime);

        // Debug speed value and animation
        float currentSpeed = inputDirection.magnitude;
        if(currentSpeed == 0.0)
        {
            anim.SetBool("isWalking", false);
        }

        else {
            anim.SetBool("isWalking", true);
            }
    }

    void LateUpdate()
    {
        playerBody.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}
