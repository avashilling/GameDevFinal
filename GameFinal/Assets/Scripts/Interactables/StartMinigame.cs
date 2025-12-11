using UnityEngine;

public class StartMinigameInteractable : Interactable, IInteractable
{
    [Header("Minigame")]
    public MiniGameController miniGameController;

    public void Interact(InventoryItem heldItem)
    {
        if (miniGameController == null)
        {
            Debug.LogError("StartMinigameInteractable: No MiniGameController assigned.");
            return;
        }

        PlayUseAudio();

        // Start the minigame
        miniGameController.BeginGame();
        AudioManager.Instance.ArcadeMinigameStart();

        // Remove this trigger from the scene
        Destroy(gameObject);
    }
}
