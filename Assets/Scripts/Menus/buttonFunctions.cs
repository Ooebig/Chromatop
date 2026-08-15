using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }
    public void restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //gameManager.instance.stateUnpause();
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void continuing(){
        gameManager.instance.stateUnpause();
    }   

    public void difficulty()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Difficulty");
    }

    public void mystery(){
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Mystery");
    }

    public void shop()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Shop");
    }

    public void inventory()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Inventory");
    }

    public void stats()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Stats");
    }

    public void settings()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Settings");
    }

    public void returning(){
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("MainMenu");
    }

    public void previous()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Previous");
    }

    public void next()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene("Next");
    }

    public void playgame()
    {
        gameManager.instance.stateUnpause();
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
