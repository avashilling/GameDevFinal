using UnityEngine;

public class ClickRaycaster : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public LayerMask interactableLayers = ~0; // Layers for nodes & items

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (mainCamera == null) mainCamera = Camera.main;

        // Convert mouse to normalized viewport coordinates
        Vector3 viewportPoint = new Vector3(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height,
            0
        );

        Ray ray = mainCamera.ViewportPointToRay(viewportPoint);

        // Raycast all hits (including triggers)
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, interactableLayers, QueryTriggerInteraction.Collide);

        if (hits.Length == 0) return;

        // Sort by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null)
                interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(GameManager.Instance.currentlyHeldItem);
                break; // Stop after first interactable
            }

            Node node = hit.collider.GetComponent<Node>();
            if (node == null)
                node = hit.collider.GetComponentInParent<Node>();

            if (node != null)
            {
                node.Arrive();
                break; // Stop at first node
            }
        }
    }
}

