using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using navdi3;
using navdi3.tiled;

public class ShootbugXXI : MonoBehaviour
{
    public Tilemap tilemap;
    public TextAsset testLevel;
    public TiledLoader loader { get { return GetComponent<TiledLoader>(); } }
    public BankLot banks { get { return GetComponent<BankLot>(); } }
    public SpriteLot sprites { get { return GetComponent<SpriteLot>(); } }

    public EntityLot players { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        players = EntityLot.NewEntLot("players");

        loader.SetupTileset(sprites,
            solidTileIds: new int[] { 3 },
            spawnTileIds: new int[] { 1 });
        loader.PlaceTiles(loader.Load(testLevel), tilemap, SpawnTileId);
    }

    void SpawnTileId(int tileId, Vector3Int tilePos)
    {
        Vector2 entPos = tilemap.layoutGrid.GetCellCenterWorld(tilePos);
        switch(tileId)
        {
            case 1: banks["marine"].Spawn(players.transform, entPos); break;
        }
    }
}
