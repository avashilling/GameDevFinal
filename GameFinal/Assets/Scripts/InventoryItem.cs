using UnityEngine;

public enum ItemType
{
    None,
    Coin,
    Key,
    Hammer,
}

[System.Serializable]
public class InventoryItem
{
    public ItemType itemType;
    public Sprite icon;
}
