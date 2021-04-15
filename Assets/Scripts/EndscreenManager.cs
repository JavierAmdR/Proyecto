using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndscreenManager : MonoBehaviour
{
    public GameObject victoryAssets;
    public GameObject defeatAssets;

    void Start()
    {
        if (GameManager.current.currentResult == GameManager.gameResult.Defeat)
        {
            defeatAssets.SetActive(true);
        }
        else
        {
            victoryAssets.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        GameManager.current.LoadReset();
    }
}
