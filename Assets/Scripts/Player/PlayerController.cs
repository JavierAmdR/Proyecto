﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector3 movementVector;

    //public BasicUpgrade test;

    public int attack;
    public int health;
    public int defense;
    public float speed;
    public float attackSpeed;
    public int stamina;

    private float moveSide;
    private float moveFront;
    private Quaternion newRotation;

    public float attackRange = 0.5f;
    

    private bool attacking = false;
    
    private CharacterController characterController;
    [SerializeField] private Transform attackObject;

    [SerializeField] private Transform playerModel;

    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerModel = GetComponentInChildren<Transform>();
    }

    void Update()
    {        
       Move();
       if (attacking == true) 
        {
            Attack();
        }
    }
    

    //INPUTS

    //Move input
    void OnMove(InputValue value) 
    {
        Debug.Log("Move");
        moveSide = value.Get<Vector2>().x;
        moveFront = value.Get<Vector2>().y;
    }

    //Attack input
    void OnAttack()
    {
        Debug.Log("Attack");
        attacking = true;
    }

    //FUNCTIONS

    //Move function
    void Move() 
    {
        movementVector = new Vector3(moveSide, 0, moveFront);
        characterController.Move(movementVector * Time.deltaTime * speed);
        newRotation = Quaternion.Euler(0, Vector3.Angle(new Vector3(0, 0, 1), movementVector),0);
        if (movementVector.y < 0) 
        {
            
        }
        playerModel.transform.rotation = newRotation;
    }

    //Attack function
    void Attack() 
    {
        //test.SetUpgrade(ref attack, ref attackRange, ref attackSpeed, ref health, ref defense, ref speed, ref stamina);

        //Debug.Log(attack);

        //HACER QUE LA FUNCIÓN ACTIVE LA ANIMACIÓN Y DESPUES QUE ESTA LLAME A OTRA FUNCIÓN DE ATAQUE
        //CAMBIAR
        Collider[] hitEnemies = Physics.OverlapSphere(attackObject.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies) 
        {
            Debug.Log("Hit " + enemy.name);
        }
        attacking = false;
        //CAMBIAR
    }

    //EDITOR

    private void OnDrawGizmosSelected()
    {
        if (attackObject == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackObject.position, attackRange);
    }


}
