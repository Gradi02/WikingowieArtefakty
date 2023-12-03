using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    private PlayerMovement move;
    private InventoryManager inventoryManager;
    private GameObject itemToRemove;
    private GameObject sun;
    private string username;
    public GameObject HealthBar;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
        inventoryManager = GetComponent<InventoryManager>();
        sun = GameObject.FindGameObjectWithTag("sun");

        if (IsLocalPlayer)
        {
            Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
            username = GameObject.FindObjectOfType<NetworkInit>().username;

            if(username.Length > 2)
                SetNameServerRpc(username, GetComponent<NetworkObject>().NetworkObjectId);
            else
                SetNameServerRpc("No_Name", GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
    void Update()
    {
        if (!IsOwner) return;

        sun.transform.localPosition = transform.position + new Vector3(0, 10, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForUse();           
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //PickUpClosestItem();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }


        if(HealthBar != null)
        {
            HealthBar.transform.Find("nick").GetComponent<TextMeshProUGUI>().text = username;
        }
    }

    void CheckForUse()
    {
        if(FindItemInRadius() != null && inventoryManager.FindEmptySlot() != null)
        {
            GameObject closestItem = FindItemInRadius();

            if (closestItem != null && closestItem.GetComponent<ItemManager>().GetPickable() && closestItem != itemToRemove)
                closestItem.GetComponent<ItemManager>().PickUp(gameObject);

            SetItemServerRpc(closestItem.GetComponent<NetworkObject>().NetworkObjectId);
            ClearDropPlaceServerRpc();
            DestroyItemServerRpc();
            return;
        }

        Slot selected = inventoryManager.GetSelectedSlot();

        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hit, 1))
        {
            if (hit.transform.GetComponent<Bridge>() != null)
            {
                if (selected.GetItemName() != "wood")
                {
                    Debug.Log("nie masz matsów kurwo");
                    return;
                }

                hit.transform.GetComponent<Bridge>().BuildBridgeServerRpc();
                selected.RemoveItem();
            }
            else if(hit.transform.GetComponent<BuildingInfo>() != null)
            {
                hit.transform.GetComponent<BuildingInfo>().CheckForUseItem(selected);
                return;
            }
            else if(hit.transform.GetComponent<ItemManager>() != null)
            {
                GameObject closestItem = hit.transform.gameObject;

                if (closestItem != null && closestItem.GetComponent<ItemManager>().GetPickable() && closestItem != itemToRemove)
                    closestItem.GetComponent<ItemManager>().PickUp(gameObject);

                SetItemServerRpc(closestItem.GetComponent<NetworkObject>().NetworkObjectId);
                ClearDropPlaceServerRpc();
                DestroyItemServerRpc();
                return;
            }
            else if(hit.transform.GetComponent<ShipManager>() != null)
            {
                hit.transform.GetComponent<ShipManager>().CheckForUseItem(selected);
                return;
            }
        }
    }

    GameObject FindItemInRadius()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("item"))
        {
            if(Vector3.Distance(transform.position, g.transform.position) <= 0.5f)
            {
                return g;
            }
        }
        return null;
    }

    [ServerRpc(RequireOwnership = false)]
    void SetItemServerRpc(ulong objectId)
    {
        SetItemClientRpc(objectId);
    }

    [ClientRpc]
    void SetItemClientRpc(ulong objectId)
    {
        GameObject o = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId].gameObject;

        itemToRemove = o;
    }

    [ServerRpc(RequireOwnership = false)]
    void ClearDropPlaceServerRpc()
    {
        ClearDropPlaceClientRpc();
    }

    [ClientRpc]
    void ClearDropPlaceClientRpc()
    {
        itemToRemove.GetComponent<ItemManager>().ClearPlace();
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyItemServerRpc()
    {
        //DestroyItemClientRpc();
        itemToRemove.GetComponent<NetworkObject>().Despawn();
    }

    void DropItem()
    {
        inventoryManager.DropSelectedItem();
    }

    [ServerRpc(RequireOwnership = false)]
    void SetNameServerRpc(string name, ulong id)
    {
        SetNameClientRpc(name, id);
    }

    [ClientRpc]
    void SetNameClientRpc(string name, ulong id)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].transform.name = name;
    }
}
