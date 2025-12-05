using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable, IInteractable
{
    [Header("Scene To Load")]
    public string sceneName;

    [Header("Door Settings")]
    public bool unlocked = false;   // false = key required, true = open freely

    [Header("Audio Settings")]
    public float useAudioDelay = 0.25f; // Delay in seconds before scene loads

    public void Interact(InventoryItem heldItem)
    {
        // If the door is unlocked, allow entry regardless of held item.
        if (unlocked)
        {
            LoadScene();
            return;
        }

        // Door is locked, must have a key
        if (heldItem != null && heldItem.itemType == ItemType.Key)
        {
            GameManager.Instance.RemoveSelectedItem();
            PlayUseAudio();
            // Delay scene load so audio can play
            Invoke(nameof(LoadScene), useAudioDelay);
        }
        else if(SceneManager.GetActiveScene().name == "Hailey Scene")
        {
            HintManager.Instance.ShowHint("It's locked. I need to find a key.");
        }
        else if(SceneManager.GetActiveScene().name == "Hallway Scene")
        {
            HintManager.Instance.ShowHint("It's locked. No keyhole...");
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("DoorInteractable: No scene name assigned.");
        }
    }
}
