
/// <summary>
/// 
///     SKRYPT ODPOWIEDZIALNY ZA PRACE IKONEK I TEKSTU PODCZAS GRY
/// 
///     by: TOBKUBOS
///     
///     version: 1.0
/// 
///     Date of creation: 25/10/2023
///     
/// </summary>

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame_UI : MonoBehaviour
{
    public List<GameObject> ResourceSidebar;
    public List<TextMeshProUGUI> names;
    public List<Image> buildings;
    void Start()
    {
        Debug.Log(ResourceSidebar.Count);
        for (int i = 0; i < ResourceSidebar.Count; i++)
        {
            ResourceSidebar[i].transform.position += new Vector3(-100f, 0, 0);
        }
        StartCoroutine(Mover_delay());
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            Debug.Log("ALT");
            names[0].text = "WOOD: ";
            names[1].text = "STONE: ";
            names[2].text = "IRON ORE: ";
            names[3].text = "IRON BARS: ";
            names[4].text = "STEEL: ";
        }
        else
        {
            names[0].text = ": ";
            names[1].text = ": ";
            names[2].text = ": ";
            names[3].text = ": ";
            names[4].text = ": ";
        }
    }
    IEnumerator Mover_delay()
    {
        for (int i = 0; i < ResourceSidebar.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log(i);
            StartCoroutine(Mover(i));
        }
    }
    IEnumerator Mover(int i)
    {
        Debug.Log("NORMAL"+ResourceSidebar[i].transform.position);
        Debug.Log("LOCAL"+ResourceSidebar[i].transform.localPosition);
        while (ResourceSidebar[i].transform.localPosition.x <= -170f)
        {
            yield return new WaitForSeconds(0.005f);
            Debug.Log("MOVER I:  " + i);
            ResourceSidebar[i].transform.position += new Vector3(1f, 0, 0);
        }
    }
}
