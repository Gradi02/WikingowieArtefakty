using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private float damage = 5;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "player")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
            Destroy(this.gameObject, 2);
        }
    }

    public void SetDamage(float dmgin)
    {
        damage = dmgin;
    }

    public float GetDamage()
    {
        return damage;
    }
}
