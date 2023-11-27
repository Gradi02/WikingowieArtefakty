using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BuildingManager : NetworkBehaviour
{
    private GameObject currentBuildPrefab = null;
    private int currentBuildIndex = 0;

    private Vector3 currentPosition = Vector3.zero;
    
    private NetworkVariable<int> maxBuildingSchemats = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> currentBuildingSchemats = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public GameObject[] buildPrefabs = new GameObject[9];
   
    public void PickNewPrefab(int num, scaler info)
    {
        RemoveCurrentBuild();

        currentBuildPrefab = Instantiate(buildPrefabs[num], transform.position, Quaternion.identity);
        currentBuildIndex = num;

        currentBuildPrefab.GetComponent<BuildingInfo>().SetBuildingInfo(info);
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
            SetNewBuildServerRpc(currentBuildIndex, currentPosition);
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
    private void SetNewBuildServerRpc(int buildingId, Vector3 pos)
    {
        /// WYWO�AJ FUNKCJE POSTAWIENIA NA SKRYPCIE BUDYNKU!!!!!

        if (CheckForSchematPlace())
        {
            GameObject b = Instantiate(buildPrefabs[buildingId], pos, Quaternion.identity);
            b.GetComponent<NetworkObject>().Spawn();
            currentBuildingSchemats.Value++;
        }
        else
        {
            Debug.Log("Masz ju� max schemat�w na mapie");
        }
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