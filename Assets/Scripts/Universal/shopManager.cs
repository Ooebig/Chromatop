using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class shopManager : MonoBehaviour
{
    public static shopManager instance;

    [System.Serializable] public class ShopSlot  //using system.serializable to make the class visible in the inspector
    {
        public GameObject root;
        public TMP_Text nameText; //using TMP_Text instead of Text for better text rendering
        public TMP_Text priceText;
        public TMP_Text descText;
        public Button purchaseButton;
        public Image icon;

        [SerializeField] public ItemData item; //using ItemData for better data management
        [SerializeField] public int price;
        [SerializeField] public bool isSold;

    }

    [Header("UI")]
    public TMP_Text currencyText;
    public TMP_Text titleText;
    public TMP_Text statusText;
    public Button rerollButton;
    public TMP_Text rerollButtonText;
    public ShopSlot[] shopSlots = new ShopSlot[3]; //initializing an array of ShopSlot with a size of 3

    [Header("Pricing")]
    public int basePrice = 20;
    public int pricePerRoom = 6;
    public int rerollPrice = 15;
    public int rerollIncrease = 10;

    int rerollCount;
    int currRerollPrice;

    void Awake()
    {
        instance = this;
    }

    public void OpenShop()
    {
        rerollCount = 0;
        if (titleText != null)
        {
            titleText.text = "Shop - Room " + gameManager.instance.currentRound;
        }

        RollStock(true);
        RefreshUI();
    }

    public void RollStock(bool replaceSoldItems) // RollStock is called when the player chooses to reroll the items in the shop. 
    {
        ItemData[] pool = gameManager.instance.itemCatalogue != null ? // Check if itemCatalogue is not null before accessing regularItemDrops
            gameManager.instance.itemCatalogue.regularItemDrops : null;

        if (pool == null || pool.Length == 0)
        {
            Debug.LogWarning("Shop is empty.");
            for (int i = 0; i < shopSlots.Length; i++)
            {
                ClearSlot(i);
            }
            return;
        }

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (!replaceSoldItems && shopSlots[i].isSold)
            {
                continue; // Skip sold items if replaceSoldItems is false
            }

            ItemData pick = null;
            for (int j = 0; j < 20; j++) // Try to pick a valid item from the pool
            {
                ItemData option = pool[Random.Range(0, pool.Length)];
                if (option == null) continue;
                if (StockContains(option, i)) continue; // Skip if the item is already in stock or if the item is already in the current slot

                pick = option;
                break;
            }

            if (pick == null)
            {
                ClearSlot(i);
                continue;
            }

            //setting the shopSlots[i] UI elements to display the item information

            shopSlots[i].item = pick;
            shopSlots[i].price = GetPrice(pick);
            shopSlots[i].isSold = false;

        }
    }
    bool StockContains(ItemData item, int excludeIndex) //StockContains checks if the shop already contains the item, excluding the current slot being filled
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i == excludeIndex) continue;
            if (!shopSlots[i].isSold && shopSlots[i].item == item)
            {
                return true;
            }
        }

        return false;
    }

    int GetPrice(ItemData item) // GetPrice calculates the price of the item based on the current round and the base price and price per room
    {
        int room = Mathf.Max(1, gameManager.instance.currentRound);
        return basePrice + room * pricePerRoom;
    }

    public void Buy(int slotIndex) // Buy is called when the player clicks the purchase button for an item in the shop
    {
        if (slotIndex < 0 || slotIndex >= shopSlots.Length)
        {
            return;
        }

        ShopSlot slot = shopSlots[slotIndex];

        if (slot.item == null || slot.isSold)
        {
            return;
        }

        Inventory inv = gameManager.instance.inventory;

        if (inv == null)
        { 
            return;
        }

        if (inv.currentCurrency < slot.price)
        {
            FlashStatus("Not enough currency to buy item: " + slot.item.name);
            audioManager.instance.PlayBackSound();
            return;
        }

        inv.currentCurrency -= slot.price;
        inv.AddItem(slot.item);
        inv.ActivatePocket(Inventory.PocketType.Passive); 
        inv.ActivatePocket(Inventory.PocketType.Weapon);

        slot.isSold = true;
        audioManager.instance.PlayConfirmSound();
        FlashStatus("Purchased item: " + slot.item.name);
        RefreshUI();
    }

    public void Reroll() // Reroll is called when the player clicks the reroll button in the shop
    {
        Inventory inv = gameManager.instance.inventory;
        currRerollPrice = rerollPrice + rerollCount * rerollIncrease;

        if (inv.currentCurrency < currRerollPrice)
        {
            FlashStatus("Not enough currency to reroll.");
            audioManager.instance.PlayBackSound();
            return;
        }

        inv.currentCurrency -= currRerollPrice;
        rerollCount++;
        audioManager.instance.PlayConfirmSound();
        RollStock(true);
        FlashStatus("Rerolled shop items for " + currRerollPrice + " currency.");
        RefreshUI();
    }

    public void Leave() // Leave is called when the player clicks the leave button in the shop
    {
        audioManager.instance.PlayConfirmSound();
        gameManager.instance.CompletedSpecialRoom();
    }

    void ClearSlot(int index) // ClearSlot clears the item in the shop slot and updates the UI
    {
        shopSlots[index].item = null;
        shopSlots[index].price = 0;
        shopSlots[index].isSold = true;
    }

    void RefreshUI() // RefreshUI updates the UI elements in the shop to reflect the current state of the shop slots and the player's currency
    {

        int money = gameManager.instance.inventory != null ? // setting money to the player's current currency if the inventory is not null, otherwise setting it to 0
        gameManager.instance.inventory.currentCurrency : 0;

        if (currencyText != null)
        {
            currencyText.text = "Currency: " + money;
        }

        gameManager.instance.UpdateCurrencyUI();

        currRerollPrice = rerollPrice + rerollCount * rerollIncrease;

        if (rerollButtonText != null)
        {
            rerollButtonText.text = "Reroll  (" + currRerollPrice + ")";
        }
        if (rerollButton != null)
        {
            rerollButton.interactable = money >= currRerollPrice;
        }

        for (int i = 0; i < shopSlots.Length; i++)
        {
            ShopSlot slot = shopSlots[i];
            if (slot == null) continue;

            bool hasItem = slot.item != null;

            if (slot.root != null)
            {
                slot.root.SetActive(true);
            }

            if (!hasItem) // if there is no item in the slot, set the UI elements to show that the slot is empty and disable the purchase button
            {
                if (slot.nameText != null) slot.nameText.text = "Empty";
                if (slot.priceText != null) slot.priceText.text = "";
                if (slot.descText != null) slot.descText.text = "";
                if (slot.purchaseButton != null) slot.purchaseButton.interactable = false;
                continue;
            }

            if (slot.nameText != null)
            {
                slot.nameText.text = slot.item.itemName;
            }
            if (slot.descText != null)
            {
                slot.descText.text = "";
            }
            if (slot.priceText != null)
            {
                slot.priceText.text = slot.isSold ? "SOLD" : slot.price + "g";
            }
            if (slot.purchaseButton != null)
            {
                slot.purchaseButton.interactable = !slot.isSold && money >= slot.price;
            }
        }

    }
    
    void FlashStatus (string message) // FlashStatus displays a temporary message in the statusText UI element
    {
        if (statusText != null)
        {
            statusText.text = message;
           Debug.Log("Shop Status: " + message);
        }
    }   
   
}
