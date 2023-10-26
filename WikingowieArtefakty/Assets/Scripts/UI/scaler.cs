using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Building;

    public TextMeshProUGUI NameTMP;
    public TextMeshProUGUI DescriptionTMP;
    public List<TextMeshProUGUI> ResourcesTMP;

    public string Name;
    public string Description;
    public List<string> resources_name;

    public List<int> resources;
    public List<Image> resoruces_img;
    public List<Sprite> resources_icon;

    public Sprite building_icon;
    public Image building_icon_place;
    

    public void Start()
    {
        for(int i = 0; i<resoruces_img.Count; i++)
        {
            resoruces_img[i].gameObject.SetActive(false);
            ResourcesTMP[i].text = "";
        }
    }
    public void Update()
    {
        if(Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt))
        {
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0);
        Building.SetActive(true);
        NameTMP.text = Name;
        DescriptionTMP.text = Description;

        int temp = 0;
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i] != 0)
            {
                ResourcesTMP[temp].text = (resources[i]).ToString() + " " + resources_name[i];
                resoruces_img[temp].gameObject.SetActive(true);
                resoruces_img[temp].sprite = resources_icon[i];
                temp++;
            }
        }
        building_icon_place.sprite = building_icon;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
        for (int i = 0; i < resoruces_img.Count; i++)
        {
            ResourcesTMP[i].text = "";
        }
        Building.SetActive(false);
    }
}
