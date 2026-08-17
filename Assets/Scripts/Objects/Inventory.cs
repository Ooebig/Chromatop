using UnityEngine;

public enum ItemType
{
    Weapon,
    Passive,
    Modular,
    Consumable,
    Currency
}

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Inventory/Item Data")]
public class Inventory : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public string itemID;

    [TextArea]
    public string description;

    public Sprite icon;
    public ItemType itemType;

    [Header("Stacking")]
    public bool isStackable;
    [Min(1)] public int maxStackSize = 1;
}
