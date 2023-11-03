using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public GameObject UpgradePlace;
    private Vector3 mouseScreenPosition;
    public TextMeshProUGUI NameTMP;
    public TextMeshProUGUI DescTMP;
    public string Name;
    public string Desc;

    public bool hover=false;
    void Update()
    {
        mouseScreenPosition = Input.mousePosition;
        Debug.Log(mouseScreenPosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.moveLocalY(UpgradePlace, 350f, 0.1f).setEase(LeanTweenType.easeInOutSine);
        NameTMP.text = Name;
        DescTMP.text = Desc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.moveLocalY(UpgradePlace, 700f, 0.1f).setEase(LeanTweenType.easeInOutSine);
    }
}
