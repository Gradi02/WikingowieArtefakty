using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BlockManager : NetworkBehaviour
{
    [Header("Block Info")]
    public int maxBreakStatus = 3;
    public enum Tools
    {
        Axe,
        Pickaxe
    }

    public Tools breakingTool;
    public GameObject loot;
    public ParticleSystem destroyParticles;
    public ParticleSystem treeParticles;
    public ParticleSystem stoneParticles;
    //public float resizeOffset = 0;
    
    
    private NetworkVariable<Vector3> breakStep = new NetworkVariable<Vector3>(new Vector3(0.02f, 0.02f, 0.02f), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private int currentBreakStatus;


    private void Start()
    {
        if (IsHost)
        {
            breakStep.Value = transform.localScale / maxBreakStatus / 5;
        }

        currentBreakStatus = 0;
    }

    [ContextMenu("Step"), ServerRpc(RequireOwnership = false)]
    public void NextBreakStepServerRpc()
    {
        /* currentBreakStatus++;
         LeanTween.scale(this.gameObject, this.gameObject.transform.localScale - breakStep, 0.2f);
         transform.position -= new Vector3(0f, breakStep.y / 2, 0f);

         float x = Random.Range(-5, 5);
         float z = Random.Range(-5, 5);

         StartCoroutine(Hitting(x, z));*/
        
        
        currentBreakStatus++;

        NextBreakClientRpc();

        if (IsServer) CheckForDestroy();
    }

    [ClientRpc]
    void NextBreakClientRpc()
    {
        //Debug.Log(breakStep);
        LeanTween.scale(this.gameObject, this.gameObject.transform.localScale - breakStep.Value, 0.2f);
        transform.position -= new Vector3(0f, breakStep.Value.y / 2, 0f);

        float x = Random.Range(-2, 2);
        float z = Random.Range(-2, 2);

        StartCoroutine(Hitting(x, z));
    }

    IEnumerator Hitting(float xin, float zin)
    {
        float x = xin;
        float z = zin;

        if (treeParticles != null) PlayTreeParticle();
        if (stoneParticles != null) PlayStoneParticle();

        LeanTween.rotate(this.gameObject, new Vector3(x, transform.localRotation.eulerAngles.y, z), 0.1f).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(0.1f);
        LeanTween.rotate(this.gameObject, new Vector3(0, transform.localRotation.eulerAngles.y, 0), 0.1f).setEase(LeanTweenType.easeOutSine);
    }

    private void PlayTreeParticle()
    {
        treeParticles.Play();
    }

    private void PlayStoneParticle()
    {
        stoneParticles.Play();
    }

    private void CheckForDestroy()
    {
        if(currentBreakStatus >= maxBreakStatus)
        {
            SpawnDestroyParticleServerRpc();
            DespawnObjectServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObjectServerRpc()
    {
        GameObject g2 = Instantiate(loot, transform.position, Quaternion.identity);
        g2.GetComponent<NetworkObject>().Spawn();

        GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc]
    void SpawnDestroyParticleServerRpc()
    {
        SpawnDestroyParticleClientRpc();
    }

    [ClientRpc]
    void SpawnDestroyParticleClientRpc()
    {
        ParticleSystem g = Instantiate(destroyParticles, transform.position, Quaternion.identity);
        Destroy(g.gameObject, 3);
    }
}