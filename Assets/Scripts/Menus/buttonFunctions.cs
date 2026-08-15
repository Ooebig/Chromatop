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
