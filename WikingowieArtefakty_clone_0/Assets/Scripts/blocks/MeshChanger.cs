using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChanger : MonoBehaviour
{
    public bool ifGround = false;
    [SerializeField] private MeshFilter MeshFilter;

    //objects
    [SerializeField] private Mesh normalMesh;
    [SerializeField] private Mesh iceMesh;

    //grounds
    private Material normalMaterial; 
    [SerializeField] private Material iceMaterial;


    private void Start()
    {
        if (ifGround)
        {
            normalMaterial = GetComponent<MeshRenderer>().material;
        }
    }

    public enum MeshType
    {
        normal,
        ice
    }

    public void RequestChangeMesh(MeshType type, Material[] mats)
    {
        if (transform.gameObject.activeSelf) StartCoroutine(ChangeAnimation(type, mats));
        else ChangeMesh(type);
    }

    private IEnumerator ChangeAnimation(MeshType type, Material[] mats)
    {
        Vector3 startScale = transform.localScale;
        Material startMaterial = ifGround ? GetComponent<MeshRenderer>().material : MeshFilter.transform.GetComponent<MeshRenderer>().material;

        for(int i=0; i<10; i++)
        {
            yield return new WaitForSeconds(0.02f);

            Vector3 newScale = new Vector3(
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.x),
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.y),
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.z));
            transform.localScale = newScale;

            if(ifGround) GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Length)];
            else MeshFilter.transform.GetComponent<MeshRenderer>().material = mats[Random.Range(0,mats.Length)];
        }

        transform.localScale = startScale;
        if (ifGround) GetComponent<MeshRenderer>().material = startMaterial;
        else MeshFilter.transform.GetComponent<MeshRenderer>().material = startMaterial;

        ChangeMesh(type);
    }

    private void ChangeMesh(MeshType type)
    {
        if (type == MeshType.normal)
        {
            if (ifGround)
            {
                GetComponent<MeshRenderer>().material = normalMaterial;
                return;
            }

            MeshFilter.mesh = normalMesh;
        }
        else if (type == MeshType.ice)
        {
            if (ifGround)
            {
                GetComponent<MeshRenderer>().material = iceMaterial;
                return;
            }
            MeshFilter.mesh = iceMesh;
        }
    }
}
