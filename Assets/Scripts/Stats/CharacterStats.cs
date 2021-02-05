﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat health;
    public Stat attack;
    public Stat defense;
    public Stat speed;
    public Stat stamina;
    public Stat staminaReg;
    public Stat crit;
    public Stat poison;

    public bool stunAttacks = false;

    public int currentHealth;
    public float currentStamina;

    
    private void Awake()
    {
        currentHealth = health.GetValue();
        currentStamina = stamina.GetValue();
    }

    //PONER EN AWAKE EN CASO DE OVERRIDE
    public virtual void InitStats() 
    {
        currentHealth = health.GetValue();
        currentStamina = stamina.GetValue();
    }

    public virtual void ReceiveDamage (int damage)
    {
        damage -= defense.GetValue();

        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " gets " + damage + " damage.");

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    public virtual bool IsLethal(int damage) 
    {
        if (currentHealth - damage <= 0) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public virtual void StaminaReg()
    {

    }

    public virtual void ReduceStamina(int staminaCon) 
    {
        
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
