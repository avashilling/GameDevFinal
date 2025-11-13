using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public InventoryItem itemData; // Assign in inspector

    public void Interact(InventoryItem heldItem)
    {
        // Pickups usually ignore what is held
        if (GameManager.Instance.AddItem(itemData))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full, cannot pick up item of type: " + itemData.itemType);
        }
    }
}
