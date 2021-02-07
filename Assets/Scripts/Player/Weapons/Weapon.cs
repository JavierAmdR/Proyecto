using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject[] comboHitbox;
    public float[] attackDamage;
    public float[] attackDrag;
    public float[] dragTime;

    public int comboNumber = 0;


    // Start is called before the first frame update
    
    public void Attack(int comboIndex)
    {
        //PlayerController.current.attackDrag += attackDrag[comboIndex];
        PlayerController.current.slash.Clear();
        PlayerController.current.slash.Play(true);
        Debug.Log("Sword Attack!");
    }

    public void ActivateHitbox(int comboIndex) 
    {
        comboHitbox[comboIndex].SetActive(true);
    }

    public void DesactivateHitbox(int comboIndex)
    {
        comboHitbox[comboIndex].SetActive(false);
    }
}
