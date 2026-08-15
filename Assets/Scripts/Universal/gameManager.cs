using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    //public GameObject checkPointPopup;
    public Image playerHPBar;
    //public GameObject playerDamageScreen;
    public TMP_Text roundTimerText;

    public bool isPaused;
    // public GameObject player;
    //public playerController playerScript;
   //  public GameObject playerStartPos;



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
        //player = GameObject.FindWithTag("Player");
        //playerScript = player.GetComponent<playerController>();
        //playerStartPos = GameObject.FindWithTag("Player Start Pos");
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if(menuActive == menuPause)
            {
                stateUnpause();
            }
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
        menuActive.SetActive(false);
        menuActive = null;
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
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }


    public enum ColorType { RED, ORANGE, YELLOW, GREEN, BLUE, PURPLE, GREY }

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





}
