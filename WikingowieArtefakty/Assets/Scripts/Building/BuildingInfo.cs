using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;


public class BuildingInfo : NetworkBehaviour
{
    private scaler info;
    private BuildingManager manager;

    private string Name;
    private string Description;
    private List<int> resources;
   // private List<Image> resoruces_img;
   // private List<Sprite> resources_icon;
    private List<string> resources_name;


    public TextMeshProUGUI progressInfo;
    public TextMeshProUGUI progressInfoName;

    public Material finished;


    private List<int> ResourcesProgress;
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<BuildingManager>();
    }

    public void DisableText()
    {
        progressInfo.gameObject.SetActive(false);
        progressInfoName.gameObject.SetActive(false);
    }


    public void SetBuildingInfo(scaler i)
    {
        info = i;

        Name = info.Name;
        Description = info.Description;
        resources = info.resources;
       // resoruces_img = info.resoruces_img;
       // resources_icon = info.resources_icon;
        resources_name = info.resources_name;
        CreateProgressList(resources);
        BuildingProgressUpdateServerRpc();
    }

    public void CreateProgressList(List<int> resources)
    {
        ResourcesProgress = new List<int>(resources);
        for (int i = 0; i < resources.Count; i++)
        {
            ResourcesProgress[i] = 0;
        }
    }

    [ServerRpc]
    public void BuildingProgressUpdateServerRpc()
    {
        BuildingProgressUpdateClientRpc();
    }

    [ClientRpc]
    public void BuildingProgressUpdateClientRpc()
    {
        progressInfoName.text = Name;
        progressInfo.text = string.Empty;
        for(int i = 0; i<resources.Count; i++)
        {
            if (resources[i] > 0 && ResourcesProgress[i] < resources[i])
            {
                progressInfo.text += ResourcesProgress[i] + "/" + resources[i].ToString();
                progressInfo.text += ": ";
                progressInfo.text += resources_name[i];
                progressInfo.text += "\n";
            }
        }
    }

    public void CheckForUseItem(Slot selectedSlot)
    {
        for(int i=0; i < resources.Count; i++)
        {
            if (resources_name[i] == selectedSlot.GetItemName() && ResourcesProgress[i] < resources[i])
            {
                selectedSlot.RemoveItem();
                ResourcesProgress[i]++;
                BuildingProgressUpdateServerRpc();
                CheckForFinishSchemat();
                return;
            }
        }
    }

    private void CheckForFinishSchemat()
    {
        for(int i=0; i<resources.Count; i++)
        {
            if(resources[i] > ResourcesProgress[i])
            {
                return;
            }
        }

        FinishBuildingServerRpc();
    }


    [ContextMenu("finish"), ServerRpc(RequireOwnership = false)]
    private void FinishBuildingServerRpc()
    {
        manager.DecreseSchematsCountServerRpc();
        FinishBuildingClientRpc();
    }

    [ClientRpc]
    private void FinishBuildingClientRpc()
    {
        gameObject.GetComponent<MeshRenderer>().material = finished;
        DisableText();
    }
}
