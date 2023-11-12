using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [Header("Block Info")]
    public int maxBreakStatus = 3;
    public enum Tools
    {
        Axe,
        Pickaxe
    }

    public Tools breakingTool;
    public int maxNumberOfLoot;
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

    [ContextMenu("Step")]
    public void NextBreakStep()
    {
        //if player tools
        currentBreakStatus++;
        //transform.localScale -= breakStep;
        LeanTween.scale(this.gameObject, this.gameObject.transform.localScale - breakStep, 0.2f);
        StartCoroutine(Hitting());
        transform.position -= new Vector3(0f,FindGroundY(transform.position.y) - resizeOffset,0f);
        CheckForDestroy();
    }

    IEnumerator Hitting()
    {
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        if(treeParticles!=null) treeParticles.Play();
        if (stoneParticles != null) stoneParticles.Play();
        LeanTween.rotate(this.gameObject, new Vector3(x, this.gameObject.transform.rotation.y, z), 0.15f).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(0.20f);
        LeanTween.rotate(this.gameObject, new Vector3(0, this.gameObject.transform.rotation.y, 0), 0.15f).setEase(LeanTweenType.easeOutSine);
    }

    float FindGroundY(float h)
    {
        float currenth = h;
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2))
        {
            return (currenth - hit.point.y);
        }
        return 0;
    }

    private void CheckForDestroy()
    {
        if(currentBreakStatus >= maxBreakStatus)
        {
            Instantiate(destroyParticles, transform.position, Quaternion.identity);

            int lootrand = Random.Range(1, maxNumberOfLoot);
            for(int i = 0; i < lootrand; i++)
                Instantiate(loot, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    public void DestroyAnimation()
    {

    }
}
