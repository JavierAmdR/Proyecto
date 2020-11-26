using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum state { Idle, Moving, Attack, Death}
    public state characterState;
    public enum attackState { Preparing, Attack, Recovery }
    public attackState attackStatus;

    public virtual void SwitchState(state newState) 
    {
        characterState = newState;
    }

    public void SwitchAttackState(attackState newState)
    {
        attackStatus = newState;
    }

    public void Update()
    {
        CharacterLoop();
    }

    public virtual void CharacterLoop() 
    {
        switch (characterState)
        {
            case state.Idle:
                Idle();
                break;
            case state.Moving:
                Moving();
                break;
            case state.Attack:
                Attack();
                break;
            case state.Death:
                Death();
                break;
        }
    }

    public virtual void Idle() 
    {
        
    }

    public virtual void Moving()
    {

    }

    public virtual void Attack()
    {
        switch (attackStatus)
        {
            case attackState.Preparing:
                PreparingAttack();
                break;
            case attackState.Attack:
                InAttack();
                break;
            case attackState.Recovery:
                Recovery();
                break;

        }
    }

    public virtual void PreparingAttack() 
    {
        
    }

    public virtual void InAttack() 
    {
        
    }

    public virtual void Recovery() 
    {
        
    }

    public virtual void Death()
    {

    }

    public state GetState() 
    {
        return characterState;
    }


}
