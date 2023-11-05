using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private ItemManager currentItem;
    public ItemManager staticSlotItem;
    
    private Image slotIcon;

    public bool staticSlot = false;


    private InventoryManager inventoryManager;
    private Color empty = Color.white;

    private void Start()
    {
        empty.a = 0;
        slotIcon = transform.Find("Icon").GetComponent<Image>();
        inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

        if (staticSlotItem != null) currentItem = staticSlotItem;
    }



    public void SetItem(GameObject new_item)
    {
        if(staticSlot)
        {
            Debug.LogWarning("You cant edit static slot!");
            return;
        }

        if (inventoryManager.GetItemFromList(new_item.GetComponent<ItemManager>().itemName) == null)
        {
            Debug.Log("dodaj item do listy itemów!");
            return;
        }

        currentItem = inventoryManager.GetItemFromList(new_item.GetComponent<ItemManager>().itemName).GetComponent<ItemManager>();
        new_item.GetComponent<ItemManager>().DestroyItem();
        SetItemInfo();
    }

    public ItemManager GetItem()
    {
        if(currentItem != null)
            return currentItem;

        return null;
    }

    public string GetItemName()
    {
        if(currentItem != null)
            return currentItem.itemName;

        return null;
    }

    public void RemoveItem()
    {
        if (staticSlot)
        {
            Debug.LogWarning("You cant edit static slot!");
            return;
        }

        currentItem = null;
        ClearItemInfo();
    }

    public bool IsEmpty()
    {
        if (staticSlot) return false;

        if (currentItem == null) return true;
        return false;
    }
    private void ClearItemInfo()
    {
        slotIcon.sprite = null;
        slotIcon.color = empty;
    }

    private void SetItemInfo()
    {
        slotIcon.sprite = currentItem.itemIcon;
        slotIcon.color = Color.white;
    }

    public bool Droppable()
    {
        if (staticSlot) return false;
        return true;
    }
}
