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
    Rigidbody2D rigidbody2d;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        relax_timer = time_to_relax;
    }
    private void Update()
    {
        if (lock_is_set && !rigidbody2d.isKinematic)
        {
            if (Mathf.Approximately(rigidbody2d.velocity.magnitude, 0))
            {
                if (relax_timer < 0)
                {
                    relax_timer = time_to_relax;
                    rigidbody2d.isKinematic = true;
                }
                relax_timer -= Time.deltaTime;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            // some animation
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            
            if (Input.GetKey("e"))
            {
                ctl.drag_dbf_change = Character_Speed_Debuff;
                ctl.drag_status_set = true;
                float amplify = 1;
                if ((rigidbody2d.position.x - ctl.GetComponent<Rigidbody2D>().position.x) > 0 && ctl.mov_pos.x<0)
                {
                    amplify = move_amplification;
                }
                if (rigidbody2d.position.x - ctl.GetComponent<Rigidbody2D>().position.x < 0 && ctl.mov_pos.x > 0)
                {
                    amplify = move_amplification;
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
        }
    }
}
