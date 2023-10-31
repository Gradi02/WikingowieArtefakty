using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public bool enableAxe = false;
    public bool enablePickaxe = false;

    private float choptime = 1;
    private float nextchop = 0;
    private bool start = false;
    private Vector3 offset = new Vector3(0,0.25f,0);
   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) start = true;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (enableAxe)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position - offset, transform.right, out hit, 0.5f))
                {
                    if(hit.transform.GetComponent<BlockManager>() != null && hit.transform.GetComponent<BlockManager>().breakingTool == BlockManager.Tools.Axe)
                    {
                        if (Time.time >= nextchop)
                        {
                            nextchop = Time.time + choptime;
                            GetComponent<PlayerInfo>().ChopAnimation();

                            if (!start) hit.transform.GetComponent<BlockManager>().NextBreakStep();
                            start = false;
                        }
                    }
                }             
            }
            else if (enablePickaxe)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position - offset, transform.right, out hit, 0.5f))
                {
                    if (hit.transform.GetComponent<BlockManager>() != null && hit.transform.GetComponent<BlockManager>().breakingTool == BlockManager.Tools.Pickaxe)
                    {
                        if (Time.time >= nextchop)
                        {
                            nextchop = Time.time + choptime;
                            GetComponent<PlayerInfo>().ChopAnimation();

                            if (!start) hit.transform.GetComponent<BlockManager>().NextBreakStep();
                            start = false;
                        }
                    }
                }
            }
        }
    }
}
