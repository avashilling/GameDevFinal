using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
        Debug.Log("Gamemanager: Awake");
    }

    private void Start()
    {
        currentlyHeldItem = null;
        Debug.Log("Gamemanager: Start");
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
        currentNode = null;   // wipe old scene state
        node.Arrive();        // this correctly assigns currentNode, enables interactables, etc.
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
