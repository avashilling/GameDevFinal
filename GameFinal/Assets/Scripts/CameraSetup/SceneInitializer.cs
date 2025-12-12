using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public Node startingNode;

    private void Awake()
    {
        Debug.Log("SceneInitializer: Awake in scene " + gameObject.scene.name);

        if (startingNode == null)
        {
            Debug.Log("SceneInitializer: No startingNode assigned in " + gameObject.scene.name);
            return;
        }

        // Wait until end of frame to ensure GameManager is fully set up
        StartCoroutine(InitializeAfterFrame());
    }

    private System.Collections.IEnumerator InitializeAfterFrame()
    {
        // Wait one frame to ensure all Awake() calls have completed
        yield return null;

        if (GameManager.Instance == null)
        {
            Debug.Log("SceneInitializer: GameManager.Instance is null!");
            yield break;
        }

        Debug.Log("SceneInitializer: Setting starting node to " + startingNode.name);
        GameManager.Instance.SetStartingNode(startingNode);
        Debug.Log("SceneInitializer: currentNode is now: " +
                  (GameManager.Instance.currentNode != null ? GameManager.Instance.currentNode.name : "NULL"));
    }
}