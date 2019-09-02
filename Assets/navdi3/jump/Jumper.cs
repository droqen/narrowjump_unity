﻿namespace navdi3.jump
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jumper : MonoBehaviour
    {
        [Header("Jumper stats")]

        public float x_MaxMoveSpeed = 75;
        public float x_Acceleration = 25;
        //public float x_Friction = 20;
        public float y_Gravity = 10;
        public float y_JumpSpeed = 200;
        public float y_MaxFallSpeed = 400;

        [Header("Runtime variables")]

        public Vector2 pin = Vector2.zero;

        public int jumpBufferFrames = 4;
        public int floorBufferFrames = 4;

        public BoxCollider2D box { get { return GetComponent<BoxCollider2D>(); } }
        public Rigidbody2D body { get { return GetComponent<Rigidbody2D>(); } }
        [HideInInspector] public bool jumpheld = false;
        [HideInInspector] public int jumpbuffer = 0;
        [HideInInspector] public int floorbuffer = 0;
        private void FixedUpdate()
        {
            if (IsFloored()) floorbuffer = floorBufferFrames;
            if (jumpbuffer > 0 && floorbuffer > 0)
            {
                jumpbuffer = 0; floorbuffer = 0;
                body.velocity = new Vector2(body.velocity.x, y_JumpSpeed);
            }
            if (jumpbuffer > 0) jumpbuffer--;
            if (floorbuffer > 0) floorbuffer--;

            var accel = x_Acceleration;

            body.velocity = new Vector2(
                Util.tow(body.velocity.x, x_MaxMoveSpeed * pin.x, accel),

                IsFloored()?body.velocity.y:
                Util.tow(body.velocity.y, -y_MaxFallSpeed, y_Gravity)
            );
        }
        
        public void PinJump()
        {
            jumpbuffer = jumpBufferFrames;
            jumpheld = true;
        }
        public void PinJumpRelease()
        {
            jumpheld = false;
        }
        public bool IsFloored()
        {
            if (body.velocity.y <= .01f)
            {
                RaycastHit2D hit;
                hit = Physics2D.BoxCast(body.position + box.offset, box.size - Vector2.right * .01f, 0, Vector2.down, .99f, LayerMask.GetMask("Solid"));
                if (hit.collider != null) return true;
            }

            return false;
        }
    }

}