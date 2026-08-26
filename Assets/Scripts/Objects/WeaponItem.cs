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
        if (weaponPrefab == null ||
            context == null ||
            context.player == null)
        {
            return;
        }

        EquippedWeaponMarker[] equippedWeapons =
            context.player.GetComponentsInChildren
                <EquippedWeaponMarker>(true);

        foreach (EquippedWeaponMarker equipped in equippedWeapons)
        {
            if (equipped.SourceItem == this)
            {
                Debug.Log(
                    $"{itemName} is already equipped.",
                    context.player
                );

                return;
            }
        }

        GameObject newWeapon = Instantiate(
            weaponPrefab,
            context.player.transform
        );

        newWeapon.transform.localPosition =
            Vector3.zero;

        EquippedWeaponMarker marker =
            newWeapon.GetComponent<EquippedWeaponMarker>();

        if (marker == null)
        {
            marker =
                newWeapon.AddComponent<EquippedWeaponMarker>();
        }

        marker.Initialize(this);

        Debug.Log(
            $"Equipped weapon: {itemName}",
            newWeapon
        );
    }
}