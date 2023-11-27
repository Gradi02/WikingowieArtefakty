using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PopupMove : MonoBehaviour
{
    public TextMeshProUGUI textmesh;

    void Start()
    {
        float move = this.gameObject.transform.position.y;
        LeanTween.value(textmesh.gameObject, 1, 0, 4).setOnUpdate(UpdateTextAlpha).setDelay(1.5f);
        LeanTween.moveLocalY(this.gameObject, move, 40f);
        Destroy(this.gameObject, 20f);
    }

    private void UpdateTextAlpha(float alpha)
    {
        Color color = textmesh.gameObject.GetComponent<TextMeshProUGUI>().color;
        color.a = alpha;
        textmesh.gameObject.GetComponent<TextMeshProUGUI>().color = color;
    }
}
