using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float dashCooldown = 0.5f;
    private float dashResetTimer = 0f;
    public int remainingDashes;
    private Vector2 dashDirection;
    private Vector2 dashVelocity;
    public float dashDuration = 0.1f;
    private float dashTimeRemaining = 0f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public PlayerStats playerStats;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerStats = PlayerStats.Instance;
        remainingDashes = playerStats.CalculateDashNumber;
    }
    void Dash() {
        dashDirection = movement.normalized;
        float dashDistance = playerStats.CalculateDashDistance;
        Debug.LogWarning(dashDistance);
        if (dashDirection != Vector2.zero) {
            dashVelocity = dashDirection * dashDistance / dashDuration;
            dashTimeRemaining = dashDuration;
            remainingDashes--;
        }
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && remainingDashes > 0) {
            Debug.LogWarning("Called Dash");
            Dash();
        }
        if (remainingDashes <= 0) {
            dashResetTimer += Time.deltaTime;
            if (dashResetTimer >= dashCooldown) {
                remainingDashes = playerStats.CalculateDashNumber;
                dashResetTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (dashTimeRemaining > 0f) {
            rb.MovePosition(rb.position + dashVelocity * Time.fixedDeltaTime);
            dashTimeRemaining -= Time.fixedDeltaTime;
            Debug.LogWarning(dashTimeRemaining);
            playerStats.isDashing = true;
        } else {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            playerStats.isDashing = false;
        }
    }
}
