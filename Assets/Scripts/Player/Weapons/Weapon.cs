using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject[] comboHitbox;
    public float[] attackDamage;
    public float[] attackDrag;

    int comboNumber = 0;


    // Start is called before the first frame update
    
    public void Attack()
    {        
        
        Debug.Log("Sword Attack!");
    }
}
