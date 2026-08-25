using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class gameManager : MonoBehaviour
{
    public enum Pending { None, ReturntoMenu, Quit } // Enum for pending actions after a menu is closed
    public Pending action = Pending.None; // Current pending action

    public enum ColorType { RED, ORANGE, YELLOW, GREEN, BLUE, PURPLE, GREY }
    public static gameManager instance;
    [Header("Starting Settings")]
    [SerializeField]
    public List<ItemData> startingItems =
        new List<ItemData>();

    [Header("Color Management")]
    [SerializeField] public Material redMat;
    [SerializeField] public Material orangeMat;
    [SerializeField] public Material yellowMat;
    [SerializeField] public Material greenMat;
    [SerializeField] public Material blueMat;
    [SerializeField] public Material purpleMat;
    [SerializeField] public Material greyMat;
    public Dictionary<ColorType, Material> colorMaterials =
        new Dictionary<ColorType, Material>();

    [SerializeField] public List<ColorType> colorsUnlocked = new List<ColorType>(6);
    public List<ColorType> colorOrder = new List<ColorType>(6);
    [SerializeField] public List<Image> slices = new List<Image>();

    [Header("Scene Management")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] public GameObject menuInBetween;
    [SerializeField] public GameObject menuSettings;
    [SerializeField] public GameObject menuStats;
    [SerializeField] public GameObject menuInventory;
    [SerializeField] public GameObject menuMain;
    [SerializeField] public GameObject menuPrompt;
    [SerializeField] public GameObject menuShop;
    [SerializeField] public GameObject menuCurse;
    [SerializeField] public GameObject menuGambling;
    [SerializeField] public GameObject menuMystery;

    GameObject menuPrevious;

    [Header("In-Between Menu UI")]
    public GameObject[] destinyButtons;
    public TMP_Text[] destinyButtonTexts;


    //public GameObject checkPointPopup;
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public GameObject playerDamageScreen;
    public TMP_Text roundTimerText;

    public bool isPaused;
    public GameObject player;
    //public playerController playerScript;
    public Inventory inventory;
    public GameObject playerStartPos;
    public ColorType activeColor;
    public Material activeMaterial;



    //int gameGoalCount;

    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicates when returning to this scene
            return;
        }

        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        //playerScript = player.GetComponent<playerController>();
        inventory = player.GetComponent<Inventory>();
        playerStartPos = GameObject.FindWithTag("Player Start Pos");
        activeColor = colorsUnlocked[0];
        colorMaterials[ColorType.RED] = redMat;
        colorMaterials[ColorType.ORANGE] = orangeMat;
        colorMaterials[ColorType.YELLOW] = yellowMat;
        colorMaterials[ColorType.GREEN] = greenMat;
        colorMaterials[ColorType.BLUE] = blueMat;
        colorMaterials[ColorType.PURPLE] = purpleMat;
        colorMaterials[ColorType.GREY] = greyMat;

        //DontDestroyOnLoad(gameObject);
        RefreshMenuRef();
    }

    void Start()
    {
        if (startingItems.Count > 0)
        {
            for (int i = 0; i < startingItems.Count; i++)
            {
                inventory.AddItem(startingItems[i]);
            }
            inventory.ActivatePocket(Inventory.PocketType.Passive);
            inventory.ActivatePocket(Inventory.PocketType.Weapon);
        }
        ChangeColor(2);

        ShowMenu(menuMain);
        audioManager.instance.PlayMainMenuMusic();
    }

    void ChangeColor(int direction)
    {
        int currentIndex = colorOrder.IndexOf(activeColor);
        bool valid = false;
        while (!valid)
        {
            if (direction == 1)
            {
                currentIndex = (currentIndex + 1) % colorOrder.Count;
            }
            else if (direction == 0)
            {
                currentIndex = (currentIndex - 1 + colorOrder.Count) % colorOrder.Count;
            }
            if (colorsUnlocked.Contains(colorOrder[currentIndex]))
            {
                valid = true;
            }
        }
        activeColor = colorOrder[currentIndex];
        switch (activeColor)
        {
            case ColorType.RED:
                activeMaterial = redMat;
                break;
            case ColorType.ORANGE:
                activeMaterial = orangeMat;
                break;
            case ColorType.YELLOW:
                activeMaterial = yellowMat;
                break;
            case ColorType.GREEN:
                activeMaterial = greenMat;
                break;
            case ColorType.BLUE:
                activeMaterial = blueMat;
                break;
            case ColorType.PURPLE:
                activeMaterial = purpleMat;
                break;
            default:
                activeMaterial = greyMat;
                break;
        }
        player.GetComponent<MeshRenderer>().material = activeMaterial;
        UpdateColorUI();
    }

    void UpdateColorUI()
    {
        int currentIndex = colorOrder.IndexOf(activeColor);
        for (int i = 0; i < colorOrder.Count; i++)
        {
            if (colorsUnlocked.Contains(colorOrder[currentIndex]))
            {
                switch (colorOrder[currentIndex])
                {
                    case ColorType.RED:
                        slices[i].color = redMat.color;
                        break;
                    case ColorType.ORANGE:
                        slices[i].color = orangeMat.color;
                        break;
                    case ColorType.YELLOW:
                        slices[i].color = yellowMat.color;
                        break;
                    case ColorType.GREEN:
                        slices[i].color = greenMat.color;
                        break;
                    case ColorType.BLUE:
                        slices[i].color = blueMat.color;
                        break;
                    case ColorType.PURPLE:
                        slices[i].color = purpleMat.color;
                        break;
                    default:
                        slices[i].color = greyMat.color;
                        break;
                }

            }
            else
            {
                slices[i].color = greyMat.color;
            }
            currentIndex++;
            if (currentIndex == 6)
            {
                currentIndex = 0;
            }
        }

    }



    void UnlockColor(ColorType color)
    {
        if (!colorsUnlocked.Contains(color))
        {
            colorsUnlocked.Add(color);
        }
    }

    public void DamageFlash(ColorType color)
    {
        if (playerDamageScreen != null)
        {
            float alpha = playerDamageScreen.GetComponent<Image>().color.a;
            switch (color)
            {
                case ColorType.RED:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, redMat.color);
                    break;
                case ColorType.ORANGE:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, orangeMat.color);
                    break;
                case ColorType.YELLOW:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, yellowMat.color);
                    break;
                case ColorType.GREEN:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, greenMat.color);
                    break;
                case ColorType.BLUE:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, blueMat.color);
                    break;
                case ColorType.PURPLE:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, purpleMat.color);
                    break;
                default:
                    playerDamageScreen.GetComponent<Image>().color = ColorFade(alpha, greyMat.color);
                    break;
            }
            StartCoroutine(StartDamageFlash());
        }
    }
    Color ColorFade(float alpha, Color color)
    {
        Color endColor = color;
        endColor = color;
        endColor.a = alpha;
        return endColor;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                ShowMenu(menuPause);
            }
            else if (menuActive == menuPause)
            {
                CloseCurrentMenu();
            }
            else
            {
                ReturnToPrevious();
            }
        }
        if (Input.GetButtonDown("RotateLeft"))
        {
            ChangeColor(0);
        }
        else if (Input.GetButtonDown("RotateRight"))
        {
            ChangeColor(1);
        }
    }



    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public void stateUnpause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }
    //public void updateGameGoal(int amount)
    //{
    //    gameGoalCount += amount;
    //    gameGoalCountText.text = gameGoalCount.ToString("F0");
    //    if (gameGoalCount <= 0)
    //    {
    //        statePause();
    //        menuActive = menuWin;
    //        menuActive.SetActive(true);
    //    }
    //}
    public void youLose()
    {
        ShowMenu(menuLose);
    }

    public void updatePlayerHP(float current, float max)
    {
        if (playerHPBar != null)
        {
            playerHPBar.fillAmount = current / max;
        }
        if (playerHPText != null)
        {
            playerHPText.text = Mathf.CeilToInt(current).ToString() + " / " + Mathf.CeilToInt(max).ToString();
        }
    }




    public static float damageCalc(float damage, ColorType A = ColorType.GREY, ColorType B = ColorType.GREY)
    {
        switch (A)
        {
            case ColorType.RED:
                switch (B)
                {

                    case ColorType.RED:
                        return (float)(damage * 0);
                    case ColorType.ORANGE:
                        return (float)(damage * 0.5);
                    case ColorType.YELLOW:
                        return (float)(damage * 1);
                    case ColorType.GREEN:
                        return (float)(damage * 2);
                    case ColorType.BLUE:
                        return (float)(damage * 1);
                    case ColorType.PURPLE:
                        return (float)(damage * 0.5);
                    default:
                        return damage;
                }
            case ColorType.ORANGE:
                switch (B)
                {
                    case ColorType.RED:
                        return (float)(damage * 0.5);
                    case ColorType.ORANGE:
                        return (float)(damage * 0);
                    case ColorType.YELLOW:
                        return (float)(damage * 0.5);
                    case ColorType.GREEN:
                        return (float)(damage * 1);
                    case ColorType.BLUE:
                        return (float)(damage * 2);
                    case ColorType.PURPLE:
                        return (float)(damage * 1);
                    default:
                        return damage;
                }
            case ColorType.YELLOW:
                switch (B)
                {
                    case ColorType.RED:
                        return (float)(damage * 1);
                    case ColorType.ORANGE:
                        return (float)(damage * 0.5);
                    case ColorType.YELLOW:
                        return (float)(damage * 0);
                    case ColorType.GREEN:
                        return (float)(damage * 0.5);
                    case ColorType.BLUE:
                        return (float)(damage * 1);
                    case ColorType.PURPLE:
                        return (float)(damage * 2);
                    default:
                        return damage;
                }
            case ColorType.GREEN:
                switch (B)
                {
                    case ColorType.RED:
                        return (float)(damage * 2);
                    case ColorType.ORANGE:
                        return (float)(damage * 1);
                    case ColorType.YELLOW:
                        return (float)(damage * 0.5);
                    case ColorType.GREEN:
                        return (float)(damage * 0);
                    case ColorType.BLUE:
                        return (float)(damage * 0.5);
                    case ColorType.PURPLE:
                        return (float)(damage * 1);
                    default:
                        return damage;
                }
            case ColorType.BLUE:
                switch (B)
                {
                    case ColorType.RED:
                        return (float)(damage * 1);
                    case ColorType.ORANGE:
                        return (float)(damage * 2);
                    case ColorType.YELLOW:
                        return (float)(damage * 1);
                    case ColorType.GREEN:
                        return (float)(damage * 0.5);
                    case ColorType.BLUE:
                        return (float)(damage * 0);
                    case ColorType.PURPLE:
                        return (float)(damage * 0.5);
                    default:
                        return damage;
                }
            case ColorType.PURPLE:
                switch (B)
                {
                    case ColorType.RED:
                        return (float)(damage * 0.5);
                    case ColorType.ORANGE:
                        return (float)(damage * 1);
                    case ColorType.YELLOW:
                        return (float)(damage * 2);
                    case ColorType.GREEN:
                        return (float)(damage * 1);
                    case ColorType.BLUE:
                        return (float)(damage * 0.5);
                    case ColorType.PURPLE:
                        return (float)(damage * 0);
                    default:
                        return damage;
                }
            default:
                return damage;
        }
    }

    public void ShowMenu(GameObject newMenu)
    {

        if (menuActive != null)
        {
            menuPrevious = menuActive;
            menuActive.SetActive(false);
        }

        menuActive = newMenu;

        newMenu.SetActive(true);

        if (audioManager.instance != null)
        {
            audioManager.instance.PlayMenuOpenSound();
        }

        statePause();

    }

    public void ReturnToPrevious()
    {
        if (menuPrevious != null)
        {
            menuActive.SetActive(false);
            menuActive = menuPrevious;
            menuPrevious = null;
            menuActive.SetActive(true);
        }
        else
        {
            stateUnpause();
        }
    }

    public void CloseCurrentMenu()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
            stateUnpause();
        }
    }

    public void RefreshMenuRef()
    {

        GameObject menuRt = GameObject.Find("Menus");
        if (menuRt == null)
        {
            Debug.LogWarning("Menus root not found.");
            return;
        }

        //Finding the menus by name under the "Menus" root object, using ? to avoid null reference exceptions if not found

        menuPause = menuRt.transform.Find("Pause Menu")?.gameObject;
        menuWin = menuRt.transform.Find("Level Complete Menu")?.gameObject;
        menuLose = menuRt.transform.Find("Death Menu")?.gameObject;
        menuInBetween = menuRt.transform.Find("In-Between Menu")?.gameObject;
        menuSettings = menuRt.transform.Find("Settings Menu")?.gameObject;
        menuStats = menuRt.transform.Find("Stats Menu")?.gameObject;
        menuInventory = menuRt.transform.Find("Inventory Menu")?.gameObject;
        menuMain = menuRt.transform.Find("Main Menu")?.gameObject;
        menuPrompt = menuRt.transform.Find("Prompt?")?.gameObject;
        menuShop = menuRt.transform.Find("Shop Menu")?.gameObject;
        menuCurse = menuRt.transform.Find("Curse Menu")?.gameObject;
        menuGambling = menuRt.transform.Find("Gambling Menu")?.gameObject;
        menuMystery = menuRt.transform.Find("Mystery Menu")?.gameObject;

        if (menuPause == null)
        {
            Debug.LogWarning("Pause Menu not found.");
        }
    }

    public IEnumerator RefreshAfterLoad()
    {

        yield return null;
        RefreshMenuRef();
        stateUnpause();

    }

    IEnumerator StartDamageFlash()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }

    public void ReqConf(Pending requestAction) // Request confirmation for an action (Return to Menu or Quit)
    {
        //Store the action. Then Show the prompt.

        action = requestAction;

        ShowMenu(menuPrompt);
    }

    public enum PossibleDestinies
    {
        Easy, Medium, Hard, Mystery, Shop, Curse, Gambling, Boss
    }

    public int currentRound;

    public List<PossibleDestinies> destinyList = new List<PossibleDestinies>();

    public void GenerateOptions()
    {
        destinyList.Clear();

        if (currentRound % 10 == 0) //boss round
        {
            destinyList.Add(PossibleDestinies.Boss);
            return;

        }

        List<PossibleDestinies> difficulties = new List<PossibleDestinies>() //at least one of each difficulty
        {
            PossibleDestinies.Easy,
            PossibleDestinies.Medium,
            PossibleDestinies.Hard
        };

        int forcediff = Random.Range(0, difficulties.Count);
        PossibleDestinies forced = difficulties[forcediff];
        destinyList.Add(forced); //force at least one difficulty into the list

        List<PossibleDestinies> others = new List<PossibleDestinies>();

        for (int i = 0; i < difficulties.Count; i++)
        {
            if (difficulties[i] != forced)
            {
                others.Add(difficulties[i]);
            }
        }

        //other possible destinies

        others.Add(PossibleDestinies.Mystery);
        others.Add(PossibleDestinies.Shop);
        others.Add(PossibleDestinies.Curse);
        others.Add(PossibleDestinies.Gambling);

        while (destinyList.Count < 3 && others.Count > 0) // while we have less than 3 destinies
                                                          //and there are still other destinies to choose from, randomly select one,
                                                          //and add it to the destinyList
        {
            int rand = Random.Range(0, others.Count);
            destinyList.Add(others[rand]);
            others.RemoveAt(rand);
        }
    }

    public void updateinbetweenUI() // update the in-between menu UI with the current destiny options
    {
        GenerateOptions();

        for (int i = 0; i < 3; i++)
        {
            if (i < destinyList.Count)
            {
                destinyButtons[i].SetActive(true);
                destinyButtonTexts[i].text = destinyList[i].ToString();
            }
            else
            {
                destinyButtons[i].SetActive(false);
            }
        }
    }

    public void OnDestinyClick(int index) // called when a destiny button is clicked, index is the button index (0, 1, or 2)
    {
        if (index < 0 || index >= destinyList.Count)
        {
            Debug.LogWarning("Invalid destiny index: " + index);

            PossibleDestinies chosendestiny = destinyList[index];

            switch (chosendestiny)
            {
                case PossibleDestinies.Easy:
                    Debug.Log("Easy destiny chosen");

                    // set difficulty to easy, spawn easy enemies, etc.
                    CloseCurrentMenu();
                    //start the next round with easy difficulty
                    break;

                case PossibleDestinies.Medium:
                    Debug.Log("Medium destiny chosen");
                    // set difficulty to medium, spawn medium enemies, etc.
                    CloseCurrentMenu();
                    //start the next round with medium difficulty
                    break;

                case PossibleDestinies.Hard:
                    Debug.Log("Hard destiny chosen");
                    // set difficulty to hard, spawn hard enemies, etc.
                    CloseCurrentMenu();
                    //start the next round with hard difficulty
                    break;

                case PossibleDestinies.Mystery:
                    Debug.Log("Mystery destiny chosen");
                    ShowMenu(menuMystery);
                    break;

                case PossibleDestinies.Shop:
                    Debug.Log("Shop destiny chosen");
                    ShowMenu(menuShop);
                    break;

                case PossibleDestinies.Curse:
                    Debug.Log("Curse destiny chosen");
                    ShowMenu(menuCurse);
                    break;

                case PossibleDestinies.Gambling:
                    Debug.Log("Gambling destiny chosen");
                    ShowMenu(menuGambling);
                    break;

                case PossibleDestinies.Boss:
                    Debug.Log("Boss destiny chosen");
                    // set difficulty to boss, spawn boss enemies, etc.
                    CloseCurrentMenu();
                    //start the next round with boss difficulty
                    break;

                default:
                    Debug.LogWarning("Unknown destiny chosen: " + chosendestiny);
                    break;
            }
        }
    }

    public void OpeningMenu(GameObject menu)
    {
        if (menuActive != null)
        {
            menuPrevious = menuActive;
            menuActive.SetActive(false);
        }

        menuActive = menu;
        menu.SetActive(true);

        if (audioManager.instance != null)
        {
            audioManager.instance.PlayMenuOpenSound();

            statePause();
        }

        else
        {
            Debug.LogWarning("OpeningMenu called with a null menu reference.");
        }

    }
}