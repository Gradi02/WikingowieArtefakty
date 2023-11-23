using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPowerMovement : MonoBehaviour
{
    public GameObject bp;

    private void Update()
    {
        bp.transform.position = Input.mousePosition;
    }
}
