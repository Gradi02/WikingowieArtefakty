using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walk", true);
        }
        else animator.SetBool("walk", false);

        /////////////////////////////////////

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("dash", true);
        }
        else animator.SetBool("dash", false);

        if (Input.GetKey(KeyCode.R))
        {
            animator.SetBool("axe", true);
        }
        else animator.SetBool("axe", false);

        if (Input.GetKey(KeyCode.F))
        {
            animator.SetBool("pickaxe", true);
        }
        else animator.SetBool("pickaxe", false);
    }
}
