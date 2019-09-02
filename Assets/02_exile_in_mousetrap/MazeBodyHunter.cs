using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using navdi3;
using navdi3.maze;

public class MazeBodyHunter : BaseExileMazeBody
{

    public bool overcrowded = false;
    public bool killForSport = false;

    public override void OnSetup()
    {
        base.OnSetup();
    }

    public void Fight(MazeBodyHerbivore herb)
    {
        if (IsPregnant()) // stress
        {
            this.fullness -= 0.15f;
        }
        else if (overcrowded)
        {
            this.fullness -= 0.05f;
            // ignore (a bit of stress)
        }
        else // fight to the death!
        {

            if (Random.value < .05f) Object.Destroy(this.gameObject);
            else Object.Destroy(herb.gameObject);

            fullness = 1;

            if (!killForSport && !IsPregnant())
            {
                BeginPregnancy();
            }
        }
    }

    public override void ChooseNextMove()
    {
        foreach(var mazer in master.GetBodiesAt(my_cell_pos))
        {
            if (mazer.GetComponent<MazeBodyHerbivore>()!=null)
            {
                Fight(mazer.GetComponent<MazeBodyHerbivore>());
                return;
            }
        }

        var options = new ChoiceStack<twin>();
        twin.StraightenCompass();
        options.AddManyThenLock(twin.compass);

        var hunter_influence = new Vector3();
        var hunter_count = 0;
        var herbivore_influence = new Vector3();
        var herbivore_count = 0;

        foreach (var mazer in GetMazeBodiesNear(24f))
        {
            if (mazer.GetComponent<MazeBodyHunter>())
            {
                hunter_influence += (mazer.transform.position - this.transform.position).normalized;
                hunter_count++;
            }
            if (mazer.GetComponent<MazeBodyHerbivore>())
            {
                herbivore_influence += (mazer.transform.position - this.transform.position).normalized;
                herbivore_count++;
            }
        }

        if (hunter_count > 0) // avoid other hunter
        {
            if (hunter_influence.x > 0) options.RemoveAll(twin.right);
            if (hunter_influence.x < 0) options.RemoveAll(twin.left);
            if (hunter_influence.y > 0) options.RemoveAll(twin.up);
            if (hunter_influence.y < 0) options.RemoveAll(twin.down);
            fullness -= 0.02f * hunter_count; // being near other hunters is stressful (we fight)

            if (hunter_count > 5) overcrowded = true;

            options.AddManyThenLock(twin.compass);
        }
        else if (herbivore_count > 0) // otherwise, approach herbivores
        {
            if (-herbivore_influence.x > 0) options.RemoveAll(twin.right);
            if (-herbivore_influence.x < 0) options.RemoveAll(twin.left);
            if (-herbivore_influence.y > 0) options.RemoveAll(twin.up);
            if (-herbivore_influence.y < 0) options.RemoveAll(twin.down);

            if (herbivore_count > 5) killForSport = true; // too many? murder murder murder

            options.AddManyThenLock(twin.compass);
        } else
        {
            killForSport = false;
            overcrowded = false;

            // no hunters/herbivores nearby?
            if (TryMove(lastMove)) // then just go straight if possible.
            {
                options = null;
            }
        }

        if (options != null)
        {
            options.RemoveAll(-lastMove); // hunters don't like turning back
            lastMove = options.GetFirstTrue(TryMove);
        }

    }

    public override void Step()
    {
        fullness -= 0.001f;
        if (fullness <= 0) Object.Destroy(this.gameObject);
    }
}
