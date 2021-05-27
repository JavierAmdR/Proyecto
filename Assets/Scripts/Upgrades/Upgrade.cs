using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Assets/Upgrade")]
public class Upgrade : ScriptableObject
{
    public enum trigger {Dash, Hit, Area, HealthLose, Walk, Idle, OnObtained}
    public enum rarity {Common, Rare, Legendary}
    public enum stat {Health, Attack, Defense, Speed, Stamina, StaminaReg, Crit, Poison, Stun}

    public string id;
    public rarity upgradeRarity;
    public string upgradeName;
    public string description;
    public stat relatedStat;
    public int value;
    public stat secondStat;
    public int secondValue;
    public stat thirdStat;
    public int thirdValue;
    public trigger triggerType;
    public bool isTriggerArea;
    public float areaSize;
    public float timeBetweenTrigger;

    //public Dictionary<string, int> values = new Dictionary<string, int>();
}
