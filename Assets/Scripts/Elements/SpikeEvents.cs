using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEvents : MonoBehaviour
{
    public Trap trap;

    public void SwitchActive() 
    {
        trap.isActive = !trap.isActive;
    }
    public void RestoreCooldown() 
    {
        trap.onCooldown = false;
    }
}
