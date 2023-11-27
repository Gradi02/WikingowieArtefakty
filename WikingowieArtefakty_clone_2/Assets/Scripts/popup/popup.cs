using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public static Popup Instance;

    public TextMeshProUGUI desc;
    public Canvas canva;
    public Transform infoTransform;

    private bool ifblock = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PopupPop(string info)
    {
        if (ifblock) return;

        TextMeshProUGUI text = Instantiate(desc, infoTransform.position, Quaternion.identity, canva.transform);
        text.transform.position = infoTransform.position;
        text.text = info;
        ifblock = true;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        ifblock = false;
    }
}
