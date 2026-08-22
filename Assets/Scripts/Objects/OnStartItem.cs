using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewOnStartItem",
    menuName = "Inventory/OnStart Item"
)]
public class OnStartItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.OnStart;

    public override void Activate(ItemContext context) //When the player starts the game, do this
    {
        Debug.Log(
            $"Activated OnStart item: {itemName}",
            this
        );
    }
}