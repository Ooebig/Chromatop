using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "NewNonActiveItem",
    menuName = "Inventory/Non-Active Item"
)]
public class NonActiveItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.NonActive;

    public override void Activate(ItemContext context, GameObject gameObject)
    {
        Debug.Log(
            $"Activated NonActive item: {itemName}",
            this
        );
    }
}