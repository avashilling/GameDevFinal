using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    public Collider col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
    }
}
