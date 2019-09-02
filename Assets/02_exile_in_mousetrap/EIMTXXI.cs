using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using navdi3;
using navdi3.tiled;
using navdi3.maze;

public class EIMTXXI : MonoBehaviour
{
    public Tilemap tilemap;
    public TextAsset levelAsset;
    public MazeMaster mazeMaster;

    TiledLoader loader { get { return GetComponent<TiledLoader>();  } }
    BankLot banks { get { return GetComponent<BankLot>();  } }
    SpriteLot sprites { get { return GetComponent<SpriteLot>();  } }

    EntityLot players, creatures;

    // Start is called before the first frame update
    void Start()
    {

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Entity"), LayerMask.NameToLayer("Entity"));

        players = EntityLot.NewEntLot("players");
        creatures = EntityLot.NewEntLot("creatures");
        var levelData = loader.Load(levelAsset);
        loader.SetupTileset(sprites,
            solidTileIds: new int[] { 1, 2, },
            spawnTileIds: new int[] { 3, 4, 5,  6,7, }
            );
        loader.PlaceTiles(levelData, this.tilemap, this.SpawnTileId);
    }

    void SpawnTileId(int tile_id, Vector3Int tile_pos)
    {
        var ent_pos = tilemap.layoutGrid.CellToWorld(tile_pos) + tilemap.layoutGrid.cellSize / 2;
        switch(tile_id)
        {
            case 3: banks["player"].Spawn(players.transform, ent_pos); break;
            case 4: banks["hunter"].Spawn<MazeBody>(creatures.transform, ent_pos).Setup(mazeMaster, new twin(tile_pos)); break;
            case 5: banks["herbivore"].Spawn<MazeBody>(creatures.transform, ent_pos).Setup(mazeMaster, new twin(tile_pos)); break;
            case 6:
            case 7: banks["plant"].Spawn<MazeBody>(creatures.transform, ent_pos).Setup(mazeMaster, new twin(tile_pos)); break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(Transform child in creatures.transform)
        {
            var ex = child.GetComponent<BaseExileMazeBody>();
            if (ex.TryGiveBirth())
            {
                banks[ex.gameObject.name].Spawn<MazeBody>(creatures.transform, ex.transform.position).Setup(mazeMaster, ex.my_cell_pos);
            }
        }
    }
}