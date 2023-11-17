using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInfo : NetworkBehaviour
{
    private PlayerMovement move;
    private InventoryManager inventoryManager;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
        inventoryManager = GetComponent<InventoryManager>();

        if (IsLocalPlayer) Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
    }
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckForBridge();           
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

    void CheckForBridge()
    {
        Slot selected = inventoryManager.GetSelectedSlot();

        if (selected.GetItemName() != "wood")
        {
            Debug.Log("nie masz matsów kurwo");
            return;
        }
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right);

            if (Physics.Raycast(transform.position, transform.right, out hit, 1))
            {
                if (hit.transform.GetComponent<Bridge>() != null)
                    hit.transform.GetComponent<Bridge>().BuildBridge();
                selected.RemoveItem();
            }
        }
    }

    void PickUpClosestItem()
    {
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
            closestItem.GetComponent<ItemManager>().PickUp();
    }

    void DropItem()
    {
        inventoryManager.DropSelectedItem();
    }

    public void ChopAnimation()
    {

    }
}
