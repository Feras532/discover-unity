using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10f;
    public float boostedSpeed = 20f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 1000f;
    public Transform playerBody;
    public Animator anim;
    public Text speedText;

    public bool isKilled = false;

    private float originalSpeed;
    private bool isBoosted = false;
    private bool isJumping = false;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private float currentYaw = 0f;
    private bool hasPlayedDeathAnim = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerBody = transform.Find("Capsule");
        originalSpeed = speed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isKilled)
        {
            if (!hasPlayedDeathAnim)
            {
                hasPlayedDeathAnim = true;
                anim.SetBool("Killed", true);
                StartCoroutine(FreezeAfterDeath());
            }
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

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        Vector3 horizontal = playerBody.TransformDirection(inputDirection) * speed;
        Vector3 vertical = new Vector3(0, velocity.y, 0);

        controller.Move((horizontal + vertical) * Time.deltaTime);

        anim.SetBool("isWalking", inputDirection.magnitude > 0f);

        // Show actual speed
        if (speedText != null)
            Debug.Log("Speed: " + speed);
            speedText.text = "Speed: " + speed.ToString("F1");
    }

    void LateUpdate()
    {
        playerBody.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }

private int activeBoosts = 0;

public void ActivateSpeedBoost(float duration, float multiplier)
{
    activeBoosts++;
    speed = originalSpeed * (1 + 0.25f * activeBoosts); // Stack 25% per boost
    StartCoroutine(RemoveBoostAfterTime(duration));
}

private IEnumerator RemoveBoostAfterTime(float duration)
{
    yield return new WaitForSeconds(duration);
    activeBoosts = Mathf.Max(0, activeBoosts - 1);
    speed = originalSpeed * (1 + 0.25f * activeBoosts);
}


    private IEnumerator SpeedBoost(float duration, float multiplier)
    {
        isBoosted = true;
        speed = originalSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isBoosted = false;
    }

    private IEnumerator FreezeAfterDeath()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
