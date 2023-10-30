using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private ItemManager currentItem;
    




    public void SetItem(ItemManager new_item)
    {
        currentItem = new_item;
    }

    public ItemManager GetItem()
    {
        return currentItem;
    }

    public void RemoveItem()
    {
        currentItem = null;
    }

    public bool isEmpty()
    {
        if (currentItem == null) return true;
        return false;
    }
}
