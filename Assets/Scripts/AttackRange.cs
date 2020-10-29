using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{

    public MeleeEnemy parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Debug.Log("Collision Detected");
            parent.enemyState = MeleeEnemy.state.Attack;
        }
    }
}
