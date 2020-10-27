using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_v3 : MonoBehaviour
{
    public float jump_height;
    public float gravity;
    public float y_speed_attenuation_by_time;
    public float speed;
    float speed_boost_time = 0;
    public float set_speed_boost_time { set { speed_boost_time = value; } }
    public bool ladder_set { set { on_ladder = value; } }
    bool on_ladder = false;
    float default_speed;
    float y_speed;
    Vector2 prw_pos = new Vector2(0, 0);
    public float y_speed_set { set { y_speed = value; } }
    bool in_flight = true;
    public bool get_flight_status { get { return in_flight; } }
    bool in_flight_last_frame_processed = true; // in case, when several colliders involved
    bool ladder_stun = false;
    public bool ladder_stun_setter { set { ladder_stun = value; } }
    bool remote_mov = false;
    public bool ch_remote_control { set { remote_mov = value; } }
    Vector2 remote_mov_v2;
    public Vector2 Remote_mov_v2_s { set { remote_mov_v2 = value; } }
    bool high_priority; // for remote synchronization 
    public bool set_high_pri { set { high_priority = value; } }
    float game_timer_offset = 0.0F;
    //public float set_player_game_timer_offset { set { game_timer_offset = value; } }
    float last_FixedUpdate_time;
    public float get_last_FU_time { get { return last_FixedUpdate_time; } }
    bool remote_next_frame_is_flip = false; // for high priority of charter
    public bool remote_NF_flip { set { remote_next_frame_is_flip = value; } }
    private float TimePlayed=0.0f;
    public float GetTimePlayed { get { return TimePlayed; } }
    Vector2 last_checkpoint;
    int last_checkpoint_priority;
    bool dead = false;
    int health;
    public int max_health;
    bool health_is_full;
    public bool check_max_hp_status { get { return health_is_full; } }
    private float drag_dir = 0;
    public float set_drag_dir { set { drag_dir = value; } }
    bool drag = false;
    public bool drag_status_set { set { drag = value; } }
    float drag_dbf;
    public float drag_dbf_change { set { drag_dbf = value; } }
    public Vector2 mov_pos;
    public Vector2 char_mov { get { return mov_pos; } }  // w/o prw_pos
    public int Fly_animation_cooldown_frames = 25; // default : 50 FUPS
    private int Fly_animation_cooldown_frames_cnt;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;
    Rigidbody2D rigidbody2d;
    public Text Timer;
    public Animator TimerAnimator;
    // Start is called before the first frame update
    private void Awake()
    {
        game_timer_offset = Time.fixedTime;
    }
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;  
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        default_speed = speed;
        last_checkpoint = rigidbody2d.position;
        last_checkpoint_priority = -1;
        health = max_health;
        health_is_full = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
    }
    private void FixedUpdate()
    {
        last_FixedUpdate_time = Time.fixedTime;
        TimePlayed= (last_FixedUpdate_time - game_timer_offset);
        Timer.text = TimePlayed.ToString("F2");
        if (dead)
        {
            speed_boost_time = 0;
            rigidbody2d.position = last_checkpoint;
            dead = false;
            Add_Health(-1);            
            return;
        }
        in_flight_last_frame_processed = true;
        if (in_flight && rigidbody2d.position.y == prw_pos.y && Mathf.Approximately(rigidbody2d.position.y, 0))
        {
            Flight_hard_reset();
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = 0.0f;
        Vector2 prw_position = rigidbody2d.position;
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookDirection.Set(horizontal, 0);
            lookDirection.Normalize();         
        }
        if (in_flight)
            drag = false;
        animator.SetFloat("Move X", lookDirection.x);
        animator.SetFloat("Move X_mag",Mathf.Abs(horizontal));
        animator.SetBool("On_ladder", on_ladder);
        animator.SetFloat("Move_Y", Input.GetAxis("Vertical"));
        animator.SetBool("Is_dragging", drag);
        animator.SetFloat("Drag_dir", drag_dir);
        if (!in_flight && Input.GetButton("Jump")) // fix animation at holding jump btn
        {
            if (Fly_animation_cooldown_frames_cnt == 0)
            {
                animator.SetBool("Jump", in_flight);
            }
            else
            {
                Fly_animation_cooldown_frames_cnt--;
            }
        }
        else
        {
            animator.SetBool("Jump", in_flight);
            Fly_animation_cooldown_frames_cnt = Fly_animation_cooldown_frames;
        }
        if (speed_boost_time > 0)
        {
            speed_boost_time -= Time.fixedDeltaTime;
        }
        else if (default_speed != speed)
        {
            speed = default_speed;
        }        
        if (on_ladder && y_speed <= 0.0F)
        {
            animator.SetBool("Jump", false);
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
        if (drag)
        {
            horizontal *= drag_dbf;
            mov_pos = new Vector2(horizontal * speed * Time.fixedDeltaTime, vertical * Time.fixedDeltaTime);
        }
        Vector2 next_position = new Vector2(prw_position.x + horizontal * Time.fixedDeltaTime * speed, prw_position.y + vertical * Time.fixedDeltaTime);
        if (remote_mov)
        {
            if (high_priority && remote_next_frame_is_flip)
            {
                remote_mov_v2 *= -1;
            }
            next_position += remote_mov_v2;
        }
        if (in_flight)
        {
            prw_pos = rigidbody2d.position;
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
                if (collision.collider.GetComponent<MovPlatform>() != null)
                {
                    remote_mov = true;
                }
            }
            if (Mathf.Approximately(point.normal.y, 1.0F))
            {
                in_flight = false;
                y_speed = 0;
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
        if (collision.collider.GetComponent<MovPlatform>() != null)
        {
            remote_mov = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Spikes")
        {
            dead = true;
        }
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (Mathf.Approximately(point.normal.y, -1.0F))
            {
                Flight_hard_reset();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        y_speed = jump_height / 2;
    }
    public void Add_time(float x)
    {
        game_timer_offset -= x;
        TimerAnimator.SetTrigger("AddTimeTrg");
    }
    public void Add_Health(int x)
    {
        health += x;
        health_is_full = (health==max_health);
        Health_ctl.instance.UpdateHealth(health);
    }

}