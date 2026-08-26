using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        audioManager.instance.PlayConfirmSound();
        audioManager.instance.PlayGameMusic();
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

    public void itemClaim(int val)
    {
        gameManager.instance.OnLevelCompleteRewardClick(val);
        audioManager.instance.PlayConfirmSound();
        gameManager.instance.updateinbetweenUI();
        gameManager.instance.ShowMenu(gameManager.instance.menuInBetween);
    }

    public void quit()
    {
        audioManager.instance.PlayBackSound();
        gameManager.instance.ReqConf(gameManager.Pending.Quit);

        Debug.Log("Quit button pressed");
    }

    public void difficultyEasy()
    {
        gameManager.instance.currentRound++;
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();
        gameManager.instance.RefreshRound();
        gameManager.instance.waveManager.StartWave(EnemySpawner.Wave.Difficulty.easy, (15 + (gameManager.instance.currentRound * 5)));
    }

    public void difficultyNormal()
    {
        gameManager.instance.currentRound++;
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();
        gameManager.instance.RefreshRound();
        gameManager.instance.waveManager.StartWave(EnemySpawner.Wave.Difficulty.normal, (15 + (gameManager.instance.currentRound * 5)));
    }

    public void difficultyHard()
    {
        gameManager.instance.currentRound++;
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();
        gameManager.instance.RefreshRound();
        gameManager.instance.waveManager.StartWave(EnemySpawner.Wave.Difficulty.hard, (15 + (gameManager.instance.currentRound * 5)));
    }

    public void difficultyBoss()
    {
        gameManager.instance.currentRound++;
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.CloseCurrentMenu();
        gameManager.instance.RefreshRound();
        gameManager.instance.waveManager.StartWave(EnemySpawner.Wave.Difficulty.boss, (15 + (gameManager.instance.currentRound * 5)));
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
        gameManager.instance.UpdateInventoryScreen();
        gameManager.instance.ShowMenu(gameManager.instance.menuInventory);
    }

    public void stats()
    {
        audioManager.instance.PlayConfirmSound();
        
        gameManager.instance.ShowMenu(gameManager.instance.menuStats);

      gameManager.instance.UpdateStatScreen();
    }


    public void settings()
    {
        audioManager.instance.PlayConfirmSound();

        gameManager.instance.ShowMenu(gameManager.instance.menuSettings);

        audioManager.instance.SyncAudioSliders(
            audioManager.instance.masterSlider,
            audioManager.instance.musicSlider,
            audioManager.instance.sfxSlider);

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
        audioManager.instance.PlayGameMusic();
        gameManager.instance.CloseCurrentMenu();
        gameManager.instance.waveManager.StartWave(EnemySpawner.Wave.Difficulty.normal, 20);
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
                gameManager.instance.resetTimeScale();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

