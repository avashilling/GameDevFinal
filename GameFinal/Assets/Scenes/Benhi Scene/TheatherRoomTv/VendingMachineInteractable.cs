using UnityEngine;
using System.Collections;

public class VendingMachineInteractable : Interactable, IInteractable
{
    [Header("Disc Objects")]
    public GameObject discInsideMachine;   // <- The disc visible inside the vending machine
    public GameObject droppedDisc;         // <- The disc that gets spawned on the floor
    public Transform dropPosition;

    [Header("Animation")]
    public Animation vendingAnimation;     // Using Animation component instead of Animator
    public string kickAnimationName = "VendingShake";

    [Header("Audio")]
    public AudioClip[] kickSounds;

    [Header("Camera Shake")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;

    private int kickCount = 0;
    private bool discHasDropped = false;

    public void Interact(InventoryItem heldItem)
    {
        // If disc has already dropped, do nothing
        if (discHasDropped)
        {
            HintManager.Instance.ShowHint("There's nothing left in the machine");
            return;
        }

        // Play kick animation
        if (vendingAnimation != null && vendingAnimation[kickAnimationName] != null)
        {
            vendingAnimation.Play(kickAnimationName);
        }

        // Play random kick sound
        if (kickSounds != null && kickSounds.Length > 0)
        {
            int index = Random.Range(0, kickSounds.Length);
            AudioManager.Instance.PlaySFX(kickSounds[index]);
        }

        // Camera shake
        if (Camera.main != null)
            StartCoroutine(ShakeCamera(Camera.main.transform, shakeDuration, shakeMagnitude));

        kickCount++;

        // On 3rd kick → drop disc
        if (kickCount >= 3)
        {
            DropDisc();
        }
    }

    private void DropDisc()
    {
        discHasDropped = true;

        if (droppedDisc != null && dropPosition != null)
        {
            // Move dropped disc into position
            droppedDisc.transform.position = dropPosition.position;
            droppedDisc.SetActive(true);
        }

        // Hide the disc inside the vending machine
        if (discInsideMachine != null)
            discInsideMachine.SetActive(false);
        HintManager.Instance.ShowHint("It dropped a tape!");
    }

    private IEnumerator ShakeCamera(Transform cam, float duration, float magnitude)
    {
        Vector3 originalPos = cam.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cam.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.localPosition = originalPos;
    }
}