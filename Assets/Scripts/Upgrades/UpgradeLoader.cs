using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLoader : MonoBehaviour
{
    public GameObject upgradeSlot;

    Upgrade storedUpgrade;
    public void LoadUpgrade(Upgrade newUpgrade) 
    {
        storedUpgrade = newUpgrade;
        switch (newUpgrade.triggerType) 
        {
            case Upgrade.trigger.OnObtained:
                SetObtainUpgrade(newUpgrade);
                break;
            case Upgrade.trigger.Hit:
                SetHitUpgrade(newUpgrade);
                break;
        }
    }

    private void SetObtainUpgrade(Upgrade newUpgrade) 
    {
        switch (newUpgrade.relatedStat) 
        {
            case Upgrade.stat.Health:
                PlayerStats.current.health.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Attack:
                PlayerStats.current.attack.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Defense:
                PlayerStats.current.defense.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Speed:
                PlayerStats.current.speed.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Stamina:
                PlayerStats.current.stamina.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.StaminaReg:
                PlayerStats.current.staminaReg.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Crit:
                PlayerStats.current.crit.AddModifier(newUpgrade.value);
                break;
            case Upgrade.stat.Poison:
                PlayerStats.current.poison.AddModifier(newUpgrade.value);
                break;
        }        
    }

    private void SetHitUpgrade(Upgrade upgrade) 
    {
        
    }

    public void UpgradeHit() 
    {
        
    }
}
