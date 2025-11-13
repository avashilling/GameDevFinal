using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    protected Collider col;

    protected virtual void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false; // Only enabled when active node allows it
    }

    // Called when player clicks on it
    public abstract void Interact();
}
