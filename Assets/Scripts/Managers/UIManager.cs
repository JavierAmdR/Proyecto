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
        healthSlider.maxValue = PlayerController.current.maxHealth;
        healthSlider.value = PlayerController.current.health;
        staminaSlider.maxValue = PlayerController.current.stamina;
        staminaSlider.value = PlayerController.current.stamina;
        PlayerController.current.onHealthChange += HealthChange;
        PlayerController.current.onStaminaChange += StaminaChange;

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


    public void HealthChange() 
    {
        healthSlider.value = PlayerController.current.health;
    }
    public void StaminaChange()
    {
        staminaSlider.value = PlayerController.current.stamina;
    }

}
