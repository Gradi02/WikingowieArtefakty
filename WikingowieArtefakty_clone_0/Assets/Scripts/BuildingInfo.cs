using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class BuildingInfo : NetworkBehaviour
{
    private scaler info;
    private BuildingManager manager;

    private string Name;
    private string Description;
    private List<int> resources;
    private List<Image> resoruces_img;
    private List<Sprite> resources_icon;
    private List<string> resources_name;

    public Material finished;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<BuildingManager>();
    }
    public void SetBuildingInfo(scaler i)
    {
        info = i;

        Name = info.Name;
        Description = info.Description;
        resources = info.resources;
        resoruces_img = info.resoruces_img;
        resources_icon = info.resources_icon;
        resources_name = info.resources_name;
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
