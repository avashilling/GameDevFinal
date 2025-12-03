using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    [Header("MiniGame Objects")]
    public RectTransform playerRect;
    public RectTransform startPointRect;
    public RectTransform[] wallImages;
    public RectTransform goalRect;

    [Header("Panel Bounds")]
    public RectTransform miniGamePanel;

    [Header("UI Buttons")]
    public GameObject exitButton;   // <-- NEW EXIT BUTTON

    [HideInInspector] public Vector2 MinBounds;
    [HideInInspector] public Vector2 MaxBounds;

    private void Awake()
    {
        // Hide all game elements at the start
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);

        if (wallImages != null)
        {
            foreach (var wall in wallImages)
                wall.gameObject.SetActive(false);
        }

        // Hide exit button at the start
        if (exitButton != null) exitButton.SetActive(false);
    }

    private void Start()
    {
        UpdatePanelBounds();
    }
    // Update the bounds of the mini-game panel in anchored space
    public void UpdatePanelBounds()
    {
        if (miniGamePanel != null)
        {
            Vector3[] corners = new Vector3[4];
            miniGamePanel.GetWorldCorners(corners);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                miniGamePanel, corners[0], null, out MinBounds);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                miniGamePanel, corners[2], null, out MaxBounds);
        }
    }

    public void BeginGame()
    {
        UpdatePanelBounds();

        if (playerRect != null) playerRect.gameObject.SetActive(true);
        if (goalRect != null) goalRect.gameObject.SetActive(true);
        foreach (var wall in wallImages) wall.gameObject.SetActive(true);

        // Reset player to start
        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;

        // Hide exit button when starting the game
        if (exitButton != null) exitButton.SetActive(false);
    }
    // Immediately end the mini-game, hiding all elements
    public void ForceEndGame()
    {
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);

        foreach (var wall in wallImages)
            wall.gameObject.SetActive(false);

        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;
    }

    public void Fail()
    {
        Debug.Log("Player hit a wall! Resetting position.");

        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;
    }

    public void Success()
    {
        Debug.Log("Player reached the goal! Game complete.");

        ForceEndGame();

        // SHOW EXIT BUTTON ONLY AFTER SUCCESS
        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public bool CheckWallCollision()
    {
        if (playerRect == null || wallImages == null) return false;

        foreach (var wall in wallImages)
        {
            if (RectOverlaps(playerRect, wall))
                return true;
        }

        return false;
    }
    // Check if two RectTransforms overlap in the mini-game panel's local space
    public bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Rect rectA = GetLocalRect(a);
        Rect rectB = GetLocalRect(b);

        return rectA.Overlaps(rectB);
    }

    private Rect GetLocalRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector2 localBL;
        Vector2 localTR;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            miniGamePanel, corners[0], null, out localBL);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            miniGamePanel, corners[2], null, out localTR);

        return new Rect(localBL, localTR - localBL);
    }
}
