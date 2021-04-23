using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Animator spikes;
    public int damage;
    public bool isActive = false;
    public bool onCooldown = false;
    public float timeToActive = 1.2f;
    public float timeToRetreat = 1.2f;
    public float timeBeforeCD = 3f;
    public float counter = 0f;
    public AudioSource trapSound;
    bool activatingTrap = false;
    bool retreatingTrap = false;
    public Collider trapDamage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && onCooldown == false) 
        {
            onCooldown = true;
            activatingTrap = true;
        }
    }

    private void Update()
    {
        if (onCooldown == true) 
        {
            ActivatingTrap();
            RetreatingTrap();
            SetCD();
            counter += Time.deltaTime;
        }
    }

    public void ActivatingTrap()
    {
        if (activatingTrap == true && counter >= timeToActive)
        {
            counter = 0f;
            activatingTrap = false;
            retreatingTrap = true;
            spikes.SetTrigger("TrapTrigger");
            trapSound.Play();
        }
    }

    public void RetreatingTrap() 
    {
        if (retreatingTrap == true && counter >= timeToRetreat) 
        {
            counter = 0f;
            retreatingTrap = false;
            spikes.SetTrigger("TrapRetreat");
            DisableDamage();
        }
        if (retreatingTrap == true && counter >= 0.3) 
        {
            ActivateDamage();
        }
    }

    public void SetCD()
    {
        if (retreatingTrap == false && activatingTrap == false && onCooldown == true && counter < timeToRetreat)
        {
            counter = 0f;
            onCooldown = false;
        }
    }

    public void ActivateDamage() 
    {
        trapDamage.enabled = true;
    }

    public void DisableDamage() 
    {
        trapDamage.enabled = false;
    }
}
