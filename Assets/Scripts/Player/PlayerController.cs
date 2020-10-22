using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public static PlayerController current;

    private Vector3 movementVector;

    //public BasicUpgrade test;

    //Añadir regeneración de stamina y critico
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
    private bool interacting = false;

    public GameObject basicUpgrades;
    private CharacterController characterController;
    [SerializeField] private Transform attackObject;

    [SerializeField] private Transform playerModel;

    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        current = this;
        basicUpgrades = transform.GetChild(0).transform.GetChild(2).gameObject;
    }

    //Event Actions

    public event Action onPlayerAttack;
    public void PlayerAttack() 
    {
        if (onPlayerAttack != null) 
        {
            onPlayerAttack();
        }
    }

    public event Action onPlayerDamaged;
    public void PlayerDamaged()
    {
        if (onPlayerDamaged != null)
        {
            onPlayerDamaged();
        }
    }

    public event Action onPlayerMove;
    public void PlayerMoving()
    {
        if (onPlayerMove != null)
        {
            onPlayerMove();
        }
    }

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
        Debug.Log(value);
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

    void OnInteract() 
    {
        Debug.Log("Interact");
        interacting = true;
    }

    //FUNCTIONS

    //Move function
    void Move() 
    {
        movementVector = new Vector3(moveSide, 0, moveFront);
        characterController.SimpleMove(movementVector * Time.deltaTime * speed);

        current.PlayerMoving();

        if (movementVector != Vector3.zero)
        {
            newRotation = Quaternion.LookRotation(movementVector);
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
        current.PlayerAttack();

        Collider[] hitEnemies = Physics.OverlapSphere(attackObject.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies) 
        {
            Debug.Log("Hit " + enemy.name);
        }
        attacking = false;
        //CAMBIAR
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Upgrade" && interacting == true)
        {
            other.gameObject.GetComponent<BasicUpgrade>().SetUpgrade(ref attack, ref attackRange, ref attackSpeed, ref health, ref defense, ref speed, ref stamina);         
        }
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
