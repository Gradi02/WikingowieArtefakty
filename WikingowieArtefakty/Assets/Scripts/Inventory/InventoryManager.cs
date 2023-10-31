using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Slot[] slots;

    




    public void PickUpItem(ItemManager item)
    {
        Slot emptySlot = FindEmptySlot();
        emptySlot.SetItem(item);
    }

    private Slot FindEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].isEmpty()) return slots[i];
        }
        return null;
    }

    public Slot FindItem(string name)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetItemName() == name) return slots[i];
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
}
