using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using navdi3;

public class GhostCreep : MonoBehaviour
{

    public void Setup(Vector3Int tile_pos)
    {
        this.tile_pos = tile_pos;
        this.transform.position = MapToWorld(tile_pos);
    }

    const float desired_speed = 50.0f;

    public UnityEngine.Tilemaps.Tilemap tilemap;

    SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }
    Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
    BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }

    public Vector3Int tile_pos;

    public twin last_move = twin.zero;

    Vector2 MapToWorld(Vector3Int map)
    {
        return (Vector2)(tilemap.layoutGrid.CellToWorld(map) + tilemap.layoutGrid.cellSize / 2);
    }

    bool TryMoveDir(twin dir)
    {
        var tile = tilemap.GetTile(tile_pos + dir);
        if (tile == null || ((UnityEngine.Tilemaps.Tile)tile).colliderType == UnityEngine.Tilemaps.Tile.ColliderType.None)
        {
            // ok!
            last_move = dir;
            tile_pos += dir;
            return true;
        } else
        {
            // nah!
            return false;
        }
    }

    private void FixedUpdate()
    {
        var to_target = MapToWorld(this.tile_pos) - body.position;
        body.velocity = 0.75f * body.velocity + 0.25f * (to_target).normalized * desired_speed;
        if (to_target.sqrMagnitude <= 1)
        {
            bool move_ok = false;
            twin.ShuffleCompass();
            if (last_move != twin.zero && Random.value < .5f) move_ok = TryMoveDir(last_move); // 50% of the time, just try continuing on in the same direction first
            foreach (var dir in twin.compass)
            {
                if (dir != -last_move && TryMoveDir(dir))
                {
                    move_ok = true; break;
                }
            }
            if (!move_ok) TryMoveDir(-last_move);
        }

        if (Mathf.Abs(body.velocity.x) > desired_speed * 0.5f) spriter.flipX = body.velocity.x < 0;
    }
}
