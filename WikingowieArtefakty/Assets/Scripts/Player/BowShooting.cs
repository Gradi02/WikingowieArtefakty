using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShooting : MonoBehaviour
{
    private float ShotCooldown = 0.1f;
    public GameObject arrow;

    private float nextShot;
    private float power = 1;
    private float maxPower = 8;
    private bool next = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !next)
        {
            next = true;
            return;
        }
        
        if(Time.time >= nextShot && next) 
        {

            if (Input.GetKey(KeyCode.Mouse0))
            {
                power += power * 0.05f;

                if(power > maxPower)
                { 
                    power = Mathf.Clamp(power, 1, maxPower);
                    next = false;
                    ShotArrow();
                    return;
                }
            }

            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                ShotArrow();
            }
        }
    }

    void ShotArrow()
    {
        GameObject a = Instantiate(arrow, transform.position, Quaternion.identity);

        float playerRotation = transform.eulerAngles.y + 90;

        float angleInRadians = playerRotation * Mathf.Deg2Rad;

        float directionX = Mathf.Sin(angleInRadians);
        float directionZ = Mathf.Cos(angleInRadians);
        Vector3 playerDirection = new Vector3(directionX, 0.02f, directionZ).normalized;

        Debug.Log(power + "; " + playerDirection);
        //a.transform.LookAt(mousePos);
        a.GetComponent<Rigidbody>().AddForce(playerDirection * power, ForceMode.Impulse);

        power = 1;
        nextShot = Time.time + ShotCooldown;
    }
}
