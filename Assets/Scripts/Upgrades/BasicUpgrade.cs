using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUpgrade : MonoBehaviour
{
    public string upgradeName;

    public int attack;
    public float attackRange;
    public float attackSpeed;
    public int health;
    public int defense;
    public float speed;    
    public int stamina;
    

    public void SetUpgrade(ref int playerAttack, ref float playerRangeAttack, ref float playerAttackSpeed, ref int playerHealth, ref int playerDefense, ref float playerSpeed, ref int playerStamina) 
    {
        playerAttack += attack;
        playerRangeAttack += attackRange;
        playerAttackSpeed += attackSpeed;
        playerHealth += health;
        playerDefense += defense;
        playerSpeed += speed;
        playerStamina += stamina;

        Debug.Log("Applied" + upgradeName);
    }

}
