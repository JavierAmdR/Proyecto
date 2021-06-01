using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject settingsMenu;

    public void StartGameButton() 
    {
        GameManager.current.LoadCinematic();
        //SceneManager.LoadScene("Ananimatica");
        //GameManager.current.StartGame();
    }
}
