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
            HintManager.Instance.ShowHint("Hm.. I need a token to start the game");
        }
    }
}
