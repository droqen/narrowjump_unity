using UnityEngine;
using System.Collections;

using navdi3.jump;

public class ExiledPlayer : MonoBehaviour
{
    public navdi3.SpriteLot sprlot;

    float anim;

    public float verticalThrustStrength = 2f;

    SpriteRenderer spriter {  get { return GetComponent<SpriteRenderer>(); } }
    public Jumper jumper { get { return GetComponent<Jumper>(); } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) jumper.PinJump();
    }
    private void FixedUpdate()
    {
        jumper.pin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!Input.GetKey(KeyCode.Space)) jumper.PinJumpRelease();
        
        bool moving = Mathf.Abs(jumper.pin.x) > .1f;

        if (jumper.IsFloored())
        {
            if (moving)
            {
                anim = (anim + .20f) % 4;
                spriter.sprite = sprlot[walkSprites[Mathf.FloorToInt(anim)]];
            }
            else
            {
                anim = 0;
                spriter.sprite = sprlot[10];
            }
        }
        else
        {
            if (jumper.floorbuffer > 0)
            {
                spriter.sprite = sprlot[12];
            }
            else
            {
                spriter.sprite = sprlot[13];
            }
        }

        if (moving) spriter.flipX = jumper.pin.x < 0;

        jumper.body.velocity += Vector2.up * verticalThrustStrength * jumper.pin.y;
    }

    static int[] walkSprites = { 11,10,12,10 };
}