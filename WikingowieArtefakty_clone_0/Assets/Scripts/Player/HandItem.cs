using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HandItem : NetworkBehaviour
{
    private InventoryManager inventoryManager;
    public Transform HandTransform;

    public GameObject[] HandItemsPrefabs;
    public GameObject[] HandModelsPrefabs;
    private Slot selectedItemSlot;

    private BowShooting bowscript;
    void Start()
    {
        if (!IsOwner) return;
        inventoryManager = GetComponent<InventoryManager>();
        bowscript = GetComponent<BowShooting>();
    }

    void Update()
    {
        if (!IsOwner) return;
        selectedItemSlot = GetSelectedItem();

        //nic
        if (selectedItemSlot.GetItemName() == null)
        {
            SetItemOnPlayerServerRpc(GetComponent<NetworkObject>().NetworkObjectId, -1);
        }

        //battleaxe
        if (selectedItemSlot.GetItemName() == "battleaxe")
        {
            GetComponent<PlayerAttack>().enableBattleAxe = true;
            HandModelsPrefabs[0].SetActive(true);
            SetItemOnPlayerServerRpc(GetComponent<NetworkObject>().NetworkObjectId, 0);
        }
        else
        {
            GetComponent<PlayerAttack>().enableBattleAxe = false;
            HandModelsPrefabs[0].SetActive(false);
        }

        //bow
        if (selectedItemSlot.GetItemName() == "bow")
        {
            bowscript.enableBow = true;
            HandModelsPrefabs[1].SetActive(true);
            SetItemOnPlayerServerRpc(GetComponent<NetworkObject>().NetworkObjectId, 1);
        }
        else
        {
            bowscript.enableBow = false;
            HandModelsPrefabs[1].SetActive(false);
        }

        //axe
        if (selectedItemSlot.GetItemName() == "axe")
        {
            GetComponent<DestroyBlock>().enableAxe = true;
            HandModelsPrefabs[2].SetActive(true);
            SetItemOnPlayerServerRpc(GetComponent<NetworkObject>().NetworkObjectId, 2);
        }
        else
        {
            GetComponent<DestroyBlock>().enableAxe = false;
            HandModelsPrefabs[2].SetActive(false);
        }

        //pickaxe
        if (selectedItemSlot.GetItemName() == "pickaxe")
        {
            GetComponent<DestroyBlock>().enablePickaxe = true;
            HandModelsPrefabs[3].SetActive(true);
            SetItemOnPlayerServerRpc(GetComponent<NetworkObject>().NetworkObjectId, 3);
        }
        else
        {
            GetComponent<DestroyBlock>().enablePickaxe = false;
            HandModelsPrefabs[3].SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SetItemOnPlayerServerRpc(ulong id, int item)
    {
        SetItemOnPlayerClientRpc(id, item);
    }

    [ClientRpc]
    void SetItemOnPlayerClientRpc(ulong id, int item)
    {
        if(item == -1)
        {
            for (int i = 0; i < HandModelsPrefabs.Length; i++)
                NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<HandItem>().HandModelsPrefabs[i].SetActive(false);
        }

        for (int i = 0; i < HandModelsPrefabs.Length; i++)
        {
            if(i == item)
                NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<HandItem>().HandModelsPrefabs[i].SetActive(true);
            else
                NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<HandItem>().HandModelsPrefabs[i].SetActive(false);
        }
    }

    Slot GetSelectedItem()
    {
        //Debug.Log(inventoryManager.GetSelectedSlot());
        return inventoryManager.GetSelectedSlot();
    }
}
