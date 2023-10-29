using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EqBar : MonoBehaviour
{
    public List<Image> eq;
    public int slot;
    void Start()
    {
        for(int i = 0; i<eq.Count; i++)
        {
            eq[i].gameObject.SetActive(false);
        }
    }

   
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput > 0)
        {
            for (int i = 0; i < eq.Count; i++)
            {
                eq[i].gameObject.SetActive(false);
            }
            slot++;
            if(slot>eq.Count-1)slot = 0;
            eq[slot].gameObject.SetActive(true);
        }

        else if (scrollInput < 0)
        {
            for (int i = 0; i < eq.Count; i++)
            {
                eq[i].gameObject.SetActive(false);
            }
            slot--;
            if (slot < 0) slot = 6;
            eq[slot].gameObject.SetActive(true);
        }
    }
}
