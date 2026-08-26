using UnityEngine;

public class EquippedWeaponMarker : MonoBehaviour
{
    public WeaponItem SourceItem { get; private set; }

    public void Initialize(WeaponItem sourceItem)
    {
        SourceItem = sourceItem;
    }
}