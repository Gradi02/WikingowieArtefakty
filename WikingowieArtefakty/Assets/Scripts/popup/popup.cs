using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI desc;
    public Canvas canva;

    public void Update()
    {
        Vector3 inGamePos = Input.mousePosition;
       // Vector3 inGamePos = Camera.main.ScreenToWorldPoint(mousepos);
        if (Input.GetKeyDown(KeyCode.Mouse0)) PopupPop("test", inGamePos);
        Debug.Log(inGamePos);
    }

    public void PopupPop(string info, Vector3 pos)
    {
        Instantiate(desc, pos, Quaternion.identity, canva.transform);
        desc.transform.position = pos;
        desc.text = info;
    }
}
