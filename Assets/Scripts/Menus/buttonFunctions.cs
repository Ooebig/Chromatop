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
        gameManager.instance.ShowMenu(gameManager.instance.menuInBetween);
    }

    public void returning()
    {
        gameManager.instance.ReturnToPrevious();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    } 

    public void difficulty()
    {
        gameManager.instance.CloseCurrentMenu();

        //Will decide later on what to do with this button
        
    }

    public void mystery(){
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
        gameManager.instance.CloseCurrentMenu();
        SceneManager.LoadScene("Level1");
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

}
