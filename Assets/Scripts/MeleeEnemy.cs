using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
   
    public GameObject attackCollider;
    public float timeUntilAttack;
    public float timeRecoveryAttack;
    public float timeActiveAttack;
    float counter = 0f;
    public override void PrepareAttackBehaviour()
    {
        base.PrepareAttackBehaviour();
        SpeedStop();
        SwitchAttackState(attackState.Preparing);
        SwitchState(state.Attack);
    }

    public override void PreparingAttack()
    {
        base.PreparingAttack();
        counter += 1 * Time.deltaTime;
        if (counter >= timeUntilAttack) 
        {
            counter = 0f;
            SwitchAttackState(attackState.Attack);
        } 
    }

    public override void InAttack()
    {
        base.InAttack();
        Debug.Log(attackCollider.activeSelf);
        if (attackCollider.activeSelf == false) 
        {
            attackCollider.SetActive(true);
        }
        else 
        {
            counter += 1 * Time.deltaTime;
            if (counter >= timeActiveAttack) 
            {
                counter = 0f;
                attackCollider.SetActive(false);
                SwitchAttackState(attackState.Recovery);
            }
        }
    }

    public override void Recovery()
    {
        base.Recovery();
        counter += 1 * Time.deltaTime;
        if (counter >= timeRecoveryAttack) 
        {
            counter = 0f;
            SetNormalSpeed();
            SwitchState(state.Idle);
        }
    }


}
