using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [ContextMenu("Refresh Equipped Items")]
    public void RefreshEquippedItems()
    {
        ActivatePocket(PocketType.Passive);
        ActivatePocket(PocketType.Weapon);
    }

    public enum PocketType
    { //comments to avoid confusion
        NonActive = 0, //items that do not have an activation, or are otherwise non-functional or unique
        Weapon = 1, //Regular firing weapons
        Passive = 2, //effects always active
        OnHit = 3, //on hitting an enemy
        OnKill = 4, //on killing an enemy
        OnDamage = 5, //on taking damage
        OnHeal = 6, //on getting healed
        OnPickup = 7, //on picking smth up, usually the currency drop
        Time = 8, //happens every x seconds, or at certain time thresholds
        OnStart = 9 //happens at the beginning of the level
    }

    //public enum ConsumableEffectType
    //{
    //    None,
    //    Heal,
    //    TemporaryDamage,
    //    TemporarySpeed,
    //    TemporaryDefense
    //}

    public enum ItemSet
    {
        Generic,
        Repeatable,
        Cursed,
        TEMP_A,
        TEMP_B,
        TEMP_C
    }


    [Header("Weapon Pocket")]
    [SerializeField]
    public List<ItemData> weaponPocket =
        new List<ItemData>();

    [Header("Passive Pocket")]
    [SerializeField]
    public List<ItemData> passivePocket =
        new List<ItemData>();

    [Header("onHit Pocket")]
    [SerializeField]
    public List<ItemData> onHitPocket =
        new List<ItemData>();

    [Header("onKill Pocket")]
    [SerializeField]
    public List<ItemData> onKillPocket =
        new List<ItemData>();

    [Header("onDamage Pocket")]
    [SerializeField]
    public List<ItemData> onDamagePocket =
        new List<ItemData>();

    [Header("onHeal Pocket")]
    [SerializeField]
    public List<ItemData> onHealPocket =
        new List<ItemData>();

    [Header("onPickup Pocket")]
    [SerializeField]
    public List<ItemData> onPickupPocket =
        new List<ItemData>();

    [Header("Time Pocket")]
    [SerializeField]
    public List<ItemData> timePocket =
        new List<ItemData>();

    [Header("OnStart Pocket")]
    [SerializeField]
    public List<ItemData> onStartPocket =
        new List<ItemData>();

    [Header("NonActive Pocket")]
    [SerializeField]
    public List<ItemData> nonActive =
        new List<ItemData>();



    public List<List<ItemData>> allPockets = new List<List<ItemData>>(10);
    public List<ItemData> allItems = new List<ItemData>();

    [SerializeField] public int currentCurrency;

    private PlayerStats playerStats;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        allPockets = new List<List<ItemData>>(10)
        {
            nonActive,
            weaponPocket,
            passivePocket,
            onHitPocket,
            onKillPocket,
            onDamagePocket,
            onHealPocket,
            onPickupPocket,
            timePocket,
            onStartPocket
        };
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }

        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                GetComponentInParent<PlayerHealth>();
        }
    }

    private void Start()
    {
        //// Activates items placed in the Inspector for testing.
        ActivatePocket(PocketType.Passive);
        ActivatePocket(PocketType.Weapon);
    }

    public void ActivatePocket(PocketType pocket)
    {
        ItemContext context = CreateContext();

        foreach (ItemData item in allPockets[(int)pocket])
        {
            if (item == null)
                continue;

            item.Activate(context);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;

        if (item.itemSet != ItemSet.Repeatable && allItems.Contains(item))
            return;
        
        allItems.Add(item);

        allPockets[(int)item.pocketType].Add(item);

        if (item.pocketType == PocketType.Weapon ||
            item.pocketType == PocketType.Passive)
        {
            item.Activate(CreateContext());
        }

    }

    //individual item activation is not needed
    //private void ActivateItem(ItemData item)
    //{
    //    if (!activeItems.Add(item))
    //        return;

    //    item.Activate(CreateContext());
    //} 

    //consumables are auto used on pickup, and are not necessary rn. Currently commenting out extra code to avoid confusion
    //public void UseConsumable(ItemData consumable)
    //{
    //    if (consumable == null)
    //        return;

    //    if (!consumables.Contains(consumable))
    //        return;

    //    consumable.Activate(CreateContext());

    //    consumables.Remove(consumable);
    //}

    public void RemoveItem(ItemData item)
    {
        if (item == null)
            return;

        allItems.Remove(item);

        allPockets[(int)item.pocketType].Remove(item);
    }

    public void AddCurrency(int amount)
    {

        currentCurrency += amount;

        //Debug.Log(
        //    $"Currency added: {amount}. Total: {currentCurrency}",
        //    this
        //);
    }

    private ItemContext CreateContext()
    {
        GameObject playerObject =
            playerStats != null
                ? playerStats.gameObject
                : gameObject;

        return new ItemContext
        {
            player = playerObject,
            playerStats = playerStats,
            playerHealth = playerHealth
        };
    }
}

public class ItemContext
{
    public GameObject player;
    public PlayerStats playerStats;
    public PlayerHealth playerHealth;
}