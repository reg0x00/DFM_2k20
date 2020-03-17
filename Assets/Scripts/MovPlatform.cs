using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPlatform : MonoBehaviour
{
    public float X_end_point;
    public float Y_end_point;
    public float speed;
    bool flip = false;
    bool remote_ctl = false;
    float dist_mag;
    bool need_to_synchronization = false;
    Rigidbody2D rigidbody2d;
    Vector2 start_point;
    Vector2 end_point;
    Vector2 path_to_target;
    Vector2 mov;
    PlayerController_v3 controller;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        start_point = rigidbody2d.position;
        end_point = new Vector2(X_end_point,Y_end_point);
        dist_mag = (start_point - end_point).magnitude;
        path_to_target = (end_point - start_point);
        path_to_target = path_to_target.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (need_to_synchronization)
        {
            controller.set_high_pri = (Time.fixedTime == controller.get_last_FU_time);
            need_to_synchronization = false;
        }
        Vector2 prw_position = rigidbody2d.position;
        if (((start_point-prw_position).magnitude>dist_mag && !flip) || ((end_point - prw_position).magnitude > dist_mag && flip))
        {
            path_to_target *= -1;
            flip ^= true;
            if (remote_ctl)
            {                
                controller.remote_NF_flip = false;
            }
        }
        mov = path_to_target * Time.fixedDeltaTime * speed;
        if (remote_ctl)
        {
            controller.Remote_mov_v2_s = mov;
        }
        rigidbody2d.MovePosition(prw_position+ path_to_target * Time.fixedDeltaTime*speed);
        Vector2 now_pos = prw_position + path_to_target * Time.fixedDeltaTime * speed;
        if ((start_point - now_pos).magnitude > dist_mag && !flip || (end_point - now_pos).magnitude > dist_mag && flip)
        {
            if (remote_ctl)
            {
                controller.remote_NF_flip = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController_v3 ctl= collision.collider.GetComponent<PlayerController_v3>();
        remote_ctl = (ctl != null);
        controller = ctl;
        //if(ctl!= null)
        //{
        //    ctl.Remote_mov_v2_s = mov;
        //}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        remote_ctl = false;
        controller = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController_v3 ctl = collision.collider.GetComponent<PlayerController_v3>();
        need_to_synchronization= (ctl != null);
        controller = ctl;
    }
}
