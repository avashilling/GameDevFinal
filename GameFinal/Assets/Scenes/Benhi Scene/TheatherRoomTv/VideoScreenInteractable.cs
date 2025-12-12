//using UnityEngine;
//using UnityEngine.Video;

//public class VideoScreenInteractable : Interactable, IInteractable
//{
//    [SerializeField] private VideoPlayer videoPlayer; // Reference to the VideoPlayer component
//    [SerializeField] private ItemType requiredItem = ItemType.Disc;

//    public void Interact(InventoryItem heldItem)
//    {
//        if (heldItem == null)
//        {
//            HintManager.Instance.ShowHint("You need a Disc to start the video.");
//            return;
//        }

//        if (heldItem.itemType != requiredItem)
//        {
//            HintManager.Instance.ShowHint("This item cannot be used here.");
//            return;
//        }

//        // Correct item → play video
//        videoPlayer.Play();

//        // Remove the disc from inventory
//        GameManager.Instance.RemoveSelectedItem();

//        // Optional: play sound
//        PlayUseAudio();
//    }
//}

using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoScreenInteractable : Interactable, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;       // Reference to VideoPlayer
    [SerializeField] private ItemType requiredItem = ItemType.Disc;
    [SerializeField] private AudioClip endSound;            // Door close or any sound
    private bool hasInsertedDisk;

    private bool hasPlayed = false; // Make sure the sound only plays once

    public void Interact(InventoryItem heldItem)
    {

        if (hasInsertedDisk)
        {
            HintManager.Instance.ShowHint("It's already playing");
            return;
        }
        if (heldItem == null)
        {
            HintManager.Instance.ShowHint("I could probably put a vhs tape in there");
            return;
        }

        if (heldItem.itemType != requiredItem)
        {
            HintManager.Instance.ShowHint("This item cannot be used here.");
            Debug.Log("You are holding a: " + heldItem.itemType);
            return;
        }

        hasInsertedDisk = true;
        // Correct item → play video
        videoPlayer.Play();
        PlayUseAudio();
        hasInsertedDisk = true;
        // Remove the disc from inventory
        GameManager.Instance.RemoveSelectedItem();

        // Subscribe to video end event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (hasPlayed) return; // Prevent multiple calls
        hasPlayed = true;

        // Play end sound
        if (endSound != null)
        {
            AudioManager.Instance.PlaySFX(endSound);
        }

        // Delay scene load slightly to let sound play
        float delay = (endSound != null) ? endSound.length : 0f;
        Invoke(nameof(LoadEndScene), delay);

        // Unsubscribe from event
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
