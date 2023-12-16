using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SkeletonMovement : NetworkBehaviour
{
    public float speed;
    public float seeDistance;
    public float abandonTime;

    
    private float newTargetDelay = 2;
    private bool searchForNewTarget = true;
    
    
    private GameObject target = null;
    private GameObject[] players;

    private Rigidbody rb;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if (!IsHost) return;

        //Wyszukiwanie nowego targetu
        if (target == null && searchForNewTarget)
        {
            foreach (GameObject player in players)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= seeDistance)
                {
                    target = player;
                    searchForNewTarget = false;
                    //Debug.Log("New target has been found!");
                }
            }
        }

        //Pod¹¿anie za nowym targetem
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            transform.position += speed * Time.fixedDeltaTime * direction;
            //rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * direction);
            //Debug.Log(direction);

            if (Vector3.Distance(target.transform.position, transform.position) > seeDistance)
            {
                target = null;
                searchForNewTarget = true;
            }
        }
    }

    IEnumerator TargetAbandonTime()
    {
        target = null;
        yield return new WaitForSeconds(newTargetDelay);
        searchForNewTarget = true;
    }

    private void Update()
    {
        //if (!IsHost) return;

        if(target != null)
        {
            //rotacja
            float angle = -AngleBetweenTwoPoints(target.transform.position, transform.position);
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * speed * Time.deltaTime);
        }
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
