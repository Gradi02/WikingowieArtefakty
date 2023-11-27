using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuManager : NetworkBehaviour
{
    public RawImage img;
    public GameObject[] invUI;


    private Color alpha;
    private bool sh = false;
    private bool isAnimation = false;

    private void Start()
    {
        alpha = Color.black;
        alpha.a = 1;
        img.color = alpha;

        foreach (var g in invUI)
        {
            g.transform.localPosition -= new Vector3(0, 200, 0);
        }

        StartCoroutine(FadeOut());
    }

    [ServerRpc]
    public void FadeAnimationServerRpc(bool inout)
    {
        if (isAnimation) return;
        isAnimation = true;

        FadeAnimationClientRpc(inout);
    }

    [ClientRpc]
    public void FadeAnimationClientRpc(bool inout)
    {
        if (inout) StartCoroutine(FadeIn());
        else StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Color a = Color.black;
        a.a = 0;

        while(img.color.a < 1)
        {
            yield return new WaitForSeconds(0.001f);
            a.a += 0.01f;
            img.color = a;
        }

        isAnimation = false;
        yield return null;
    }

    IEnumerator FadeOut()
    {
        Color a = Color.black;
        a.a = 1;

        while (img.color.a > 0)
        {
            yield return new WaitForSeconds(0.001f);
            a.a -= 0.01f;
            img.color = a;
        }

        isAnimation=false;
        yield return null;
    }

    [ClientRpc]
    public void SetInstantClientRpc(bool inout)
    {
        if(inout)
        {
            Color a = Color.black;
            a.a = 1;
            img.color = a;
        }
        else
        {
            Color a = Color.black;
            a.a = 0;
            img.color = a;
        }
    }

    [ClientRpc]
    public void SetInventoryUIClientRpc()
    {
        sh = !sh;
        if(sh)
        {
            foreach(var g in invUI)
            {
                g.transform.localPosition += new Vector3(0, 200, 0);
            }
        }
        else
        {
            foreach (var g in invUI)
            {
                g.transform.localPosition -= new Vector3(0, 200, 0);
            }
        }
    }
}
