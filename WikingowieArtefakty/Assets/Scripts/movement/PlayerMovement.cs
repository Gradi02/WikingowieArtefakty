using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    private float Horizontal, Vertical;
    [Min(0.1f)] public float speed;
    public bool canMove = true;
    private Rigidbody rb;
    private float nextDash = 0;
    public float dashCooldown = 3;
    private Vector3 direction = Vector3.zero;
    public float dashPower = 5;
    public ParticleSystem dashParticle;

    private void Awake()
    {
        player = this.gameObject;
        rb = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //Movement Basic
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            direction = new Vector3(Vertical, 0, -Horizontal);
            Vector3 targtPos = transform.position + speed * Time.deltaTime * direction;

            //transform.position += direction * speed * Time.deltaTime;
            //rb.velocity = new Vector2(targtPos.x, targtPos.z);
            //rb.MovePosition(targtPos);
            //rb.AddForce(direction, ForceMode.VelocityChange);
            transform.position = Vector3.Lerp(transform.position, targtPos, speed);
            SetRotation(direction);


            //Dash
            if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDash && direction != Vector3.zero)
            {
                nextDash = Time.time + dashCooldown;
                rb.AddForce(direction * dashPower, ForceMode.Impulse);
                ParticleSystem dash = Instantiate(dashParticle, transform.position, player.transform.rotation);
                dash.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles - new Vector3(0f, 90f, 0f));
                dash.Play();
            }
        }
        
    }
    void SetRotation(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.x, dir.z);

            // Zamieñ k¹t z radianów na stopnie
            float angleDegrees = Mathf.Rad2Deg * angle;

            // Tworzymy obrót wokó³ osi Y (góra-dó³)
            Quaternion targetRotation = Quaternion.Euler(0, angleDegrees-90, 0);

            // Interpolujemy p³ynnie obrotu gracza
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * speed * Time.deltaTime);
        }
        else
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = -AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen) - 90;
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * speed * Time.deltaTime);
        }
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
    public void SetStartPosition(Vector3 pos)
    {
        Vector3 startPos = pos;
        startPos.y = 0.75f;
        player.transform.position = startPos;
    }
}
