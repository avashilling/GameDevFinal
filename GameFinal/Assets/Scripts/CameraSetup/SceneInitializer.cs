using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public Node startingNode;

    private void Start()
    {
        if (startingNode == null)
        {
            Debug.LogError("SceneInitializer: No startingNode assigned in " + gameObject.scene.name);
            return;
        }

        GameManager.Instance.SetStartingNode(startingNode);
        Debug.Log("GameManager currentNode is: " + GameManager.Instance.currentNode);
    }
}
