using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using navdi3;
using navdi3.maze;

public class RedboyMazer : VoidFactionMazer
{

    public Sprite blueboySprite;

    override public bool CanMoveTo(twin target_pos) {
        if (!IsSolid(target_pos))
        {
            foreach (var mazer in master.GetBodiesAt(target_pos)) if (mazer.GetComponent<BoyRejectionMazer>()) return false;
            return true;
        }
        return false;
    }


    public void FactionSetup(int faction, MazeMaster master, twin cell_pos)
    {
        this.faction = faction;
        if (this.faction == 2) spriter.sprite = blueboySprite;
        Setup(master, cell_pos);
        GetComponent<DotFactionizer>().faction = faction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Solid"))
        {
            // ignore
        } else if (Util.boundbump(this.gameObject, lastMove, collision.gameObject))
        {
            TryMove(-lastMove);
        }
    }

    //   override public void OnMoved(twin prev_pos, twin target_pos)
    //   {
    //       // check for collisions.
    //       bool bounce = false;
    //       HashSet<RedboyMazer> turnbackers = new HashSet<RedboyMazer>();
    //       foreach(var mazer in master.GetBodiesAt(target_pos))
    //       {
    //           if (mazer.gameObject.name == "player")
    //           {
    //               bounce = true;
    //           }
    //           var boy = mazer.GetComponent<RedboyMazer>();
    //           if (boy != this && boy != null)
    //           {
    //               bounce = true;
    //               turnbackers.Add(boy);
    //               //boy.TryMove(-boy.lastMove);
    //           }
    //       }

    //       foreach(var boy in turnbackers)
    //       {
    //           boy.TryMove(-boy.lastMove);
    //       }

    //	if (bounce)
    //	{
    //		TryMove(-lastMove); // bounce back
    //	    Dj.Tempf("Bounce {0}", lastMove);
    //	}
    //}

    public override void OnSetup()
    {
        
    }



    virtual public void ChooseNextMove()
    {
        // find adjacent enemy dots and prefer to move into them over any other space.



        ChoiceStack<twin> stack = new navdi3.ChoiceStack<twin>();
        var enemyDotsArray = FindEnemyDots().ToArray();
        if (enemyDotsArray.Length > 0)
        {
            desired_speed += 5;
        } else
        {
            desired_speed -= 1; // slowly get less angry
        }
        desired_speed = Mathf.Clamp(desired_speed, lazy_speed, angry_speed);

        stack.AddManyThenLock(enemyDotsArray);
        stack.AddManyThenLock(twin.compass);
        stack.RemoveAll(-lastMove);
        stack.AddManyThenLock(-lastMove);
        stack.GetFirstTrue(TryMove);
    }

    virtual public void Step()
    {

    }


    private List<twin> FindEnemyDots()
    {
        List<twin> enemyDotDirs = new List<twin>(); // starts empty
        foreach (var dir in twin.compass)
        {
            foreach(var bodi in master.GetBodiesAt(my_cell_pos + dir))
            {
                var dot = bodi.GetComponent<DotMazer>();
                if (dot != null && dot.faction != this.faction) enemyDotDirs.Add(dir);
            }
        }
        return enemyDotDirs;
    }


    public float angry_speed = 50;
    public float lazy_speed = 20;
    public float desired_speed = 20;
    public float mult_accel_rate = 0.25f;
    public float tile_snap_dist = 1f;

    public Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    public SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }

    private void FixedUpdate()
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

        //Step();

        //if (Mathf.Abs(body.velocity.x) > desired_speed * 0.5f) spriter.flipX = body.velocity.x < 0;
    }
}
