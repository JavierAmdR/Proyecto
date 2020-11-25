using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public int damage;

    public virtual int GetDamageValue() 
    {
        return damage;
    }

    public virtual void SetDamage(int newDamage) 
    {
        damage = newDamage;
    }
}
