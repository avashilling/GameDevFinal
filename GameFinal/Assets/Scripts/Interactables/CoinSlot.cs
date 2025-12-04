using UnityEngine;

public class CoinSlot : Interactable, IInteractable
{
    [Header("UI / Objects")]
    public GameObject insertCoinText;   // UI canvas element to remove
    public GameObject startButton;      // Object that becomes active

    public void Interact(InventoryItem heldItem)
    {
        if (heldItem != null && heldItem.itemType == ItemType.Coin)
        {
            // Remove coin
            GameManager.Instance.RemoveSelectedItem();

            // Play sound
            PlayUseAudio();

            // Update UI and objects
            if (insertCoinText != null)
                Destroy(insertCoinText);

            if (startButton != null)
                startButton.SetActive(true);
        }
        else
        {
            HintManager.Instance.ShowHint("Hm.. I need a token to start the game");
        }
    }
}
