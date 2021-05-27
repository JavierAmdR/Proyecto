using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLoader : MonoBehaviour
{

    Upgrade storedUpgrade;
    public void LoadUpgrade(Upgrade newUpgrade) 
    {
        storedUpgrade = newUpgrade;
        PlayerController.current.upgrades.Play();
        GameManager.current.AddUpgradesObtained();
        UpgradeList.current.AddUpgrade(newUpgrade);
        GameManager.current.ActivateExit();
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
                PlayerStats.current.currentHealth += (newUpgrade.value);
                UIManager.ui.HealthUpdate();
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
        if (newUpgrade.secondValue != 0)
        {
            switch (newUpgrade.secondStat)
            {
                case Upgrade.stat.Health:
                    PlayerStats.current.health.AddModifier(newUpgrade.secondValue);
                    PlayerStats.current.currentHealth += (newUpgrade.secondValue);
                    UIManager.ui.HealthUpdate();
                    break;
                case Upgrade.stat.Attack:
                    PlayerStats.current.attack.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.Defense:
                    PlayerStats.current.defense.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.Speed:
                    PlayerStats.current.speed.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.Stamina:
                    PlayerStats.current.stamina.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.StaminaReg:
                    PlayerStats.current.staminaReg.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.Crit:
                    PlayerStats.current.crit.AddModifier(newUpgrade.secondValue);
                    break;
                case Upgrade.stat.Poison:
                    PlayerStats.current.poison.AddModifier(newUpgrade.secondValue);
                    break;
            }
        }
        if (newUpgrade.thirdValue != 0)
        {
            switch (newUpgrade.thirdStat)
            {
                case Upgrade.stat.Health:
                    PlayerStats.current.health.AddModifier(newUpgrade.thirdValue);
                    PlayerStats.current.currentHealth += (newUpgrade.thirdValue);
                    UIManager.ui.HealthUpdate();
                    break;
                case Upgrade.stat.Attack:
                    PlayerStats.current.attack.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.Defense:
                    PlayerStats.current.defense.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.Speed:
                    PlayerStats.current.speed.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.Stamina:
                    PlayerStats.current.stamina.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.StaminaReg:
                    PlayerStats.current.staminaReg.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.Crit:
                    PlayerStats.current.crit.AddModifier(newUpgrade.thirdValue);
                    break;
                case Upgrade.stat.Poison:
                    PlayerStats.current.poison.AddModifier(newUpgrade.thirdValue);
                    break;
            }
        }
    }

    private void SetHitUpgrade(Upgrade upgrade) 
    {
        
    }

    public void UpgradeHit() 
    {
        
    }
}
