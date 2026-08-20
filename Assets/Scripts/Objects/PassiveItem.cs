using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewPassiveItem",
    menuName = "Inventory/Passive Item"
)]
public class PassiveItem : ItemData
{
    [Header("Passive Settings")]
    public List<StatModifier> statModifiers =
        new List<StatModifier>();
    public int maxApplications = 1; // Maximum number of times the passive effect can be applied

    public override Inventory.PocketType pocketType => Inventory.PocketType.Passive;

    int appliedCount = 0; // Track how many times the passive effect has been applied

    public override void Activate(ItemContext context)
    {
        if (appliedCount >= maxApplications)
            return;
        appliedCount++;
        Debug.Log(
            $"Activated Passive item: {itemName}",
            this
        );
        context.playerStats.AddModifiers(this, statModifiers);
    }
}