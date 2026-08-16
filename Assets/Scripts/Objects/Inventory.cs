using UnityEngine;

public enum ItemType
{
    Weapon,
    Passive,
    Modular,
    Consumable,
    Currency
}

[CreateAssetMenu(fileName = "Inventory", menuName = "Persistence/Inventory")]

public class Inventory : ScriptableObject
{
    public int playerScore;
    public string playerName;

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
