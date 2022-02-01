using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject Gameover;
    int count = 0;

    public void QuitGame()
    {
        Application.Quit();
    }


    //Game Over Screen
    public void Update()
    {
        if (RH.endofEvolution == true)
        {
            Time.timeScale = 0f;
            Gameover.SetActive(true);


/*            if (count == 0)
            {
                PlayerWriteScript.CreateAvg();
                count++;
            }*/
        }
    }
}
