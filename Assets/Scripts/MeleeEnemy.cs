using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
       
    public enum state {Idle, Moving, Patrol, Attack, Death}
    public state enemyState;
    
    public void Update() 
    {
        switch (enemyState)
        {
            case state.Idle:
                break;
            case state.Moving:
                break;
            case state.Patrol:
                break;
            case state.Attack:
                break;
            case state.Death:
                break;
        }
    }

    void StateChange() 
    {
        
    }

    public override void Attack() 
    {
        
    }
    public override void Death() 
    {
        
    }
    public override void Movement() 
    {
        
    }
}
