using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        gameManager.instance.CloseCurrentMenu();
    }

    public void continuing()
    {
        gameManager.instance.updateinbetweenUI();
        gameManager.instance.ShowMenu(gameManager.instance.menuInBetween);
    }

    public void returning()
    {
        gameManager.instance.ReturnToPrevious();
    }

    //public void restart()
    //{
    //    Debug.Log("Restart button pressed");
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    //    gameManager.instance.StartCoroutine(gameManager.instance.RefreshAfterLoad());
    //}

    public void quit()
    {
        gameManager.instance.ReqConf(gameManager.Pending.Quit);

        Debug.Log("Quit button pressed");
    }

    public void difficulty()
    {
        gameManager.instance.CloseCurrentMenu();

        //Will decide later on what to do with this button

    }

    public void mystery()
    {
        gameManager.instance.CloseCurrentMenu();

    }

    public void shop()
    {
        gameManager.instance.CloseCurrentMenu();

    }

    public void inventory()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuInventory);
    }

    public void stats()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuStats);
    }


    public void settings()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuSettings);
    }


    public void previous()
    {
        //visits previous inventory page
    }

    public void next()
    {

        //visits next inventory page

    }

    public void playgame()
    {
        Debug.Log("Play game button pressed");
        gameManager.instance.CloseCurrentMenu();
    }

    public void returntoMainMenu()
    {
        gameManager.instance.ReqConf(gameManager.Pending.ReturntoMenu); //Request confirmation to return to main menu
    }

    public void promptYes()
    {
        switch (gameManager.instance.action)
        {
            case gameManager.Pending.ReturntoMenu:
               gameManager.instance.ShowMenu(gameManager.instance.menuMain);
                break;
            case gameManager.Pending.Quit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                break;
        }

        gameManager.instance.action = gameManager.Pending.None; // Reset the action to None after handling the confirmation
    }

    public void promptNo()
    {
        gameManager.instance.action = gameManager.Pending.None; // Reset the action to None if the user cancels
        gameManager.instance.ReturnToPrevious(); // Return to the previous menu without taking any action

    }
}

    //public void loadLevel(int lvl)
    //{
    //    //SceneManager.LoadScene(lvl);
    //    //gameManager.instance.stateUnpause();
    //}

    //public void playerRespawn()
    //{
    //    //gameManager.instance.playerScript.changePlayerPos();
    //    //gameManager.instance.stateUnpause();
    //}

