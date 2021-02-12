using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Stat
{
    public int baseValue;

    public List<int> modifiersList = new List<int>();
    public string statName;
    public string statDescription;
    public int finalValue;
    public int levelValueAdd;
    public int levelNumber = 0;

    public bool modified = false;



    public int GetValue() 
    {
        if (modified == false)
        {
            return baseValue;
        }
        else
        {
            return finalValue;
        }
    }

    public int GetBaseValue() 
    {
        return baseValue;
    }

    public void RecalculateValue() 
    {
        finalValue = baseValue;
        modifiersList.ForEach(x => finalValue += x);
        finalValue += levelNumber * levelValueAdd;
    }

    public void AddLevel() 
    {
        levelNumber += 1;
    }

    public void AddModifier (int modifier) 
    {
        if (modifier != 0) 
        {
            modifiersList.Add(modifier);
            RecalculateValue();
            finalValue += modifier;
            modified = true;
        }
    }

    public void RemoveModifier (int modifier) 
    {
        if (modifier != 0) 
        {
            modifiersList.Remove(modifier);
            finalValue -= modifier;
            modified = true;
        } 
    }

}
