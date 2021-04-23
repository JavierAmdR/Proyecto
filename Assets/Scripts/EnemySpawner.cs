using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rangedGroup;
    public GameObject meleeGroup;
    GameObject actualMeleeGroup;
    GameObject actualRangedGroup;

    public float timeToSpawn = 10f;
    private float timeCounter = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.current.enemiesInRoom != 0)
        {
            if (timeCounter < timeToSpawn)
            {
                timeCounter += Time.deltaTime;
            }
            else
            {
                timeCounter = 0;
                if (actualMeleeGroup == null)
                {
                    actualMeleeGroup = Instantiate(meleeGroup);
                }
                if (actualRangedGroup == null)
                {
                    actualRangedGroup = Instantiate(rangedGroup);
                }
            }
        }
        
    }
}
