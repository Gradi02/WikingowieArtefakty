using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeClick : MonoBehaviour
{
    public Image line1;
    public Image line2;
    public Image UpgradeBGC;
    public Image Upgrade;
    public ParticleSystem PSys;
    public GameObject bought;


    public bool unlockedd;
    public GameObject[] unlocked_nodes;

    public void ClickUpgrade()
    {
        foreach (GameObject u in unlocked_nodes)
        {
            if (u != null) 
            {
                if (u.activeSelf)
                {
                    unlockedd = true;
                }
            }
        }

        if (!UpgradeBGC.gameObject.activeSelf) { 
            if (unlockedd)
                {
                        bought.SetActive(true);
                        Upgrade.color = Color.white;
                        if (line1 != null) line1.color = Color.white;
                        if (line2 != null) line2.color = Color.white;
                        UpgradeBGC.gameObject.SetActive(true);
                        UpgradeBGC.color = Color.cyan;
                        PSys.Play();
                }
        }
    }
}

