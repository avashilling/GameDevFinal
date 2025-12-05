using UnityEngine;
using System.Collections;

public class VendingMachineInteractable : Interactable, IInteractable
{
    [Header("Disc & Animation")]
    [Tooltip("Animator controlling the disc object")]
    public Animator discAnimator;

    [Tooltip("Names of kick animations in order")]
    public string[] kickAnimations;

    [Tooltip("Disc GameObject to drop after final kick")]
    public GameObject discObject;

    [Tooltip("Position where the disc will be dropped")]
    public Transform dropPosition;

    [Header("Audio")]
    [Tooltip("Assign 3 different kick sounds to play sequentially")]
    public AudioClip[] kickSounds; // <-- array for 3 different sounds

    [Header("Camera Shake")]
    [Tooltip("Duration of camera shake on kick")]
    public float shakeDuration = 0.2f;

    [Tooltip("Magnitude of camera shake")]
    public float shakeMagnitude = 0.1f;

    private int currentKick = 0;

    public void Interact(InventoryItem heldItem)
    {
        if (discAnimator == null || kickAnimations.Length == 0 || discObject == null || dropPosition == null)
        {
            Debug.LogError("VendingMachineInteractable: Assign all references.");
            return;
        }

        if (currentKick < kickAnimations.Length)
        {
            // Play next kick animation
            discAnimator.Play(kickAnimations[currentKick]);

            // Play kick sound (loop if not enough clips)
            if (kickSounds != null && kickSounds.Length > 0)
            {
                int soundIndex = currentKick % kickSounds.Length;
                AudioManager.Instance.PlaySFX(kickSounds[soundIndex]);
            }

            // Shake camera
            if (Camera.main != null)
                StartCoroutine(ShakeCamera(Camera.main.transform, shakeDuration, shakeMagnitude));

            currentKick++;
        }

        if (currentKick >= kickAnimations.Length)
        {
            // Drop disc
            discObject.transform.position = dropPosition.position;
            discObject.SetActive(true); // make it visible / interactable

            currentKick = 0; // reset counter for next time
        }
    }

    private IEnumerator ShakeCamera(Transform camTransform, float duration, float magnitude)
    {
        Vector3 originalPos = camTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camTransform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;

            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}
