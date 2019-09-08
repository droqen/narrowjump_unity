using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using navdi3.jump;

public class Player_Shoot_Control : MonoBehaviour
{
    public ShootbugXXI xxi;

    public Jumper jumper { get { return GetComponent<Jumper>(); } }

    public Sprite shootStunnedSprite;

    public int shootStunFrameDuration = 5;
    public int shootReloadFrameDuration = 20;

    int shootStunBuffer = 0;
    int shootReloadBuffer = 0;

    bool doShootNext = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) doShootNext = true;
    }

    private void FixedUpdate()
    {
        if (shootStunBuffer > 0)
        {
            shootStunBuffer--;
            if (shootStunBuffer > 0)
            {
                //jumper.pin = Vector2.zero; // stop
                //jumper.PinJumpRelease();
            } else {
                SetEnabledJumperControls(true); // reenable
            }
        }
        
        if (shootReloadBuffer > 0)
        {
            shootReloadBuffer--;
        }
        else if (doShootNext)
        {
            GetComponent<SpriteRenderer>().sprite = shootStunnedSprite;
            Stun();
            shootReloadBuffer = shootReloadFrameDuration;
            doShootNext = false;
            xxi.banks["fireball"].Spawn(xxi.attacks.transform, transform.position);
        }
    }

    public void Stun()
    {
        if (shootStunFrameDuration > 0)
        {
            shootStunBuffer = shootStunFrameDuration;
            SetEnabledJumperControls(false);
        }
    }

    public void SetEnabledJumperControls(bool enabled) {
        //GetComponent<JumperExamplePlayer>().enabled = enabled;
        GetComponent<JumperSpriterExample>().enabled = enabled;
    }
}
