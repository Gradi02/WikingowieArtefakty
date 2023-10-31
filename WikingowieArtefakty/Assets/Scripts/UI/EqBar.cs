using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EqBar : MonoBehaviour
{
    public List<Image> eqFight;
    public List<Image> eqBuild;
    public int slotFight;
    public int slotBuild;
    void Start()
    {
        for(int i = 0; i<eqFight.Count; i++)
        {
            eqFight[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < eqBuild.Count; i++)
        {
            eqBuild[i].gameObject.SetActive(false);
        }
    }

   
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0)
        {
            for (int i = 0; i < eqFight.Count; i++)
            {
                eqFight[i].gameObject.SetActive(false);
            }
            slotFight++;
            if(slotFight>eqFight.Count-1)slotFight = 0;
            eqFight[slotFight].gameObject.SetActive(true);
        }

        else if (scrollInput < 0)
        {
            for (int i = 0; i < eqFight.Count; i++)
            {
                eqFight[i].gameObject.SetActive(false);
            }
            slotFight--;
            if (slotFight < 0) slotFight = eqFight.Count-1;
            eqFight[slotFight].gameObject.SetActive(true);
        }
        //BUILD EQ
        if (scrollInput > 0)
        {
            for (int i = 0; i < eqBuild.Count; i++)
            {
                eqBuild[i].gameObject.SetActive(false);
            }
            slotBuild++;
            if (slotBuild > eqBuild.Count - 1) slotBuild = 0;
            eqBuild[slotBuild].gameObject.SetActive(true);
        }

        else if (scrollInput < 0)
        {
            for (int i = 0; i < eqBuild.Count; i++)
            {
                eqBuild[i].gameObject.SetActive(false);
            }
            slotBuild--;
            if (slotBuild < 0) slotBuild = eqBuild.Count - 1;
            eqBuild[slotBuild].gameObject.SetActive(true);
        }
    }
}
