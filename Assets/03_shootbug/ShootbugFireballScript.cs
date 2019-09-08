using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootbugFireballScript : MonoBehaviour
{
    public float fireballSpeed = 100;
    Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    // Start is called before the first frame update
    void Start()
    {
        body.velocity = Vector2.up * fireballSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Solid"))
        {
            Destroy(gameObject);
        }
    }
}
