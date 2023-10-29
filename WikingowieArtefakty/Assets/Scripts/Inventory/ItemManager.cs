using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Item")]
public class ItemManager : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemModel;
}
