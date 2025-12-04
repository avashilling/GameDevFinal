using UnityEngine;

public class PickupItem : Interactable, IInteractable
{
    public InventoryItem itemData;

    public void Interact(InventoryItem heldItem)
    {
        if (GameManager.Instance.AddItem(itemData))
        {
            PlayPickupAudio();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full, cannot pick up item: " + itemData.itemType);
        }
    }
}
