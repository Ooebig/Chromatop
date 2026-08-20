using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnKillItem",
    menuName = "Inventory/OnKill Item"
)]
public class OnKillItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnKill;

    public override void Activate(ItemContext context) //When the player kills an enemy, do this
    {
        Debug.Log(
            $"Activated OnKill item: {itemName}",
            this
        );
    }
}