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

    public AudioSource walk;
    public AudioSource dash;
    public AudioSource damage;
    public AudioSource upgrades;

    public Animator playerAnimator;

    public int Attackanim;

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
    public bool usingAbility = false;

    public bool interactableInRange = false;

    public ParticleSystem slash;
    public ParticleSystem hit;

    private Interactable interactableItem;

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
        //basicUpgrades = transform.GetChild(0).transform.GetChild(2).gameObject;
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
            dash.Play();
            invincible = true;
            SwitchMovementState(movingState.inDash);
        }
        if (dashTimeCounter < dashTime)
        {
            dashTimeCounter += 1 * Time.deltaTime;
        }
        else
        {
            invincible = false;
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
            if (Attackanim == 1)
            {
                playerAnimator.SetTrigger("AttackAnim");
            }
            else if (Attackanim == 2)
            {
                playerAnimator.SetTrigger("Attack2");
            }
            else if (Attackanim == 3)
            {
                playerAnimator.SetTrigger("Attack3");
            }
            else if (Attackanim > 3)
            {
                playerAnimator.SetTrigger("AttackAnim");
                Attackanim = 1;
            }
            print(Attackanim);
            Attackanim += 1;
        }
        SwitchAttackState(attackState.Recovery);
    }

    public override void Recovery()
    {
        if (attackTimeCounter >= attackSpeed) 
        {
            attackTimeCounter = 0f;
            mainWeapon.DesactivateHitbox(comboCounter);
            base.Recovery();
            SwitchAttackState(attackState.Preparing);
            SwitchMovementState(movingState.inMove);
            SwitchPlayerState(playerStates.Moving);
        }
        else 
        {
            attackTimeCounter += Time.deltaTime;
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
        MovementLoop();
    }

    public void MovementLoop() 
    {
        CreateMovementVector(moveSide, moveFront);
        if (moveState != movingState.inAttackDrag) 
        {
            CheckMovementState(movementVector);
        }        
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
        characterController.SimpleMove(playerModel.transform.forward * PlayerStats.current.dashSpeed.GetValue() * Time.deltaTime);
        if (CheckIdle(movementVector) == true) 
        {
            playerModel.transform.rotation = Quaternion.LookRotation(movementVector);
        }
    }

    public void InMove() 
    {
        PlayerEvents.current.Moving();
        characterController.SimpleMove(movementVector * Time.deltaTime * PlayerStats.current.speed.GetValue());
        if (movementVector != Vector3.zero && dashing == false)
        {
            
            Quaternion newLook = Quaternion.LookRotation(movementVector);
            playerModel.transform.rotation = Quaternion.Slerp(transform.rotation, newLook, Time.deltaTime * 15);
        }
    }

    public void AttackDrag() 
    {
        characterController.SimpleMove(playerModel.transform.forward * attackDrag * Time.deltaTime);        
    }

    public void CheckMovementState (Vector3 movement) 
    {
        if (CheckIdle(movement) == true) 
        {
            SwitchMovementState(movingState.inIdle);
            playerAnimator.SetBool("IsMoving", false);
            if (walk.isPlaying == true)
            {
                walk.Stop();
            }
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
                playerAnimator.SetBool("IsMoving", true);
                if (walk.isPlaying == false)
                {
                    walk.Play();
                }
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

    void OnAbility()
    {
        usingAbility = true;
    }

    void OnInteract() 
    {
        if (interactableInRange == true) 
        {
            interactableItem.interacting = true;
        }
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
            mainWeapon.DesactivateHitbox(comboCounter);
            SwitchPlayerState(playerStates.Dash);
            SwitchAttackState(attackState.Preparing);
            dashing = true;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {
            interactableInRange = true;
            interactableItem = other.gameObject.GetComponent<Interactable>();
        }
        switch (other.tag) 
        {
            case ("EnemyAttack"):
                PlayerStats.current.ReceiveDamage(other.GetComponent<MeleeCollider>().GetDamageValue());
                break;
            case ("EnemyProjectile"):
                PlayerStats.current.ReceiveDamage(other.GetComponent<Projectile>().GetDamageValue());
                break;
            case ("TrapDamage"):
                /*if (other.transform.parent.GetComponent<Trap>().isActive == true) 
                {*/
                    PlayerStats.current.ReceiveDamage(other.transform.parent.GetComponent<Trap>().damage);
                    Debug.Log("Damage from trap");
                    //other.transform.parent.GetComponent<Trap>().DisableDamage();
                /*}*/
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && moveState == movingState.inDash) 
        {
                        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable") 
        {
            interactableInRange = false;
            interactableItem = null;
        }
        
    }
    
    public void Interact() 
    {
        
    }


}
