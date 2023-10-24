using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    [Min(1)] public int size;
    private int seed;
    public int middleRadius;
    [Range(0, 0.5f)] public float treeOffset;

    [Header("Base 1")]
    public GameObject[] blocks_prefabs1;
    public GameObject[] ground_prefabs1;
    public Material defaultMaterial1;

    [Header("Others")]
    public GameObject firstLayer;
    public GameObject cam;

    private GameObject start;
    private Vector3 middle;
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> ground = new List<GameObject>();

    private void Start()
    {
        start = this.gameObject;
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        middle = start.transform.position + new Vector3(size / 2, 0, size / 2);
        GenerateWorld();
        SetMiddleMap();
    }

    void GenerateWorld()
    {
        firstLayer.transform.localScale = new Vector3(size/10,size/10,size/10);
        firstLayer.transform.position = middle + new Vector3(0,-1.25f,0);
        cam.transform.position = middle;
        cam.transform.position += new Vector3(0, 20, -20);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int id = GetIdPerlinNoise(x,y);
                int ifTree = Random.Range(0, 2);

                if (x <= 3 || y <= 3 || x >= size - 3 || y >= size - 3) id = 0;
                if ((x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6) && id > 1) id = 1;

                if (id > 0)
                {
                    GameObject new_obj = Instantiate(blocks_prefabs1[id - 1], transform.position, Quaternion.identity, start.transform);
                    new_obj.transform.localPosition = new Vector3(x, 0.25f, y);
                    new_obj.name = "Object" + x + y;

                    if (id == 1 && ifTree == 1) Destroy(new_obj);
                    else if (id == 1) new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), 0, Random.Range(-treeOffset, treeOffset));

                    if (new_obj != null)
                    {
                        blocks.Add(new_obj);

                        GameObject gr = Instantiate(ground_prefabs1[id - 1], transform.position, Quaternion.identity, start.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                }
                else if(id == 0)
                {
                    GameObject gr = Instantiate(ground_prefabs1[3], transform.position, Quaternion.identity, start.transform);
                    gr.transform.localPosition = new Vector3(x, -0.25f, y);
                    gr.name = "Ground" + x + y;
                    ground.Add(gr);
                }
            }
        }
    }

    int GetIdPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * 4;

        return Mathf.Clamp(Mathf.FloorToInt(perlin), 0, 3);
    }
    void SetMiddleMap()
    {
        foreach (GameObject g in blocks)
        {
            if (Vector3.Distance(g.transform.position, middle) < middleRadius)
            {      
                Destroy(g);
            }
        }

        foreach (GameObject g in ground)
        {
            if (Vector3.Distance(g.transform.position, middle) < middleRadius)
            {
                g.GetComponent<MeshRenderer>().material = defaultMaterial1;
            }
        }
    }
}
