using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum state { Idle, Moving, Attack, Death}
    public state characterState;

    public void SwitchState(state newState) 
    {
        characterState = newState;
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

    }

    public virtual void Death()
    {

    }

    public state GetState() 
    {
        return characterState;
    }


}
