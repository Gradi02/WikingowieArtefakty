using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    void Start()
    {

        // Uruchom animacj� o nazwie "NazwaTwojejAnimacji"
        animator.Play("player");
    }
}
