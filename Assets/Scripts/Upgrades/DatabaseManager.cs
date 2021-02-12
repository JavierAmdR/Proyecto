using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{

    public UpgradesDB upgradeDB;

    public static DatabaseManager current;
    // Start is called before the first frame update

    private void Awake()
    {
        if (current == null) 
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    public Upgrade GetUpgradeByID (string ID) 
    {
        return current.upgradeDB.allUpgrades.FirstOrDefault(i => i.id == ID);
        /*foreach(Upgrade upgrade in current.upgradeDB.allUpgrades) 
        {
            if (upgrade.id == ID) 
            {
                return upgrade;
            }
        }
        return null;*/
    }

    public Upgrade GetRandomUpgrade()
    {
        return current.upgradeDB.allUpgrades[Random.Range(0, current.upgradeDB.allUpgrades.Count())];
        /*foreach(Upgrade upgrade in current.upgradeDB.allUpgrades) 
        {
            if (upgrade.id == ID) 
            {
                return upgrade;
            }
        }
        return null;*/
    }
}
