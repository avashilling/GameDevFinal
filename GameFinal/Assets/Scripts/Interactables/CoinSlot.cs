using UnityEngine;

public class CoinSlot : Interactable, IInteractable
{
    [Header("UI / Objects")]
    public GameObject insertCoinText;   // UI canvas element to remove
    public GameObject startButton;      // Object that becomes active
    private bool coinInserted = false;

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
            coinInserted = true;
        }
        else if (coinInserted) {
            HintManager.Instance.ShowHint("I've already put in my token");
        }
        else {
            HintManager.Instance.ShowHint("I don't have any coins on me");
        }
    }
}
