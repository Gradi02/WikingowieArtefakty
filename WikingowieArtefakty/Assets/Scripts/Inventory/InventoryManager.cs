using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    //public Slot[] slots;
    public List<Slot> slots = new();
    private EqBar selection;
    public GameObject[] itemList;
    private ItemsDropManager dropManager;

    private void Awake()
    {
        selection = GameObject.FindGameObjectWithTag("uimanager").GetComponent<EqBar>();
        dropManager = GameObject.FindGameObjectWithTag("manager").GetComponent<ItemsDropManager>();

        slots.Clear();
        GameObject[] gmslots = GameObject.FindGameObjectsWithTag("slots");
        foreach(var gmslot in gmslots)
        {
            slots.Add(gmslot.GetComponent<Slot>());
        }
        SortujSloty();
    }

    void SortujSloty()
    {
        // Sortowanie listy wg nazw slotów
        slots.Sort((slot1, slot2) =>
        {
            int numerSlotu1 = Int32.Parse(slot1.name.Substring(4));
            int numerSlotu2 = Int32.Parse(slot2.name.Substring(4));

            return numerSlotu1.CompareTo(numerSlotu2);
        });
    }
    public void PickUpItem(GameObject itemObj)
    {
        Slot emptySlot = FindEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.SetItem(itemObj);
        }
    }

    private Slot FindEmptySlot()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty()) return slots[i];
        }
        return null;
    }

    public Slot FindItem(string name)
    {
        foreach(Slot slot in slots)
        {
            if (slot.GetItemName() == name)
            {
                return slot;
            }
        }
        return null;
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetItemName() == name) slots[i].RemoveItem();
        }
    }

    public GameObject GetItemFromList(string name)
    {
        for(int i=0; i<itemList.Length; i++)
        {
            Debug.Log(itemList[i]);
            if (itemList[i].GetComponent<ItemManager>().itemName == name) return itemList[i];
        }
        return null;
    }

    public Slot GetSelectedSlot()
    {
        int num = selection.GetSelectedSlot();

        return slots[num];
    }

    public void DropSelectedItem()
    {
        Slot drop = GetSelectedSlot();
        
        if(drop.Droppable() && !drop.IsEmpty())
        {
            if(CheckForSpace())
            {
                Debug.Log("nie ma miejsca");
                return;
            }

            GameObject dropped = GetItemFromList(drop.GetItemName());
            Instantiate(dropped, transform.position, transform.rotation);
            drop.RemoveItem();
        }
    }

    bool CheckForSpace()
    {
        return dropManager.IsPlaceEmpty(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));
    }
}
