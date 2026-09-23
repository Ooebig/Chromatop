using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnKillItem",
    menuName = "Inventory/OnKill Item"
)]
public class OnKillItem : ItemData
{
    [Header("OnKill Settings")]
    public int healAmount = 0;
    public enum healEffectType
    {
        Value,
        Percent
    }
    public healEffectType healType = healEffectType.Value;

    public List<StatModifier> statModifiers =
        new List<StatModifier>();

    public int maxApplications = -1;

    public override Inventory.PocketType pocketType => Inventory.PocketType.OnKill;

    int appliedCount = 0;

    public override void Activate(ItemContext context, GameObject gameObject)
    {
        if (maxApplications == -1 || appliedCount >= maxApplications)
            return;
        Debug.Log(
            $"Activated OnKill item: {itemName}",
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

    }
}