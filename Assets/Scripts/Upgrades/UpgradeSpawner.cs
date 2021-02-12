using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSpawner : MonoBehaviour
{
    public GameObject[] upgrades;
    int seed = 0;
    public Upgrade storedUpgrade;
    public TextMeshProUGUI upgradeName;
    public TextMeshProUGUI upgradeRarity;
    public TextMeshProUGUI upgradeDescription;
    public Interactable interacting;
    public GameObject instantiatedUpgrade;
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        /*if (seed > 33) 
        {
            Instantiate(upgrades[0],transform);
        }
        else if(seed > 10) 
        {
            Instantiate(upgrades[1], transform);
        }
        else 
        {
            Instantiate(upgrades[2], transform);
        }*/
        storedUpgrade = DatabaseManager.current.GetRandomUpgrade();
        switch (storedUpgrade.upgradeRarity) 
        {
            case Upgrade.rarity.Common:
                instantiatedUpgrade = Instantiate(upgrades[0], transform);
                upgradeRarity.text = "Common";
                break;
            case Upgrade.rarity.Rare:
                instantiatedUpgrade = Instantiate(upgrades[1], transform);
                upgradeRarity.text = "Rare";
                break;
            case Upgrade.rarity.Legendary:
                instantiatedUpgrade = Instantiate(upgrades[2], transform);
                upgradeRarity.text = "Legendary";
                break;
        }
        upgradeName.text = storedUpgrade.upgradeName;
        upgradeDescription.text = storedUpgrade.description;
        
    }

    private void Update()
    {
        if (interacting.interacting == true) 
        {
            this.GetComponent<UpgradeLoader>().LoadUpgrade(storedUpgrade);
            instantiatedUpgrade.SetActive(false);
            Destroy(gameObject);
        }
    }
}
