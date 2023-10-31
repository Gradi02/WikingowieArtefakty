using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private ItemManager currentItem;

    public Sprite slotIcon;





    public void SetItem(ItemManager new_item)
    {
        currentItem = new_item;
        SetItemInfo();
    }

    public ItemManager GetItem()
    {
        return currentItem;
    }

    public string GetItemName()
    {
        return currentItem.name;
    }

    public void RemoveItem()
    {
        currentItem = null;
        ClearItemInfo();
    }

    public bool isEmpty()
    {
        if (currentItem == null) return true;
        return false;
    }
    private void ClearItemInfo()
    {
        slotIcon = null;
    }

    private void SetItemInfo()
    {
        slotIcon = currentItem.itemIcon;
    }
}
