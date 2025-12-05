using UnityEngine;
using UnityEngine.Video;

public class VideoScreenInteractable : Interactable, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    [SerializeField] private ItemType requiredItem = ItemType.Disc;

    public void Interact(InventoryItem heldItem)
    {
        if (heldItem == null)
        {
            HintManager.Instance.ShowHint("You need a Disc to start the video.");
            return;
        }

        if (heldItem.itemType != requiredItem)
        {
            HintManager.Instance.ShowHint("This item cannot be used here.");
            return;
        }

        // Correct item → play video
        videoPlayer.Play();

        // Remove the disc from inventory
        GameManager.Instance.RemoveSelectedItem();

        // Optional: play sound
        PlayUseAudio();
    }
}
