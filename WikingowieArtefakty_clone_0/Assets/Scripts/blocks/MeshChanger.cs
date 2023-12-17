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

    public void ChangeMesh(MeshType type)
    {

        if(type == MeshType.normal)
        {
            if(ifGround)
            {
                GetComponent<MeshRenderer>().material = normalMaterial;
                return;
            }

            MeshFilter.mesh = normalMesh;
        }
        else if(type == MeshType.ice)
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
