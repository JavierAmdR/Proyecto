using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCinematic : MonoBehaviour
{
    public GameObject skipPanel;
    public void SkipButton() 
    {
        Debug.Log("he presionado skip");
        SceneManager.LoadScene("HUB");
        //GameManager.current.StartGame();
    }
}
