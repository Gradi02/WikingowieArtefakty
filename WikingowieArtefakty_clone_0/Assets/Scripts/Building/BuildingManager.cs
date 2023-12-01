using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuildingManager : NetworkBehaviour
{
    public scaler[] scalerList;

    private scaler buildingInfo;
    private GameObject currentBuildPrefab = null;
    private int currentBuildIndex = 0;

    private Vector3 currentPosition = Vector3.zero;
    
    private NetworkVariable<int> maxBuildingSchemats = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> currentBuildingSchemats = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public GameObject[] buildPrefabs = new GameObject[9];
   
    public void PickNewPrefab(int num, scaler info)
    {
        RemoveCurrentBuild();
        buildingInfo = info;
        currentBuildPrefab = Instantiate(buildPrefabs[num], transform.position, Quaternion.identity);
        currentBuildIndex = num;
        currentBuildPrefab.GetComponent<BuildingInfo>().DisableText();
        
    }

    private void Update()
    {
        if(currentBuildPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPos = new Vector3(Mathf.RoundToInt(hit.point.x), 0.75f, Mathf.RoundToInt(hit.point.z));
                currentPosition = newPos;

                currentBuildPrefab.transform.position = newPos;
            }
        }
        else
        {
            currentPosition = Vector3.zero;
        }

        if(Input.GetMouseButtonDown(1) && currentBuildPrefab != null)
        {
            RemoveCurrentBuild();
        }

        /*if(Input.GetKeyDown(KeyCode.E) && currentBuildPrefab != null)
        {
            currentBuildPrefab.transform.localEulerAngles += new Vector3(0, 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentBuildPrefab != null)
        {
            currentBuildPrefab.transform.localEulerAngles -= new Vector3(0, 90, 0);
        }*/

        if (Input.GetKeyDown(KeyCode.B) && currentBuildPrefab != null)
        {
            if (CheckForSchematPlace())
            {
                SetNewBuildServerRpc(currentBuildIndex, currentPosition, buildingInfo.Name);
            }
            else
            {
                Popup.Instance.PopupPop("You can't create more building schemats!");
            }

            RemoveCurrentBuild();
        }
    }

    private void RemoveCurrentBuild()
    {
        if (currentBuildPrefab != null)
        {
            Destroy(currentBuildPrefab);
            currentBuildPrefab = null;
            currentBuildIndex = 0;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetNewBuildServerRpc(int buildingId, Vector3 pos, string Name)
    {
        GameObject b = Instantiate(buildPrefabs[buildingId], pos, Quaternion.identity);
        b.GetComponent<NetworkObject>().Spawn();
        //b.GetComponent<BuildingInfo>().SetBuildingInfo(BuildingInfo);
        currentBuildingSchemats.Value++;
        SetNewBuildClientRpc(b.GetComponent<NetworkObject>().NetworkObjectId, Name);
    }

    [ClientRpc]
    private void SetNewBuildClientRpc(ulong id, string name)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<BuildingInfo>().SetBuildingInfo(GetScalerId(name));
    }

    private int GetScalerId(string name)
    {
        for(int i=0; i<scalerList.Length; i++)
        {
            if (scalerList[i].Name == name)
            {
                return i;
            }
        }
        return 0;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DecreseSchematsCountServerRpc()
    {
        currentBuildingSchemats.Value--;
    }

    public bool CheckForSchematPlace()
    {
        return (currentBuildingSchemats.Value < maxBuildingSchemats.Value);
    }
}
