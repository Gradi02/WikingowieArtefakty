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
    [Range(0, 0.5f)] public float rockOffset;
    public int islandLevel;
    [Range(1, 100)]  public int oresChance;

    [Header("Base 1")]
    public GameObject[] blocks_prefabs1;

    public GameObject[] trees_variants1;
    public GameObject[] rocks_variants1;
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
    public GameObject Trees, Rocks, Air, Ground; 

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
        GenerateWorld1();
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

                if (Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius) id = 1;

                if (id > 0)
                {
                    GameObject new_obj;
                    
                    
                    if (id == 2)
                    {
                        int randvar = Random.Range(0, rocks_variants1.Length);
                        new_obj = Instantiate(rocks_variants1[randvar], transform.position, Quaternion.identity, start.transform);
                    }
                    else
                    {
                        new_obj = Instantiate(blocks_prefabs1[id - 1], transform.position, Quaternion.identity, start.transform);
                    }

                    if (id == 1) new_obj.transform.parent = Trees.transform;
                    else new_obj.transform.parent = Rocks.transform;

                    new_obj.transform.localPosition = new Vector3(x, 0.75f, y);
                    new_obj.name = "Object" + x + y;

                    if (id == 1 && ifTree == 1) Destroy(new_obj);
                    else if (id == 1)
                    {
                        new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), -0.5f, Random.Range(-treeOffset, treeOffset));
                        
                        float randscale = Random.Range(1f, 1.5f);
                        new_obj.transform.localScale = new Vector3(randscale, randscale, randscale);

                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                    }
                    else if(id == 2)
                    {
                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                        new_obj.transform.localPosition = new Vector3(x, 0.2f, y);
                        new_obj.transform.localPosition += new Vector3(Random.Range(-rockOffset, rockOffset), 0, Random.Range(-rockOffset, rockOffset));
                    }
                    else if (id == 3 || id == 4)
                    {
                        int randore = Random.Range(1, 100);

                        new_obj.transform.localScale = new Vector3(1, GetFloatPerlinNoise(x, y) / 2 - 0.2f, 1);

                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                        new_obj.transform.localPosition = new Vector3(x, 0.5f, y);
                        if (oresChance > randore && oresMaterials.Length < 0)
                        {
                            new_obj.transform.GetComponent<MeshRenderer>().material = oresMaterials[Random.Range(0, oresTypes)];
                        }
                    }

                    if (new_obj != null)
                    {
                        blocks.Add(new_obj);

                        GameObject gr = Instantiate(ground_prefabs1[id - 1], transform.position, Quaternion.identity, Ground.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);

                        if (id == 2 || id == 3 || id == 4)
                        {
                            int randrot = Random.Range(1, 4);
                            gr.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);
                        }

                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                }
                else if (id == 0)
                {
                    if (x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6)
                    {
                        GameObject gr = Instantiate(ground_prefabs1[ground_prefabs1.Length - 1], transform.position, Quaternion.identity, Ground.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                    else
                    {
                        GameObject a = Instantiate(air, transform.position, Quaternion.identity, Air.transform);
                        a.transform.localPosition = new Vector3(x, 0.25f, y);
                        a.name = "Air" + x + y;
                        airBlocks.Add(a);
                    }
                }
            
            }
        }

        player.GetComponent<PlayerMovement>().SetStartPosition(middle);
    }

    void GenerateWorld1()
    {
        waterLayer.transform.localScale = new Vector3(size / 5, size / 5, size / 5);
        waterLayer.transform.position = middle + new Vector3(0, 0, 0);

        int oresTypes = oresMaterials.Length;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int id = GetIdPerlinNoise(x, y);
                int ifempty = Random.Range(0, 3);

                if (x <= 3 || y <= 3 || x >= size - 3 || y >= size - 3) id = 0;
                if ((x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6) && id > 1) id = 1;
                if (Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius) id = 1;
                if (Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius + 1 && id > 2) id = 2;

                if (id >= 0)
                {
                    GameObject new_obj;

                    if (id == 0)
                    {
                        if (x <= 6 || y <= 6 || x >= size - 6 || y >= size - 6)
                        {
                            GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity, Ground.transform);
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Sand" + x + y;
                            ground.Add(gr);
                        }
                        else
                        {
                            GameObject a = Instantiate(air, transform.position, Quaternion.identity, Air.transform);
                            a.transform.localPosition = new Vector3(x, 0.25f, y);
                            a.name = "Air" + x + y;
                            airBlocks.Add(a);
                        }
                    }
                    else if (id == 1)
                    {
                        if (ifempty == 1 || ifempty == 2)
                        {
                            //Losowanie drzewa
                            int randvar = Random.Range(0, trees_variants1.Length);
                            new_obj = Instantiate(trees_variants1[randvar], transform.position, Quaternion.identity, start.transform);

                            //Offset drzewa na kratce
                            new_obj.transform.localPosition = new Vector3(x, 0.75f, y);
                            new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), -0.5f, Random.Range(-treeOffset, treeOffset));

                            //Losowa skala
                            float randscale = Random.Range(0.5f, 1f);
                            new_obj.transform.localScale = new Vector3(randscale, randscale, randscale);

                            //Losowa rotacja
                            int randrot = Random.Range(1, 4);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                            //Przypisanie do rodzica
                            new_obj.name = "Tree" + x + y;
                            new_obj.transform.parent = Trees.transform;
                            blocks.Add(new_obj);
                        }

                        //Pod這瞠
                        GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity, Ground.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                    else if (id == 2)
                    {
                        if (ifempty == 1 || ifempty == 2)
                        {
                            //Losowanie kamienia
                            int randvar = Random.Range(1, rocks_variants1.Length);
                            new_obj = Instantiate(rocks_variants1[randvar], transform.position, Quaternion.identity, start.transform);

                            //Losowa rotacja
                            int randrot = Random.Range(1, 4);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                            //Pozycja
                            new_obj.transform.localPosition = new Vector3(x, 0.19f, y);

                            //Offset kamienia na kratce
                            new_obj.transform.localPosition += new Vector3(Random.Range(-rockOffset, rockOffset), 0, Random.Range(-rockOffset, rockOffset));

                            //Przypisanie do rodzica
                            new_obj.name = "Rock" + x + y;
                            new_obj.transform.parent = Rocks.transform;
                            blocks.Add(new_obj);


                            //Pod這瞠
                            GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity, Ground.transform);
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                        else
                        {
                            //Pod這瞠
                            GameObject gr = Instantiate(ground_prefabs1[id-1], transform.position, Quaternion.identity, Ground.transform);
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                    }
                    else
                    {
                        //Spawn kamienia
                        new_obj = Instantiate(rocks_variants1[0], transform.position, Quaternion.identity, start.transform);

                        //Skala na bazie noise
                        new_obj.transform.localScale = new Vector3(1, GetFloatPerlinNoise(x, y) / 2 - 0.2f, 1);
                        new_obj.transform.localPosition = new Vector3(x, 0.5f, y);

                        //Losowa rotacja
                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                        //Losowanie rudy
                        int randore = Random.Range(1, 100);
                        if (oresChance > randore && oresMaterials.Length < 0)
                        {
                            new_obj.transform.GetComponent<MeshRenderer>().material = oresMaterials[Random.Range(0, oresTypes)];
                        }

                        //Przypisanie do rodzica
                        new_obj.name = "Mountain" + x + y;
                        new_obj.transform.parent = Rocks.transform;
                        blocks.Add(new_obj);

                        //Pod這瞠
                        GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity, Ground.transform);
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
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
        float perlin = Mathf.PerlinNoise(nx, ny) * (5+1);

        return Mathf.Clamp(Mathf.FloorToInt(perlin), 0, 5);
    }

    float GetFloatPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * (5 + 1);

        return Mathf.Clamp(perlin, 0, 5); //blocks_prefabs1.Length
    }
    void SetMiddleMap()
    {
        
        foreach (GameObject g in blocks)
        {
            if (Vector3.Distance(g.transform.position, middle) < middleRadius - 0.5f)
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
