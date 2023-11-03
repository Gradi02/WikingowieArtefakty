using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;


    private void Start()
    {
        SetPosition();
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    [ContextMenu("pick")]
    public void PickUp()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().PickUpItem(this.gameObject);
    }

    void SetPosition()
    {
        transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x), 0.25f, Mathf.RoundToInt(transform.localPosition.z));
        transform.rotation = Quaternion.identity;
    }
}
