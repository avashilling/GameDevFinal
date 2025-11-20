using UnityEngine;

public class ArcadeMachine : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startButton;          // Start button outside ScreenPanel
    public GameObject arcadeUI;             // Screen Panel containing the arcade UI
    public GameObject miniGameArea;         // Parent of PlayerCube, Walls, Goal
    public MiniGameController miniGameController; // Reference to your minigame controller

    [Header("Camera References (Optional)")]
    public ArcadeCameraController cameraController; // Optional camera controller

    private bool isActive = false;

    // Called when player clicks the Start button or arcade node
    public void Interact()
    {
        StartArcadeGame();
    }

    public void StartArcadeGame()
    {
        if (isActive) return;
        isActive = true;

        Debug.Log("Arcade started!");

        if (arcadeUI != null) arcadeUI.SetActive(true);
        if (startButton != null) startButton.SetActive(false);
        if (miniGameArea != null) miniGameArea.SetActive(true);
        if (cameraController != null) cameraController.MoveToArcadeView();
        if (miniGameController != null) miniGameController.BeginGame();
    }

    public void ExitArcadeGame()
    {
        if (!isActive) return;
        isActive = false;

        Debug.Log("Arcade exited!");

        if (miniGameArea != null) miniGameArea.SetActive(false);
        if (arcadeUI != null) arcadeUI.SetActive(false);
        if (startButton != null) startButton.SetActive(true);
        if (cameraController != null) cameraController.MoveToDefaultView();
        if (miniGameController != null) miniGameController.ForceEndGame();
    }
}
