using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesManager : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private Vector3 middlePosition;
    private List<GameObject> newBiomeObjects = new List<GameObject>();

    [ContextMenu("change0")]
    public void SetBiomWithMiddle()
    {
        Collider[] objects = Physics.OverlapSphere(middlePosition, distance);

        foreach (Collider col in objects)
        {       
            if (col.GetComponent<MeshChanger>() != null)
            {
                newBiomeObjects.Add(col.gameObject);
            }
        }

        StartCoroutine(SetBiom());
    }

    private IEnumerator SetBiom()
    {
        int num = newBiomeObjects.Count;
        for (int i=0; i<num; i++)
        {
            yield return new WaitForSeconds(0.1f);

            int rand = Random.Range(0, newBiomeObjects.Count);
            newBiomeObjects[rand].GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.ice);
            newBiomeObjects.Remove(newBiomeObjects[rand]);
        }
    }








    [ContextMenu("change")]
    public void ChangeBiome()
    {
        foreach(Transform m in GetComponentInChildren<Transform>())
        {
            if (m.GetComponent<MeshChanger>() != null)
            {
                m.GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.ice);
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
                        n.GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.ice);
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
                m.GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.normal);
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
                        n.GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.normal);
                    }
                }
            }
        }
    }
}
