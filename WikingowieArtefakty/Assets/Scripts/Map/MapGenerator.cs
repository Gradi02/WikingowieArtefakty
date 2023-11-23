using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapGenerator : NetworkBehaviour
{
    [Header("Generator Settings")]
    public int islandLevel;
    [Min(1)] public int size;
    private int seed;
    public int middleRadius;
    public int ruinRadius;
    [Range(0, 0.5f)] public float treeOffset;
    [Range(0, 0.5f)] public float rockOffset;
    [Range(1, 100)]  public int oresChance;

    [Header("Base")]
    public GameObject[] trees_variants1;
    public GameObject[] rocks_variants1;
    public GameObject[] ground_prefabs1;
    public Material defaultMaterial1;
    public Material[] oresMaterials1;
    public GameObject shipPrefab;
    public GameObject grass;

    [Header("Other Prefabs")]
    public GameObject waterLayerPrefab;
    public GameObject waterLayer;
    public GameObject campfire;
    public GameObject air;

    [Header("Others")]
    public Camera cam;
    public GameObject manager;
    public GameObject startPlane;
    public TimeManager timeManager;

    private Vector3 ruinsPos;
    private Vector3[] treeArea = new Vector3[5];
    private int treeAreaSize = 6;

    private GameObject start;
    public static Vector3 middle = Vector3.zero;
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> ground = new List<GameObject>();
    private List<GameObject> airBlocks = new List<GameObject>();

    private bool isnew = false;
    [HideInInspector] public NetworkVariable<bool> started = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        start = this.gameObject;

        middle = start.transform.position + new Vector3(size / 2, 0, size / 2);
        manager.GetComponent<Manager>().SetMiddle(middle);
        
        startPlane.transform.position = middle + new Vector3(0,0.25f,0);

        waterLayer = Instantiate(waterLayerPrefab, transform.position, Quaternion.identity);
        //waterLayer.GetComponent<NetworkObject>().Spawn();
        waterLayer.name = "waterLayer";
        waterLayer.transform.localScale = new Vector3(size / 5, size / 5, size / 5);
        waterLayer.transform.position = middle + new Vector3(0, 0, 0);

        GameObject campfire1 = SpawnBase();
        cam.GetComponent<CameraFollow>().Target = campfire1.transform;
    }

    private void Update()
    {
        if (IsClient && started.Value && !isnew)
        {
            isnew = true;
            waterLayer.SetActive(true);
            startPlane.SetActive(false);
            timeManager.StartDayOne();
        }
    }

    [ServerRpc]
    public void GenerateWorldServerRpc()
    {
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        /* waterLayer = Instantiate(waterLayerPrefab, transform.position, Quaternion.identity);
         //waterLayer.GetComponent<NetworkObject>().Spawn();
         waterLayer.name = "waterLayer";
         waterLayer.transform.localScale = new Vector3(size / 5, size / 5, size / 5);
         waterLayer.transform.position = middle + new Vector3(0, 0, 0);*/

        SetRuins();

        for(int i=0; i<5; i++)
        {
            treeArea[i] = RandomXY();

            if (i == 1)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20) i--;
            }
            else if(i == 2)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20) i--;
            }
            else if (i == 3)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 3]) < 20) i--;
            }
            else if (i == 4)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 3]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 4]) < 20) i--;
            }
        }

        int oresTypes = oresMaterials1.Length;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int id = GetIdPerlinNoise(x, y);
                int ifempty = Random.Range(0, 4);

                if (x <= 9 || y <= 9 || x >= size - 9 || y >= size - 9) id = 1;
                if (x <= 7 || y <= 7 || x >= size - 7 || y >= size - 7) id = 0;
                
                if ((Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius) || (Vector3.Distance(ruinsPos, new Vector3(x, 0, y)) < ruinRadius)) id = 1;
                if ((Vector3.Distance(middle, new Vector3(x, 0, y)) < middleRadius + 1 && id > 2) || (Vector3.Distance(ruinsPos, new Vector3(x, 0, y)) < ruinRadius + 1 && id > 2)) id = 2;

                for (int i = 0; i < 5; i++)
                {
                    if (Vector3.Distance(treeArea[i], new Vector3(x, 0, y)) < treeAreaSize)
                    {
                        id = 1;
                        ifempty = Random.Range(0, 2);
                    }
                }

                if (id >= 0)
                {
                    GameObject new_obj;

                    if (id == 0) //Blok Powietrza
                    {
                        if(x <= 2 || y <= 2 || x >= size - 2 || y >= size - 2)
                        {
                            int rand = Random.Range(1, 3);

                            if(rand == 2)
                            {
                                GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Sand" + x + y;
                                ground.Add(gr);
                            }
                            else
                            {
                                GameObject a = Instantiate(air, transform.position, Quaternion.identity);
                                a.GetComponent<NetworkObject>().Spawn();
                                a.transform.localPosition = new Vector3(x, 0.25f, y);
                                a.name = "Air" + x + y;
                                airBlocks.Add(a);
                            }
                        }
                        else if (x <= 5 || y <= 5 || x >= size - 5 || y >= size - 5)
                        {
                            GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Sand" + x + y;
                            ground.Add(gr);
                        }
                        else if(x <= 7 || y <= 7 || x >= size - 7 || y >= size - 7)
                        {
                            int rand = Random.Range(1, 3);

                            if (rand == 2)
                            {
                                GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Sand" + x + y;
                                ground.Add(gr);
                            }
                            else
                            {
                                GameObject gr = Instantiate(ground_prefabs1[1], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Ground" + x + y;
                                ground.Add(gr);
                            }

                        }
                        else
                        {
                            GameObject a = Instantiate(air, transform.position, Quaternion.identity);
                            a.GetComponent<NetworkObject>().Spawn();
                            a.transform.localPosition = new Vector3(x, 0.25f, y);
                            a.name = "Air" + x + y;
                            airBlocks.Add(a);
                        }
                    }
                    else if (id == 1) //Blok Trawy/Drzewa 
                    {
                        if (ifempty == 1)
                        {
                            //Losowanie drzewa
                            int randvar = Random.Range(0, trees_variants1.Length);
                            new_obj = Instantiate(trees_variants1[randvar], transform.position, Quaternion.identity);
                            new_obj.GetComponent<NetworkObject>().Spawn();

                            //Offset drzewa na kratce
                            new_obj.transform.localPosition = new Vector3(x, 0.75f, y);
                            new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), -0.5f, Random.Range(-treeOffset, treeOffset));

                            //Losowa skala
                            float randscale = Random.Range(0.5f, 0.75f);
                            new_obj.transform.localScale = new Vector3(randscale, randscale, randscale);

                            //Losowa rotacja
                            int randrot = Random.Range(1, 360);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot, 0);

                            //Przypisanie do rodzica
                            new_obj.name = "Tree" + x + y;
                            //new_obj.transform.parent = Trees.transform;
                            blocks.Add(new_obj);



                            GameObject gr = Instantiate(ground_prefabs1[6], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                        else
                        {
                            int ifgrass = Random.Range(0, 3);

                            if (ifgrass == 1)
                            {
                                int numOfGrass = Random.Range(2, 5);
                                for (int i = 0; i < numOfGrass; i++)
                                {
                                    Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                    GameObject tr = Instantiate(grass, transform.position, Quaternion.identity);
                                    tr.GetComponent<NetworkObject>().Spawn();
                                    tr.transform.localPosition = rand;
                                    tr.transform.localRotation.eulerAngles.Set(0, Random.Range(0, 360), 0);
                                    float width = Random.Range(0.02f, 0.06f);
                                    tr.transform.localScale = new Vector3(width, Random.Range(0.2f, 0.3f), width);
                                    tr.transform.parent = this.gameObject.transform;
                                }
                            }

                            //Pod這瞠
                            GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                    }
                    else if (id == 2) //Blok ma貫go kamienia
                    {
                        if (ifempty != 1 && ifempty != 2)
                        {
                            //Losowanie kamienia
                            int randvar = Random.Range(1, rocks_variants1.Length);
                            new_obj = Instantiate(rocks_variants1[randvar], transform.position, Quaternion.identity);
                            new_obj.GetComponent<NetworkObject>().Spawn();
                            //Losowa rotacja
                            int randrot = Random.Range(1, 360);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot, 0);

                            //Pozycja
                            new_obj.transform.localPosition = new Vector3(x, 0.18f, y);

                            //Offset kamienia na kratce
                            new_obj.transform.localPosition += new Vector3(Random.Range(-rockOffset, rockOffset), 0, Random.Range(-rockOffset, rockOffset));

                            //Przypisanie do rodzica
                            new_obj.name = "Rock" + x + y;
                            //new_obj.transform.parent = Rocks.transform;
                            blocks.Add(new_obj);


                            //Pod這瞠
                            GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                        else
                        {
                            int ifgrass = Random.Range(0, 3);

                            if (ifgrass == 1)
                            {
                                int numOfGrass = Random.Range(2, 5);
                                for (int i = 0; i < numOfGrass; i++)
                                {
                                    Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                    GameObject tr = Instantiate(grass, transform.position, Quaternion.identity);
                                    tr.GetComponent<NetworkObject>().Spawn();
                                    tr.transform.localPosition = rand;
                                    tr.transform.localRotation.eulerAngles.Set(0, Random.Range(0, 360), 0);
                                    float width = Random.Range(0.02f, 0.06f);
                                    tr.transform.localScale = new Vector3(width, Random.Range(0.2f, 0.3f), width);
                                    tr.transform.parent = this.gameObject.transform;
                                }
                            }

                            //Pod這瞠
                            GameObject gr = Instantiate(ground_prefabs1[id - 1], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                        }
                    }
                    else //Blok ska造
                    {
                        //Spawn kamienia
                        new_obj = Instantiate(rocks_variants1[0], transform.position, Quaternion.identity);
                        new_obj.GetComponent<NetworkObject>().Spawn();
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
                        //new_obj.transform.parent = Rocks.transform;
                        blocks.Add(new_obj);

                        //Pod這瞠
                        GameObject gr = Instantiate(ground_prefabs1[id], transform.position, Quaternion.identity);
                        gr.GetComponent<NetworkObject>().Spawn();
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.transform.rotation = Quaternion.Euler(0, randrot * 90, 0);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                    }
                }
            }
        }

        foreach (GameObject g in blocks)
        {
            g.transform.parent = start.transform;
        }

        foreach (GameObject g in airBlocks)
        {
            g.transform.parent = start.transform;
        }

        foreach (GameObject g in ground)
        {
            g.transform.parent = start.transform;
        }

        ClearMap(middle, middleRadius);
        SetShip();
        started.Value = true;
        EnableWaterClientRpc();
        ClearMap(ruinsPos, ruinRadius);

        foreach (var g in GameObject.FindGameObjectsWithTag("Player"))
            g.GetComponent<PlayerMovement>().SetStartPosition(middle);
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
        float h = GetFloatPerlinNoise(x, y) / 2 - 0.3f;

        return (Mathf.Round(h*10)/10);
    }
    float GetFloatPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * (5 + 1);

        return Mathf.Clamp(perlin, 0, 5); //blocks_prefabs1.Length
    }
    void ClearMap(Vector3 pos, float r)
    {
        
        foreach (GameObject g in blocks)
        {
            if (Vector3.Distance(g.transform.position, pos) < r - 0.5f)
            {      
                Destroy(g);
            }
        }

        foreach (GameObject g in ground)
        {
            if (Vector3.Distance(g.transform.position, pos) < r)
            {
                g.GetComponent<MeshRenderer>().material = defaultMaterial1;
            }
        }
    }

    Vector3 RandomXY()
    {
        Vector3 pos;

        do {
            pos = new Vector3(Random.Range(10, size - 10), 0, Random.Range(10, size - 10));
        } while (Vector3.Distance(pos, middle) < 10);


        return pos;
    }

    Vector3 SetRuins()
    {
        int offset = size / 4;
        int x, y;

        do {

            x = Random.Range(0 + offset, size - offset);
            y = Random.Range(0 + offset, size - offset);
            ruinsPos = new Vector3(x, 0, y);

        } while (Vector3.Distance(ruinsPos, middle) < offset);


        GameObject ship = Instantiate(shipPrefab, ruinsPos, Quaternion.identity);
        ship.GetComponent<NetworkObject>().Spawn();
        ship.name = "ruina";
        
        return ruinsPos;
    }

    void SetShip()
    {
        if (!IsServer) return;
        //Przywo豉j statek na brzegu mapy w losowym miejscu
        GameObject ship = Instantiate(shipPrefab, transform.position, Quaternion.identity);
        ship.GetComponent<NetworkObject>().Spawn();
        ship.name = "Ship";

        int xy = GetRandomSign();
        int ss = GetRandomSign() * size;

        Vector3 shipPos;

        if (xy == 0) shipPos = new Vector3(ss, 0, Random.Range(0, size));
        else shipPos = new Vector3(Random.Range(0, size), 0, ss);

        ship.transform.position = shipPos;
    }

    int GetRandomSign()
    {
        return Random.Range(-10, 9) < 0 ? 0 : 1;
    }

    GameObject SpawnBase()
    {
        //if (!IsServer) return;
        GameObject b = Instantiate(campfire, middle + new Vector3(0,0.25f,0), Quaternion.identity);
        //b.GetComponent<NetworkObject>().Spawn();
        b.name = "campfire";
        return b;
    }

    public List<GameObject> GetGroundBlocks()
    {
        return ground;
    }

    public List<GameObject> GetObjectsBlocks()
    {
        return blocks;
    }

    [ClientRpc]
    void EnableWaterClientRpc()
    {
        waterLayer.SetActive(true);
        startPlane.SetActive(false);
    }
}
