using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShooting : MonoBehaviour
{
    private float ShotCooldown = 0.1f;
    public GameObject arrow;
    public GameObject bowPowerUI;
    public Animation anim;

    private float nextShot;
    private float power = 1;
    private float maxPower = 8;
    private bool next = true;

    private void Start()
    {
        bowPowerUI.transform.localScale = Vector3.zero;
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && !next)
        {
            next = true;
            return;
        }
        
        if(Time.time >= nextShot && next) 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                anim.Play();

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
        GameObject a = Instantiate(arrow, transform.position, transform.rotation);

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 hitPoint = Vector3.zero;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance)) hitPoint = ray.GetPoint(distance);

        Vector3 playerDirection = new Vector3(hitPoint.x - transform.position.x, 0.02f, hitPoint.z - transform.position.z).normalized;
        a.GetComponent<Rigidbody>().AddForce(playerDirection * power, ForceMode.Impulse);

        anim.Stop();
        bowPowerUI.transform.localScale = Vector3.zero;
        power = 1;
        nextShot = Time.time + ShotCooldown;
    }
}
