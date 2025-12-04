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

        PlayUseAudio(); // Optional, safe if null

        // Start the minigame
        miniGameController.BeginGame();

        // Remove this trigger from the scene
        Destroy(gameObject);
    }
}
