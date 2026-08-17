using System.Collections.Generic;
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
    menuName = "Inventory/Item Data"
)]
public class ItemData : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public int itemID;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("World Appearance")]
    [Tooltip("Physical prefab spawned when this item is dropped.")]
    public GameObject worldPrefab;

    [Header("Inventory Pocket")]
    public PocketType pocketType;

    [Header("Stacking")]
    public bool isStackable;

    [Min(1)]
    public int maxStackSize = 1;

    [Header("Weapon Settings")]
    [Tooltip("Only used by Weapon items.")]
    public GameObject weaponPrefab;

    [Header("Relic Settings")]
    [Tooltip("Only used by Relic items.")]
    public List<StatModifier> statModifiers =
        new List<StatModifier>();

    [Header("Consumable Settings")]
    public ConsumableEffectType consumableEffect;
    public float consumableValue;

    [Min(0f)]
    public float consumableDuration;

    [Header("Currency Settings")]
    [Min(0)]
    public int currencyValue;

    public virtual void Activate(ItemContext context)
    {
        if (context == null || context.player == null)
        {
            Debug.LogWarning(
                $"{itemName} was activated without a valid ItemContext.",
                this
            );

            return;
        }

        switch (pocketType)
        {
            case PocketType.Relic:
                ActivateRelic(context);
                break;

            case PocketType.Weapon:
                ActivateWeapon(context);
                break;

            case PocketType.Consumable:
                ActivateConsumable(context);
                break;

            case PocketType.Currency:
                break;
        }
    }

    private void ActivateRelic(ItemContext context)
    {
        if (context.playerStats == null)
        {
            Debug.LogWarning(
                $"{itemName} could not find PlayerStats.",
                this
            );

            return;
        }

        context.playerStats.AddModifiers(
            this,
            statModifiers
        );

        Debug.Log(
            $"Equipped relic: {itemName}",
            this
        );
    }

    private void ActivateWeapon(ItemContext context)
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning(
                $"{itemName} does not have a weapon prefab.",
                this
            );

            return;
        }

        GameObject newWeapon = Instantiate(
            weaponPrefab,
            context.player.transform
        );

        newWeapon.transform.localPosition =
            Vector3.zero;

        Debug.Log(
            $"Equipped weapon: {itemName}",
            this
        );
    }

    private void ActivateConsumable(ItemContext context)
    {
        if (context.playerHealth == null)
        {
            Debug.LogWarning(
                $"{itemName} could not find PlayerHealth.",
                this
            );

            return;
        }

        switch (consumableEffect)
        {
            case ConsumableEffectType.Heal:
                context.playerHealth.Heal(
                    consumableValue
                );
                break;

            case ConsumableEffectType.None:
                Debug.LogWarning(
                    $"{itemName} does not have an effect.",
                    this
                );
                break;

            default:
                Debug.LogWarning(
                    $"{consumableEffect} is not implemented yet.",
                    this
                );
                break;
        }
    }

}