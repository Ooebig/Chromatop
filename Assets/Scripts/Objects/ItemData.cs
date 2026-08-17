using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public enum ConsumableEffectType
{
    None,
    Heal,
    TemporaryDamage,
    TemporarySpeed,
    TemporaryDefense
}

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Game/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public int itemID;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Inventory Pocket")]
    public PocketType pocketType;

    public bool isStackable;

    [Min(1)]
    public int maxStackSize = 1;

    [Header("Weapon Settings")]
    public GameObject weaponPrefab;

    [Header("Passive/Relic Settings")]
    public List<StatModifier> statModifiers =
        new List<StatModifier>();

    [Header("Consumable Settings")]
    public ConsumableEffectType consumableEffect;
    public float consumableValue;
    public float consumableDuration;

    [Header("Currency Settings")]
    [Min(0)]
    public int currencyValue;

    public virtual void Activate(ItemContext context)
    {
        Debug.Log($"Activated {itemName}");
    }

    private void ActivateWeapon(ItemContext context)
    {
        if (weaponPrefab == null)
            return;

        Instantiate(
            weaponPrefab,
            context.player.transform
        );
    }

    private void ActivateConsumable(ItemContext context)
    {
        if (context.playerStats == null)
            return;

        switch (consumableEffect)
        {
            case ConsumableEffectType.Heal:
                context.playerStats.Heal(
                    consumableValue
                );
                break;
        }
    }
}