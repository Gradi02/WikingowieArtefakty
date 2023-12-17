using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesManager : MonoBehaviour
{




    [ContextMenu("change")]
    public void ChangeBiome()
    {
        foreach(Transform m in GetComponentInChildren<Transform>())
        {
            if (m.GetComponent<MeshChanger>() != null)
            {
                m.GetComponent<MeshChanger>().ChangeMesh(MeshChanger.MeshType.ice);
            }
            else if(m.CompareTag("grass"))
            {
                m.gameObject.SetActive(false);
            }
            else
            {
                foreach (Transform n in m.GetComponentInChildren<Transform>())
                {
                    if (n.GetComponent<MeshChanger>() != null)
                    {
                        n.GetComponent<MeshChanger>().ChangeMesh(MeshChanger.MeshType.ice);
                    }
                }
            }
        }
    }

    [ContextMenu("change2")]
    public void ChangeBackBiome()
    {
        foreach (Transform m in GetComponentInChildren<Transform>())
        {
            if (m.GetComponent<MeshChanger>() != null)
            {
                m.GetComponent<MeshChanger>().ChangeMesh(MeshChanger.MeshType.normal);
            }
            else if (m.CompareTag("grass"))
            {
                m.gameObject.SetActive(true);
            }
            else
            {
                foreach (Transform n in m.GetComponentInChildren<Transform>())
                {
                    if (n.GetComponent<MeshChanger>() != null)
                    {
                        n.GetComponent<MeshChanger>().ChangeMesh(MeshChanger.MeshType.normal);
                    }
                }
            }
        }
    }
}
