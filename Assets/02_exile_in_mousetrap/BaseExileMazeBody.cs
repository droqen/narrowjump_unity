using UnityEngine;
using System.Collections;

using navdi3;
using navdi3.maze;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

abstract public class BaseExileMazeBody : MazeBody
{

    public int pregnancy_duration = 200;

    override public bool CanMoveTo(twin target_pos) {
        if (!base.CanMoveTo(target_pos)) return false;
        foreach(var mazer in master.GetBodiesAt(target_pos))
        {
            if (mazer != this && mazer.gameObject.name == this.gameObject.name) return false;
        }
        return true;
    }

    virtual public void ChooseNextMove()
    {
        TryMove(lastMove);
    }

    virtual public void Step()
    {

    }



    public float desired_speed = 50;
    public float mult_accel_rate = 0.25f;
    public float tile_snap_dist = 1f;

    [Range(0,1)] public float fullness = 1;
    bool pregnant = false;
    protected int pregnancyTimer = 0;

    public Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    public SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }

    private void FixedUpdate()
    {
        if (pregnant)
        {
            // don't move no mo'
            body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, mult_accel_rate);
            if (pregnancyTimer>0) pregnancyTimer--;
        }
        else
        {
            if (IsWithinDistOfCentered(tile_snap_dist))
            {
                ChooseNextMove();

                if (IsWithinDistOfCentered(tile_snap_dist))
                {
                    // slowing down cuz i'm chilling here for a bit
                    body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, mult_accel_rate);
                }
            }
            else
            {
                body.velocity = Vector2.Lerp(body.velocity, (ToCentered()).normalized * desired_speed, mult_accel_rate);
            }
        }

        Step();

        if (Mathf.Abs(body.velocity.x) > desired_speed * 0.5f) spriter.flipX = body.velocity.x < 0;
    }

    public void BeginPregnancy()
    {
        pregnant = true;
        pregnancyTimer = pregnancy_duration;
    }
    public bool IsPregnant()
    {
        return pregnant;
    }
    public bool TryGiveBirth()
    {
        if (pregnant && pregnancyTimer <= 0)
        {
            pregnant = false;
            return true;
        } else
        {
            return false;
        }
    }
}
