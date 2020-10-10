using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector3 movementVector;

    private float moveSide;
    private float moveFront;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float speed;

    private bool attacking = false;
    
    private CharacterController characterController;
    [SerializeField] private Transform attackObject;

    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        attackObject = GetComponentInChildren<Transform>();
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
    }

    //Attack function
    void Attack() 
    {
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
