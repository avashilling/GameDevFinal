using UnityEngine;

public class DVDPlayerInteractable : MonoBehaviour, IInteractable
{
    public VideoScreen videoScreen; // assign in inspector

    public void Interact(InventoryItem heldItem)
    {
        if (heldItem != null && heldItem.itemType == ItemType.Disc)
        {
            videoScreen.PlayVideo();
            Debug.Log("DVD Player: Disc inserted, playing video!");
        }
        else
        {
            Debug.Log("DVD Player: You need a DISC to start this.");
        }
    }
}
