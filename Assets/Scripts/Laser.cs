using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float ON_time;
    public float OFF_time;
    public bool init_state_eq_ON;
    private bool now_state;
    private float flip_time;
    // Start is called before the first frame update
    void Start()
    {
        now_state = init_state_eq_ON;
        ChangeState();
        flip_time = init_state_eq_ON ? ON_time : OFF_time;
    }

    private void FixedUpdate()
    {
        flip_time -= Time.deltaTime;
        if (flip_time <= 0)
        {
            flip_time = now_state ? ON_time : OFF_time;
            ChangeState();
        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
    void ChangeState()
    {
        GetComponent<BoxCollider2D>().enabled = now_state;
        GetComponent<SpriteRenderer>().enabled = now_state;
        now_state = !now_state;
    }
}
