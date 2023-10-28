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
    public int islandLevel;
    [Range(1, 100)]  public int oresChance;

    [Header("Base 1")]
    public GameObject[] blocks_prefabs1;
    public GameObject[] ground_prefabs1;
    public Material defaultMaterial1;
    public Material[] oresMaterials;

    [Header("Base 2")]
    public GameObject[] blocks_prefabs2;
    public GameObject[] ground_prefabs2;
    public Material defaultMaterial2;

    [Header("Base 3")]
    public GameObject[] blocks_prefabs3;
    public GameObject[] ground_prefabs3;
    public Material defaultMaterial3;

    [Header("Base 4")]
    public GameObject[] blocks_prefabs4;
    public GameObject[] ground_prefabs4;
    public Material defaultMaterial4;

    [Header("Base 5")]
    public GameObject[] blocks_prefabs5;
    public GameObject[] ground_prefabs5;
    public Material defaultMaterial5;

    [Header("Others")]
    public GameObject waterLayer;
    public GameObject player;
    public GameObject air;

    private GameObject start;
    private Vector3 middle;
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> ground = new List<GameObject>();
    private List<GameObject> airBlocks = new List<GameObject>();

    private void Start()
    {
        start = this.gameObject;
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        if(islandLevel < 1 || islandLevel > 5) islandLevel = 1;
        middle = start.transform.position + new Vector3(size / 2, 0, size / 2);
        GenerateWorld();
        SetMiddleMap();
    }

    void GenerateWorld()
    {
        waterLayer.transform.localScale = new Vector3(size/5,size/5,size/5);
        waterLayer.transform.position = middle + new Vector3(0,0,0);

        int oresTypes = oresMaterials.Length;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int id = GetIdPerlinNoise(x,y);
                int ifTree = Random.Range(0, 2);

                if (x <= 3 || y <= 3 || x >= size - 3 || y >= size - 3) id = 0;
                if ((x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6) && id > 1) id = 1;
                //Debug.Log("Odleglosc: " + Vector3.Distance(middle, new Vector3(x,0,y)) + " middle: " + middle + " x,y: " + x + ";" + y);
                if (Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius) id = 1;

                if (id > 0)
                {
                    GameObject new_obj = Instantiate(blocks_prefabs1[id - 1], transform.position, Quaternion.identity, start.transform);
                    new_obj.transform.localPosition = new Vector3(x, 0.75f, y);
                    new_obj.name = "Object" + x + y;

                    if (id == 1 && ifTree == 1) Destroy(new_obj);
                    else if (id == 1) new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), 0, Random.Range(-treeOffset, treeOffset));
                    else if (id == 2)
                    {
                        int randore = Random.Range(1, 100);
                        if(oresChance > randore)
                        {
                            new_obj.transform.GetComponent<MeshRenderer>().material = oresMaterials[Random.Range(0, oresTypes)];
                        }
                    }

                    if (new_obj != null)
                    {
                        blocks.Add(new_obj);

                        GameObject gr = Instantiate(ground_prefabs1[id - 1], transform.position, Quaternion.identity, start.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                }
                else if (id == 0)
                {
                    if (x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6)
                    {
                        GameObject gr = Instantiate(ground_prefabs1[ground_prefabs1.Length - 1], transform.position, Quaternion.identity, start.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                    else
                    {
                        GameObject a = Instantiate(air, transform.position, Quaternion.identity, start.transform);
                        a.transform.localPosition = new Vector3(x, 0.25f, y);
                        a.name = "Air" + x + y;
                        airBlocks.Add(a);
                    }
                }

            }
        }

        player.GetComponent<PlayerMovement>().SetStartPosition(middle);
    }

    int GetIdPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * (blocks_prefabs1.Length+1);

        return Mathf.Clamp(Mathf.FloorToInt(perlin), 0, blocks_prefabs1.Length);
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

    public List<GameObject> GetGroundBlocks()
    {
        return ground;
    }
}
