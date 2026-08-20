using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnHitItem",
    menuName = "Inventory/OnHit Item"
)]
public class OnHitItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnHit;

    public override void Activate(ItemContext context) //When the player hits an enemy, do this
    {
        Debug.Log(
            $"Activated OnHit item: {itemName}",
            this
        );
    }
}