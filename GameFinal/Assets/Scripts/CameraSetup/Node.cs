using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Node : MonoBehaviour
{
    public Transform cameraPosition;

    //Nodes other than left/right/back that the player can go to
    //like interactable props or alernate locations
    public List<Node> reachableNodes = new List<Node>();
    public List<Interactable> interactables = new List<Interactable>();

    public Node left;
    public Node right;
    public Node back;

    [HideInInspector]
    //collider that clicking on will navigate to this node
    public Collider col;

    //used to ignore clicks while camera is animating
    private static bool isCameraMoving = false;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    //private void OnMouseDown()
    //{
    //    Debug.Log("OnMouseDown called on Node: " + name);
    //    if (!isCameraMoving)
    //        Arrive();
    //}

    public void Arrive()
    {
        if (isCameraMoving) return;

        if (GameManager.Instance.currentNode != null)
            GameManager.Instance.currentNode.Leave();

        GameManager.Instance.currentNode = this;
        isCameraMoving = true;
        StartCoroutine(MoveCameraToNode(Camera.main.transform, cameraPosition.position, cameraPosition.rotation, 1.0f));

        if (col != null)
        {
            col.enabled = false;

            foreach (Node node in reachableNodes)
                if (node.col != null)
                    node.col.enabled = true;

            // Enable interactables for this node
            foreach (var interactable in interactables)
                if (interactable != null)
                    interactable.GetComponent<Collider>().enabled = true;
                    Debug.Log("enabling collider for interactable");
        }
    }

    public void Leave()
    {
        if (col != null)
        {
            col.enabled = true;

            foreach (Node node in reachableNodes)
                if (node.col != null)
                    node.col.enabled = false;

            // Disable interactables
            foreach (var interactable in interactables)
                if (interactable != null)
                    interactable.GetComponent<Collider>().enabled = false;
        }
    }


    private IEnumerator MoveCameraToNode(Transform cam, Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        cam.position = targetPos;
        cam.rotation = targetRot;

        // Re-enable input
        isCameraMoving = false;
    }
}
