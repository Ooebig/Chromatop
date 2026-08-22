using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewTimeItem",
    menuName = "Inventory/Time Item"
)]
public class TimeItem : ItemData
{
    public override Inventory.PocketType pocketType => Inventory.PocketType.Time;

    public override void Activate(ItemContext context) //When the clock hits a certain time or on set intervals, do this
    {
        Debug.Log(
            $"Activated Time item: {itemName}",
            this
        );
    }

}
