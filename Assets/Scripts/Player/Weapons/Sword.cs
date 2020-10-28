using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.current.onPlayerAttack += Attack;
    }

    public void Attack() 
    {        
        PlayerController.current.health++;
        Debug.Log("Sword Attack!");
        Debug.Log(PlayerController.current.health);
    }
}
