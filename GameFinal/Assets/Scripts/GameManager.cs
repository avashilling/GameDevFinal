using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Inventory Settings")]
    public int maxSlots = 4;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public InventoryItem currentlyHeldItem;

    [Header("Reward Item Sprites")]
    public Sprite keySprite;

    [Header("Node State")]
    public Node currentNode;

    [Header("Audio")]
    public AudioSource pickupAudio;        // <-- assign your item pickup sound
    public AudioSource movementAudio;      // <-- assign node movement sound (different clip)

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentlyHeldItem = null;
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

        // Play pickup sound
        if (pickupAudio != null)
            pickupAudio.Play();

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

    // Call this from Node.Arrive() when the player moves
    public void SetCurrentNode(Node newNode)
    {
        if (newNode == null) return;
        if (newNode == currentNode) return;

        currentNode = newNode;

        // Play node movement sound
        if (movementAudio != null)
            movementAudio.Play();
    }

    public void startArcadeGame()
    {
        InventoryItem keyItem = new InventoryItem();
        keyItem.itemType = ItemType.Key;
        keyItem.icon = keySprite;

        AddItem(keyItem);

        Debug.Log("Arcade game complete! Key awarded.");
    }
}
