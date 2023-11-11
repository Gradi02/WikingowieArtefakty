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
        transform.localScale -= breakStep;
        StartCoroutine(Hitting());
        transform.position -= new Vector3(0f,FindGroundY(transform.position.y) - resizeOffset,0f);
        CheckForDestroy();
    }

    IEnumerator Hitting()
    {
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);
        LeanTween.rotate(this.gameObject, new Vector3(x, this.gameObject.transform.rotation.y, z), 0.2f).setEase(LeanTweenType.easeInSine);
        yield return new WaitForSeconds(0.25f);
        LeanTween.rotate(this.gameObject, new Vector3(0, this.gameObject.transform.rotation.y, 0), 0.2f).setEase(LeanTweenType.easeOutSine);
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
