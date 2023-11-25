using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuManager : NetworkBehaviour
{
    public RawImage img;
    private Color alpha;

    private bool isAnimation = false;

    private void Start()
    {
        alpha = Color.black;
        alpha.a = 1;
        img.color = alpha;
        FadeAnimation(false);
    }

    public void FadeAnimation(bool inout)
    {
        if (isAnimation) return;
        isAnimation = true;

        if (inout) StartCoroutine(FadeIn());
        else StartCoroutine(FadeOut());
    }

    public IEnumerator InvokeFunction(bool inout, float time)
    {
        yield return new WaitForSeconds(time);
        FadeAnimation(inout);
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
}
