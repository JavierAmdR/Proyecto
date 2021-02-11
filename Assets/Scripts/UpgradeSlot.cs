using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    public Upgrade upgradeStored;

    public void StoreUpgrade(Upgrade newUpgrade) 
    {
        upgradeStored = newUpgrade;
    }
}
