
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStartScript : MonoBehaviour
{
	public navdi3.BankLot banks;
    public TextAsset firstLevel;
    public navdi3.tiled.TiledLoader loader;
    public navdi3.SpriteLot spriteLot;
    public UnityEngine.Tilemaps.Tilemap tilemap;

    public EntityLot players, dots, ghosts;

    // Start is called before the first frame update
    void Start()
    {
        players = EntityLot.NewEntLot(transform, "players");
        dots = EntityLot.NewEntLot(transform, "dots");
        ghosts = EntityLot.NewEntLot(transform, "ghosts");

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Entity"), LayerMask.NameToLayer("Entity"));
        loader.SetupTileset(spriteLot,
            new int[] { 22, 30, 31, 32, 42, },
            new int[] { 01, 02, 03, });
        var firstLevelData = loader.Load(firstLevel);
        loader.PlaceTiles(firstLevelData, tilemap, this.SpawnTileId);
    }

    void SpawnTileId(int tile_id, Vector3Int tile_pos)
    {
        var spawn_pos = tilemap.layoutGrid.CellToWorld(tile_pos) + tilemap.layoutGrid.cellSize / 2;
        switch(tile_id) {
            case 01: banks["player"].Spawn(players.transform, spawn_pos); break;
            case 02: banks["dumbdot"].Spawn(dots.transform, spawn_pos); break;
            case 03: banks["ghostcreep"].Spawn<GhostCreep>(ghosts.transform, spawn_pos).Setup(tile_pos); break;
        }
    }

    private void FixedUpdate()
    {
        // TODO: check if players are overlapping dots
    }
}
