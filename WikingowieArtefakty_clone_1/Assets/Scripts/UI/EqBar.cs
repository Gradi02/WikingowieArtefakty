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
        SetDefaultSlotSelected();
    }

   
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.R))
        {
            bar1= !bar1;
            bar2= !bar2;
            SetDefaultSlotSelected();
        }

        if (scrollInput < 0 && bar1)
        {
            for (int i = 0; i < eqFight.Count; i++)
            {
                eqFight[i].gameObject.SetActive(false);
            }
            slotFight++;
            if(slotFight>eqFight.Count-1)slotFight = 0;
            eqFight[slotFight].gameObject.SetActive(true);
        }

        else if (scrollInput > 0 && bar1)
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
        if (scrollInput < 0 && bar2)
        {
            for (int i = 0; i < eqBuild.Count; i++)
            {
                eqBuild[i].gameObject.SetActive(false);
            }
            slotBuild++;
            if (slotBuild > eqBuild.Count - 1) slotBuild = 0;
            eqBuild[slotBuild].gameObject.SetActive(true);
        }

        else if (scrollInput > 0 && bar2)
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

    private void SetDefaultSlotSelected()
    {
        if(bar1)
        {
            eqBuild[slotBuild].gameObject.SetActive(false);
            eqFight[slotFight].gameObject.SetActive(true);
        }
        else if(bar2)
        {
            eqFight[slotFight].gameObject.SetActive(false);
            eqBuild[slotBuild].gameObject.SetActive(true);
        }
    }

    public int GetSelectedSlot()
    {
        if(bar1)
        {
            return slotFight;
        }
        else if(bar2)
        {
            return (slotBuild + 3);
        }

        return 0;
    }
}
