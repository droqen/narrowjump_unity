using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using navdi3;
using navdi3.maze;

public class MazerFallingRock : MazeBody
{
    public float energy = .1f;

	public BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }
	public Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }

    override public bool CanMoveTo(twin target_pos) {
        if (!base.CanMoveTo(target_pos)) return false;

        foreach(var mazer in master.GetBodiesAt(target_pos))
        {
            if (mazer.GetComponent<MazerFallingRock>()) return false;
        }

        return true;
    }

    private void FixedUpdate()
    {
        if (ToCentered().sqrMagnitude < 1) {
            transform.position += ToCentered() * 0.5f;
            body.velocity *= 0.5f;
        }
        else
        {
            body.velocity *= 0.9f;
            body.velocity += 0.1f * ((Vector2)ToCentered()).normalized * 50 * energy;
        }

        if (Mathf.Abs(ToCentered().y) < 2f && Mathf.Abs(ToCentered().x) <= 8f)
        {
            var moves = new ChoiceStack<twin>();
            moves.AddManyThenLock(twin.down);
            moves.AddManyThenLock(twin.left, twin.right);
            moves.RemoveAll(-lastMove);

            lastMove = moves.GetFirstTrue(TryMove);

            if (lastMove == twin.down) energy = 1f;
            else if (energy > .1f) energy -= .1f;
        }
    }

    private bool WillMoveX(twin move)
    {
        return move.x * lastMove.x >= 0;
    }
}
