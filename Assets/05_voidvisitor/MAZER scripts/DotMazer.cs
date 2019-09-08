using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using navdi3;
using navdi3.maze;

public class DotMazer : VoidFactionMazer
{
    public SpriteLot sprites;

    static int[] differentDotSprids = { 4, 3, 13, };

    public void FactionSetup(int faction, MazeMaster master, twin cell_pos)
    {
        Setup(master, cell_pos);
        SetFaction(faction);
    }
    public void SetFaction(int faction)
    {
        this.faction = faction;
        GetComponent<SpriteRenderer>().sprite = sprites[differentDotSprids[faction]];
    }
}
