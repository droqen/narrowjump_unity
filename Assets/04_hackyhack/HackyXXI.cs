using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using navdi3;
using navdi3.tiled;

public class HackyXXI : MonoBehaviour
{
    public Tilemap tilemap;
    public TextAsset testLevel;
    public TiledLoader loader { get { return GetComponent<TiledLoader>(); } }
    public BankLot banks { get { return GetComponent<BankLot>(); } }
    public SpriteLot sprites { get { return GetComponent<SpriteLot>(); } }

    public EntityLot players { get; set; }
    public EntityLot attacks { get; set; }
    public EntityLot ladders { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Entity"),
            LayerMask.NameToLayer("Entity")
        );

        players = EntityLot.NewEntLot("players");
        attacks = EntityLot.NewEntLot("attacks");
        ladders = EntityLot.NewEntLot("ladders");

        loader.SetupTileset(sprites,
            solidTileIds: new int[] { 11 },
            spawnTileIds: new int[] { 1, 2 });
        loader.PlaceTiles(loader.Load(testLevel), tilemap, SpawnTileId);
    }

    void SpawnTileId(int tileId, Vector3Int tilePos)
    {
        Vector2 entPos = tilemap.layoutGrid.GetCellCenterWorld(tilePos);
        switch (tileId)
        {
            case 1: banks["player"].Spawn(players.transform, entPos); break;
            case 2: banks["ladder"].Spawn(ladders.transform, entPos); break;
        }
    }
}
