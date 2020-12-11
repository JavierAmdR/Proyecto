﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            GameManager.current.LoadSceneRandomZone1();
        }        
    }
}
