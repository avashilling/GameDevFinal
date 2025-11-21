using UnityEngine;

public class DestroyOnClick : Interactable, IInteractable
{
    [Header("Optional: Required Held Item")]
    public ItemType requiredItem; // Leave unset if no item required

    public void Interact(InventoryItem heldItem)
    {
        // Conditional destroy: only if a specific item is required
        if (requiredItem != 0) // assuming ItemType enum default = 0
        {
            if (heldItem != null && heldItem.itemType == requiredItem)
            {
                GameManager.Instance.RemoveSelectedItem();
                Destroy(gameObject);
                Debug.Log($"Object destroyed using {requiredItem}.");
            }
            else
            {
                Debug.Log($"You need a {requiredItem} to use this.");
            }
        }
        else
        {
            // No item required: destroy immediately
            Destroy(gameObject);
            HintManager.Instance.ShowHint("Got it!");
        }
    }
}
