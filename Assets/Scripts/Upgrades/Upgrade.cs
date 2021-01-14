using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Assets/Upgrade")]
public class Upgrade : ScriptableObject
{
    public int id;
    public string upgradeName;
    public string description;

    public Dictionary<string, int> values = new Dictionary<string, int>();
}
