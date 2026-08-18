using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IPickup
{
    [ContextMenu("Refresh Equipped Items")]
    public void RefreshEquippedItems()
    {
        ActivatePocket(PocketType.Relic);
        ActivatePocket(PocketType.Weapon);
    }

    [Header("Relic Pocket")]
    [SerializeField]
    private List<ItemData> relics =
        new List<ItemData>();

    [Header("Weapon Pocket")]
    [SerializeField]
    private List<ItemData> weapons =
        new List<ItemData>();

    [Header("Consumable Pocket")]
    [SerializeField]
    private List<ItemData> consumables =
        new List<ItemData>();

    [Header("Currency Pocket")]
    [SerializeField]
    private List<ItemData> currencies =
        new List<ItemData>();

    [SerializeField] private int currentCurrency;

    private readonly HashSet<ItemData> activeItems =
        new HashSet<ItemData>();

    private PlayerStats playerStats;
    private PlayerHealth playerHealth;

    public int CurrentCurrency => currentCurrency;

    private void Awake()
    {
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
        // Activates items placed in the Inspector for testing.
        ActivatePocket(PocketType.Relic);
        ActivatePocket(PocketType.Weapon);
    }

    public void ActivatePocket(PocketType pocket)
    {
        switch (pocket)
        {
            case PocketType.Relic:
                ActivateItems(relics);
                break;

            case PocketType.Weapon:
                ActivateItems(weapons);
                break;

            case PocketType.Consumable:
                Debug.LogWarning(
                    "Consumables should be activated individually.",
                    this
                );
                break;

            case PocketType.Currency:
                // Currency stores an amount and is not activated.
                break;
        }
    }

    private void ActivateItems(
        List<ItemData> pocket
    )
    {
        ItemContext context = CreateContext();

        foreach (ItemData item in pocket)
        {
            if (item == null)
                continue;

            // Prevents relic bonuses and weapons from being added twice.
            if (!activeItems.Add(item))
                continue;

            item.Activate(context);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;

        switch (item.pocketType)
        {
            case PocketType.Relic:
                AddRelic(item);
                break;

            case PocketType.Weapon:
                AddWeapon(item);
                break;

            case PocketType.Consumable:
                consumables.Add(item);
                break;

            case PocketType.Currency:
                if (!currencies.Contains(item))
                {
                    currencies.Add(item);
                }

                AddCurrency(item.currencyValue);
                break;
        }
    }

    private void AddRelic(ItemData relic)
    {
        if (relics.Contains(relic))
            return;

        relics.Add(relic);
        ActivateItem(relic);
    }

    private void AddWeapon(ItemData weapon)
    {
        if (weapons.Contains(weapon))
            return;

        weapons.Add(weapon);
        ActivateItem(weapon);
    }

    private void ActivateItem(ItemData item)
    {
        if (!activeItems.Add(item))
            return;

        item.Activate(CreateContext());
    }

    public void UseConsumable(ItemData consumable)
    {
        if (consumable == null)
            return;

        if (!consumables.Contains(consumable))
            return;

        consumable.Activate(CreateContext());

        consumables.Remove(consumable);
    }

    public void RemoveItem(ItemData item)
    {
        if (item == null)
            return;

        switch (item.pocketType)
        {
            case PocketType.Relic:
                if (relics.Remove(item))
                {
                    playerStats?.RemoveModifiers(item);
                    activeItems.Remove(item);
                }
                break;

            case PocketType.Weapon:
                weapons.Remove(item);
                activeItems.Remove(item);

                // Destroying its spawned weapon will be added
                // when unequipping weapons is implemented.
                break;

            case PocketType.Consumable:
                consumables.Remove(item);
                break;

            case PocketType.Currency:
                currencies.Remove(item);
                break;
        }
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0)
            return;

        currentCurrency += amount;

        Debug.Log(
            $"Currency added: {amount}. Total: {currentCurrency}",
            this
        );
    }

    private ItemContext CreateContext()
    {
        return new ItemContext
        {
            player = gameObject,
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