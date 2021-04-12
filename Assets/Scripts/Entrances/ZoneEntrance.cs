using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneEntrance : MonoBehaviour
{
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
        DisableCollider();
        Random.InitState(System.DateTime.Now.Millisecond);
        GameManager.current.LoadSceneRandomZone1();
    }
}
