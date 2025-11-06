using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button leftArrow;
    public Button rightArrow;
    public Button backArrow;

    void Update()
    {
        var node = GameManager.Instance.currentNode;
        if (node == null) return;

        // Show/hide arrows depending on available directions
        leftArrow.gameObject.SetActive(node.left != null);
        rightArrow.gameObject.SetActive(node.right != null);
        backArrow.gameObject.SetActive(node.back != null);
    }

    public void OnClickArrow(string direction)
    {
        if (GameManager.Instance.currentNode == null) return;

        var node = GameManager.Instance.currentNode;

        switch (direction)
        {
            case "left":
                node.left?.Arrive();
                break;
            case "right":
                node.right?.Arrive();
                break;
            case "back":
                node.back?.Arrive();
                break;
        }
    }
}
