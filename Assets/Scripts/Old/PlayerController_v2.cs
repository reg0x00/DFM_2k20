using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_v2 : MonoBehaviour
{
    public float jump_height;
    public float gravity;
    public float y_speed_attenuation_by_time;
    public float speed;
    float y_speed;
    public bool in_flight = true;
    public bool wall_collision = false;
    int prw_cont_count=0;
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;  
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (!in_flight && Input.GetButton("Jump") && y_speed<jump_height/2.0F)
        {
            y_speed += jump_height;
            //in_flight = true;
            //Debug.Log("Add force");
        }else if (y_speed > 0)
        {
            y_speed -= y_speed_attenuation_by_time;
        }
        float vertical = -gravity + y_speed;
        Vector2 prw_position = rigidbody2d.position;
        Vector2 next_position=new Vector2(prw_position.x + horizontal*Time.fixedDeltaTime * speed,prw_position.y+ vertical * Time.fixedDeltaTime);
        rigidbody2d.MovePosition(next_position);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("contact");
        if (collision.collider.name == "Tilemap")
        {
            //Debug.Log(collision.contactCount);
            if (collision.contactCount == 3 || collision.contactCount == 1)
            {
                in_flight = false;
            }
            else
            {
                wall_collision = true;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.contactCount);
        if(!wall_collision && prw_cont_count==3 && collision.contactCount != 1)
        {
            in_flight = true;
            wall_collision = true;
        }
        if(wall_collision)
        {
            if(collision.contactCount == 3)
            {

                wall_collision = false;
                in_flight = false;
            }
        }
        prw_cont_count = collision.contactCount;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Tilemap")
        {
            in_flight = true;
        }
        wall_collision = false;
    }
}
