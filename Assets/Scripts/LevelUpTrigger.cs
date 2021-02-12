using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTrigger : MonoBehaviour
{
    public Interactable interacting;

    private void Update()
    {
        if (interacting.interacting == true && UIManager.ui.levelUpPanel.activeSelf == false) 
        {
            UIManager.ui.OpenLevelUpPanel();
            interacting.interacting = false;
        }
    }

}
