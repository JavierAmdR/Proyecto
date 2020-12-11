using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    public GameObject[] upgrades;
    int seed = 0;
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        seed = Random.Range(1, 101);
        if (seed > 33) 
        {
            Instantiate(upgrades[0],transform);
        }
        else if(seed > 10) 
        {
            Instantiate(upgrades[1], transform);
        }
        else 
        {
            Instantiate(upgrades[2], transform);
        }
    }
}
