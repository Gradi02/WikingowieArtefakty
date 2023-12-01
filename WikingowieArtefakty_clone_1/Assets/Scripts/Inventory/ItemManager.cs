using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ItemManager : NetworkBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    private ItemsDropManager dropManager;
    private bool pickable = true;

    private void Start()
    {
        dropManager = GameObject.FindGameObjectWithTag("manager").GetComponent<ItemsDropManager>();

        SetPosition();
        dropManager.SetItem(true, Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));
    }

    public void PickUp(GameObject player)
    {
        player.GetComponent<InventoryManager>().PickUpItem(gameObject);
        //SetDropManagerServerRpc();
        //dropManager.SetItem(false ,Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));     
        //GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().PickUpItem(gameObject);
    }

    public void ClearPlace()
    {
        dropManager.SetItem(false, Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.z));
    }

    [ClientRpc]
    public void SetDropManagerClientRpc()
    {
    }

    void SetPosition()
    {
        transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x), 0.25f, Mathf.RoundToInt(transform.localPosition.z));
        transform.rotation = Quaternion.identity;
    }

    public void SetPickable(bool pin)
    {
        pickable = pin;
    }

    public bool GetPickable()
    {
        return pickable;
    }
}
