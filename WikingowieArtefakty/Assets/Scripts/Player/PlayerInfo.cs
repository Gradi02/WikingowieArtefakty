using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfo : NetworkBehaviour
{
    private PlayerMovement move;
    private InventoryManager inventoryManager;
    private GameObject itemToRemove;
    private GameObject sun;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
        inventoryManager = GetComponent<InventoryManager>();
        sun = GameObject.FindGameObjectWithTag("sun");

        if (IsLocalPlayer) Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
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
            PickUpClosestItem();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    void CheckForUse()
    {
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
            }
        }

        /*if (selected.GetItemName() != "wood")
        {
            Debug.Log("nie masz matsów kurwo");
            return;
        }
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward);

            if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
            {
                if (hit.transform.GetComponent<Bridge>() != null)
                {
                    hit.transform.GetComponent<Bridge>().BuildBridgeServerRpc();
                    selected.RemoveItem();
                }
            }
        }*/
    }

    void PickUpClosestItem()
    {
        if (GameObject.FindGameObjectsWithTag("item").Length == 0) return;
        GameObject closestItem = GameObject.FindGameObjectWithTag("item");
        GameObject[] items = GameObject.FindGameObjectsWithTag("item");

        for(int i=0; i<items.Length; i++)
        {
            if (Vector3.Distance(items[i].transform.position, transform.position) < Vector3.Distance(closestItem.transform.position, transform.position))
            {
                closestItem = items[i];
            }
        }

        if(closestItem != null)
            closestItem.GetComponent<ItemManager>().PickUp(gameObject);

        //itemToRemove = closestItem;
        SetItemServerRpc(closestItem.GetComponent<NetworkObject>().NetworkObjectId);
        ClearDropPlaceServerRpc();
        DestroyItemServerRpc();
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
}
