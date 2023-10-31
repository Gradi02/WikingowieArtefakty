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

    private bool bar1 = false;
    private bool bar2 = true;
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

        if (Input.GetKey(KeyCode.Alpha1))
        {
            bar1= true;
            bar2= false;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            bar1 = false;
            bar2 = true;
        }

        if (scrollInput > 0 && bar1)
        {
            for (int i = 0; i < eqFight.Count; i++)
            {
                eqFight[i].gameObject.SetActive(false);
            }
            slotFight++;
            if(slotFight>eqFight.Count-1)slotFight = 0;
            eqFight[slotFight].gameObject.SetActive(true);
        }

        else if (scrollInput < 0 && bar1)
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
        if (scrollInput > 0 && bar2)
        {
            for (int i = 0; i < eqBuild.Count; i++)
            {
                eqBuild[i].gameObject.SetActive(false);
            }
            slotBuild++;
            if (slotBuild > eqBuild.Count - 1) slotBuild = 0;
            eqBuild[slotBuild].gameObject.SetActive(true);
        }

        else if (scrollInput < 0 && bar2)
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
