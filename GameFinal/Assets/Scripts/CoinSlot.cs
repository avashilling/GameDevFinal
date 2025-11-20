using UnityEngine;

public class CoinSlot : Interactable, IInteractable
{
    public void Interact(InventoryItem heldItem)
    {
        if (heldItem != null && heldItem.itemType == ItemType.Coin)
        {
            GameManager.Instance.RemoveSelectedItem();
            GameManager.Instance.startArcadeGame();
        }
        else
        {
            Debug.Log("You need a coin to use this slot.");
        }
    }
}
