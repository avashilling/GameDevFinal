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
    public GameObject exitButton;

    [Header("Audio")]
    public AudioSource arcadeMusic;  // looping music while minigame is running
    public AudioSource winSound;     // plays on success
    public AudioSource deathSound;   // plays on wall collision

    [HideInInspector] public Vector2 MinBounds;
    [HideInInspector] public Vector2 MaxBounds;

    private void Awake()
    {
        // Hide all game elements
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);

        if (wallImages != null)
        {
            foreach (var wall in wallImages)
                wall.gameObject.SetActive(false);
        }

        if (exitButton != null)
            exitButton.SetActive(false);

        // Ensure arcade music isn't playing at start
        if (arcadeMusic != null)
            arcadeMusic.Stop();
    }

    private void Start()
    {
        UpdatePanelBounds();
    }

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

    // -------------------------------------------------------
    // GAME START
    // -------------------------------------------------------
    public void BeginGame()
    {
        UpdatePanelBounds();

        if (playerRect != null) playerRect.gameObject.SetActive(true);
        if (goalRect != null) goalRect.gameObject.SetActive(true);
        foreach (var wall in wallImages) wall.gameObject.SetActive(true);

        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;

        if (exitButton != null)
            exitButton.SetActive(false);

        // Start arcade music
        if (arcadeMusic != null)
        {
            arcadeMusic.loop = true;
            arcadeMusic.Play();
        }
    }

    // -------------------------------------------------------
    // GAME END
    // -------------------------------------------------------
    public void ForceEndGame()
    {
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);

        foreach (var wall in wallImages)
            wall.gameObject.SetActive(false);

        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;

        // Stop arcade music when forcibly ended
        if (arcadeMusic != null)
            arcadeMusic.Stop();
    }

    // -------------------------------------------------------
    // FAIL: Hit wall
    // -------------------------------------------------------
    public void Fail()
    {
        Debug.Log("Player hit a wall! Resetting position.");

        // Play death sound
        if (deathSound != null)
            deathSound.Play();

        // Reset back to the start
        if (playerRect != null && startPointRect != null)
            playerRect.anchoredPosition = startPointRect.anchoredPosition;
    }

    // -------------------------------------------------------
    // SUCCESS
    // -------------------------------------------------------
    public void Success()
    {
        Debug.Log("Player reached the goal! Game complete.");

        // Stop arcade music
        if (arcadeMusic != null)
            arcadeMusic.Stop();

        // Play win sound
        if (winSound != null)
            winSound.Play();

        ForceEndGame();
        GameManager.Instance.winArcadeGame();
    }

    // -------------------------------------------------------
    // WALL COLLISION CHECK
    // -------------------------------------------------------
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
