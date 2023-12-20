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

    public void RequestChangeMesh(MeshType type)
    {
        StartCoroutine(ChangeAnimation(type));    
    }

    private IEnumerator ChangeAnimation(MeshType type)
    {
        Vector3 startScale = transform.localScale;
        for(int i=0; i<10; i++)
        {
            yield return new WaitForSeconds(0.02f);

            Vector3 newScale = new Vector3(
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.x),
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.y),
                Mathf.Abs(Random.Range(-0.1f, 0.1f) + transform.localScale.z));

            transform.localScale = newScale;
        }
        transform.localScale = startScale;
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
