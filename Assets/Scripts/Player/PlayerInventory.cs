using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> items =
        new List<ItemData>();

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }
    }

    public void ActivatePocket(PocketType pocket)
    {
        ItemContext context = new ItemContext
        {
            player = gameObject,
            playerStats = playerStats
        };

        foreach (ItemData item in items)
        {
            if (item == null)
                continue;

            if (item.pocketType != pocket)
                continue;

            item.Activate(context);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;

        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        if (item == null)
            return;

        items.Remove(item);
    }
}

public class ItemContext
{
    public GameObject player;
    public PlayerStats playerStats;
}