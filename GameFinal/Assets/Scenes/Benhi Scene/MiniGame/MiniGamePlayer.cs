using UnityEngine;

public class MiniGamePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 500f;
    public RectTransform playerRect;
    public MiniGameController miniGameController;

    private Vector2 moveInput;

    private void Update()
    {
        if (playerRect == null || miniGameController == null) return;

        // Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Move player in ANCHORED UI space
        playerRect.anchoredPosition += moveInput * moveSpeed * Time.deltaTime;

        // Clamp inside UI panel (anchored space)
        Vector2 pos = playerRect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, miniGameController.MinBounds.x, miniGameController.MaxBounds.x);
        pos.y = Mathf.Clamp(pos.y, miniGameController.MinBounds.y, miniGameController.MaxBounds.y);
        playerRect.anchoredPosition = pos;

        // Wall collision
        if (miniGameController.CheckWallCollision())
        {
            miniGameController.Fail();
        }

        // Goal detection
        if (miniGameController.goalRect != null &&
            miniGameController.RectOverlaps(playerRect, miniGameController.goalRect))
        {
            miniGameController.Success();
        }
    }
}
