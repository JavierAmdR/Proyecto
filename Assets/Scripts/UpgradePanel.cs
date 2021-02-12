using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    public GameObject upgradeStorage;
    public GameObject upgradeSlot;
    private GameObject newSlot;

    public void LoadUpgrades() 
    {
        foreach (Transform child in upgradeStorage.GetComponentInChildren<Transform>()) 
        {
            Destroy(child.gameObject);
        }
        foreach (Upgrade upgrade in UpgradeList.current.obtainedUpgrades)
        {
            newSlot = Instantiate(upgradeSlot, upgradeStorage.transform);
            newSlot.SetActive(true);
            newSlot.transform.Find("UpgradeName").GetComponent<TextMeshProUGUI>().text = upgrade.upgradeName;
            switch (upgrade.upgradeRarity) 
            {
                case Upgrade.rarity.Common:
                    newSlot.transform.Find("UpgradeRarity").GetComponent<TextMeshProUGUI>().text = "Common";
                    break;
                case Upgrade.rarity.Rare:
                    newSlot.transform.Find("UpgradeRarity").GetComponent<TextMeshProUGUI>().text = "Rare";
                    break;
                case Upgrade.rarity.Legendary:
                    newSlot.transform.Find("UpgradeRarity").GetComponent<TextMeshProUGUI>().text = "Legendary";
                    break;
            }
            newSlot.transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = upgrade.description;

        }
    }
}
