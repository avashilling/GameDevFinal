using UnityEngine;

public class ClickRaycaster : MonoBehaviour
{
    public Camera mainCamera;

    private void Update()
    {
        RunHoverCheck();

        if (Input.GetMouseButtonDown(0))
            RunClickCheck();
    }

    // ---------------------------------------------------------
    // HOVER CHECK
    // ---------------------------------------------------------
    private void RunHoverCheck()
    {
        Debug.Log("Hover Check Running");

        var node = GameManager.Instance.currentNode;
        if (node == null)
        {
            CursorManager.Instance.UpdateHoverState(false);
            return;
        }

        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 viewportPoint = new Vector3(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height,
            0
        );

        Ray ray = mainCamera.ViewportPointToRay(viewportPoint);
        bool hovering = false;

        // reachable nodes
        foreach (var reachable in node.reachableNodes)
        {
            if (reachable != null && reachable.col != null)
            {
                if (reachable.col.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    hovering = true;
                    break;
                }
            }
        }

        // interactables
        if (!hovering)
        {
            foreach (var interactable in node.interactables)
            {
                if (interactable != null && interactable.col != null)
                {
                    if (interactable.col.Raycast(ray, out RaycastHit hit, 1000f))
                    {
                        hovering = true;
                        break;
                    }
                }
            }
        }
        Debug.Log("HOVER STATE: " + hovering);

        CursorManager.Instance.UpdateHoverState(hovering);
    }

    // ---------------------------------------------------------
    // CLICK CHECK
    // ---------------------------------------------------------
    private void RunClickCheck()
    {
        var node = GameManager.Instance.currentNode;
        if (node == null) return;

        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 viewportPoint = new Vector3(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height,
            0
        );

        Ray ray = mainCamera.ViewportPointToRay(viewportPoint);

        RaycastHit closestHit = default;
        float closestDistance = float.MaxValue;
        object closestTarget = null;

        // check nodes
        foreach (var reachable in node.reachableNodes)
        {
            if (reachable != null && reachable.col != null)
            {
                if (reachable.col.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        closestHit = hit;
                        closestTarget = reachable;
                    }
                }
            }
        }

        // check interactables
        foreach (var interactable in node.interactables)
        {
            if (interactable != null && interactable.col != null)
            {
                if (interactable.col.Raycast(ray, out RaycastHit hit, 1000f))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        closestHit = hit;
                        closestTarget = interactable;
                    }
                }
            }
        }

        if (closestTarget == null)
            return;

        if (closestTarget is Interactable interBase)
        {
            var I = interBase.GetComponent<IInteractable>();
            if (I != null)
                I.Interact(GameManager.Instance.currentlyHeldItem);
        }
        else if (closestTarget is Node clickedNode)
        {
            clickedNode.Arrive();
        }
    }
}
