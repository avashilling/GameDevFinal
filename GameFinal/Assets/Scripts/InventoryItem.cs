using UnityEngine;

public enum ItemType
{
    None,
    Battery,
    Coin,
    Key,
    Hammer,
    Disc   //<-- Added Disc item type benny
   
}

[System.Serializable]
public class InventoryItem
{
    public ItemType itemType;
    public Sprite icon;
}
