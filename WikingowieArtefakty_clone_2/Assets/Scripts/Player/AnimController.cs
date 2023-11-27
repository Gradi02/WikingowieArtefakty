using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class AnimController : NetworkBehaviour
{
    // Start is called before the first frame update
    public Animator animator;


    private void Update()
    {
        if (!IsOwner) return;

        // WASD ANIM
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }






        /*if (Input.GetKey(KeyCode.R))
        {
            animator.SetBool("axe", true);
        }
        else animator.SetBool("axe", false);

        if (Input.GetKey(KeyCode.F))
        {
            animator.SetBool("pickaxe", true);
        }
        else animator.SetBool("pickaxe", false);*/
    }


    public void DashAnim()
    {
        animator.SetBool("walk", true);
        //animator.SetBool("dash", true);
        animator.speed = 5;
        Invoke(nameof(ResetSpeed), 0.5f);
    }

    private void ResetSpeed()
    {
        animator.speed = 1;
    }
}
