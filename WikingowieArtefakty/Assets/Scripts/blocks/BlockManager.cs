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
    public float resizeOffset = 0;
    
    
    private Vector3 normalScale, breakStep;
    private int currentBreakStatus;


    private void Start()
    {
        normalScale = transform.localScale;
        breakStep = normalScale / maxBreakStatus / 3;
        currentBreakStatus = 0;
    }

    [ContextMenu("Step"), ServerRpc(RequireOwnership = false)]
    public void NextBreakStepServerRpc()
    {
        NextBreakStepClientRpc();
    }

    [ClientRpc]
    void NextBreakStepClientRpc()
    {
        currentBreakStatus++;
        LeanTween.scale(this.gameObject, this.gameObject.transform.localScale - breakStep, 0.2f);
        transform.position -= new Vector3(0f, breakStep.y / 2, 0f);

        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);

        StartCoroutine(Hitting(x, z));

        if(IsServer) CheckForDestroy();
    }

    IEnumerator Hitting(float xin, float zin)
    {
        float x = xin;
        float z = zin;

        if (treeParticles != null) PlayParticleServerRpc(0);
        if (stoneParticles != null) PlayParticleServerRpc(1);

        LeanTween.rotate(this.gameObject, new Vector3(x, transform.localRotation.eulerAngles.y, z), 0.1f).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(0.1f);
        LeanTween.rotate(this.gameObject, new Vector3(0, transform.localRotation.eulerAngles.y, 0), 0.1f).setEase(LeanTweenType.easeOutSine);
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayParticleServerRpc(int i)
    {
        if (i == 0) PlayTreeParticleClientRpc();
        else if (i == 1) PlayStoneParticleClientRpc();
    }

    [ClientRpc]
    private void PlayTreeParticleClientRpc()
    {
        treeParticles.Play();
    }

    [ClientRpc]
    private void PlayStoneParticleClientRpc()
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

        gameObject.GetComponent<NetworkObject>().Despawn();
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
