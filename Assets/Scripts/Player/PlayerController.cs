using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public static PlayerController current;

    public Weapon mainWeapon;
    public int comboCounter = 0;

    private Vector3 movementVector;

    //public BasicUpgrade test;

    //Añadir regeneración de stamina y critico
    public int attack;
    public int health;
    public float maxHealth;
    public int defense;
    public float speed;
    public float attackSpeed;
    public float stamina;
    public float maxStamina;
    public float staminaRegeneration;
    public float dashSpeed;
    public float dashTime;
    public float dashTimeCounter;
    public int dashCost;

    public float totalDamage;

    public float attackDrag;

    private float moveSide;
    private float moveFront;
    private Quaternion newRotation;

    public float attackRange = 0.5f;

    public bool invincible = false;
    public bool attacking = false;
    public bool inAttack = false;
    public bool preparingAttack = false;
    public bool recoveringAttack = false;

    private bool interacting = false;
    bool dashing = false;
    private int frameCounter = 0;

    public GameObject basicUpgrades;
    private CharacterController characterController;
    [SerializeField] private Transform attackObject;

    [SerializeField] private Transform playerModel;

    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        current = this;
        mainWeapon = GameObject.FindGameObjectWithTag("MainWeapon").GetComponent<Weapon>();
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

    public event Action onGetDamage;
    public void GetDamage()
    {
        if (onGetDamage != null)
        {
            onGetDamage();
        }
    }

    public event Action onDie;
    public void Die()
    {
        if (onDie != null)
        {
            onDie();
        }
    }

    public event Action onHealthChange;
    public void HealthChange()
    {
        if (onHealthChange != null)
        {
            onHealthChange();
        }
    }

    public event Action onStaminaChange;
    public void StaminaChange()
    {
        if (onStaminaChange != null)
        {
            onStaminaChange();
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerModel = GetComponentInChildren<Transform>();
    }

    void Update()
    {  
       if (interacting == true) 
       {
            if (frameCounter >= 2) 
            {
                interacting = false;
                frameCounter = 0;
            }
            else 
            {
                frameCounter++;
            }
       }

       if (dashing == false && stamina < maxStamina) 
       {
            stamina += staminaRegeneration * Time.deltaTime;
            if (stamina > maxStamina) 
            {
                stamina = maxStamina;
            }
            StaminaChange();
       }
       
       if (attacking == true) 
       {
            Attack();
       }
       
    }

    private void FixedUpdate()
    {
        Move();
    }

    //INPUTS

    //Move input
    void OnMove(InputValue value)
    {
        if (preparingAttack == false && inAttack == false)
        {
            Debug.Log("Move");
            moveSide = value.Get<Vector2>().x;
            moveFront = value.Get<Vector2>().y;
        }
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

    void OnDash() 
    {
        Debug.Log("Dash");
        dashing = true;

    }

    //FUNCTIONS

    //Move function
    void Move() 
    {
        if (inAttack == false && preparingAttack == false)
        {

            if (dashing == true && stamina < dashCost && dashTimeCounter == 0)
            {
                dashing = false;
            }
            if (dashing == false && dashTimeCounter == 0)
            {
                movementVector = new Vector3(moveSide, 0, moveFront);
            }
            if (dashing == true)
            {
                if (recoveringAttack == true) 
                {
                    recoveringAttack = false;
                }
                if (movementVector == Vector3.zero)
                {
                    movementVector = transform.forward;
                }
                if (dashTimeCounter == 0)
                {
                    OnDash();
                    stamina -= dashCost;
                    if (stamina < 0)
                    {
                        stamina = 0;
                    }
                    StaminaChange();
                }
                characterController.SimpleMove(movementVector * dashSpeed * Time.deltaTime);
                if (dashTimeCounter < dashTime)
                {
                    dashTimeCounter += 1 * Time.deltaTime;
                }
                else
                {
                    dashing = false;
                    dashTimeCounter = 0f;
                }

            }
            else
            {
                current.PlayerMoving();
                characterController.SimpleMove(movementVector * Time.deltaTime * speed);
            }




            if (movementVector != Vector3.zero && dashing == false)
            {
                newRotation = Quaternion.LookRotation(movementVector);
            }
            playerModel.transform.rotation = newRotation;
        }
        
    }

    //Attack function
    void Attack() 
    {
        //test.SetUpgrade(ref attack, ref attackRange, ref attackSpeed, ref health, ref defense, ref speed, ref stamina);

        //Debug.Log(attack);

        //HACER QUE LA FUNCIÓN ACTIVE LA ANIMACIÓN Y DESPUES QUE ESTA LLAME A OTRA FUNCIÓN DE ATAQUE
        //CAMBIAR
        
        
        
        PlayerAttack();

        /*Collider[] hitEnemies = Physics.OverlapSphere(attackObject.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies) 
        {
            Debug.Log("Hit " + enemy.name);
        }*/
        attacking = false;
        //CAMBIAR
    }

    //GetDamage function
    public void Damage(int damage) 
    {
        onGetDamage();
        health -= damage;
        if (health < 0) 
        {
            health = 0;
            HealthChange();
            Dead();
        }
        else 
        {
            HealthChange();
        }
        
    }

    public void Dead() 
    {
        onDie();
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
