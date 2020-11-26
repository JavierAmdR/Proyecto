using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : Character
{

    public static PlayerController current;
    public enum playerStates {Moving, Attack, Dash, Death, Interacting}
    public playerStates mainState;
    public enum movingState {inIdle, inMove, inDash}
    public movingState moveState;


    public Weapon mainWeapon;
    public int comboCounter = 0;

    private Vector3 movementVector;

    public float attackSpeed;
    public float dashTime;
    public float dashTimeCounter;
    public float totalDamage;

    public float attackDrag;
    
    public float dragCounter = 0;

    private float moveSide;
    private float moveFront;
    private Quaternion newRotation;

    public float attackRange = 0.5f;

    public bool invincible = false;
    public bool attacking = false;
    public bool inAttack = false;
    public bool nextAttack = false;
    public bool preparingAttack = false;
    public bool recoveringAttack = false;

    public ParticleSystem slash;

    private IInteractable interactable;

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
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerModel = GetComponentInChildren<Transform>();
    }
    void Update()
    {
        if (UIManager.ui.gamePaused == false)
        {
            /*if (interacting == true)
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
            }*/

            if (dashing == false)
            {
                PlayerStats.current.StaminaReg();
            }

            if (attacking == true)
            {
                Attack();
            }
        }
       
    }

    public override void CharacterLoop()
    {
        if (UIManager.ui.gamePaused == false)
        {
            switch (mainState)
            {
                case playerStates.Moving:
                    Move();
                    break;
                case playerStates.Attack:
                    Attack();
                    break;
                case playerStates.Dash:
                    Dash();
                    break;
                case playerStates.Death:
                    Death();
                    break;
            }
        }        
    }


    public void Dash() 
    {
        if (movementVector == Vector3.zero)
        {
            movementVector = transform.forward;
        }
        if (dashTimeCounter == 0)
        {
            OnDash();
            PlayerStats.current.StaminaDash();
        }
        characterController.SimpleMove(movementVector * PlayerStats.current.dashSpeed.GetValue() * Time.deltaTime);
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

    public void SwitchPlayerState (playerStates newState) 
    {
        mainState = newState;
    }

    public void SwitchMovementState(movingState newState) 
    {
        moveState = newState;
    }



    private void FixedUpdate()
    {
        CreateMovementVector(moveSide, moveFront);
        if (movementVector == Vector3.zero && dashing == false) 
        {
            moveState = movingState.inIdle;
        }
        switch (moveState) 
        {
            case movingState.inIdle:
                break;
            case movingState.inDash:
                break;
            case movingState.inMove:
                Move();
                break;
        }
    }



    public bool CheckIdle (Vector3 movement) 
    {
        if (movementVector == Vector3.zero && dashing == false) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public void CreateMovementVector(float x, float y) 
    {
        movementVector = new Vector3(moveSide, 0, moveFront);
    }

    //INPUTS

    //Move input
    void OnMove(InputValue value)
    {        
        moveSide = value.Get<Vector2>().x;
        moveFront = value.Get<Vector2>().y;
    }

    //Attack input
    void OnAttack()
    {
        if (UIManager.ui.gamePaused == false)
        {
            if (attacking == false)
            {
                attacking = true;
            }
            else if (comboCounter < mainWeapon.comboNumber)
            {
                nextAttack = true;
            }
        }
        
    }

    void OnInteract() 
    {
        interacting = true;
    }

    void OnDash() 
    {
        dashing = true;
    }

    //FUNCTIONS

    //Move function
    void Move() 
    {
        if (attacking == true) 
        {
            mainWeapon.Attack(comboCounter);
            inAttack = true;
            attacking = false;
        }
        if (inAttack == true) 
        {
            characterController.SimpleMove(transform.forward * attackDrag * Time.deltaTime);       
            if (dragCounter < mainWeapon.dragTime[comboCounter]) 
            {
                dragCounter += 1 * Time.deltaTime;
            }
            else 
            {
                dragCounter = 0;
                attackDrag = 0;
                inAttack = false;
            }
        }
        if (inAttack == false)
        {

            if (dashing == true && PlayerStats.current.CanDash() == false && dashTimeCounter == 0)
            {
                dashing = false;
            }
            if (dashing == false)
            {
                movementVector = new Vector3(moveSide, 0, moveFront);
            }
            if (dashing == true)
            {
                
                if (movementVector == Vector3.zero)
                {
                    movementVector = transform.forward;
                }
                if (dashTimeCounter == 0)
                {
                    OnDash();
                    PlayerStats.current.StaminaDash();
                }
                characterController.SimpleMove(movementVector * PlayerStats.current.dashSpeed.GetValue() * Time.deltaTime);
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
                PlayerEvents.current.Moving();
                characterController.SimpleMove(movementVector * Time.deltaTime * PlayerStats.current.speed.GetValue());
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
        
        
        
        PlayerEvents.current.Attack();

        /*Collider[] hitEnemies = Physics.OverlapSphere(attackObject.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies) 
        {
            Debug.Log("Hit " + enemy.name);
        }*/
        //CAMBIAR
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Upgrade" || other.tag == "NPC" || other.tag == "Door")
        {
            interactable = GetComponent<IInteractable>();
        }
        switch (other.tag) 
        {
            case ("EnemyAttack"):
                PlayerStats.current.ReceiveDamage(other.GetComponent<MeleeCollider>().GetDamageValue());
                break;
            case ("EnemyProjectile"):
                PlayerStats.current.ReceiveDamage(other.GetComponent<Projectile>().GetDamageValue());
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Upgrade" || other.tag == "NPC" || other.tag == "Door") 
        {
            interactable = null;
        }
        
    }
    
    public void Interact() 
    {
        interactable.Interact();
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
