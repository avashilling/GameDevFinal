using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable, IInteractable
{
    [Header("Scene To Load")]
    public string sceneName;

    [Header("Door Settings")]
    public bool unlocked = false;   // false = key required, true = open freely

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
            LoadScene();
        }
        else
        {
            HintManager.Instance.ShowHint("It's locked. I need to find a key.");
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
