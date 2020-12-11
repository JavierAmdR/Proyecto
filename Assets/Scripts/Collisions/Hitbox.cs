using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public EntityEvents eventSystem;

    private void Awake()
    {
        eventSystem = gameObject.GetComponentInParent<EntityEvents>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case ("PlayerAttack"):
                eventSystem.Damaged();
                break;
        }
    }
}
