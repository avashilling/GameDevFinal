using UnityEngine;

public enum ItemType
{
    None,
    Coin,
    Key,
}

[System.Serializable]
public class InventoryItem
{
    public ItemType itemType;
    public Sprite icon;
}
