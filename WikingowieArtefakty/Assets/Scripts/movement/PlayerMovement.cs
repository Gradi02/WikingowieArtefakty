using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    private float Horizontal, Vertical;
    [Min(0.1f)] public float speed;
    public bool canMove = true;

    private void Awake()
    {
        player = this.gameObject;
    }

    private void Update()
    {
        if (canMove)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(Vertical, 0, -Horizontal);

            transform.position += direction * speed * Time.deltaTime;
            SetRotation();
        }
    }

    void SetRotation()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = -AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen)-90;

        transform.rotation = Quaternion.Euler(new Vector3(0f,angle,0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public void SetStartPosition(Vector3 pos)
    {
        Vector3 startPos = pos;
        startPos.y = 1;
        player.transform.position = startPos;
        Debug.Log("t");
    }
}
