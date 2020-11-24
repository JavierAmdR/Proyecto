using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{

    public string target;
    bool targetFound;
    GameObject closestTarget;

    Transform[] targetList;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == target) 
        {
            targetFound = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == target) 
        {
            targetFound = false;
        }
    }

    public void SetNewTarget(string newTarget) 
    {
        target = newTarget;
    }

    public bool targetInRange() 
    {
        return targetFound;
    }

    public GameObject ClosestTarget() 
    {
        return closestTarget;
    }
}
