
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
    // Start is called before the first frame update
    void Start()
    {
		banks["player"].Spawn<SpriteRenderer>();
        loader.SetupTileset(spriteLot,
            new int[] { 22, 30, 31, 32, 42, },
            new int[] { 01, 02, 03, });
        var firstLevelData = loader.Load(firstLevel);
        loader.PlaceTiles(firstLevelData, tilemap);
    }
}
