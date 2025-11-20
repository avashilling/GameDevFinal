//using UnityEngine;

//public class MiniGameController : MonoBehaviour
//{
//    [Header("MiniGame Objects")]
//    public GameObject playerCube;
//    public Transform startPoint;
//    public GameObject wallsParent;
//    public GameObject goalObject;

//    [Header("Panel Bounds")]
//    public RectTransform miniGamePanel; // assign the Screen Panel here

//    // Calculated world bounds
//    [HideInInspector] public Vector2 MinBounds;
//    [HideInInspector] public Vector2 MaxBounds;

//    private void Awake()
//    {
//        // Initially deactivate mini-game objects
//        if (playerCube != null) playerCube.SetActive(false);
//        if (wallsParent != null) wallsParent.SetActive(false);
//        if (goalObject != null) goalObject.SetActive(false);
//    }

//    private void Start()
//    {
//        UpdatePanelBounds();
//    }

//    public void UpdatePanelBounds()
//    {
//        if (miniGamePanel != null)
//        {
//            Vector3[] corners = new Vector3[4];
//            miniGamePanel.GetWorldCorners(corners);

//            MinBounds = corners[0]; // bottom-left
//            MaxBounds = corners[2]; // top-right
//        }
//    }

//    public void BeginGame()
//    {
//        UpdatePanelBounds();

//        if (playerCube != null) playerCube.SetActive(true);
//        if (wallsParent != null) wallsParent.SetActive(true);
//        if (goalObject != null) goalObject.SetActive(true);

//        if (playerCube != null && startPoint != null)
//            playerCube.transform.position = startPoint.position;
//    }

//    public void ForceEndGame()
//    {
//        if (playerCube != null) playerCube.SetActive(false);
//        if (wallsParent != null) wallsParent.SetActive(false);
//        if (goalObject != null) goalObject.SetActive(false);

//        if (playerCube != null && startPoint != null)
//            playerCube.transform.position = startPoint.position;
//    }

//    public void Fail()
//    {
//        Debug.Log("Player hit a wall! Resetting position.");
//        if (playerCube != null && startPoint != null)
//            playerCube.transform.position = startPoint.position;
//    }
//}
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    [Header("MiniGame Objects")]
    public RectTransform playerRect;       // Player UI element
    public RectTransform startPointRect;   // Start position
    public RectTransform[] wallImages;     // Wall UI Images
    public RectTransform goalRect;         // Goal UI Image

    [Header("Panel Bounds")]
    public RectTransform miniGamePanel;    // Screen panel containing mini-game

    // Calculated world bounds
    [HideInInspector] public Vector2 MinBounds;
    [HideInInspector] public Vector2 MaxBounds;

    private void Awake()
    {
        // Hide all mini-game objects initially
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);
        if (wallImages != null)
        {
            foreach (var wall in wallImages)
                wall.gameObject.SetActive(false);
        }
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
            MinBounds = corners[0]; // bottom-left
            MaxBounds = corners[2]; // top-right
        }
    }

    public void BeginGame()
    {
        UpdatePanelBounds();

        // Show mini-game objects
        if (playerRect != null) playerRect.gameObject.SetActive(true);
        if (goalRect != null) goalRect.gameObject.SetActive(true);
        if (wallImages != null)
        {
            foreach (var wall in wallImages)
                wall.gameObject.SetActive(true);
        }

        // Reset player position
        if (playerRect != null && startPointRect != null)
            playerRect.position = startPointRect.position;
    }

    public void ForceEndGame()
    {
        if (playerRect != null) playerRect.gameObject.SetActive(false);
        if (goalRect != null) goalRect.gameObject.SetActive(false);
        if (wallImages != null)
        {
            foreach (var wall in wallImages)
                wall.gameObject.SetActive(false);
        }

        if (playerRect != null && startPointRect != null)
            playerRect.position = startPointRect.position;
    }

    public void Fail()
    {
        Debug.Log("Player hit a wall! Resetting position.");
        if (playerRect != null && startPointRect != null)
            playerRect.position = startPointRect.position;
    }

    public void Success()
    {
        Debug.Log("Player reached the goal! Game complete.");
        ForceEndGame();
        // Optional: add reward, message, or unlock next step
    }

    // Check if player overlaps any wall
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

    // RectTransform overlap detection
    public bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Vector3[] aCorners = new Vector3[4];
        Vector3[] bCorners = new Vector3[4];
        a.GetWorldCorners(aCorners);
        b.GetWorldCorners(bCorners);

        Rect rectA = new Rect(aCorners[0].x, aCorners[0].y,
                              aCorners[2].x - aCorners[0].x,
                              aCorners[2].y - aCorners[0].y);
        Rect rectB = new Rect(bCorners[0].x, bCorners[0].y,
                              bCorners[2].x - bCorners[0].x,
                              bCorners[2].y - bCorners[0].y);

        return rectA.Overlaps(rectB);
    }
}
