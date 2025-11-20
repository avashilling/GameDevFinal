//using UnityEngine;

//public class MiniGamePlayer : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float moveSpeed = 5f;

//    private Rigidbody2D rb;
//    private Vector2 moveInput;
//    private MiniGameController miniGameController;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        rb.bodyType = RigidbodyType2D.Dynamic;
//        rb.gravityScale = 0;
//        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

//        miniGameController = Object.FindFirstObjectByType<MiniGameController>();
//        if (miniGameController == null)
//            Debug.LogWarning("MiniGameController not found!");
//    }

//    private void Update()
//    {
//        // Player input
//        moveInput.x = Input.GetAxisRaw("Horizontal");
//        moveInput.y = Input.GetAxisRaw("Vertical");
//        moveInput.Normalize();
//    }

//    private void FixedUpdate()
//    {
//        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

//        // Clamp inside panel bounds
//        if (miniGameController != null && miniGameController.miniGamePanel != null)
//        {
//            newPos.x = Mathf.Clamp(newPos.x, miniGameController.MinBounds.x, miniGameController.MaxBounds.x);
//            newPos.y = Mathf.Clamp(newPos.y, miniGameController.MinBounds.y, miniGameController.MaxBounds.y);
//        }

//        rb.MovePosition(newPos);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.collider.CompareTag("Wall"))
//        {
//            Debug.Log("Hit Wall! Player is dead.");
//            if (miniGameController != null)
//            {
//                miniGameController.Fail();
//                rb.linearVelocity = Vector2.zero; // stop immediately
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Goal"))
//        {
//            Debug.Log("Reached Goal!");
//            // Optionally add miniGameController.Success() logic here
//        }
//    }
//}
using UnityEngine;

public class MiniGamePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 500f;
    public RectTransform playerRect;
    public MiniGameController miniGameController;

    private Vector2 moveInput;
    private bool gameActive = true;  // Prevent multiple success/fail calls

    private void Update()
    {
        if (playerRect == null || miniGameController == null || !gameActive) return;

        // Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Move player using ONLY anchoredPosition (local UI space)
        Vector2 newPos = playerRect.anchoredPosition + moveInput * moveSpeed * Time.deltaTime;

        // Clamp inside panel (assuming bounds are in local UI space)
        newPos.x = Mathf.Clamp(newPos.x, miniGameController.MinBounds.x, miniGameController.MaxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, miniGameController.MinBounds.y, miniGameController.MaxBounds.y);

        playerRect.anchoredPosition = newPos;

        // Wall collision
        if (miniGameController.CheckWallCollision())
        {
            gameActive = false;
            miniGameController.Fail();
            return;
        }

        // Goal detection
        if (miniGameController.goalRect != null &&
            miniGameController.RectOverlaps(playerRect, miniGameController.goalRect))
        {
            gameActive = false;
            miniGameController.Success();
        }
    }

    // Call this if you need to restart the minigame
    public void ResetGame()
    {
        gameActive = true;
    }
}