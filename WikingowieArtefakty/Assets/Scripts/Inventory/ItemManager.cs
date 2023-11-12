using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    private ItemsDropManager dropManager;

    private void Start()
    {
        dropManager = GameObject.FindGameObjectWithTag("manager").GetComponent<ItemsDropManager>();

        SetPosition();
        dropManager.SetItem(true, Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    [ContextMenu("pick")]
    public void PickUp()
    {
        dropManager.SetItem(false ,Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));     
        GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().PickUpItem(gameObject);
    }

    void SetPosition()
    {
        transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x), 0.25f, Mathf.RoundToInt(transform.localPosition.z));
        transform.rotation = Quaternion.identity;
    }
}
