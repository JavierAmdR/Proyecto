using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager ui;

    public GameObject pausePanel;

    public bool gamePaused = false;

    public TextMeshProUGUI currencyText;



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

    }

    public void UpdateLevelUpInterface() 
    {
        //foreach(GameObject statcell in )
    }

    public void IncreseStatPoint(GameObject statcell)
    {

    }

    public void ApplyStat() 
    {
        
    }

    public void ReduceStatPoint(GameObject statcell) 
    {
        
    }


    public void OnPause() 
    {      
        
        Debug.Log("Pause");
        gamePaused = !gamePaused;
        SetPauseTime();
        pausePanel.SetActive(!pausePanel.activeSelf);
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


    public void HealthUpdate() 
    {
        healthSlider.value = PlayerStats.current.currentHealth;
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
