using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game manager instance persists across scenes
    // Currently only stores currentNode, will eventually keep track of what player is currently holding, state of the game, etc.
    public static GameManager Instance { get; private set; }

    public Node currentNode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
