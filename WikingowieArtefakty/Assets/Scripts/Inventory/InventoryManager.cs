using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Slot[] slots;

    





    public Slot FindEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].isEmpty()) return slots[i];
        }
        return null;
    }

    public Slot FindItem(ItemManager item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetItem() == item) return slots[i];
        }
        return null;
    }
}
