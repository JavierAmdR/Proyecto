using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject settingsMenu;

    public void StartGameButton() 
    {
        GameManager.current.StartGame();
    }
}
