using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager ui;

    public GameObject pausePanel;

    public bool gamePaused = false;



    public Slider healthSlider;
    public Slider staminaSlider;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ui = this;
        healthSlider.maxValue = PlayerStats.current.health.GetValue();
        healthSlider.value = PlayerStats.current.currentHealth;
        staminaSlider.maxValue = PlayerStats.current.stamina.GetValue();
        staminaSlider.value = PlayerStats.current.currentStamina;
        //PlayerController.current.onHealthChange += HealthChange;
        //PlayerController.current.onStaminaChange += StaminaChange;

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

}
