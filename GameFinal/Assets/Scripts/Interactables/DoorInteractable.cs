using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable, IInteractable
{
    [Header("Scene To Load")]
    public string sceneName;

    public void Interact(InventoryItem heldItem)
    {
        // Must be holding a key
        if (heldItem != null && heldItem.itemType == ItemType.Key)
        {
            // Remove the key from inventory
            GameManager.Instance.RemoveSelectedItem();

            // Switch scenes
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("DoorInteractable: No scene name assigned.");
            }
        }
        else
        {
            HintManager.Instance.ShowHint("It's locked, I need to find a key.");

        }
    }
}
