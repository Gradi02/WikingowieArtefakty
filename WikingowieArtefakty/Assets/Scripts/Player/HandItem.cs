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
        if (selectedItemSlot.GetItemName() == "Bow") bowscript.enableBow = true;
        else bowscript.enableBow = false;
    }

    Slot GetSelectedItem()
    {
        //Debug.Log(inventoryManager.GetSelectedSlot());
        return inventoryManager.GetSelectedSlot();
    }
}
