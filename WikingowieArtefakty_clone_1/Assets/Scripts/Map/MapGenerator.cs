using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MapGenerator : NetworkBehaviour
{
    [Header("Generator Settings")]
    public int islandLevel;
    [Min(1)] public int size;
    [Min(1)] public int chunksize;
    private int seed;
    public int middleRadius;
    public int enemiesBasesSize;
    public int enemiesBasesCount;
    [Range(0, 0.5f)] public float treeOffset;
    [Range(0, 0.5f)] public float rockOffset;
    [Range(1, 100)]  public int oresChance;

    [Header("Base")]
    public GameObject[] trees_variants1;
    public GameObject[] rocks_variants1;
    public GameObject[] ground_prefabs1;
    public GameObject grassPrefab;
    public GameObject[] plantsPrefabs;
    public GameObject[] waterPrefabs;
    //public Material defaultMaterial1;
    public GameObject[] oresPrefabs1;
    public GameObject shipPrefab;
    public GameObject castlePrefab;
    //public Material grassMaterial;

    [Header("Other Prefabs")]
    public GameObject waterLayerPrefab;
    [HideInInspector] public GameObject waterLayer;
    public GameObject campfire;
    public GameObject air;
    public GameObject chunkPrefab;

    [Header("Others")]
    public Camera cam;
    public GameObject manager;
    public GameObject startPlane;
    public TimeManager timeManager;
    public MenuManager menuManager;

    private Vector3[] enemiesBasesPos;
    private Vector3[] treeArea = new Vector3[5];
    private int treeAreaSize = 9;

    private GameObject start;
    public static Vector3 middle = Vector3.zero;
    private List<GameObject> blocks = new List<GameObject>();
    private List<GameObject> ground = new List<GameObject>();
    private List<GameObject> airBlocks = new List<GameObject>();

    private List<GameObject> block = new List<GameObject>();
    private List<GameObject> chunks = new List<GameObject>();
    private ItemsDropManager dropManager;

    private bool isnew = false;
    [HideInInspector] public NetworkVariable<bool> started = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Start()
    {
        start = this.gameObject;
        dropManager = manager.GetComponent<ItemsDropManager>();

        enemiesBasesPos = new Vector3[enemiesBasesCount];

        middle = start.transform.position + new Vector3(size / 2, 0, size / 2);
        manager.GetComponent<Manager>().SetMiddle(middle);
        
        startPlane.transform.position = middle + new Vector3(0,0.25f,0);
        startPlane.transform.localScale = new Vector3(size,1,size);

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
            startPlane.transform.position = new Vector3(middle.x, -0.25f, middle.z);
            timeManager.StartDayOne();
        }
    }


    [ServerRpc]
    public void SetUpGeneratorServerRpc()
    {
        menuManager.FadeAnimationServerRpc(true);
        seed = Random.Range(100, 9999);
        Debug.Log("Seed: " + seed);

        StartCoroutine(GenerateWorld());
    }

    IEnumerator GenerateWorld()
    {
        yield return new WaitForSeconds(2);
        menuManager.SetInventoryUIClientRpc();
        //manager.GetComponent<Manager>().StartGameServerRpc();

        SetRuins();

        int att = 0;
        for(int i=0; i<5; i++)
        {
            if (att >= 20) break;

            do
            {
                att++;
                treeArea[i] = RandomXY();
            } while (Vector3.Distance(treeArea[i], middle) >= size/2 - 13);

            if (i == 1)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20)
                {
                    i--;
                    att++;
                }
            }
            else if(i == 2)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20)
                {
                    i--;
                    att++;
                }
            }
            else if (i == 3)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 3]) < 20)
                {
                    i--;
                    att++;
                }
            }
            else if (i == 4)
            {
                if (Vector3.Distance(treeArea[i], treeArea[i - 1]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 2]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 3]) < 20 ||
                    Vector3.Distance(treeArea[i], treeArea[i - 4]) < 20)
                {
                    i--;
                    att++;
                }
            }
            yield return null;
        }

        StartCoroutine( GenerateChunks() );      
    }

    IEnumerator EndGenerate()
    {
        //ClearMapServerRpc(middle, middleRadius);
        SetShipServerRpc();
        started.Value = true;
        EnableWaterClientRpc();
        //ClearMapServerRpc(ruinsPos, ruinRadius);
        manager.GetComponent<Manager>().StartGameClientRpc(NetworkManager.Singleton.ConnectedClients.Count);

        foreach (var g in GameObject.FindGameObjectsWithTag("Player"))
            g.GetComponent<PlayerMovement>().SetStartPosition(middle);

        manager.GetComponent<Manager>().StartGameServerRpc();
        menuManager.FadeAnimationServerRpc(false);
        yield return null;
    }

    IEnumerator GenerateChunks()
    {
        int numofchunk = Mathf.CeilToInt(size / chunksize);

        for (int x = 0; x < numofchunk; x++)
        {
            for (int y = 0; y < numofchunk; y++)
            {
                GameObject chunk = Instantiate(chunkPrefab, transform.position, Quaternion.identity);
                chunk.GetComponent<NetworkObject>().Spawn();
                float chunkoffset = chunksize / 2;
                chunk.transform.position = new Vector3(x * chunksize + chunkoffset, 0, y * chunksize + chunkoffset);
                chunk.transform.parent = transform;

                GenerateChunk(chunk, chunksize*x, chunksize*y);
                ConfigureChunkClientRpc(chunk.GetComponent<NetworkObject>().NetworkObjectId, "chunk" + x + "_" + y);
                //chunk.name = "chunk" + x + y;
                //chunks.Add(chunk);
                yield return null;
            }
        }

        StartCoroutine(EndGenerate());
    }

    [ClientRpc]
    void ConfigureChunkClientRpc(ulong chunkId, string name)
    {
        GameObject chunk = NetworkManager.Singleton.SpawnManager.SpawnedObjects[chunkId].gameObject;
        chunk.name = name;
        chunks.Add(chunk);
    }

    void GenerateChunk(GameObject chunk, int Xoffset, int Yoffset)
    {
        for (int xp = 0; xp < chunksize; xp++)
        {
            for (int yp = 0; yp < chunksize; yp++)
            {
                int x = xp + Xoffset;
                int y = yp + Yoffset;

                int id = GetIdPerlinNoise(x, y);
                int ifempty = Random.Range(0, 4);

                Vector3 temppos = new Vector3(x, 0, y);
                if (Vector3.Distance(temppos, middle) >= size/2 - 9) id = 1;
                if (Vector3.Distance(temppos, middle) >= size / 2 - 7) id = 0;

                for (int i = 0; i < 5; i++)
                {
                    if (Vector3.Distance(treeArea[i], temppos) < treeAreaSize)
                    {
                        id = 2;
                        ifempty = Random.Range(0, 2);
                    }
                }

                if ((Vector3.Distance(middle, temppos) < middleRadius)) id = 1;
                if ((Vector3.Distance(middle, temppos) < middleRadius + 1 && id > 3)) id = 3;


                for(int i = 0; i < enemiesBasesCount; i++)
                {
                    if (Vector3.Distance(enemiesBasesPos[i], temppos) < enemiesBasesSize)
                    {
                        id = 1;
                    }
                    else if(Vector3.Distance(enemiesBasesPos[i], temppos) < enemiesBasesSize + 1 && id > 3)
                    {
                        id = 3;
                    }
                }
               
                if (id >= 0)
                {
                    GameObject new_obj = null;

                    if (id == 0) //Blok Powietrza
                    {
                        if(Vector3.Distance(temppos, middle) >= size / 2)
                        {
                            GameObject a = Instantiate(air, transform.position, Quaternion.identity);
                            a.GetComponent<NetworkObject>().Spawn();
                            a.transform.localPosition = new Vector3(x, 0.25f, y);
                            a.name = "Air" + x + y;
                            airBlocks.Add(a);
                            a.transform.parent = chunk.transform;
                        }
                        else if (Vector3.Distance(temppos, middle) >= size / 2 - 2)
                        {
                            int rand = Random.Range(1, 3);
                            
                            if (rand == 2)
                            {
                                GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Sand" + x + y;
                                ground.Add(gr);
                                gr.transform.parent = chunk.transform;
                            }
                            else
                            {
                                GameObject a = Instantiate(air, transform.position, Quaternion.identity);
                                a.GetComponent<NetworkObject>().Spawn();
                                a.transform.localPosition = new Vector3(x, 0.25f, y);
                                a.name = "Air" + x + y;
                                airBlocks.Add(a);
                                a.transform.parent = chunk.transform;
                            }
                        }
                        else if (Vector3.Distance(temppos, middle) >= size / 2 - 5)
                        {
                            GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Sand" + x + y;
                            ground.Add(gr);
                            gr.transform.parent = chunk.transform;
                        }
                        else if (Vector3.Distance(temppos, middle) >= size / 2 - 7)
                        {
                            int rand = Random.Range(1, 3);

                            if (rand == 2)
                            {
                                GameObject gr = Instantiate(ground_prefabs1[0], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Sand" + x + y;
                                ground.Add(gr);
                                gr.transform.parent = chunk.transform;
                            }
                            else
                            {
                                int rand2 = Random.Range(0, 2);
                                GameObject gr = Instantiate(ground_prefabs1[rand2 == 1 ? id : 7], transform.position, Quaternion.identity);
                                gr.GetComponent<NetworkObject>().Spawn();
                                gr.transform.localPosition = new Vector3(x, -0.25f, y);
                                gr.name = "Ground" + x + y;
                                ground.Add(gr);
                                gr.transform.parent = chunk.transform;
                            }

                        }
                        else
                        {
                            GameObject a = Instantiate(air, transform.position, Quaternion.identity);
                            a.GetComponent<NetworkObject>().Spawn();
                            a.transform.localPosition = new Vector3(x, 0.25f, y);
                            a.name = "Air" + x + y;
                            airBlocks.Add(a);
                            a.transform.parent = chunk.transform;

                            int ifobj = Random.Range(0, 10);

                            if (ifobj == 0)
                            {
                                GameObject tr = Instantiate(waterPrefabs[Random.Range(0, waterPrefabs.Length)], transform.position, Quaternion.identity);
                                tr.GetComponent<NetworkObject>().Spawn();
                                //tr.transform.localPosition = new Vector3(x + Random.Range(-0.1f, 0.1f), -0.25f, y + Random.Range(-0.1f, 0.1f));
                                tr.transform.parent = a.transform;
                                tr.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), -0.26f, Random.Range(-0.1f, 0.1f));
                                //tr.transform.localEulerAngles.Set(0, Random.Range(0, 360), 0);
                                tr.name = "water";
                                
                            }
                        }
                    }
                    else if (id == 1)
                    {
                        int ifgrass = Random.Range(0, 3);

                        if (ifgrass == 0)
                        {
                            int num = Random.Range(2, 4);
                            int type = (int)Random.Range(0, plantsPrefabs.Length);
                            for (int i = 0; i < num; i++)
                            {
                                Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                float width = Random.Range(0.3f, 0.6f);

                                SetFlowerClientRpc(type, rand, width, chunk.GetComponent<NetworkObject>().NetworkObjectId);
                            }
                        }
                        else if (ifgrass == 1)
                        {
                            int num = Random.Range(7, 10);
                            for (int i = 0; i < num; i++)
                            {
                                Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                float width = Random.Range(0.02f, 0.04f);

                                SetGrassClientRpc(rand, width, chunk.GetComponent<NetworkObject>().NetworkObjectId);
                            }
                        }

                        //Pod³o¿e
                        int rand2 = Random.Range(0, 2);
                        GameObject gr = Instantiate(ground_prefabs1[rand2 == 1 ? id : 7], transform.position, Quaternion.identity);
                        gr.GetComponent<NetworkObject>().Spawn();
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                        gr.transform.parent = chunk.transform;
                    }
                    else if (id == 2) //Blok Trawy/Drzewa 
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
                            new_obj.transform.localPosition += new Vector3(0, 0.01f, 0); //eliminacja z-fighting

                            //Losowa skala
                            float randscale = Random.Range(0.6f, 0.7f);
                            new_obj.transform.localScale = new Vector3(randscale, randscale, randscale);

                            //Losowa rotacja
                            int randrot = Random.Range(1, 360);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot, 0);

                            //Przypisanie do rodzica
                            new_obj.name = "Tree" + x + y;
                            //new_obj.transform.parent = Trees.transform;
                            blocks.Add(new_obj);
                            new_obj.transform.parent = chunk.transform;


                            GameObject gr = Instantiate(ground_prefabs1[6], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                            gr.transform.parent = chunk.transform;
                        }
                        else
                        {
                            int ifgrass = Random.Range(0, 3);

                            if (ifgrass == 0)
                            {
                                int num = Random.Range(2, 4);
                                int type = (int)Random.Range(0, plantsPrefabs.Length);
                                for (int i = 0; i < num; i++)
                                {
                                    Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                    float width = Random.Range(0.3f, 0.6f);

                                    SetFlowerClientRpc(type, rand, width, chunk.GetComponent<NetworkObject>().NetworkObjectId);
                                }
                            }
                            else if (ifgrass == 1)
                            {
                                int num = Random.Range(7, 10);
                                for (int i = 0; i < num; i++)
                                {
                                    Vector3 rand = new Vector3(x + Random.Range(-0.4f, 0.4f), 0.25f, y + Random.Range(-0.4f, 0.4f));
                                    float width = Random.Range(0.02f, 0.04f);

                                    SetGrassClientRpc(rand, width, chunk.GetComponent<NetworkObject>().NetworkObjectId);
                                }
                            }

                            //Pod³o¿e
                            int rand2 = Random.Range(0, 2);
                            GameObject gr = Instantiate(ground_prefabs1[rand2 == 1 ? id-1 : 7], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                            gr.transform.parent = chunk.transform;
                        }
                    }
                    else if (id == 3) //Blok ma³ego kamienia
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
                            new_obj.transform.localPosition += new Vector3(0, 0.01f, 0); //eliminacja z-fighting

                            //Przypisanie do rodzica
                            new_obj.name = "Rock" + x + y;
                            //new_obj.transform.parent = Rocks.transform;
                            blocks.Add(new_obj);
                            new_obj.transform.parent = chunk.transform;


                            //Pod³o¿e
                            GameObject gr = Instantiate(ground_prefabs1[id-1], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                            gr.transform.parent = chunk.transform;
                        }
                        else
                        {
                            //Losowanie drzewa
                            int randvar = Random.Range(0, trees_variants1.Length);
                            new_obj = Instantiate(trees_variants1[randvar], transform.position, Quaternion.identity);
                            new_obj.GetComponent<NetworkObject>().Spawn();

                            //Offset drzewa na kratce
                            new_obj.transform.localPosition = new Vector3(x, 0.75f, y);
                            new_obj.transform.localPosition += new Vector3(Random.Range(-treeOffset, treeOffset), -0.5f, Random.Range(-treeOffset, treeOffset));
                            new_obj.transform.localPosition += new Vector3(0, 0.01f, 0); //eliminacja z-fighting

                            //Losowa skala
                            float randscale = Random.Range(0.6f, 0.7f);
                            new_obj.transform.localScale = new Vector3(randscale, randscale, randscale);

                            //Losowa rotacja
                            int randrot = Random.Range(1, 360);
                            new_obj.transform.rotation = Quaternion.Euler(0, randrot, 0);

                            //Przypisanie do rodzica
                            new_obj.name = "Tree" + x + y;
                            //new_obj.transform.parent = Trees.transform;
                            blocks.Add(new_obj);
                            new_obj.transform.parent = chunk.transform;


                            GameObject gr = Instantiate(ground_prefabs1[6], transform.position, Quaternion.identity);
                            gr.GetComponent<NetworkObject>().Spawn();
                            gr.transform.localPosition = new Vector3(x, -0.25f, y);
                            gr.name = "Ground" + x + y;
                            ground.Add(gr);
                            gr.transform.parent = chunk.transform;
                        }
                    }
                    else //Blok ska³y
                    {
                        int rand = Random.Range(0, 10);

                        if (rand == 0)
                        {
                            //Spawn kamienia rudy
                            new_obj = Instantiate(oresPrefabs1[0], transform.position, Quaternion.identity);
                            new_obj.GetComponent<NetworkObject>().Spawn();
                        }
                        else
                        {
                            //Spawn kamienia
                            new_obj = Instantiate(rocks_variants1[0], transform.position, Quaternion.identity);
                            new_obj.GetComponent<NetworkObject>().Spawn();
                        }

                        //Skala na bazie noise
                        new_obj.transform.localScale = new Vector3(0.67f, GetHeightByNoise(x, y)/4f, 0.67f);
                        new_obj.transform.localPosition = new Vector3(x, 0.25f, y);
                        new_obj.transform.localPosition += new Vector3(0, 0.01f, 0); //eliminacja z-fighting
                        new_obj.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);

                        //Przypisanie do rodzica
                        new_obj.name = "Mountain" + x + y;
                        //new_obj.transform.parent = Rocks.transform;
                        blocks.Add(new_obj);
                        new_obj.transform.parent = chunk.transform;


                        //Pod³o¿e
                        GameObject gr = Instantiate(ground_prefabs1[id-1], transform.position, Quaternion.identity);
                        gr.GetComponent<NetworkObject>().Spawn();
                        gr.transform.localPosition = new Vector3(x, -0.25f, y);
                        gr.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                        gr.name = "Ground" + x + y;
                        ground.Add(gr);
                        gr.transform.parent = chunk.transform;
                    }

                    if (new_obj != null)
                    {
                        dropManager.SetItem(true, x, y);
                    }
                }
            }
        }
    }
    int GetIdPerlinNoise(int x, int y)
    {
        float nx = (seed + x * 0.15f);
        float ny = (seed + y * 0.15f);
        float perlin = Mathf.PerlinNoise(nx, ny) * (5+2);

        return Mathf.Clamp(Mathf.FloorToInt(perlin), 0, 6);
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

    [ClientRpc]
    void SetGrassClientRpc(Vector3 pos, float width, ulong chunkId)
    {
        GameObject tr = Instantiate(grassPrefab, transform.position, Quaternion.identity);
        tr.transform.localPosition = pos;
        tr.transform.localScale = new Vector3(width, Random.Range(0.1f, 0.3f), width);
        tr.transform.parent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[chunkId].transform;
    }

    [ClientRpc]
    void SetFlowerClientRpc(int type, Vector3 pos, float width, ulong chunkId)
    {
        GameObject tr = Instantiate(plantsPrefabs[type], transform.position, Quaternion.identity);
        tr.transform.localPosition = pos;
        tr.transform.localScale = new Vector3(width, Random.Range(0.2f,0.4f), width);
        tr.transform.parent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[chunkId].transform;
    }

    [ServerRpc]
    void ClearMapServerRpc(Vector3 pos, float r)
    {   
        foreach (GameObject g in blocks)
        {
            if (Vector3.Distance(g.transform.position, pos) < r - 0.5f)
            {      
                g.GetComponent<NetworkObject>().Despawn();
                dropManager.SetItem(false, Mathf.RoundToInt(g.transform.position.x), Mathf.RoundToInt(g.transform.position.z));
            }
        }

        foreach (GameObject g in ground)
        {
            if (Vector3.Distance(g.transform.position, pos) < r)
            {
                SetObjectMaterialClientRpc(g.GetComponent<NetworkObject>().NetworkObjectId, Random.Range(0, 2) == 1 ? 1 : 7);
                //g.GetComponent<MeshRenderer>().material = ground_prefabs1[Random.Range(0, 2) == 1 ? 1 : 7].GetComponent<MeshRenderer>().sharedMaterial;
                //g.GetComponent<MeshRenderer>().material = grassMaterial;
            }
        }
    }

    [ClientRpc]
    void SetObjectMaterialClientRpc(ulong id, int i)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].GetComponent<MeshRenderer>().material = ground_prefabs1[i].GetComponent<MeshRenderer>().sharedMaterial;
    }

    Vector3 RandomXY()
    {
        Vector3 pos;

        do {
            pos = new Vector3(Random.Range(10, size - 10), 0, Random.Range(10, size - 10));
        } while (Vector3.Distance(pos, middle) < 10);


        return pos;
    }

    void SetRuins()
    {
        for (int i = 0; i < enemiesBasesCount; i++)
        {
            int offset = size / 4;
            int x, y;
            int counter = 0;

            do
            {
                counter++;

                x = Random.Range(0 + offset, size - offset);
                y = Random.Range(0 + offset, size - offset);
                enemiesBasesPos[i] = new Vector3(x, 0, y);

                if (counter > 20) break;

            } while (Vector3.Distance(enemiesBasesPos[i], middle) < offset || CheckForBase(enemiesBasesPos[i], i, offset));


            GameObject ship = Instantiate(castlePrefab, enemiesBasesPos[i], Quaternion.identity);
            ship.GetComponent<NetworkObject>().Spawn();
        }
    }

    bool CheckForBase(Vector3 pos, int j, int offset)
    {
        for(int i=0; i<j; i++)
        {
            if (Vector3.Distance(enemiesBasesPos[i], pos) < offset)
            {
                return true;
            }
        }

        return false;
    }

    [ServerRpc]
    void SetShipServerRpc()
    {
        //Przywo³aj statek na brzegu mapy w losowym miejscu
        GameObject ship = Instantiate(shipPrefab, transform.position, Quaternion.identity);
        ship.GetComponent<NetworkObject>().Spawn();
        ship.name = "Ship";

        Vector3 shipPos = middle + GetPointOnCircle(size/2, Random.Range(0, 360));

        SetShipClientRpc(ship.GetComponent<NetworkObject>().NetworkObjectId, shipPos);
    }

    [ClientRpc]
    void SetShipClientRpc(ulong id, Vector3 pos)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].transform.position = pos;
    }

    public Vector3 GetPointOnCircle(float radius, float angleInDegrees)
    {
        // Konwersja k¹ta na radiany
        float angleInRadians = Mathf.Deg2Rad * angleInDegrees;

        // Wspó³rzêdne biegunowe
        float x = radius * Mathf.Cos(angleInRadians);
        float y = radius * Mathf.Sin(angleInRadians);

        // Zwrócenie punktu
        return new Vector3(x, 0, y);
    }

    GameObject SpawnBase()
    {
        //if (!IsServer) return;
        GameObject b = Instantiate(campfire, middle + new Vector3(0,0.25f,0), Quaternion.identity);
        b.transform.localPosition += new Vector3(0, 0.01f, 0); //eliminacja z-fighting
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

    public List<GameObject> GetChunks()
    {
        return chunks;
    }

    [ClientRpc]
    void EnableWaterClientRpc()
    {
        waterLayer.SetActive(true);
        //startPlane.SetActive(false);
    }
}
