using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        GameManager.current.LoadScene(Random.Range(3,6));
    }
}
