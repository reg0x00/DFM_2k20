using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_v3 : MonoBehaviour
{
    public float jump_height;
    public float gravity;
    public float y_speed_attenuation_by_time;
    public float speed;
    public bool ladder_set { set { on_ladder=value; } }
    bool on_ladder = false;
    float y_speed;
    bool in_flight = true;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;
    Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;  
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = 0.0f;
        Vector2 prw_position = rigidbody2d.position;
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookDirection.Set(horizontal, 0);
            lookDirection.Normalize();
        }
        animator.SetFloat("Move X", lookDirection.x);
        if (on_ladder)
        {
            vertical = Input.GetAxis("Vertical");
            Vector2 mov = new Vector2(horizontal,vertical);
            rigidbody2d.MovePosition(prw_position+mov * speed*Time.fixedDeltaTime);
            return;
        }
        if (!in_flight && Input.GetButton("Jump") && y_speed < jump_height / 2.0F)
        {
            y_speed += jump_height;
            in_flight = true;
            //Debug.Log("Add force");
        }
        else if (y_speed > 0)
        {
            y_speed -= y_speed_attenuation_by_time;
        }
        vertical = -gravity + y_speed;        
        Vector2 next_position = new Vector2(prw_position.x + horizontal * Time.fixedDeltaTime * speed, prw_position.y + vertical * Time.fixedDeltaTime);
        rigidbody2d.MovePosition(next_position);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D point in collision.contacts)
        {
            in_flight = true;
            //Debug.Log(point.normal);
            if (Mathf.Approximately(point.normal.y, 1.0F))
            {
                in_flight = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Tilemap")
        {
            in_flight = true;
        }
    }
}