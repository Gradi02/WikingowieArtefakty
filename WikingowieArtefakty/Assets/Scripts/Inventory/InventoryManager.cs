using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Slot[] slots;
    public EqBar selection;
    public GameObject[] itemList;



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
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].IsEmpty()) return slots[i];
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
        for (int i = 0; i < slots.Length; i++)
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
            GameObject dropped = GetItemFromList(drop.GetItemName());
            Instantiate(dropped, transform.position, transform.rotation);
            drop.RemoveItem();
        }
    }
}
