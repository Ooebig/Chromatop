using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GamblingManager : MonoBehaviour
{
    public static GamblingManager instance;

    static readonly gameManager.ColorType[] BetColors =  //using readonly to ensure that the array cannot be modified after initialization
    {
        gameManager.ColorType.RED,
        gameManager.ColorType.ORANGE,
        gameManager.ColorType.YELLOW,
        gameManager.ColorType.GREEN,
        gameManager.ColorType.BLUE,
        gameManager.ColorType.PURPLE
    };

    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text currencyText;
    public TMP_Text stakeText;
    public TMP_Text statusText;
    public TMP_Text resultText;
    public Button spinButton;
    public Button leaveButton;
    public Button[] colorButtons = new Button[6];
    public Image[] colorButtonImages = new Image[6];
    public Image spinResultImage;

    [Header("Stakes")]
    public int minStake = 10;
    public int stakeStep = 10; // The amount by which the stake increases or decreases
    public int exactPayoutMultiplier = 5; // The multiplier for an exact match
    public int adjacentPayoutMultiplier = 2; // The multiplier for an adjacent match

    int stake;
    int selectedColorIndex = -1; // -1 means no color selected
    bool isSpinning;

    void Awake()
    {
        instance = this;
    }

    public void OpenGambling() // Method to initialize the gambling interface when the player enters the gambling room
    {
        isSpinning = false;
        selectedColorIndex = -1;
        stake = minStake;

        if (titleText != null)
        {
            titleText.text = "GAMBLE  —  Room " + gameManager.instance.currentRound;
        }
        if (resultText != null)
        {
            resultText.text = "";
        }

        PaintColorButtons();
        HighlightSelected();
        RefreshUI();
        FlashStatus("Pick a color, enter your stake, then spin the wheel!");
    }

    public void AdjustStake(int direction) // Method to adjust the stake based on the direction parameter (1 for increase, -1 for decrease)
    {
        if (isSpinning) return;

        int money = Money();
        int cap = Mathf.Min(minStake, money);

        stake += direction * stakeStep;
        if (stake < minStake)
        {
            stake = minStake;
        }
        if (stake > money)
        {
            stake = money;
        }
        audioManager.instance.PlayConfirmSound();
        RefreshUI();
    }

    public void PresetStake(int amount) // Method to set the stake to a specific amount, ensuring it is within valid bounds
    {
        if (isSpinning)
        {
            return;
        }

        int money = Money();

        if (amount <= 0)
        {
            stake = Mathf.Max(minStake, money);
        }
        else
        {
            stake = Mathf.Clamp(amount, minStake, Mathf.Max(minStake, money));
        }

        audioManager.instance.PlayConfirmSound();
        RefreshUI();

    }

    public void SelectColor(int index) // Method to select a color for the bet based on the index parameter
    {
        if (isSpinning)
        {
            return;
        }

        if (index < 0 || index >= BetColors.Length)
        {
            return;
        }

        selectedColorIndex = index;
        audioManager.instance.PlayConfirmSound();
        HighlightSelected();
        RefreshUI();

    }

    public void Spin() // Method to initiate the spinning of the wheel, checking for valid conditions before starting the spin
    {

        if (isSpinning)
        {
            return;
        }

        if (selectedColorIndex < 0)
        {
            FlashStatus("Pick a color first.");
            audioManager.instance.PlayBackSound();
            return;
        }

        int money = Money();
        if (money < minStake)
        {
            FlashStatus("You're broke. Leave.");
            audioManager.instance.PlayBackSound();
            return;
        }

        if (stake < minStake || stake > money)
        {
            FlashStatus("Invalid stake.");
            audioManager.instance.PlayBackSound();
            return;
        }

        StartCoroutine(SpinRoutine());

    }

    public void Leave() // Method to handle leaving the gambling room, returning to the previous game state
    {
        if (isSpinning)
        {
            return;
        }
        audioManager.instance.PlayConfirmSound();
        gameManager.instance.CompletedSpecialRoom();
    }

    IEnumerator SpinRoutine() // Coroutine to handle the spinning process, including determining the result and updating the UI
    {
        isSpinning = true;
        RefreshUI();

        Inventory inv = gameManager.instance.inventory;
        inv.currentCurrency -= stake;
        UpdateMoneyHUD();

        FlashStatus("Spinning..."); ////HANDLES THE SPINNING ANIMATION AND RESULT DETERMINATION
        if (resultText != null) resultText.text = "...";

        int flicker = BetColors.Length * 2;
        for (int i = 0; i < flicker; i++)
        {
            int shown = Random.Range(0, BetColors.Length);
            ShowLanded(shown);
            yield return new WaitForSecondsRealtime(0.06f);
        }

        int landed = Random.Range(0, BetColors.Length);
        ShowLanded(landed);

        gameManager.ColorType bet = BetColors[selectedColorIndex];
        gameManager.ColorType hit = BetColors[landed];
        int payout = 0;
        string outcome;

        if (landed == selectedColorIndex) ////HANDLES THE OUTCOME OF THE SPIN AND CALCULATES PAYOUTS
        {
            payout = stake * exactPayoutMultiplier;
            outcome = "EXACT HIT  " + hit + "  x" + exactPayoutMultiplier;

            if (bet == gameManager.instance.activeColor)
            {
                payout += stake;
                outcome += "  +ACTIVE COLOR BONUS";
            }
        }
        else if (isAdjacent(selectedColorIndex, landed))
        {
            payout = stake * adjacentPayoutMultiplier;
            outcome = "ADJACENT  " + hit + "  x" + adjacentPayoutMultiplier;
        }
        else
        {
            payout = 0;
            outcome = "MISS  " + hit + "  —  lost " + stake + "g";
        }

        if (payout > 0)
        {
            inv.currentCurrency += payout;
            audioManager.instance.PlayConfirmSound();
        }
        else
        {
            audioManager.instance.PlayBackSound();
        }

        UpdateMoneyHUD(); ////UPDATES THE MONEY HUD TO REFLECT THE NEW CURRENCY AMOUNT AFTER THE SPIN

        if (resultText != null)
            resultText.text = outcome + (payout > 0 ? "\nWon " + payout + "g" : "");

        FlashStatus(payout > 0 ? "Paid " + payout + "g." : "House keeps it.");

        int cap = Mathf.Max(minStake, inv.currentCurrency);
        if (stake > cap) stake = cap;

        isSpinning = false;
        RefreshUI();
    }

    void ShowLanded(int index) // Method to update the UI to show the color that was landed on after the spin
    {
        if (spinResultImage == null)
        {
            return;
        }

        spinResultImage.color = ColorOf(BetColors[index]);
    }

    void PaintColorButtons() // Method to set the colors of the betting buttons based on the predefined BetColors array
    {
        for (int i = 0; i < BetColors.Length; i++)
        {
            Color c = ColorOf(BetColors[i]);
            if (i < colorButtonImages.Length && colorButtonImages[i] != null)
            {
                colorButtonImages[i].color = c;
            }
        }
    }

    void HighlightSelected() // Method to visually highlight the selected color button in the UI
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] == null)
            {
                continue;
            }
            var colors = colorButtons[i].colors;
            colors.colorMultiplier = (i == selectedColorIndex) ? 1.4f : 1f;
            colorButtons[i].colors = colors;

            if (i < colorButtonImages.Length && colorButtonImages[i] != null) //Basically, this will set the alpha of the color button images to 1 if selected,
                                                                              //or 0.55 if not selected, to visually indicate which color is currently selected for betting.
            {
                Color c = ColorOf(BetColors[i]);
                c.a = (i == selectedColorIndex) ? 1f : 0.55f;
                colorButtonImages[i].color = c;
            }
        }
    }

    bool isAdjacent(int a, int b) // Method to determine if two color indices are adjacent on the betting wheel
    {
        int n = BetColors.Length;
        return Mathf.Abs(a - b) == 1 || Mathf.Abs(a - b) == n - 1; // This checks if the two indices are next to each other, considering the circular nature of the wheel
    }

    Color ColorOf(gameManager.ColorType colorType) // Method to convert a ColorType enum value to a Unity Color object
    {

        if (gameManager.instance.colorMaterials != null && gameManager.instance.colorMaterials.TryGetValue( //using tryGetValue to safely attempt to retrieve the Material associated
                                                                                                            //with the given ColorType from the colorMaterials dictionary in the gameManager instance.
            colorType, out Material mat) && mat != null)
        {
            return mat.color;
        }

        switch (colorType)
        {
            case gameManager.ColorType.RED: return Color.red;
            case gameManager.ColorType.ORANGE: return new Color(1f, 0.5f, 0f);
            case gameManager.ColorType.YELLOW: return Color.yellow;
            case gameManager.ColorType.GREEN: return Color.green;
            case gameManager.ColorType.BLUE: return Color.blue;
            case gameManager.ColorType.PURPLE: return new Color(0.6f, 0f, 1f);
            default: return Color.white;
        }
    }
    int Money() // Method to get the current amount of currency the player has by checking the inventory
                // in the gameManager instance. If the inventory is null, it returns 0.
    {
        return gameManager.instance.inventory != null
            ? gameManager.instance.inventory.currentCurrency
            : 0;
    }

    void UpdateMoneyHUD() // Method to update the currency display in the UI based on the player's current currency
    {
        gameManager.instance.UpdateCurrencyUI();
        if (currencyText != null)
        {
            currencyText.text = "Currency: " + Money();
        }
    }

    void RefreshUI() // Method to refresh the UI elements related to the gambling interface, including stake and button states
    {
        int money = Money();

        if (currencyText != null)
        {
            currencyText.text = "Money: " + money;
        }

        if (stakeText != null)
        {
            stakeText.text = "Stake: " + stake + "g";
        }
        bool canSpin = !isSpinning && selectedColorIndex >= 0 && money >= minStake && stake <= money; // This checks if the player can spin the wheel based on whether they are currently spinning,
                                                                                                      // have selected a color, and have enough money for the minimum stake.

        if (spinButton != null)
        {
            spinButton.interactable = canSpin;
        }
        if (leaveButton != null)
        {
            leaveButton.interactable = !isSpinning;
        }
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (colorButtons[i] != null)
                colorButtons[i].interactable = !isSpinning;
        }
    }

    void FlashStatus(string message) // FlashStatus displays a temporary message in the statusText UI element
    {
        if (statusText != null)
        {
            statusText.text = message;
            Debug.Log("Gambling Status: " + message);
        }
    }

}


