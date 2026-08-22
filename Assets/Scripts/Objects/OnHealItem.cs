using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnHealItem",
    menuName = "Inventory/OnHeal Item"
)]
public class OnHealItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnHeal;

    public override void Activate(ItemContext context) //When the player heals, do this
    {
        Debug.Log(
            $"Activated OnHeal item: {itemName}",
            this
        );
    }
}
