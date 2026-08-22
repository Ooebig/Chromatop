using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnPickupItem",
    menuName = "Inventory/OnPickup Item"
)]
public class OnPickupItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnPickup;

    public override void Activate(ItemContext context) //When the player picks up an item (currency), do this
    {
        Debug.Log(
            $"Activated OnPickup item: {itemName}",
            this
        );
    }
}
