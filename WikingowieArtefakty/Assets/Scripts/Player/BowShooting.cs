using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowShooting : MonoBehaviour
{
    private float ShotCooldown = 0.5f;
    public GameObject arrow;
    public GameObject bowPowerUI;
    public Transform bpParent;

    private float nextShot;
    private float power = 1;
    private float maxPower = 9;
    private bool next = true;
    private GameObject bp;

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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if(bp == null)
                {
                    bp = Instantiate(bowPowerUI, transform.position, Quaternion.identity, bpParent);
                }

                power += power * 0.04f;

                if(power > maxPower)
                { 
                    power = Mathf.Clamp(power, 1, maxPower);
                    next = false;
                    ShotArrow();
                    nextShot = Time.time;
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
        Destroy(bp);
        GameObject a = Instantiate(arrow, transform.position, Quaternion.identity);
        a.GetComponent<BulletManager>().SetDamage(power);

        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 hitPoint = Vector3.zero;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance)) hitPoint = ray.GetPoint(distance);

        Vector3 playerDirection = new Vector3(hitPoint.x - transform.position.x, 0.02f, hitPoint.z - transform.position.z).normalized;
        a.transform.right = playerDirection;
        playerDirection += new Vector3(Random.Range(-power / 200, power / 200), 0, Random.Range(-power / 200, power / 200));
        a.GetComponent<Rigidbody>().AddForce(playerDirection * power, ForceMode.Impulse);

        bowPowerUI.transform.localScale = Vector3.zero;
        power = 1;
        nextShot = Time.time + ShotCooldown;
    }
}
