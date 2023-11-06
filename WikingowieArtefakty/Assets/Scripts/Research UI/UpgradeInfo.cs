using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public GameObject UpgradePlace;
    public GameObject UpgradePlaceBGC;
    public TextMeshProUGUI NameTMP;
    public TextMeshProUGUI DescTMP;
    public string Name;
    public string Desc;



    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.moveLocalY(UpgradePlace, 350f, 0.1f).setEase(LeanTweenType.easeInOutSine);
        if (!UpgradePlaceBGC.activeSelf) gameObject.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        if (Name!=null)   NameTMP.text = Name;
        if (Desc!= null) DescTMP.text = Desc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.moveLocalY(UpgradePlace, 700f, 0.1f).setEase(LeanTweenType.easeInOutSine);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
