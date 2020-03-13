using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_v3 : MonoBehaviour
{
    public float jump_height;
    public float gravity;
    public float y_speed_attenuation_by_time;
    public float speed;
    bool changed_speed = false;
    public bool changed_speed_status { set { changed_speed = value; } }
    public bool ladder_set { set { on_ladder = value; } }
    bool on_ladder = false;
    float default_speed;
    float y_speed;
    public float y_speed_set { set { y_speed = value; } }
    bool in_flight = true;
    bool in_flight_last_frame_processed = true; // in case, when several colliders involved
    bool ladder_stun = false;
    public bool ladder_stun_setter { set { ladder_stun = value; } }
    bool remote_mov = false;
    public bool ch_remote_control { set { remote_mov = value; } }
    Vector2 remote_mov_v2;
    public Vector2 Remote_mov_v2_s { set { remote_mov_v2 = value; } }
    Vector2 last_checkpoint;
    int last_checkpoint_priority;
    public bool dead = false;
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
        default_speed = speed;
        last_checkpoint = rigidbody2d.position;
        last_checkpoint_priority = -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (dead)
        {
            rigidbody2d.MovePosition(last_checkpoint);
            dead = false;
            return;
        }
        in_flight_last_frame_processed = true;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = 0.0f;
        Vector2 prw_position = rigidbody2d.position;
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookDirection.Set(horizontal, 0);
            lookDirection.Normalize();
        }
        animator.SetFloat("Move X", lookDirection.x);
        if(changed_speed && speed != default_speed)
        {
            speed = default_speed;
        }
        if (on_ladder && y_speed <= 0.0F)
        {
            if (Input.GetButton("Jump"))
            {
                y_speed += jump_height;
                in_flight = true;                
            }
            vertical = Input.GetAxis("Vertical");
            if (ladder_stun)
            {
                if (vertical > 0)
                {
                    vertical = 0;
                }
                if (vertical < 0)
                {
                    ladder_stun = false;
                }

            }
            Vector2 mov = new Vector2(horizontal, vertical);
            rigidbody2d.MovePosition(prw_position + mov * speed * Time.fixedDeltaTime);
            return;
        }
        if (!in_flight && Input.GetButton("Jump") && y_speed < jump_height / 2.0F)
        {
            y_speed += jump_height;
            in_flight = true;
        }
        else if (y_speed > 0)
        {
            y_speed -= y_speed_attenuation_by_time;
        }
        vertical = -gravity + y_speed;
        Vector2 next_position = new Vector2(prw_position.x + horizontal * Time.fixedDeltaTime * speed, prw_position.y + vertical * Time.fixedDeltaTime);
        if (remote_mov)
        {
            next_position += remote_mov_v2;
        }
        rigidbody2d.MovePosition(next_position);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (in_flight_last_frame_processed)
        {
            in_flight = true;
            in_flight_last_frame_processed = false;
        }
        //in_flight = true;
        foreach (ContactPoint2D point in collision.contacts)
        {
            
            if (Mathf.Approximately(point.normal.y, 1.0F))
            {
                in_flight = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.collider.name == "Tilemap")
        //{
        //    in_flight = true;
        //}
        in_flight = true; // (lol?)
        if(collision.collider.GetComponent<MovPlatform>() != null)
        {
            remote_mov = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (Mathf.Approximately(point.normal.y, 1.0F))
            {
                if (collision.collider.GetComponent<MovPlatform>() != null)
                {
                    remote_mov = true;
                }
            }
                if (Mathf.Approximately(point.normal.y, -1.0F))
            {
                Flight_hard_reset();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KillZone>() != null)
        {
            dead = true;
        }
        CheckPoint ctl = collision.GetComponent<CheckPoint>();
        if (ctl != null)
        {
            if (ctl.priority > last_checkpoint_priority)
            {
                last_checkpoint = ctl.GetComponent<Rigidbody2D>().position;
            }
        }
    }
    private void Flight_hard_reset()
    {
        y_speed = jump_height/2;
    }

}