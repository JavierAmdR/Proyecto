using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public static PlayerStats current;

    public Stat dashCost;
    public Stat dashSpeed;
    public bool godMode = false;
    private void Awake()
    {
        InitStats();
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (current == null)
        {
            current = this;
        }
    }



    public override void StaminaReg()
    {
        base.StaminaReg();
        if (currentStamina < stamina.GetValue())
        {
            currentStamina += staminaReg.GetValue() * Time.deltaTime;
            if (currentStamina > stamina.GetValue())
            {
                currentStamina = stamina.GetValue();
            }
            UIManager.ui.StaminaUpdate();
            PlayerEvents.current.StaminaChange();
        }
    }
    public override void ReduceStamina(int staminaCon)
    {
        if (currentStamina > 0)
        {
            base.ReduceStamina(staminaCon);
            currentStamina -= staminaCon;
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            UIManager.ui.StaminaUpdate();
            PlayerEvents.current.StaminaChange();
        }
    }

    public bool HasStamina(int staminaCon) 
    {
        if (currentStamina > staminaCon) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public bool CanDash() 
    {
        return HasStamina(dashCost.GetValue());
    }

    public void StaminaDash() 
    {
        ReduceStamina(dashCost.GetValue());
    }

    public override void ReceiveDamage(int damage)
    {
        if (godMode == false) 
        {
            base.ReceiveDamage(damage);
            UIManager.ui.HealthUpdate();
        }      
        
    }
    public override void Die()
    {
        if (currentHealth <= 0)
        {
            UIManager.ui.HealthUpdate();
            GameManager.current.LoadDefeat();
            currentHealth = health.GetBaseValue();
            UIManager.ui.HealthUpdate();
        }
    }
}
