using UnityEngine;

public class CoinSlot : MonoBehaviour, IInteractable
{
    public void Interact(InventoryItem heldItem)
    {
        // Must be holding a coin
        if (heldItem != null && heldItem.itemType == ItemType.Coin)
        {
            // Remove the coin from inventory
            GameManager.Instance.RemoveSelectedItem();

            // Trigger arcade logic
            GameManager.Instance.startArcadeGame();
        }
        else
        {
            Debug.Log("You need a coin to use this slot.");
        }
    }
}
