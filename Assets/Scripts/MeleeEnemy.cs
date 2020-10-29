using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
       
    public enum state {Idle, Moving, Patrol, Attack, Death}
    public state enemyState;

    public GameObject model;

    public NavMeshAgent navMesh;

    public GameObject target;

    float timeCounter = 0;
    public bool preparingAttack = false;
    public bool inRecovery = false;

    public float attackPreparationTime;
    public float recoveryAttackTime;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyState = state.Idle;
    }
    public void Update() 
    {
        switch (enemyState)
        {
            case state.Idle:
                Idle();
                break;
            case state.Moving:
                Moving();
                break;
            case state.Patrol:
                break;
            case state.Attack:
                Attack();
                break;
            case state.Death:
                break;
        }
    }

    public void Idle() 
    {
        target = PlayerController.current.gameObject;
        enemyState = state.Moving;
                
    }
    void StateChange(state changeState) 
    {
        
    }

    public void Attack() 
    {
        
    }
    public void Death() 
    {
        
    }
    public void Moving() 
    {
        navMesh.SetDestination(target.transform.position);        
        Debug.Log("Enemy Moving");
    }
}
