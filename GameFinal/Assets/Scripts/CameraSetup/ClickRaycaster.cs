using UnityEngine;

public class ClickRaycaster : MonoBehaviour
{
    [Tooltip("Main camera rendering the scene")]
    public Camera mainCamera;

    [Tooltip("Optional: LayerMask to only raycast interactable objects")]
    public LayerMask interactableLayers = ~0; // default: everything

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            // Convert mouse to normalized viewport coordinates
            Vector3 viewportPoint = new Vector3(
                Input.mousePosition.x / Screen.width,
                Input.mousePosition.y / Screen.height,
                0
            );

            // Generate ray from camera
            Ray ray = mainCamera.ViewportPointToRay(viewportPoint);

            // Raycast all hits, including triggers
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, interactableLayers, QueryTriggerInteraction.Collide);

            if (hits.Length == 0)
            {
                Debug.Log("Clicked on nothing.");
                return;
            }

            // Sort hits by distance so closest object is processed first
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"Hit collider: {hit.collider.name} | type: {hit.collider.GetType()}");

                // Try to get Node component on the collider itself
                Node node = hit.collider.GetComponent<Node>();
                if (node == null)
                {
                    // Optional: also check parent in case collider is child
                    node = hit.collider.GetComponentInParent<Node>();
                }

                if (node != null)
                {
                    Debug.Log("Calling Arrive() on Node: " + node.name);
                    node.Arrive();
                    break; // stop at first interactable node
                }
            }
        }
    }
}
