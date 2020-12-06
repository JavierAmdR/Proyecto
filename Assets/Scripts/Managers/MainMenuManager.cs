using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject settingsMenu;

    private void Awake()
    {
        mainMenu = GameObject.Find("MainMenu");
        //settingsMenu = GameObject.Find("");
    }
}
