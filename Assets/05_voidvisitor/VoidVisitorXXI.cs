using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using navdi3;
using navdi3.maze;
using navdi3.tiled;

public class VoidVisitorXXI : MonoBehaviour
{
	public TiledLoader tiledLoader;
	public TextAsset testLevel;
	public Tilemap tilemap;
    public MazeMaster mazeMaster;

    public BankLot banks { get { return GetComponent<BankLot>(); } }
    public SpriteLot sprites { get { return GetComponent<SpriteLot>(); } }

    [HideInInspector] public EntityLot players, dots, boys, walls;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Entity"), LayerMask.NameToLayer("Entity"));

        players = EntityLot.NewEntLot("players");
        dots = EntityLot.NewEntLot("dots");
        boys = EntityLot.NewEntLot("boys");
        walls = EntityLot.NewEntLot("walls");

        tiledLoader.SetupTileset(sprites,
            new int[] { 10, },
            new int[] { 1, 2, 3, 4, 11, 12, 13, 14, }
        );
        tiledLoader.PlaceTiles(tiledLoader.Load(testLevel), tilemap, this.SpawnTileId);
    }

    private void SpawnTileId(int tileId, Vector3Int tilePos)
    {
        var entPos = tilemap.layoutGrid.GetCellCenterWorld(tilePos);

        switch(tileId)
        {
            case 1:
                var player = banks["player"].Spawn(players.transform, entPos);
                player.GetComponent<MazeBodyKinematic>().master = mazeMaster;
                break;
            case 2: SpawnDot(1, tilePos); banks["redboy"].Spawn<RedboyMazer>(boys.transform, entPos).FactionSetup(1, mazeMaster, new twin(tilePos)); break;
            case 3: SpawnDot(1, tilePos); break;
            case 4: SpawnDot(0, tilePos); break;
            case 11: banks["boywall"].Spawn<BoyRejectionMazer>(walls.transform, entPos).Setup(mazeMaster, new twin(tilePos)); break;
            case 12: SpawnDot(2, tilePos); banks["redboy"].Spawn<RedboyMazer>(boys.transform, entPos).FactionSetup(2, mazeMaster, new twin(tilePos)); break;
            case 13: SpawnDot(2, tilePos); break;
        }
    }

    private void SpawnDot(int dotFaction, Vector3Int tilePos)
    {
        banks["dot"].Spawn<DotMazer>(dots.transform, tilemap.layoutGrid.GetCellCenterWorld(tilePos)).FactionSetup(dotFaction, mazeMaster, new twin(tilePos));
    }
}
