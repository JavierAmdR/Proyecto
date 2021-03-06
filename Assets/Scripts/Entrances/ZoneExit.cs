﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.current.LoadVictory();
        }
    }

    public void EnableCollider() 
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
