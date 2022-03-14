using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject ButtonsMenu;
    public GameObject LoadMenu;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        MainMenu.SetActive(false);
        ButtonsMenu.SetActive(false);
        LoadMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        MainMenu.SetActive(true);
        ButtonsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        LoadMenu.SetActive(false);
    }

    public void ButtonMenuButton()
    {
        // Show Play Menu (choosing buttons)
        MainMenu.SetActive(false); ;
        ButtonsMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        LoadMenu.SetActive(false);
    }

    public void LoadMenuButton()
    {
        // Show Load Menu
        MainMenu.SetActive(false); ;
        ButtonsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        LoadMenu.SetActive(true);
    }



    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}