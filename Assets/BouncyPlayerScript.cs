using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlayerScript : MonoBehaviour
{
    public const float moveSpeed = 40.0f;
    public const float jumpSpeed = 40.0f;
    public const float gravity = 5.0f;

    Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }

    // Update is called once per frame
    void Update()
    {
        var pinx = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(pinx*moveSpeed, body.velocity.y);
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(body.position + box.offset, box.size, 0, Vector2.down, 0.5f, LayerMask.GetMask("Solid"));
        if (hit.collider)
        {
            // floored
            if (Input.GetKeyDown(KeyCode.Space))
            {
                body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            }
        } else
        {
            body.velocity += Vector2.down * gravity;
        }
    }
}
