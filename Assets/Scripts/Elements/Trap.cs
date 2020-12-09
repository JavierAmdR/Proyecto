using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Animator spikes;
    public int damage;
    public bool isActive = false;
    public bool onCooldown = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && onCooldown == false) 
        {
            onCooldown = true;
            spikes.SetTrigger("TrapTrigger");
        }
    }
}
