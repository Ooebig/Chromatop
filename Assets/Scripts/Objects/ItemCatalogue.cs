using UnityEngine;

public class ItemCatalogue : MonoBehaviour
{
    [Header("Level-complete draft")]
    public ItemData[] regularItemDrops;

    [Header("Shop only")]
    public ItemData[] shopItemDrops;

    [Header("Curse room only")]
    public ItemData[] cursedItemDrops;
}