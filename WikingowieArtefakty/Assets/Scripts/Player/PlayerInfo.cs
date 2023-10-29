using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private PlayerMovement move;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            CheckForBridge();
        }
    }

    void CheckForBridge()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right);

        if(Physics.Raycast(transform.position, transform.right, out hit, 1))
        {
            if(hit.transform.GetComponent<Bridge>() != null)
                hit.transform.GetComponent<Bridge>().BuildBridge();
        }
    }
}
