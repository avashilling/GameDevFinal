using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Setup")]
    public Node startingNode; // Assign this in the Inspector for each scene!

    [Header("Inventory Settings")]
    public int maxSlots = 3;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public InventoryItem currentlyHeldItem;

    [Header("Reward Item Sprites")]
    public Sprite keySprite;

    [Header("Node State")]
    public Node currentNode;

    [Header("Puzzle State")]
    public bool batteriesInserted = false;
    public bool keypadCorrect = false;

    public delegate void InventoryChangeHandler();
    public event InventoryChangeHandler OnInventoryChanged;

    private void Awake()
    {
        Debug.Log("GameManager: Awake in scene " + SceneManager.GetActiveScene().name);

        // If there's an old instance from a previous scene, destroy it
        if (Instance != null && Instance != this)
        {
            Debug.Log("GameManager: Destroying old instance from previous scene");
            Destroy(Instance.gameObject);
        }

        // Set this as the new instance
        Instance = this;
        Debug.Log("GameManager: This instance is now active");
    }

    private void Start()
    {
        currentlyHeldItem = null;

        // Initialize the starting node for this scene
        if (startingNode == null)
        {
            Debug.LogError("GameManager: No startingNode assigned in scene " + SceneManager.GetActiveScene().name + "! Please assign it in the Inspector.");
        }
        else
        {
            Debug.Log("GameManager: Initializing starting node: " + startingNode.name);
            SetStartingNode(startingNode);
            Debug.Log("GameManager: currentNode is now " + (currentNode != null ? currentNode.name : "NULL"));
        }
    }

    private void Update()
    {
        // Right-click to deselect currently held item
        if (Input.GetMouseButtonDown(1))
        {
            currentlyHeldItem = null;
            OnInventoryChanged?.Invoke();
        }
    }

    public void SetStartingNode(Node node)
    {
        Debug.Log("GameManager: SetStartingNode called with " + (node != null ? node.name : "NULL"));
        currentNode = null;   // wipe old scene state
        node.Arrive();        // this correctly assigns currentNode, enables interactables, etc.
        Debug.Log("GameManager: After Arrive(), currentNode is " + (currentNode != null ? currentNode.name : "NULL"));
    }

    // ------------------------------------------------------------
    // INVENTORY LOGIC
    // ------------------------------------------------------------
    public bool AddItem(InventoryItem newItem)
    {
        if (inventory.Count >= maxSlots)
        {
            Debug.Log("Inventory full, cannot add item of type: " + newItem.itemType);
            return false;
        }
        inventory.Add(newItem);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(ItemType itemType)
    {
        InventoryItem item = inventory.Find(i => i.itemType == itemType);
        if (item != null)
        {
            inventory.Remove(item);
            if (currentlyHeldItem == item)
                currentlyHeldItem = null;
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveSelectedItem()
    {
        if (currentlyHeldItem == null) return;
        inventory.Remove(currentlyHeldItem);
        currentlyHeldItem = null;
        OnInventoryChanged?.Invoke();
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            currentlyHeldItem = inventory[index];
            OnInventoryChanged?.Invoke();
        }
    }

    // ------------------------------------------------------------
    // NODE MOVEMENT LOGIC
    // ------------------------------------------------------------
    public void SetCurrentNode(Node newNode)
    {
        if (newNode == null) return;
        if (newNode == currentNode) return;
        currentNode = newNode;
        AudioManager.Instance.PlayFootstep();
    }

    public void winArcadeGame()
    {
        AudioManager.Instance.ArcadeMinigameStop();
        InventoryItem keyItem = new InventoryItem();
        keyItem.itemType = ItemType.Key;
        keyItem.icon = keySprite;
        AddItem(keyItem);
        HintManager.Instance.ShowHint("A key came out of the machine.");
    }
}