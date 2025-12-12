using UnityEngine;

public class PlaceBattery : Interactable, IInteractable
{
    [Header("Battery Slots")]
    public GameObject batterySlot1;  // First battery object (initially off)
    public GameObject batterySlot2;  // Second battery object (initially off)

    private int batteriesPlaced = 0; // Tracks how many batteries have been placed

    public void Interact(InventoryItem heldItem)
    {
        if (heldItem != null && heldItem.itemType == ItemType.Battery)
        {
            // Only place batteries if slots are available
            if (batteriesPlaced == 0)
            {
                if (batterySlot1 != null)
                    batterySlot1.SetActive(true);

                batteriesPlaced++;
                GameManager.Instance.RemoveSelectedItem();
            }
            else if (batteriesPlaced == 1)
            {
                if (batterySlot2 != null)
                    batterySlot2.SetActive(true);

                batteriesPlaced++;
                GameManager.Instance.RemoveSelectedItem();
                HintManager.Instance.ShowHint("Both the batteries are in. I bet the keypad works now");
                GameManager.Instance.batteriesInserted = true;
                PlayUseAudio();
            }
            else
            {
                // All battery slots are filled
                HintManager.Instance.ShowHint("Both battery slots are already filled");
            }
        }
        else if(batteriesPlaced < 2)
        {
            HintManager.Instance.ShowHint("There's 2 empty battery slots");
        }

        else
        {
            HintManager.Instance.ShowHint("There's nothing else to touch here");
        }
    }
}
