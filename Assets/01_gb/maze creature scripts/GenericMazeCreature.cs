using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using navdi3;

public class GenericMazeCreature : MonoBehaviour
{

    virtual public void CustomPostSetup() { }

    virtual public bool CanMoveTo(Vector3Int target_pos)
    {
        return !IsTileSolid(target_pos);
    }
    virtual public void ChooseNextMove()
    {
        if (last_move != twin.zero && Random.value < .5f) if (TryMoveDir(last_move)) return;

        if (TryChooseRandomNon180Move()) return;

        if (TryMoveDir(-last_move)) return;

        Dj.Warnf("mazecreature {0} is stuck", name);
    }
    virtual public void Step()
	{
        // basic sprite update
		if (Mathf.Abs(body.velocity.x) > desired_speed * 0.5f) spriter.flipX = body.velocity.x < 0;
	}



    public float desired_speed = 50.0f;
    public float mult_accel_rate = 0.25f;
    public float tile_snap_dist = 1f;

    public void Setup(Vector3Int tile_pos)
	{
		this.my_tile_pos = tile_pos;
		this.transform.position = MapToWorld(tile_pos);
        CustomPostSetup();
	}

    protected Vector2 MapToWorld(Vector3Int map)
    {
        return (Vector2)(tilemap.layoutGrid.CellToWorld(map) + tilemap.layoutGrid.cellSize / 2);
    }

	protected UnityEngine.Tilemaps.Tilemap tilemap { get { return GenericMazeMaster.Instance.tilemap; } }

	SpriteRenderer spriter { get { return GetComponent<SpriteRenderer>(); } }
	Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
	BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }

    private Vector3Int _my_tile_pos;
	public Vector3Int my_tile_pos { get
        {
            return _my_tile_pos;
        } set
        {
            if (_my_tile_pos!=value)
            {
                _my_tile_pos = value;
                GenericMazeMaster.Instance.Register(gameObject, _my_tile_pos);
            }
        }
    }
	[HideInInspector] public twin last_move = twin.zero;

    protected bool IsTileSolid(Vector3Int tile_pos)
    {
        var tile = tilemap.GetTile(tile_pos);
        if (tile == null || ((UnityEngine.Tilemaps.Tile)tile).colliderType == UnityEngine.Tilemaps.Tile.ColliderType.None)
        {
            return false; // not solid
        }
        else
        {
            return true; // yes, is solid
        }
    }
    
    protected bool TryMoveDir(twin dir)
	{
        if (CanMoveTo(my_tile_pos + dir))
        {
            last_move = dir;
            my_tile_pos += dir;
            return true;
        }
        else return false;
	}

    protected bool TryChooseRandomNon180Move()
    {
        twin.ShuffleCompass();

        foreach (var dir in twin.compass)
            if (dir != -last_move && TryMoveDir(dir))
                return true;

        return false;
    }

    protected Vector3Int[] GetRelativePositions(params twin[] dirs)
    {
        Vector3Int[] relativePositions = new Vector3Int[dirs.Length];
        for (int i = 0; i < dirs.Length; i++)
        {
            relativePositions[i] = this.my_tile_pos + dirs[i];
        }
        return relativePositions;
    }



	private void FixedUpdate()
	{
		var to_target = MapToWorld(this.my_tile_pos) - body.position;
		if (to_target.sqrMagnitude <= tile_snap_dist*tile_snap_dist) // once movement has taken me far enough, choose next tile
		{
            ChooseNextMove();
		}

        body.velocity = Vector2.Lerp(body.velocity, (to_target).normalized * desired_speed, mult_accel_rate);

		Step();
	}

    private void OnDestroy()
    {
        GenericMazeMaster.Instance.Unregister(gameObject);
    }
}
