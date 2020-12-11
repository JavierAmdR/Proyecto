using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : Character
{

    public static PlayerController current;
    public enum playerStates {Moving, Attack, Dash, Death, Interacting, Ability}
    public playerStates mainState;
    public enum movingState {inIdle, inMove, inDash, inAttackDrag}
    public movingState moveState;


    public Weapon mainWeapon;
    public int comboCounter = 0;

    private Vector3 movementVector;
    private float attackTimeCounter = 0;
    public float inAttackTime = 0;
    public float recoveryTime = 0;

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
    public CharacterController characterController;
    [SerializeField] private Transform attackObject;

    [SerializeField] private Transform playerModel;

    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainWeapon = GameObject.FindGameObjectWithTag("MainWeapon").GetComponent<Weapon>();
        basicUpgrades = transform.GetChild(0).transform.GetChild(2).gameObject;
        DontDestroyOnLoad(this);       
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerModel = GetComponentInChildren<Transform>();
    }

    public override void CharacterLoop()
    {
        if (UIManager.ui.gamePaused == false)
        {
            switch (mainState)
            {
                case playerStates.Moving:
                    Moving();
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

    public override void Moving() 
    {
        PlayerStats.current.StaminaReg();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public void Dash()
    {
        if (dashTimeCounter == 0)
        {
            OnDash();
            PlayerStats.current.StaminaDash();
            SwitchMovementState(movingState.inDash);
        }
        if (dashTimeCounter < dashTime)
        {
            dashTimeCounter += 1 * Time.deltaTime;
        }
        else
        {
            dashing = false;
            dashTimeCounter = 0f;
            SwitchPlayerState(playerStates.Moving);
            SwitchMovementState(movingState.inMove);
        }
    }

    //ATTACK STATES
    public override void PreparingAttack()
    {
        base.PreparingAttack();
        SwitchAttackState(attackState.Attack);
    }

    public override void InAttack()
    {
        base.InAttack();
        if (moveState != movingState.inAttackDrag) 
        {
            mainWeapon.Attack(comboCounter);
            mainWeapon.ActivateHitbox(comboCounter);
            SwitchMovementState(movingState.inAttackDrag);
        }
        SwitchAttackState(attackState.Recovery);
    }

    public override void Recovery()
    {

        mainWeapon.DesactivateHitbox(comboCounter);
        base.Recovery();
        SwitchAttackState(attackState.Preparing);
        SwitchMovementState(movingState.inMove);
        SwitchPlayerState(playerStates.Moving);
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
        MovementLoop();
    }

    public void MovementLoop() 
    {
        CreateMovementVector(moveSide, moveFront);
        CheckMovementState(movementVector);
        switch (moveState)
        {
            case movingState.inIdle:
                InMove();
                break;
            case movingState.inDash:
                InDash();
                break;
            case movingState.inMove:
                InMove();
                break;
            case movingState.inAttackDrag:
                AttackDrag();
                break;
        }
    }

    public void InDash() 
    {
        characterController.SimpleMove(transform.forward * PlayerStats.current.dashSpeed.GetValue() * Time.deltaTime);
    }

    public void InMove() 
    {
        PlayerEvents.current.Moving();
        characterController.SimpleMove(movementVector * Time.deltaTime * PlayerStats.current.speed.GetValue());
        if (movementVector != Vector3.zero && dashing == false)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(movementVector);
        }        
    }

    public void AttackDrag() 
    {
        characterController.SimpleMove(transform.forward * attackDrag * Time.deltaTime);
    }

    public void CheckMovementState (Vector3 movement) 
    {
        if (CheckIdle(movement) == true) 
        {
            SwitchMovementState(movingState.inIdle);
        }
        else 
        {
            if (dashing == true) 
            {
                SwitchMovementState(movingState.inDash);
            }
            else 
            {
                SwitchMovementState(movingState.inMove);
            }
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
        if (UIManager.ui.gamePaused == false && (mainState == playerStates.Moving || attackStatus == attackState.Recovery))
        {
            /*if (attacking == false)
            {
                attacking = true;
            }
            else if (comboCounter < mainWeapon.comboNumber)
            {
                nextAttack = true;
            }*/
            SwitchPlayerState(playerStates.Attack);
        }
        
    }

    void OnInteract() 
    {
        interacting = true;
    }

    void OnGodMode() 
    {
        PlayerStats.current.godMode = !PlayerStats.current.godMode;
        GameManager.current.godModeUI.SetActive(PlayerStats.current.godMode);
    }

    void OnDash() 
    {
        if ((mainState == playerStates.Moving || attackStatus == attackState.Recovery) && PlayerStats.current.CanDash() == true) 
        {
            SwitchPlayerState(playerStates.Dash);
            dashing = true;
        }
    }

    //FUNCTIONS

    //Move function
    void Move() 
    {
        if (attacking == true) 
        {
            
            inAttack = true;
            attacking = false;
        }
        if (inAttack == true) 
        {
                  
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
    /*void Attack() 
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
    //}
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
            case ("Trap"):
                if (other.GetComponent<Trap>().isActive == true) 
                {
                    PlayerStats.current.ReceiveDamage(other.GetComponent<Trap>().damage);
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.tag) 
        {
            case ("Trap"):
                if (other.GetComponent<Trap>().isActive == true) 
                {
                    PlayerStats.current.ReceiveDamage(other.GetComponent<Trap>().damage);
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && moveState == movingState.inDash) 
        {
            
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
