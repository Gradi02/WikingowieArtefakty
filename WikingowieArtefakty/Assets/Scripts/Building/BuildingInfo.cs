using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

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
        BuildingProgressUpdate();
    }

    public void CreateProgressList(List<int> resources)
    {
        ResourcesProgress = new List<int>(resources);
        for (int i = 0; i < resources.Count; i++)
        {
            ResourcesProgress[i] = 0;
        }
    }

    public void BuildingProgressUpdate()
    {
        progressInfoName.text = Name;
        progressInfo.text = string.Empty;
        for(int i = 0; i<resources.Count; i++)
        {
            if (resources[i] > 0)
            {
                progressInfo.text += ResourcesProgress[i] + "/" + resources[i].ToString();
                progressInfo.text += ": ";
                progressInfo.text += resources_name[i];
                progressInfo.text += "\n";
            }
        }
    }
    /*    [ContextMenu("Build Step")]
        public void BuildStep()
        {
            Slot selected = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().GetSelectedSlot();
            ItemManager selecteditem = selected.GetItem();
            CheckForNeededItem(selecteditem, selected);
        }

        private void CheckForNeededItem(ItemManager item, Slot slot)
        {
            for(int i = 0; i < resources.Count; i++)
            {
                if (resources[i] < 0)
                {
                    if (resources_name[i] == item.itemName)
                    {
                        slot.RemoveItem();
                        resources[i]--;
                        CheckForFinishBuilding();
                        return;
                    }
                }
            }
        }

        private void CheckForFinishBuilding()
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if(resources[i] < 0)
                {
                    return;
                }
            }

            FinishBuilding();
        }*/

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
    }
}
