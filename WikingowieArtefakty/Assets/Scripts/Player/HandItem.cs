using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItem : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public Transform HandTransform;

    public GameObject[] HandItemsPrefabs;
    private Slot selectedItemSlot;

    private BowShooting bowscript;
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        bowscript = GetComponent<BowShooting>();
    }

    void Update()
    {
        selectedItemSlot = GetSelectedItem();

        //Debug.Log(selectedItemSlot);

        if (selectedItemSlot.GetItemName() == "bow") bowscript.enableBow = true;
        else bowscript.enableBow = false;

        if (selectedItemSlot.GetItemName() == "axe") GetComponent<DestroyBlock>().enableAxe = true;
        else GetComponent<DestroyBlock>().enableAxe = false;

        if (selectedItemSlot.GetItemName() == "pickaxe") GetComponent<DestroyBlock>().enablePickaxe = true;
        else GetComponent<DestroyBlock>().enablePickaxe = false;
    }

    Slot GetSelectedItem()
    {
        return inventoryManager.GetSelectedSlot();
    }
}
