using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EntityEvents : MonoBehaviour
{
    public event Action onAttack;
    public void Attack()
    {
        if (onAttack != null)
        {
            onAttack();
        }
    }

    public event Action onDamaged;
    public void Damaged()
    {
        if (onDamaged != null)
        {
            onDamaged();
        }
    }

    public event Action onMove;
    public void Moving()
    {
        if (onMove != null)
        {
            onMove();
        }
    }

    public event Action onDie;
    public void Die()
    {
        if (onDie != null)
        {
            onDie();
        }
    }

    public event Action onHealthChange;
    public void HealthChange()
    {
        if (onHealthChange != null)
        {
            onHealthChange();
        }
    }

    public event Action onStaminaChange;
    public void StaminaChange()
    {
        if (onStaminaChange != null)
        {
            onStaminaChange();
        }
    }
}
