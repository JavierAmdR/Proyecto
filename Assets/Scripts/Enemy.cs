using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public int health;
    public float speed;
    public float attackSpeed;

    public LayerMask Target;

    Transform objective;

    public void Start() 
    {
        objective = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public abstract void Attack();
    public abstract void Death();
    public abstract void Movement();


}
