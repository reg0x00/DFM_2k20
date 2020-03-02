using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //movement vars
    public float x_acceleration=50;
    public float jump_Impulse=6;
    public float max_horizontal_speed=8;
    bool in_flight=false;
    public float x_stop_intensity=0.08f;
    public bool on_ladder = false;
    float default_gravityScale;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    // Start is called before the first frame update
    void Start()
    {
         //QualitySettings.vSyncCount = 0;
         //Application.targetFrameRate = 30;  
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        default_gravityScale = rigidbody2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(1 / Time.deltaTime);
        //Debug.Log(Mathf.Abs(rigidbody2d.velocity[0])); // abs speed
        float horizontal = Input.GetAxis("Horizontal");        
        rigidbody2d.gravityScale = (on_ladder) ? 0 : default_gravityScale;
        if (!Mathf.Approximately(horizontal, 0.0f))
        {
            lookDirection.Set(horizontal, 0);
            lookDirection.Normalize();
        }
        animator.SetFloat("Move X", lookDirection.x);
        if (on_ladder)
        {
            return;
        }
        float x_force =(max_horizontal_speed > Mathf.Abs(rigidbody2d.velocity[0]) || (Mathf.Sign(rigidbody2d.velocity[0]) != Mathf.Sign(horizontal))) ? (horizontal * Time.deltaTime * x_acceleration) :0;
        if(Mathf.Approximately(horizontal, 0) && !in_flight)
        {
            x_force = (-rigidbody2d.velocity[0] * x_stop_intensity);
        }
        float y_force = (Input.GetButtonDown("Jump") && !in_flight) ? jump_Impulse : 0;
        rigidbody2d.AddForce(new Vector2(x_force,y_force),ForceMode2D.Impulse);

          
    }
    private void FixedUpdate()
    {
        if (on_ladder)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 move = new Vector2(horizontal, vertical);
            Vector2 position = rigidbody2d.position;
            position += move * 10.0f * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("contact");
        if (collision.collider.name == "Tilemap")
        {
            //Debug.Log(collision.contactCount);
            in_flight = false;
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
