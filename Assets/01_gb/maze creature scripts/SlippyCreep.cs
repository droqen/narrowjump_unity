using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using navdi3;

public class SlippyCreep : GenericMazeCreature
{

    // these guys stick together.
    public override void ChooseNextMove()
    {
        List<twin> detection_dir_list = new List<twin>(twin.compass);
        detection_dir_list.Add(twin.zero);
        detection_dir_list.Add(-last_move*2);
        detection_dir_list.Remove(last_move);
        var detection_dirs = detection_dir_list.ToArray();
        Util.shufl(ref detection_dirs);

        List<twin> friendDirections = new List<twin>();
        foreach (var possible_friend in GenericMazeMaster.Instance.GetAtPositions(GetRelativePositions(detection_dirs)))
        {
            if (possible_friend!=gameObject&&possible_friend.GetComponent<SlippyCreep>()!=null)
            {
                var dir = possible_friend.GetComponent<SlippyCreep>().last_move;
                if (dir != twin.zero && !friendDirections.Contains(dir)) friendDirections.Add(dir);
            }
        }

        foreach (var friendDir in friendDirections) if (TryMoveDir(friendDir)) return; // move chosen.

        base.ChooseNextMove();
    }

    //public override bool CanMoveTo(Vector3Int target_pos)
    //{
    //    if (!IsTileSolid(my_tile_pos) && Random.value < .02f) return true;
    //    else return base.CanMoveTo(target_pos);
    //}
}
