using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
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
        if (!IsOwner) return;
        if (canMove)
        {
            //Movement Basic
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");

            direction = new Vector3(Vertical, 0, -Horizontal).normalized;
            
            //if(Mathf.Abs(rb.velocity.x) < 4 && Mathf.Abs(rb.velocity.z) < 4) rb.AddForce(direction * speed / 10, ForceMode.VelocityChange);
            //transform.position += direction * speed * Time.deltaTime;
            rb.MovePosition(transform.position + speed * Time.deltaTime * direction);
            
            //Debug.Log(rb.velocity);
            //if (Mathf.Abs(rb.velocity.x) > 4 || Mathf.Abs(rb.velocity.z) > 4) rb.mass = 1.5f;
            //else rb.mass = 1f;

            SetRotation(direction);

            //Vector3 targtPos = transform.position + speed * Time.deltaTime * direction;
            //rb.velocity = new Vector2(targtPos.x, targtPos.z);
            //transform.position = Vector3.Lerp(transform.position, targtPos, speed);
        }
        
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDash && direction != Vector3.zero)
        {
            nextDash = Time.time + dashCooldown;
            rb.AddForce(direction * dashPower, ForceMode.Impulse);
            SpawnParticleServerRpc(GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnParticleServerRpc(ulong playerId)
    {
        SpawnParticleClientRpc(playerId);
    }

    [ClientRpc]
    void SpawnParticleClientRpc(ulong playerId)
    {
        GameObject playerDash = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerId].gameObject;

        ParticleSystem dash = Instantiate(dashParticle, playerDash.transform.position + new Vector3(0, 0.1f, 0), playerDash.transform.rotation);
        dash.transform.rotation = Quaternion.Euler(playerDash.transform.rotation.eulerAngles - new Vector3(0f, 180f, 0f));
        Destroy(dash.gameObject, 2);
    }
    void SetRotation(Vector3 dir)
    {

        if (dir == Vector3.zero || Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = -AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * speed * Time.deltaTime);
        }
        else
        {
            float angle = Mathf.Atan2(dir.x, dir.z);

            // Zamieñ k¹t z radianów na stopnie
            float angleDegrees = Mathf.Rad2Deg * angle;

            // Tworzymy obrót wokó³ osi Y (góra-dó³)
            Quaternion targetRotation = Quaternion.Euler(0, angleDegrees, 0);

            // Interpolujemy p³ynnie obrotu gracza
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
