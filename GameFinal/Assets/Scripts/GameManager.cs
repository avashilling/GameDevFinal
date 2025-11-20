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
    public Sprite keySprite;         //hardcoded key for now, once we implement game the key will be a reward for finishing


    public delegate void InventoryChangeHandler();
    public event InventoryChangeHandler OnInventoryChanged;

    [Header("Node State")]
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

    // Add an item to inventory (goes to leftmost empty slot)
    public bool AddItem(InventoryItem newItem)
    {
        if (inventory.Count >= maxSlots)
        {
            Debug.Log("Inventory full, cannot add item of type: " + newItem.itemType);
            return false;
        }

        inventory.Add(newItem);

        // Play pickup sound if an AudioSource is attached
        var audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();

        OnInventoryChanged?.Invoke();
        return true;
    }


    // Remove an item by type
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

    // Remove currently held item
    public void RemoveSelectedItem()
    {
        if (currentlyHeldItem == null) return;

        inventory.Remove(currentlyHeldItem);
        currentlyHeldItem = null;
        OnInventoryChanged?.Invoke();
    }

    // Select an inventory item by index
    public void SelectItem(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            currentlyHeldItem = inventory[index];
            OnInventoryChanged?.Invoke();
        }
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
