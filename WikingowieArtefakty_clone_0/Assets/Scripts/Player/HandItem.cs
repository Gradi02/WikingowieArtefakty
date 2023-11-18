using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HandItem : NetworkBehaviour
{
    private InventoryManager inventoryManager;
    public Transform HandTransform;

    public GameObject[] HandItemsPrefabs;
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

        if (selectedItemSlot.GetItemName() == "bow") bowscript.enableBow = true;
        else bowscript.enableBow = false;

        if (selectedItemSlot.GetItemName() == "axe") GetComponent<DestroyBlock>().enableAxe = true;
        else GetComponent<DestroyBlock>().enableAxe = false;

        if (selectedItemSlot.GetItemName() == "pickaxe") GetComponent<DestroyBlock>().enablePickaxe = true;
        else GetComponent<DestroyBlock>().enablePickaxe = false;
    }

    Slot GetSelectedItem()
    {
        //Debug.Log(inventoryManager.GetSelectedSlot());
        return inventoryManager.GetSelectedSlot();
    }
}
