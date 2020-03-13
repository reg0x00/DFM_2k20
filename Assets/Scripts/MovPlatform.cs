using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPlatform : MonoBehaviour
{
    public float X_end_point;
    public float Y_end_point;
    public float speed;
    bool flip = false;
    float dist_mag;
    Rigidbody2D rigidbody2d;
    Vector2 start_point;
    Vector2 end_point;
    Vector2 now_target_point;
    Vector2 path_to_target;
    Vector2 mov;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        start_point = rigidbody2d.position;
        end_point = new Vector2(X_end_point,Y_end_point);
        now_target_point = end_point;
        dist_mag = (start_point - end_point).magnitude;
        path_to_target = (end_point - start_point);
        path_to_target = path_to_target.normalized;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector2 prw_position = rigidbody2d.position;
        if ((start_point-prw_position).magnitude>dist_mag && !flip)
        {
            path_to_target *= -1;
            flip = true;
        }
        if((end_point - prw_position).magnitude > dist_mag && flip)
        {
            path_to_target *= -1;
            flip = false;
        }
        mov = path_to_target * Time.deltaTime * speed;
        rigidbody2d.MovePosition(prw_position+ path_to_target * Time.deltaTime*speed);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController_v3 ctl= collision.collider.GetComponent<PlayerController_v3>();
        if(ctl!= null)
        {
            ctl.Remote_mov_v2_s = mov;
        }
    }
}
