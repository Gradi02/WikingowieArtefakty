using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyBlock : NetworkBehaviour
{
    public bool enableAxe = false;
    public bool enablePickaxe = false;

    private float choptime = 1;
    private float nextchop = 0;
    private bool start = false;
    private Vector3 offset = new Vector3(0,0.25f,0);
   

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Mouse0)) start = true;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (enableAxe)
            {
                //animacja
                GetComponent<AnimController>().animator.SetBool("axe", true);

                RaycastHit hit;
                if(Physics.Raycast(transform.position + offset, transform.forward, out hit, 0.75f))
                {
                    
                    //Debug.DrawRay(transform.position + offset, transform.forward, Color.red, 0.75f);
                    if(hit.transform.GetComponent<BlockManager>() != null && hit.transform.GetComponent<BlockManager>().breakingTool == BlockManager.Tools.Axe)
                    {
                        if (Time.time >= nextchop)
                        {
                            nextchop = Time.time + choptime;

                            if (!start) hit.transform.GetComponent<BlockManager>().NextBreakStepServerRpc();
                            start = false;
                        }
                    }
                }             
            }
            else if (enablePickaxe)
            {
                GetComponent<AnimController>().animator.SetBool("pickaxe", true);
                RaycastHit hit;
                if (Physics.Raycast(transform.position + offset, transform.forward, out hit, 0.75f))
                {
                    if (hit.transform.GetComponent<BlockManager>() != null && hit.transform.GetComponent<BlockManager>().breakingTool == BlockManager.Tools.Pickaxe)
                    {
                        if (Time.time >= nextchop)
                        {
                            nextchop = Time.time + choptime;

                            if (!start) hit.transform.GetComponent<BlockManager>().NextBreakStepServerRpc();
                            start = false;
                        }
                    }
                }
            }
        }
        else
        {
            //wylacz anim
            GetComponent<AnimController>().animator.SetBool("axe", false);
            GetComponent<AnimController>().animator.SetBool("pickaxe", false);
        }
    }
}
