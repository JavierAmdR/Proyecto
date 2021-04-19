using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndscreenManager : MonoBehaviour
{
    public GameObject victoryAssets;
    public GameObject defeatAssets;

    private int durationValue;
    private int durationValue2;

    public TextMeshProUGUI durationText;
    public TextMeshProUGUI enemiesDefeated;
    public TextMeshProUGUI upgradesObtained;
    public TextMeshProUGUI healthLost;
    public TextMeshProUGUI attacksReceived;

    void Start()
    {
        if (GameManager.current.currentResult == GameManager.gameResult.Defeat)
        {
            defeatAssets.SetActive(true);
            victoryAssets.SetActive(false);
        }
        else
        {
            victoryAssets.SetActive(true);
            defeatAssets.SetActive(false);
        }
        SetTextValues();
    }

    public void SetTextValues()
    {
        durationValue = (int)GameManager.current.duration / 60;
        durationValue2 = (int)GameManager.current.duration % 60;
        durationText.SetText(durationValue.ToString() + ":" + durationValue2.ToString());
        enemiesDefeated.SetText(GameManager.current.enemiesDefeated.ToString());
        upgradesObtained.SetText(GameManager.current.upgradesObtained.ToString());
        healthLost.SetText(GameManager.current.healthLost.ToString());
        attacksReceived.SetText(GameManager.current.attacksReceived.ToString());
    }

    public void ReturnToMenu()
    {
        GameManager.current.LoadReset();
    }
}
