using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField]public NavMeshAgent navMesh;
    public Range rangeDetection;
    public Range attackRange;
    public CharacterStats enemyStats;

    public string targetTag;
    public GameObject target;


    private void Awake()
    {
        Initialization();
    }

    public virtual void Initialization() 
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<CharacterStats>();
        if (targetTag == null)
        {
            targetTag = "Player";
        }
        SwitchState(state.Idle);
    }

    public virtual void SetNewTarget(string newTarget) 
    {
        rangeDetection.SetNewTarget(newTarget);
        attackRange.SetNewTarget(newTarget);
    }

    public override void Idle()
    {
        base.Idle();
        if (rangeDetection.targetInRange() == true) 
        {
            target = rangeDetection.ClosestTarget();
            SwitchState(state.Moving);
        }
    }

    public override void Moving()
    {
        base.Moving();
        navMesh.SetDestination(target.transform.position);
        if (attackRange.targetInRange() == true) 
        {
            SwitchState(state.Attack);            
        }
    }

    public override void Attack()
    {
        base.Attack();
        Damage(enemyStats.attack.GetValue());

    }

    public virtual void Damage(int damageDealt) 
    {
        
    }
}
