using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using navdi3;
using navdi3.maze;

public class MazeBodyHerbivore : BaseExileMazeBody
{
    public int hermitage = 0;
    public int gluttony = 0;
    public int plants_eaten = 0;

	public override void ChooseNextMove()
    {
        foreach (var mazer in master.GetBodiesAt(my_cell_pos))
        {
            var plant = mazer.GetComponent<MazeBodyPlant>();
            if (plant && plant.IsPregnant())
            {
                fullness += 0.25f;
                plants_eaten ++;
                Object.Destroy(plant.gameObject);
                if (plants_eaten > gluttony)
                {
                    this.BeginPregnancy();
                    return;
                } else if (fullness > 1)
                {
                    // overeating? reduce gluttony
                    fullness = 1;
                    gluttony--;
                }
            }

            if (mazer.GetComponent<MazeBodyHunter>())
            {
                mazer.GetComponent<MazeBodyHunter>().Fight(this);
            }
        }

        var options = new ChoiceStack<twin>();
        twin.StraightenCompass();
        options.AddManyThenLock(twin.compass);

        var hunter_influence = new Vector3();
        var hunter_count = 0;
        var herbivore_influence = new Vector3();
        var herbivore_count = 0;
        var plant_influence = new Vector3();
        var plant_count = 0;

        foreach (var mazer in GetMazeBodiesNear(24f))
        {
            if (mazer.GetComponent<MazeBodyHunter>() && !mazer.GetComponent<MazeBodyHunter>().IsPregnant())
            {
                hunter_influence += (mazer.transform.position - this.transform.position).normalized;
                hunter_count++;
            }
            if (mazer.GetComponent<MazeBodyHerbivore>())
            {
                herbivore_influence += (mazer.transform.position - this.transform.position).normalized;
                herbivore_count++;
            }
            if (mazer.GetComponent<MazeBodyPlant>())
            {
                if (mazer.GetComponent<MazeBodyPlant>().IsPregnant())
                {
                    plant_influence += (mazer.transform.position - this.transform.position).normalized;
                    plant_count++;
                }
            }
        }

        hermitage = Mathf.Max(hermitage, herbivore_count);

        if (hunter_count > 0) // run from hunter
        {
            if (hunter_influence.x > 0) options.RemoveAll(twin.right);
            if (hunter_influence.x < 0) options.RemoveAll(twin.left);
            if (hunter_influence.y > 0) options.RemoveAll(twin.up);
            if (hunter_influence.y < 0) options.RemoveAll(twin.down);
        }
        else if (herbivore_count + hermitage > 20) // run from overcrowding
        {
            if (herbivore_influence.x > 0) options.RemoveAll(twin.right);
            if (herbivore_influence.x < 0) options.RemoveAll(twin.left);
            if (herbivore_influence.y > 0) options.RemoveAll(twin.up);
            if (herbivore_influence.y < 0) options.RemoveAll(twin.down);
            options.AddManyThenLock(twin.compass);
        }
        else if (plant_count > 0) // chase plant
        {
            gluttony = Mathf.Max(plant_count, gluttony);
            if (-plant_influence.x > 0) options.RemoveAll(twin.right);
            if (-plant_influence.x < 0) options.RemoveAll(twin.left);
            if (-plant_influence.y > 0) options.RemoveAll(twin.up);
            if (-plant_influence.y < 0) options.RemoveAll(twin.down);
            options.AddManyThenLock(twin.compass);
        }
        //else if (herbivore_count > 0 && Random.value < .5f) // stick with friends
        //{
        //    if (-herbivore_influence.x > 0) options.RemoveAll(twin.right);
        //    if (-herbivore_influence.x < 0) options.RemoveAll(twin.left);
        //    if (-herbivore_influence.y > 0) options.RemoveAll(twin.up);
        //    if (-herbivore_influence.y < 0) options.RemoveAll(twin.down);
        //    options.AddManyThenLock(twin.compass);
        //}
        else
        {
            options.RemoveAll(-lastMove);
            options.AddManyThenLock(-lastMove);
        }

        lastMove = options.GetFirstTrue(TryMove);
    }

    public override void Step()
    {
        fullness -= 0.001f;
        if (fullness <= 0) Object.Destroy(this.gameObject);
    }
}
