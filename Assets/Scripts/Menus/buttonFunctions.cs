using System.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();
    }

    public void continuing()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.updateinbetweenUI();
        gameManager.instance.ShowMenu(gameManager.instance.menuInBetween);
    }

    public void returning()
    {
        audioManager.instance.PlayBackSound();
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
        audioManager.instance.PlayBackSound();
        gameManager.instance.ReqConf(gameManager.Pending.Quit);

        Debug.Log("Quit button pressed");
    }

    public void difficulty()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();

        //Will decide later on what to do with this button

    }

    public void mystery()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();

    }

    public void shop()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();

    }

    public void inventory()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.ShowMenu(gameManager.instance.menuInventory);
    }

    public void stats()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.ShowMenu(gameManager.instance.menuStats);
    }


    public void settings()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.ShowMenu(gameManager.instance.menuSettings);
    }


    public void previous()
    {
        audioManager.instance.PlayBackSound();
        //visits previous inventory page
    }

    public void next()
    {
        audioManager.instance.PlayConfirmSound();

        //visits next inventory page

    }

    public void playgame()
    {
        Debug.Log("Play game button pressed");
        audioManager.instance.PlayConfirmSound();
        gameManager.instance.CloseCurrentMenu();
    }

    public void returntoMainMenu()
    {
        audioManager.instance.PlayBackSound();
        gameManager.instance.ReqConf(gameManager.Pending.ReturntoMenu); //Request confirmation to return to main menu
    }

    public void returntoInBetween()
    {
        audioManager.instance.PlayBackSound();
        gameManager.instance.updateinbetweenUI();

        gameManager.instance.ShowMenu(gameManager.instance.menuInBetween);
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
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.action = gameManager.Pending.None; // Reset the action to None after handling the confirmation
    }

    public void promptNo()
    {
        audioManager.instance.PlayBackSound();
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

