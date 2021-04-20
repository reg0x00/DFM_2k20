using UnityEngine;

public class Phantom : MonoBehaviour
{
    public float JumpForce;
    public float ActionCd;
    public float Speed;
    public float MovDuration;
    float mov_timer;
    float tim;
    Animator anim;
    Rigidbody2D rb;
    Vector2 lookDirection = new Vector2(1, 0);    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        tim = Time.fixedTime + Random.Range(0, ActionCd);        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        SetLookDir();
        SetAnimatorVals();
        if ((tim - Time.fixedTime) < 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                rb.AddForce(new Vector2(0, Random.Range(0, JumpForce)));
            }
            else
            {
                float f = Random.Range(-Speed, Speed);
                rb.AddForce(new Vector2(f, 0));
            }
            tim = Time.fixedTime + Random.Range(0, ActionCd);
        }
    }
    private void SetAnimatorVals()
    {
        anim.SetFloat("Move X", lookDirection.x);
        anim.SetFloat("Move X_mag", Mathf.Abs(rb.velocity.x));
        //anim.SetBool("On_ladder", turn_on_ladder_animatron);
        anim.SetFloat("Move_Y", Input.GetAxis("Vertical"));
    }
    private void SetLookDir()
    {
        if (!Mathf.Approximately(rb.velocity.normalized.x,0) && Mathf.Ceil(lookDirection.x) != Mathf.Ceil(rb.velocity.normalized.x)) 
        {
            lookDirection.Set(Mathf.Ceil(rb.velocity.normalized.x), 0);                    
        }
    }
}
