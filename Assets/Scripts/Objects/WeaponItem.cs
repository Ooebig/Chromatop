using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(
    fileName = "NewWeaponItem",
    menuName = "Inventory/Weapon Item"
)]
public class WeaponItem : ItemData
{
    [Header("Weapon Settings")]
    [Tooltip("Only used by Weapon items.")]
    public GameObject weaponPrefab;

    public override Inventory.PocketType pocketType => Inventory.PocketType.Weapon;

    public override void Activate(ItemContext context)
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning(
                $"{itemName} does not have a weapon prefab.",
                this
            );

            return;
        }

        GameObject newWeapon = Instantiate(
            weaponPrefab,
            context.player.transform
        );

        newWeapon.transform.localPosition =
            Vector3.zero;

        Debug.Log(
            $"Equipped weapon: {itemName}",
            this
        );
    }
}