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

    private void Awake()
    {
        selection = GameObject.FindGameObjectWithTag("uimanager").GetComponent<EqBar>();

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
            if(!CheckForSpace())
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
        Vector3 dropPos = new Vector3(Mathf.RoundToInt(transform.position.x), 1f, Mathf.RoundToInt(transform.position.z));
        Debug.DrawRay(dropPos, Vector3.down, Color.red, 1);

        RaycastHit[] hits = new RaycastHit[1];
        int hitCount = Physics.RaycastNonAlloc(dropPos, Vector3.down, hits, 1.0f);
        Debug.Log(hitCount);

        // SprawdŸ, czy istnieje trafienie i czy miejsce jest zajête
        if (hitCount > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.transform != null && (hit.transform.GetComponent<BlockManager>() != null || hit.transform.GetComponent<ItemManager>() != null))
                {
                    Debug.Log("Nie mo¿na nic tu upuœciæ - zajête przez obiekt.");
                    return false;
                }
            }
        }

        return true;
    }
}
