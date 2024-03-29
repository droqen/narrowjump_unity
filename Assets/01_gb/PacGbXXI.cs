﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacGbXXI : MonoBehaviour
{
    public navdi3.BankLot banks;
    public TextAsset[] levelAssets;
    public navdi3.tiled.TiledLoader loader;
    public navdi3.SpriteLot spriteLot;
    public UnityEngine.Tilemaps.Tilemap tilemap;

    [HideInInspector] public EntityLot players, dots, ghosts;

    int currentLevelNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        players = EntityLot.NewEntLot("players");
        dots = EntityLot.NewEntLot("dots");
        ghosts = EntityLot.NewEntLot("ghosts");

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Entity"), LayerMask.NameToLayer("Entity"));
        loader.SetupTileset(spriteLot,
            solidTileIds: new int[] { 22, 30, 31, 32, 42, },
            spawnTileIds: new int[] { 01, 02, 03, 04, });

        StartLevel(levelAssets[currentLevelNumber]);
    }

    void SpawnTileId(int tile_id, Vector3Int tile_pos)
    {
        Dj.Tempf("Spawn Tile {0}", tile_id);
        var spawn_pos = tilemap.layoutGrid.CellToWorld(tile_pos) + tilemap.layoutGrid.cellSize / 2;
        switch (tile_id)
        {
            case 01: banks["player"].Spawn(players.transform, spawn_pos); break;
            case 02: banks["dumbdot"].Spawn(dots.transform, spawn_pos); break;
            case 03: banks["ghostcreep"].Spawn<GenericMazeCreature>(ghosts.transform, spawn_pos).Setup(tile_pos); break;
            case 04: banks["slippy"].Spawn<GenericMazeCreature>(ghosts.transform, spawn_pos).Setup(tile_pos); break;
            default: Dj.Warnf("failed to spawn unknown tile_id {0}", tile_id); break;
        }
    }

    private void StartLevel(TextAsset levelAsset)
    {
        players.Clear(); dots.Clear(); ghosts.Clear();

        var levelData = loader.Load(levelAsset);
        loader.PlaceTiles(levelData, tilemap, this.SpawnTileId);

        //Time.timeScale = 0;
        //timeSlowStart = Time.unscaledTime;
        //timeSlowEnd = timeSlowStart + 1;
    }
}
