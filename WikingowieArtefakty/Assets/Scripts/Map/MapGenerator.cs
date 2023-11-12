using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    public int islandLevel;
    [Min(1)] public int size;
    private int seed;
    public int middleRadius;
    [Range(0, 0.5f)] public float treeOffset;
    [Range(0, 0.5f)] public float rockOffset;
    [Range(1, 100)]  public int oresChance;

    [Header("Base 1")]
    public GameObject[] trees_variants1;
    public GameObject[] rocks_variants1;
    public GameObject[] ground_prefabs1;
    public Material defaultMaterial1;
    public Material[] oresMaterials1;
    public GameObject shipPrefab;

    [Header("Base 2")]
    public GameObject[] trees_variants2;
    public GameObject[] rocks_variants2;
    public GameObject[] ground_prefabs2;
    public Material defaultMaterial2;
    public Material[] oresMaterials2;

    [Header("Base 3")]
    public GameObject[] trees_variants3;
    public GameObject[] rocks_variants3;
    public GameObject[] ground_prefabs3;
    public Material defaultMaterial3;
    public Material[] oresMaterials3;

    [Header("Base 4")]
    public GameObject[] trees_variants4;
    public GameObject[] rocks_variants4;
    public GameObject[] ground_prefabs4;
    public Material defaultMaterial4;
    public Material[] oresMaterials4;

    [Header("Base 5")]
    public GameObject[] trees_variants5;
    public GameObject[] rocks_variants5;
    public GameObject[] ground_prefabs5;
    public Material defaultMaterial5;
    public Material[] oresMaterials5;

    [Header("Other Prefabs")]
    public GameObject waterLayer;
    public GameObject baseBuilding;
    public GameObject air;

    [Header("Others")]
    public Camera cam;
    public GameObject manager;
    public GameObject Trees, Rocks, Air, Ground; 

    private GameObject start;
    private Vector3 middle;
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> ground = new List<GameObject>();
    private List<GameObject> airBlocks = new List<GameObject>();


    private void Start()
    {
        start = this.gameObject;

        if(islandLevel < 1 || islandLevel > 5) islandLevel = 1;
        middle = start.transform.position + new Vector3(size / 2, 0, size / 2);
        manager.GetComponent<Manager>().SetMiddle(middle);

        if(islandLevel == 1) GenerateWorld1();
        else if (islandLevel == 2) GenerateWorld2();
        else if (islandLevel == 3) GenerateWorld3();
        else if (islandLevel == 4) GenerateWorld4();
        else if (islandLevel == 5) GenerateWorld5();

        SetMiddleMap();
        SetShip();
        SpawnBase();
        //player.GetComponent<PlayerMovement>().SetStartPosition(middle);
    }
    void GenerateWorld1()
    {
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        waterLayer.transform.localScale = new Vector3(size / 5, size / 5, size / 5);
        waterLayer.transform.position = middle + new Vector3(0, 0, 0);

        int oresTypes = oresMaterials1.Length;

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

                    if (id == 0) //Blok Powietrza
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
                    else if (id == 1) //Blok Trawy/Drzewa 
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
                            float randscale = Random.Range(0.5f, 0.75f);
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
                    else if (id == 2) //Blok ma貫go kamienia
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
                    else //Blok ska造
                    {
                        //Spawn kamienia
                        new_obj = Instantiate(rocks_variants1[0], transform.position, Quaternion.identity, start.transform);

                        //Skala na bazie noise
                        new_obj.transform.localScale = new Vector3(1, GetHeightByNoise(x,y) , 1);
                        new_obj.transform.localPosition = new Vector3(x, 0.5f, y);

                        //Losowa rotacja
                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                        //Losowanie rudy
                        int randore = Random.Range(1, 100);
                        if (oresChance > randore && oresMaterials1.Length < 0)
                        {
                            new_obj.transform.GetComponent<MeshRenderer>().material = oresMaterials1[Random.Range(0, oresTypes)];
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
    }

    void GenerateWorld2()
    {
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        waterLayer.transform.localScale = new Vector3(size / 5, size / 5, size / 5);
        waterLayer.transform.position = middle + new Vector3(0, 0, 0);

        int oresTypes = oresMaterials1.Length;

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

                    if (id == 0) //Blok Powietrza
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
                    else if (id == 1) //Blok Trawy/Drzewa 
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
                            float randscale = Random.Range(0.5f, 0.75f);
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
                    else if (id == 2) //Blok ma貫go kamienia
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
                            GameObject gr = Instantiate(ground_prefabs1[id - 1], transform.position, Quaternion.identity, Ground.transform);
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                    }
                    else //Blok ska造
                    {
                        //Spawn kamienia
                        new_obj = Instantiate(rocks_variants1[0], transform.position, Quaternion.identity, start.transform);

                        //Skala na bazie noise
                        new_obj.transform.localScale = new Vector3(1, GetHeightByNoise(x, y), 1);
                        new_obj.transform.localPosition = new Vector3(x, 0.5f, y);

                        //Losowa rotacja
                        int randrot = Random.Range(1, 4);
                        new_obj.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);

                        //Losowanie rudy
                        int randore = Random.Range(1, 100);
                        if (oresChance > randore && oresMaterials1.Length < 0)
                        {
                            new_obj.transform.GetComponent<MeshRenderer>().material = oresMaterials1[Random.Range(0, oresTypes)];
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
    }

    void GenerateWorld3()
    {

    }

    void GenerateWorld4()
    {

    }

    void GenerateWorld5()
    {

    }

    int GetIdPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * (5+1);

        return Mathf.Clamp(Mathf.FloorToInt(perlin), 0, 5);
    }

    float GetHeightByNoise(int x, int y)
    {
        float h = GetFloatPerlinNoise(x, y) / 2 - 0.2f;

        return (Mathf.Round(h*10)/10);
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

    void SetShip()
    {
        //Przywo豉j statek na brzegu mapy w losowym miejscu
        GameObject ship = Instantiate(shipPrefab, transform.position, Quaternion.identity);
        ship.name = "Ship";

        int xy = GetRandomSign();
        int ss = GetRandomSign() * 70;

        Vector3 shipPos = new();

        if (xy == 0) shipPos = new Vector3(ss, 0, Random.Range(0, size));
        else shipPos = new Vector3(Random.Range(0, size), 0, ss);

        ship.transform.position = shipPos;

        //Ustaw kamere na statek
        cam.GetComponent<CameraFollow>().Target = ship.transform;
        cam.GetComponent<CameraFollow>().SetPosition(shipPos);
        cam.GetComponent<CameraFollow>().SmoothTime = 2;
    }

    int GetRandomSign()
    {
        return Random.Range(-10, 9) < 0 ? 0 : 1;
    }

    void SpawnBase()
    {
        GameObject b = Instantiate(baseBuilding, middle + new Vector3(0,0.25f,0), Quaternion.identity);
        b.name = "base";
    }

    public List<GameObject> GetGroundBlocks()
    {
        return ground;
    }

    public List<GameObject> GetObjectsBlocks()
    {
        return blocks;
    }
}
