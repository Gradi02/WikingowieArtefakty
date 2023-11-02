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
    private bool activated= false;

    void Update()
    {
        mouseScreenPosition = Input.mousePosition;
        Debug.Log(mouseScreenPosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradePlace.SetActive(true);
        LeanTween.scale(UpgradePlace, new Vector3(1,1,1), 0.2f).setEase(LeanTweenType.easeInOutSine);
        NameTMP.text = Name;
        DescTMP.text = Desc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(UpgradePlace, new Vector3(0, 0, 0), 0.2f).setEase(LeanTweenType.easeInOutSine);
        UpgradePlace.SetActive(false);

    }
}
