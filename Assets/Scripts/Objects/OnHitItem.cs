using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnHitItem",
    menuName = "Inventory/OnHit Item"
)]
public class OnHitItem : ItemData
{
    [Header("OnHit Settings")]
    
    public int healAmount = 0;
    public enum healEffectType
    {
        Value,
        Percent
    }
    public healEffectType healType = healEffectType.Value;

    public List<StatModifier> statModifiers =
        new List<StatModifier>();

    public enum debuffType
    {
        DOT,
        Slow,
        Weakness,
        Stun
    }
    public List<debuffType> debuffs =
        new List<debuffType>();
    public int maxApplications = -1;

    public override Inventory.PocketType pocketType => Inventory.PocketType.OnHit;

    int appliedCount = 0;

    public override void Activate(ItemContext context, GameObject gameObject) //When the player hits an enemy, do this
    {
        if (maxApplications == -1 || appliedCount >= maxApplications)
            return;
        Debug.Log(
            $"Activated OnHit item: {itemName}",
            this
        );

        if (healAmount > 0)
        {
            if (healType == healEffectType.Value)
            {
                context.playerHealth.Heal(healAmount);
            }
            else
            {
                int healValue = Mathf.RoundToInt(context.playerHealth.MaxHealth * (healAmount / 100f));
                context.playerHealth.Heal(healValue);
            }
        }

        if (statModifiers.Count > 0)
        {
            //temp modifiers not implemented
        }

        if (debuffs.Count > 0)
        {
            //debuffs not implemented
        }

    }
}