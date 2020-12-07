using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.current.LoadScene(2);
    }
}
