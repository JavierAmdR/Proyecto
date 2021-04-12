using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{

    private void Awake()
    {
        DisableCollider();
    }

    public void DisableCollider() 
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    public void EnableCollider() 
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            GameManager.current.LoadSceneRandomZone1();
            DisableCollider();
        }
    }
}
