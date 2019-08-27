using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlayerScript : MonoBehaviour
{
    public navdi3.SpriteLot sprlot;

    SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }
    Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }

    public const float moveAccel = 20.0f;
    public const float moveSpeed = 60.0f;
    public const float jumpSpeed = 135.0f;
    public const float gravity = 5.0f;

    int floorbuffer = 0;
    int jumpbuffer = 0;
    float anim = 0;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) jumpbuffer = 4;
        if (Input.GetKeyDown(KeyCode.W)) jumpbuffer = 4;
        if (Input.GetKeyDown(KeyCode.UpArrow)) jumpbuffer = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool floor = false;

        if (body.velocity.y <= 0.01f) {
            RaycastHit2D hit;
            hit = Physics2D.BoxCast(body.position + box.offset, box.size - Vector2.right * .01f, 0, Vector2.down, .1f, LayerMask.GetMask("Solid"));
            if (hit.collider != null) { floor = true; floorbuffer = 20; }
        }

        bool moving;

        var pinx = Input.GetAxisRaw("Horizontal");
        moving = Mathf.Abs(pinx) > float.Epsilon;

        float accel = moveAccel;

        if (jumpbuffer > 0 && floorbuffer > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            jumpbuffer = 0; floorbuffer = 0; floor = false;
        }
        if (jumpbuffer > 0) jumpbuffer--;
        if (floorbuffer > 0) floorbuffer--;

        body.velocity = new Vector2(navdi3.Util.tow(body.velocity.x, pinx * moveSpeed, accel), body.velocity.y);
        if (moving) GetComponent<SpriteRenderer>().flipX = pinx < 0;

        if (!floor)
        {
            body.velocity += Vector2.down * gravity;
        }

        if (floor)
        {
            if (moving)
            {
                anim = (anim + .20f) % 4;
                spriter.sprite = sprlot[walkSprites[Mathf.FloorToInt(anim)]];
            } else
            {
                anim = 0;
                spriter.sprite = sprlot[60];
            }
        } else
        {
            if (floorbuffer > 0)
            {
                spriter.sprite = sprlot[62];
            } else
            {
                spriter.sprite = sprlot[63];
            }
        }
    }

    static int[] walkSprites = new int[] { 61, 60, 62, 60 };
}
