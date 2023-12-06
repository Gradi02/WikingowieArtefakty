using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class ShipManager : NetworkBehaviour
{
    [Header("Main Values")]
    public float visitDistance;
    public TextMeshProUGUI progressInfo;
    public TextMeshProUGUI progressInfoName;

    [Header("Items")]
    public string[] resources_name;

    public int[] needed_resources1;
    public int[] needed_resources2;
    public int[] needed_resources3;

    public int[] current_resources;



    private int buildingPhase = 0;
    private CameraFollow cam;
    private bool visited = false;


    private void Start()
    {
        cam = Camera.main.GetComponent<CameraFollow>();
        current_resources = new int[needed_resources1.Length];

        if(IsHost)
            BuildingProgressUpdateServerRpc();
    }
    void Update()
    {
        if (!IsOwner) return;

        Transform player = NetworkManager.LocalClient.PlayerObject.transform;

        if (!visited)
        {
            if (Vector3.Distance(player.position, transform.position) <= visitDistance)
            {
                visited = true;
                cam.SmoothTime = 1;
                cam.Target = this.gameObject.transform;
                Invoke(nameof(ResetCamTarget), 3);
            }
        }
    }

    private void ResetCamTarget()
    {
        cam.Target = NetworkManager.LocalClient.PlayerObject.transform;
        cam.ResetSmooth();
    }

    public void CheckForUseItem(Slot selectedSlot)
    {
        if(buildingPhase == 0)
        {
            for(int i=0; i<needed_resources1.Length; i++)
            {
                
                if (resources_name[i] == selectedSlot.GetItemName() && current_resources[i] < needed_resources1[i])
                {
                    selectedSlot.RemoveItem();
                    AddItemServerRpc(i);
                    CheckForNextStep();
                    BuildingProgressUpdateServerRpc();
                    return;
                }
            }
        }
        else if (buildingPhase == 1)
        {
            for (int i = 0; i < needed_resources2.Length; i++)
            {
                if (resources_name[i] == selectedSlot.GetItemName() && current_resources[i] < needed_resources2[i])
                {
                    selectedSlot.RemoveItem();
                    AddItemServerRpc(i);
                    CheckForNextStep();
                    BuildingProgressUpdateServerRpc();
                    return;
                }
            }
        }
        if (buildingPhase == 2)
        {
            for (int i = 0; i < needed_resources3.Length; i++)
            {
                if (resources_name[i] == selectedSlot.GetItemName() && current_resources[i] < needed_resources3[i])
                {
                    selectedSlot.RemoveItem();
                    AddItemServerRpc(i);
                    CheckForNextStep();
                    BuildingProgressUpdateServerRpc();
                    return;
                }
            }
        }
        else
        {

        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void BuildingProgressUpdateServerRpc()
    {
        progressInfo.text = string.Empty;

        if (buildingPhase == 0)
        {
            for (int i = 0; i < needed_resources1.Length; i++)
            {
                
                if (needed_resources1[i] > 0 && current_resources[i] < needed_resources1[i])
                {
                    progressInfo.text += current_resources[i] + "/" + needed_resources1[i].ToString();
                    progressInfo.text += ": ";
                    progressInfo.text += resources_name[i];
                    progressInfo.text += "\n";
                }
            }
        }
        else if (buildingPhase == 1)
        {
            for (int i = 0; i < needed_resources2.Length; i++)
            {
                if (needed_resources2[i] > 0 && current_resources[i] < needed_resources2[i])
                {
                    progressInfo.text += current_resources[i] + "/" + needed_resources2[i].ToString();
                    progressInfo.text += ": ";
                    progressInfo.text += resources_name[i];
                    progressInfo.text += "\n";
                }
            }
        }
        else if (buildingPhase == 2)
        {
            for (int i = 0; i < needed_resources3.Length; i++)
            {
                if (needed_resources3[i] > 0 && current_resources[i] < needed_resources3[i])
                {
                    progressInfo.text += current_resources[i] + "/" + needed_resources3[i].ToString();
                    progressInfo.text += ": ";
                    progressInfo.text += resources_name[i];
                    progressInfo.text += "\n";
                }
            }
        }
        else
        {

        }

        BuildingProgressUpdateClientRpc(progressInfo.text);
    }

    [ClientRpc]
    public void BuildingProgressUpdateClientRpc(string s)
    {
        progressInfoName.text = "Boat - phase " + (buildingPhase+1).ToString();
        progressInfo.text = s;
    }

    [ServerRpc(RequireOwnership = false)]
    void AddItemServerRpc(int i)
    {
        AddItemClientRpc(i);
    }

    [ClientRpc]
    void AddItemClientRpc(int i)
    {
        current_resources[i]++;
    }

    private void CheckForNextStep()
    {
        if (buildingPhase == 0)
        {
            for (int i = 0; i < needed_resources1.Length; i++)
            {
                if (current_resources[i] < needed_resources1[i])
                {
                    return;
                }
            }
        }
        else if (buildingPhase == 1)
        {
            for (int i = 0; i < needed_resources2.Length; i++)
            {
                if (current_resources[i] < needed_resources2[i])
                {
                    return;
                }
            }
        }
        else if (buildingPhase == 2)
        {
            for (int i = 0; i < needed_resources3.Length; i++)
            {
                if (current_resources[i] < needed_resources3[i])
                {
                    return;
                }
            }
        }


        NextStepServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void NextStepServerRpc()
    {
        NextStepClientRpc();
    }

    [ClientRpc]
    private void NextStepClientRpc()
    {
        buildingPhase++;
        current_resources = new int[needed_resources1.Length];
    }

    [ServerRpc(RequireOwnership = false)]
    private void FinishBuildingServerRpc()
    {
        FinishBuildingClientRpc();
    }

    [ClientRpc]
    private void FinishBuildingClientRpc()
    {

    }
}

