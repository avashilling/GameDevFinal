using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [Header("Optional Audio")]
    public AudioClip onPickupAudio;
    public AudioClip onUseAudio;

    public Collider col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
    }

    // Helper methods subclasses can call
    protected void PlayPickupAudio()
    {
        if (onPickupAudio != null)
            AudioManager.Instance.PlaySFX(onPickupAudio);
    }

    protected void PlayUseAudio()
    {
        if (onUseAudio != null)
            AudioManager.Instance.PlaySFX(onUseAudio);
    }
}
