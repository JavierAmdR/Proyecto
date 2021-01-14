using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField]public NavMeshAgent navMesh;
    public EntityEvents eventSystem;
    public Hitbox hitbox;

    public int currency;
    public Range rangeDetection;
    public Range attackRange;
    public CharacterStats enemyStats;
    public ParticleSystem hit;

    public string targetTag;
    public GameObject target;

    public bool die = false;
    private int damage;


    private void Awake()
    {
        Initialization();
        if (eventSystem != null)
        {
            eventSystem.onDamaged += GetDamage;
        }
        else 
        {
            eventSystem = GetComponent<EntityEvents>();
            eventSystem.onDamaged += GetDamage;
        }
    }

    public virtual void Initialization() 
    {
        navMesh = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<CharacterStats>();
        if (targetTag == "")
        {
            targetTag = "Player";
            SetNewTarget(targetTag);
        }

        SetNormalSpeed();
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
        if (rangeDetection.targetInRange() == true && target == null) 
        {
            target = rangeDetection.ClosestTarget();
            Debug.Log(target);
            SwitchState(state.Moving);
        }
        else if (target != null) 
        {
            SwitchState(state.Moving);
        }
    }

    public override void Moving()
    {
        base.Moving();
        navMesh.SetDestination(target.transform.position);
        if (attackRange.targetInRange() == true) 
        {
            SpeedStop();
            PrepareAttackBehaviour();       
        }
    }

    public virtual void GetDamage() 
    {

        damage = PlayerStats.current.CalculateDamage();
        if (enemyStats.IsLethal(damage) == true) 
        {
            GameManager.current.AddCurency(currency);
        }
        enemyStats.ReceiveDamage(damage);
        if (hit != null)
        {
            hit.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        switch (other.tag) 
        {
            case "PlayerAttack":               
                break;
        }
    }

    public virtual void SpeedStop() 
    {
        navMesh.velocity = Vector3.zero;        
    }

    public virtual void SetSpeed(int newSpeed) 
    {
        navMesh.speed = newSpeed;
    }

    public virtual void SetNormalSpeed() 
    {
        SetSpeed(enemyStats.speed.GetValue());
    }

    public virtual void PrepareAttackBehaviour() 
    {
        
    }
}
