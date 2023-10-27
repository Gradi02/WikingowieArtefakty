using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float time = 2;
    private void Awake()
    {
        Destroy(gameObject, time);
    }
}
