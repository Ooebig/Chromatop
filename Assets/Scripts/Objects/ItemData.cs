using System.Collections.Generic;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    [Header("Basic Information")]
    
    public string itemName;
    [Tooltip("For ItemID:\nNonActive: 0000+\nWeapon: 1000+\nPassive: 2000+\nOnHit: 3000+\nOnKill: 4000+\nOnDamage: 5000+\nOnHeal: 6000+\nOnPickup: 7000+\nTime: 8000+\nOnStart: 9000+")]
    public int itemID = 0;
    

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Inventory Pocket")]
    public abstract Inventory.PocketType pocketType { get; }

    [Header("Set")]
    public Inventory.ItemSet itemSet = Inventory.ItemSet.Generic;

    //[Header("Weapon Settings")]
    //[Tooltip("Only used by Weapon items.")]
    //public GameObject weaponPrefab;

    //[Header("Passive Settings")]
    //public PassiveEffectType passiveEffect = PassiveEffectType.None;
    //public float passiveValue = 0f;

    //MORE TO BE ADDED IN BETA

    //[Header("Relic Settings")]
    //[Tooltip("Only used by Relic items.")]
    //public List<StatModifier> statModifiers =
    //    new List<StatModifier>();

    //[Header("Consumable Settings")]
    //public ConsumableEffectType consumableEffect;
    //public float consumableValue;

    //[Min(0f)]
    //public float consumableDuration;

    //[Header("Currency Settings")]
    //[Min(0)]
    //public int currencyValue;

    public virtual void Activate(ItemContext context)
    {
        Debug.Log(
            $"Activated NonActive item: {itemName}",
            this
        );
    }

    //private void ActivateRelic(ItemContext context)
    //{
    //    if (context.playerStats == null)
    //    {
    //        Debug.LogWarning(
    //            $"{itemName} could not find PlayerStats.",
    //            this
    //        );

    //        return;
    //    }

    //    context.playerStats.AddModifiers(
    //        this,
    //        statModifiers
    //    );

    //    Debug.Log(
    //        $"Equipped relic: {itemName}",
    //        this
    //    );
    //}

    //private void ActivateWeapon(ItemContext context)
    //{
    //    if (weaponPrefab == null)
    //    {
    //        Debug.LogWarning(
    //            $"{itemName} does not have a weapon prefab.",
    //            this
    //        );

    //        return;
    //    }

    //    GameObject newWeapon = Instantiate(
    //        weaponPrefab,
    //        context.player.transform
    //    );

    //    newWeapon.transform.localPosition =
    //        Vector3.zero;

    //    Debug.Log(
    //        $"Equipped weapon: {itemName}",
    //        this
    //    );
    //}

    //private void ActivateConsumable(ItemContext context)
    //{
    //    if (context.playerHealth == null)
    //    {
    //        Debug.LogWarning(
    //            $"{itemName} could not find PlayerHealth.",
    //            this
    //        );

    //        return;
    //    }

    //    switch (consumableEffect)
    //    {
    //        case ConsumableEffectType.Heal:
    //            context.playerHealth.Heal(
    //                consumableValue
    //            );
    //            break;

    //        case ConsumableEffectType.None:
    //            Debug.LogWarning(
    //                $"{itemName} does not have an effect.",
    //                this
    //            );
    //            break;

    //        default:
    //            Debug.LogWarning(
    //                $"{consumableEffect} is not implemented yet.",
    //                this
    //            );
    //            break;
    //    }
    //}

}
