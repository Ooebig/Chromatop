using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnDamageItem",
    menuName = "Inventory/OnDamage Item"
)]
public class OnDamageItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnDamage;

    public override void Activate(ItemContext context) //When the player takes damage, do this
    {
        Debug.Log(
            $"Activated OnDamage item: {itemName}",
            this
        );
    }
}