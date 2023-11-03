using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    [ContextMenu("pick")]
    public void PickUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().PickUpItem(this.gameObject);
    }
}
