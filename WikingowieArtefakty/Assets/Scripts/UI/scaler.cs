using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0);
        Transform desc = transform.GetChild(0);
        desc.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
        Transform desc = transform.GetChild(0);
        desc.gameObject.SetActive(false);
    }
}
