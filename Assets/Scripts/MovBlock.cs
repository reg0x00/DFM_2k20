using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovBlock : MonoBehaviour
{
    public float Character_Speed_Debuff = 0.3F;
    bool lock_is_set = true;
    const float time_to_relax = 0.5F;
    float relax_timer;
    float move_amplification = 1.5F;
    float post_force_direction=1;
    bool apply_post_force = false;
    bool right_dir = false;
    float post_velocity = 100.0F;
    float delay_to_apply_post_force = 0.1F;
    float PF_timer;
    Rigidbody2D rigidbody2d;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        relax_timer = time_to_relax;
        PF_timer = delay_to_apply_post_force;
    }
    private void FixedUpdate()
    {
        if (apply_post_force)
        {
            if (PF_timer < 0)
            {
                rigidbody2d.AddForce(new Vector2(post_velocity*post_force_direction, 0));
                PF_timer = delay_to_apply_post_force;
                apply_post_force = false;
            }
            else
            {
                PF_timer -= Time.fixedDeltaTime;
            }
        }
        if (lock_is_set && !rigidbody2d.isKinematic)
        {
            if (Mathf.Approximately(rigidbody2d.velocity.magnitude, 0))
            {
                if (relax_timer < 0)
                {
                    relax_timer = time_to_relax;
                    rigidbody2d.isKinematic = true;
                }
                relax_timer -= Time.fixedDeltaTime;
            }
            else
            {
                relax_timer = time_to_relax;
            }
        }
        if (!lock_is_set && rigidbody2d.isKinematic)
        {
            rigidbody2d.isKinematic = false;
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
    //    if (ctl != null)
    //    {
    //        // some animation
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            if (Input.GetKey("e"))
            {
                right_dir = false;
                ctl.set_drag_dir= rigidbody2d.position.x - ctl.GetComponent<Rigidbody2D>().position.x;
                ctl.drag_dbf_change = Character_Speed_Debuff;
                ctl.drag_status_set = true;
                float amplify = 1;
                if ((rigidbody2d.position.x - ctl.GetComponent<Rigidbody2D>().position.x) > 0 && ctl.mov_pos.x<0)
                {
                    amplify = move_amplification;
                    if (!Mathf.Approximately(ctl.mov_pos.x, 0))
                    {
                        right_dir = true;
                        post_force_direction = Mathf.Sign(ctl.mov_pos.x);
                    }
                    
                }
                if (rigidbody2d.position.x - ctl.GetComponent<Rigidbody2D>().position.x < 0 && ctl.mov_pos.x > 0)
                {
                    amplify = move_amplification;
                    if (!Mathf.Approximately(ctl.mov_pos.x, 0))
                    {
                        right_dir = true;
                        post_force_direction = Mathf.Sign(ctl.mov_pos.x);
                    }
                }
                if (Mathf.Approximately(rigidbody2d.velocity.x, 0))
                {                    
                    rigidbody2d.velocity = new Vector2(0,rigidbody2d.velocity.y);
                }
                else
                {
                    rigidbody2d.MovePosition(rigidbody2d.position + new Vector2(ctl.mov_pos.x * amplify, 0));
                }                
            }
            else
            {
                ctl.drag_status_set = false;
            }
            lock_is_set = !Input.GetKey("e");
            if (!Input.GetKey("e"))
            {                
                rigidbody2d.velocity = new Vector2(0, 0);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            lock_is_set = true;
            if (Input.GetKey("e"))
            {
                rigidbody2d.velocity = new Vector2(0, 0);
                apply_post_force = right_dir;
            }
            ctl.drag_status_set = false;
        }
    }
}
