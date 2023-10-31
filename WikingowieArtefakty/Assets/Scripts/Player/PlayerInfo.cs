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
        if(Input.GetKey(KeyCode.Space))
        {
            CheckForBridge();
        }
    }

    void CheckForBridge()
    {
        if (inventoryManager.FindItem("wood") == null)
        {
            inventoryManager.RemoveItem("wood");
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right);

            if (Physics.Raycast(transform.position, transform.right, out hit, 1))
            {
                if (hit.transform.GetComponent<Bridge>() != null)
                    hit.transform.GetComponent<Bridge>().BuildBridge();
            }
        }
    }
}
