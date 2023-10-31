using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public GameObject bridgePrefab;
    public ParticleSystem buildParticle;
    private GameObject bridgeObj;

    public void BuildBridge()
    {
        if (bridgeObj != null)
        {
            Debug.Log("Most ju¿ zosta³ tutaj zbudowany!");
            return;
        }

        bridgeObj = Instantiate(bridgePrefab, transform.position, Quaternion.identity, transform);
        bridgeObj.transform.position -= new Vector3(0,0.5f,0);
        buildParticle.Play();
        GetComponent<BoxCollider>().enabled = false;
    }
}
