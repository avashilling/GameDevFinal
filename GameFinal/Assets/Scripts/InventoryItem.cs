using UnityEngine;

public enum ItemType
{
    None,
    Coin,
    Key,
    Hammer,
    Disc,   //<-- Added Disc item type benny
    Battery,

}

[System.Serializable]
public class InventoryItem
{
    public ItemType itemType;
    public Sprite icon;
}
