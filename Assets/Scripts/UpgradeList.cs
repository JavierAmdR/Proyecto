using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeList : MonoBehaviour
{
    public List<Upgrade> obtainedUpgrades;
    public static UpgradeList current;

    private void Awake()
    {
        if (UpgradeList.current == null) 
        {
            UpgradeList.current = this;
        }
    }

    public void AddUpgrade(Upgrade newUpgrade) 
    {
        obtainedUpgrades.Add(newUpgrade);
    }
}
