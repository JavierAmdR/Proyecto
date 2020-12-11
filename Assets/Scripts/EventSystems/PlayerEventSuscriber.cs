using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventSuscriber : MonoBehaviour
{
    public virtual void Suscribe()
    {
        PlayerEvents.current.onAttack += Effect;
    }

    public virtual void Effect() 
    {
        
    }
}
