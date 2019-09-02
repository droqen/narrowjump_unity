using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericMazeMaster : MonoBehaviour
{
	public Dictionary<GameObject, Vector3Int> objectsToTilePositions;
	public Dictionary<Vector3Int, HashSet<GameObject>> tilePositionsToHeaps;

    public static GenericMazeMaster Instance;
    void Awake()
    {
		objectsToTilePositions = new Dictionary<GameObject, Vector3Int>();
		tilePositionsToHeaps = new Dictionary<Vector3Int, HashSet<GameObject>>();
        Instance = this;
    }
    public UnityEngine.Tilemaps.Tilemap tilemap;
    public void Register(GameObject gameObject, Vector3Int tile_pos)
	{
        if (objectsToTilePositions.TryGetValue(gameObject, out var previous_pos))
		{
			tilePositionsToHeaps[previous_pos].Remove(gameObject);
		}

        if (!tilePositionsToHeaps.ContainsKey(tile_pos))
		{
			tilePositionsToHeaps.Add(tile_pos, new HashSet<GameObject>());
		}
		tilePositionsToHeaps[tile_pos].Add(gameObject);

		objectsToTilePositions[gameObject] = tile_pos;
	}
    public void Unregister(GameObject gameObject)
	{
        if (objectsToTilePositions.TryGetValue(gameObject, out var tile_pos))
		{
            if (tilePositionsToHeaps.TryGetValue(tile_pos, out var heap)) {
				heap.Remove(gameObject);
			} else
			{
				Dj.Warnf("GenericMazeMaster unregistered {0} but couldn't unheap it from tilePosition {1}", gameObject, tile_pos);
			}
			objectsToTilePositions.Remove(gameObject);
		} else
		{
			Dj.Warnf("GenericMazeMaster couldn't unregister {0}", gameObject);
		}
	}

    public HashSet<GameObject> GetAtPositions(params Vector3Int[] positions)
    {
        var mazers = new HashSet<GameObject>();
        foreach (var tile_pos in positions)
        {
            foreach (var obj in GetAtPosition(tile_pos))
            {
                mazers.Add(obj);
            }
        }
        return mazers;
    }

    public HashSet<GameObject> GetAtPosition(Vector3Int position)
    {
        if (tilePositionsToHeaps.TryGetValue(position, out var heap)) return heap;
        else return new HashSet<GameObject>(); // empty set
    }
}
