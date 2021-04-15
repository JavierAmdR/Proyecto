﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{

    public static UIManager ui;

    public GameObject pausePanel;

    public bool gamePaused = false;

    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthCounter;

    public GameObject abilityPanel;
    public GameObject levelUpPanel;



    public Slider healthSlider;
    public Slider staminaSlider;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (ui == null)
        {
            ui = this;
        }
        else
        {
            Destroy(gameObject);
        }
        healthSlider.maxValue = PlayerStats.current.health.GetValue();
        healthSlider.value = PlayerStats.current.currentHealth;
        staminaSlider.maxValue = PlayerStats.current.stamina.GetValue();
        staminaSlider.value = PlayerStats.current.currentStamina;
        HealthUpdate();

    }

    public void UpdateLevelUpInterface() 
    {
        //foreach(GameObject statcell in )
    }

    public void IncreseStatPoint(GameObject statcell)
    {
        if (GameManager.current.currency > 50)
        {
            GameManager.current.currency -= 50;
            CurrencyUpdate();
            switch (statcell.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text) 
            {
                case ("Health"): 
                    PlayerStats.current.health.AddLevel();
                    break;
                case ("Attack"):
                    PlayerStats.current.attack.AddLevel();
                    break;
                case ("Speed"):
                    PlayerStats.current.speed.AddLevel();
                    break;
                case ("Stamina"):
                    PlayerStats.current.stamina.AddLevel();
                    break;
                case ("Stamina reg."):
                    PlayerStats.current.staminaReg.AddLevel();
                    break;
            }
        }
    }

    public void ApplyStat() 
    {
        
    }

    public void OpenLevelUpPanel() 
    {
        levelUpPanel.SetActive(true);
    }

    public void ReduceStatPoint(GameObject statcell) 
    {
        
    }

    public void OnAbilityPanel() 
    {
        if (gamePaused == false && abilityPanel.activeSelf == false) 
        {
            abilityPanel.SetActive(true);
            abilityPanel.GetComponent<UpgradePanel>().LoadUpgrades();
        }
        else if (abilityPanel.activeSelf == true)
        {
            abilityPanel.SetActive(false);
        }
    }

    public void OnPause() 
    {      
        
        Debug.Log("Pause");
        gamePaused = !gamePaused;
        SetPauseTime();
        pausePanel.SetActive(!pausePanel.activeSelf);
        CloseAllPanels();
    }

    public void CloseAllPanels() 
    {
        abilityPanel.SetActive(false);
        levelUpPanel.SetActive(false);
    }

    public void SetPauseTime()
    {
        if (Time.timeScale != 0)
        {
            GameManager.current.ChangeTimeScale(0f);
        }
        else
        {
            GameManager.current.ChangeTimeScale(1f);
        }
    }

    public void EnableBlur() 
    {
        
    }

    public void DisableBlur() 
    {
        
    }

    public void HealthUpdate() 
    {
        healthSlider.maxValue = PlayerStats.current.health.GetValue();
        healthSlider.value = PlayerStats.current.currentHealth;
        healthCounter.SetText(PlayerStats.current.currentHealth.ToString() + "/" + PlayerStats.current.health.GetValue());

    }
    public void StaminaUpdate()
    {
        staminaSlider.value = PlayerStats.current.currentStamina;
    }

    public void CurrencyUpdate() 
    {
        currencyText.SetText(GameManager.current.currency.ToString());
    }

}
