using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private PlayerMovement move;
    private InventoryManager inventoryManager;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
        inventoryManager = GetComponent<InventoryManager>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckForBridge();           
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CheckForItems();
        }
        if(Input.GetKeyDown(KeyCode.Q))
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

    void CheckForItems()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("item"))
        {
            if(Vector3.Distance(transform.position, g.transform.position) <= 10)
            {
                inventoryManager.PickUpItem(g);
            }
        }
    }

    void DropItem()
    {
        inventoryManager.DropSelectedItem();
    }

    public void ChopAnimation()
    {

    }
}
