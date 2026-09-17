using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurseManager : MonoBehaviour
{
    public static CurseManager instance;

    [System.Serializable]
    public class CurseSlot
    {
        public GameObject root;
        public TMP_Text nameText;
        public TMP_Text priceText;
        public TMP_Text descText;
        public TMP_Text curseText;
        public Button takeButton;
        [SerializeField] public ItemData item;
        [SerializeField] public int price;
        [SerializeField] public bool isTaken;
    }

    [Header("UI")]
    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text currencyText;
    public TMP_Text statusText;
    public Button leaveButton;
    public CurseSlot[] slots = new CurseSlot[2];

    [Header("Pricing")]
    [Range(1, 2)] public int offers = 2;
    public bool optional = true;
    public int basePrice = 35;
    public int pricePerRoom = 8;

    void Awake()
    {
        instance = this;
    }

    public void OpenCurseRoom() // OpenCurseRoom is called when the player enters a curse room.
    {
        if (titleText != null)
        {
            titleText.text = "Curse Room - Room " + gameManager.instance.currentRound;
        }
        RollOffers();
        FlashStatus("Choose an item to take the buff and a curse.");
        RefreshUI();
    }

    void RollOffers() // RollOffers is called when the player enters a curse room to generate the items available for selection.
    {
        ItemData[] pool = gameManager.instance.itemCatalogue != null ?
        gameManager.instance.itemCatalogue.cursedItemDrops : null;

        int count = Mathf.Clamp(offers, 1, slots.Length);

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots != null)
            {
                continue;
            }

            slots[i].item = null;
            slots[i].price = 0;
            slots[i].isTaken = true;
        }

        if (pool == null || pool.Length == 0)
        {
            FlashStatus("Add cursedItemDrops.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            ItemData pick = null;
            for (int t = 0; t < 16; t++)
            {
                ItemData candidate = pool[Random.Range(0, pool.Length)];
                if (candidate == null)
                {
                    continue;
                }

                bool used = false;

                for (int s = 0; s < i; s++)
                {
                    if (slots[s] != null && slots[s].item == candidate)
                    {
                        used = true;
                    }
                }
                if (used)
                {
                    continue;
                }

                pick = candidate;
                break;
            }

            slots[i].item = pick;
            slots[i].price = pick != null ? GetPrice(pick) : 0;
            slots[i].isTaken = pick == null;

        }
    }

    int GetPrice(ItemData item) // GetPrice calculates the price of a given item based on the current round and the base price.
    {
        int room = Mathf.Max(1, gameManager.instance.currentRound);
        return basePrice + room * pricePerRoom;
    }

    public void Take(int index) // Take is called when the player clicks the take button for an item in the curse room.
    {
        if (index < 0 || index >= slots.Length) return;

        CurseSlot slot = slots[index];
        if (slot.item == null || slot.isTaken) return;

        Inventory inv = gameManager.instance.inventory;
        if (inv == null) return;

        if (inv.currentCurrency < slot.price)
        {
            FlashStatus("Not enough currency.");
            audioManager.instance.PlayBackSound();
            return;
        }

        inv.currentCurrency -= slot.price; 
        inv.AddItem(slot.item);
        inv.ActivatePocket(Inventory.PocketType.Passive);
        inv.ActivatePocket(Inventory.PocketType.Weapon);

        slot.isTaken = true;
        audioManager.instance.PlayConfirmSound();
        FlashStatus("Bound: " + slot.item.itemName);
        RefreshUI();
    }

    public void Leave() // Leave is called when the player clicks the leave button in the curse room.
    {
        if (!optional)
        {
            bool tookAny = false;
            for (int i = 0; i < slots.Length; i++)
                if (slots[i] != null && slots[i].item != null && slots[i].isTaken)
                    tookAny = true;

            if (!tookAny)
            {
                FlashStatus("You cannot leave emptyhanded.");
                audioManager.instance.PlayBackSound();
                return;
            }
        }

        audioManager.instance.PlayConfirmSound();
        gameManager.instance.CompletedSpecialRoom();
    }

    void RefreshUI() 
    {
        int money = gameManager.instance.inventory != null
            ? gameManager.instance.inventory.currentCurrency
            : 0;

        if (currencyText != null)
            currencyText.text = "Money: " + money;

        gameManager.instance.UpdateCurrencyUI();

        if (leaveButton != null)
            leaveButton.interactable = true;

        for (int i = 0; i < slots.Length; i++)
        {
            CurseSlot slot = slots[i];
            if (slot == null) continue;

            bool show = slot.item != null;
            if (slot.root != null) slot.root.SetActive(show);
            if (!show) continue;

            if (slot.nameText != null) slot.nameText.text = slot.item.itemName;
            if (slot.descText != null) slot.descText.text = slot.item.description;
            if (slot.curseText != null)
                slot.curseText.text = string.IsNullOrEmpty(slot.item.curseLine)
                    ? "Cursed."
                    : slot.item.curseLine;

            if (slot.isTaken)
            {
                if (slot.priceText != null) slot.priceText.text = "BOUND";
                if (slot.takeButton != null) slot.takeButton.interactable = false;
            }
            else
            {
                if (slot.priceText != null) slot.priceText.text = slot.price + "g";
                if (slot.takeButton != null)
                    slot.takeButton.interactable = money >= slot.price;
            }
        }
    }

    void FlashStatus(string message) // FlashStatus displays a temporary message in the statusText UI element
    {
        if (statusText != null)
        {
            statusText.text = message;
            Debug.Log("Curse Status: " + message);
        }
    }
    
}