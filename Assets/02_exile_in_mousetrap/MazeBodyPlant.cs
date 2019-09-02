using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using navdi3;
using navdi3.maze;

public class MazeBodyPlant : BaseExileMazeBody
{
    twinrect alldirs = new twinrect(-1, -1, 1, 1);
    bool AdjacentToWall(twin target_pos)
    {
        foreach(var dir in alldirs.GetAllPoints())
        {
            if (IsSolid(target_pos + dir)) return true;
        }
        return false;
    }
    //override public bool CanMoveTo(twin target_pos)
    //{
    //    if (!AdjacentToWall(target_pos)) return false;
    //    return base.CanMoveTo(target_pos);
    //}

    CircleCollider2D box { get { return GetComponent<CircleCollider2D>(); } }

    public SpriteLot spritelot;

    public int windStrength = 0;

    public override void OnSetup()
    {
        base.OnSetup();
        windStrength = Random.Range(10, 20);
        gameObject.name = "seed#" + Random.Range(0, 1000);
        box.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Entity");
    }

    public override void Step()
    {
        fullness -= 0.001f;
        if (fullness <= 0) Object.Destroy(gameObject);

        if (IsPregnant() && pregnancyTimer == 1)
        {
            bool trapped = true;
            foreach(var dir in twin.compass)
            {
                if (CanMoveTo(my_cell_pos + dir))
                {
                    trapped = false; break;
                }
            }
            if (trapped) BeginPregnancy(); // reset pregnancy to beginning
        }
    }

    public override void ChooseNextMove()
    {
        if (windStrength > 0)
        {
            var options = new ChoiceStack<twin>();
            options.AddManyThenLock(twin.right, twin.up, twin.left, twin.down);
            options.RemoveAll(-lastMove);
            options.GetFirstTrue(TryMove); // move!!!
            windStrength--;
        } else
        {
            gameObject.name = "plant";
            box.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Solid");
            if (CanMoveTo(my_cell_pos))
            {
                BeginPregnancy();
            } else
            {
                Object.Destroy(gameObject);
            }
        }

        spriter.sprite = spritelot[IsPregnant() ? 6 : 7];
    }
}