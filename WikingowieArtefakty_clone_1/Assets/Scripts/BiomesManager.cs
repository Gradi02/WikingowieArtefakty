using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float distance;
    [SerializeField] private Vector3 middlePosition;
    private List<GameObject> newBiomeObjects = new List<GameObject>();

    [Header("Materials")]
    public Material[] materials;


    [ContextMenu("change")]
    public void SetBiomWithMiddle()
    {
        /*Collider[] objects = Physics.OverlapSphere(middlePosition, distance);

        foreach (Collider col in objects)
        {       
            if (col.GetComponent<MeshChanger>() != null)
            {
                newBiomeObjects.Add(col.gameObject);
            }
        }*/

        RaycastHit[] hits = Physics.SphereCastAll(middlePosition, distance, Vector3.up, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            MeshChanger meshChanger = hit.collider.GetComponent<MeshChanger>();
            if (meshChanger != null)
            {
                newBiomeObjects.Add(hit.collider.gameObject);
            }
        }

        StartCoroutine(SetBiom());
    }

    private IEnumerator SetBiom()
    {
        int num = newBiomeObjects.Count;
        for (int i=0; i<num; i++)
        {
            yield return new WaitForSeconds(0.01f);

            int rand = Random.Range(0, newBiomeObjects.Count);
            newBiomeObjects[rand].GetComponent<MeshChanger>().RequestChangeMesh(MeshChanger.MeshType.ice, materials);
            newBiomeObjects.Remove(newBiomeObjects[rand]);
        }
    }

    
}
