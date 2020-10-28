using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager ui;

    public GameObject pausePanel;



    public Slider healthSlider;
    public Slider staminaSlider;

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

    public void HealthChange() 
    {
        healthSlider.value = PlayerController.current.health;
    }
    public void StaminaChange()
    {
        staminaSlider.value = PlayerController.current.stamina;
    }

}
