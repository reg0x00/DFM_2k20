using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    public float JumpForce;
    public float MaxMovRange;
    public float ActionCd;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {        

        if (Time.fixedTime % 2 == 0)
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Random.Range(0,JumpForce)));
    }
}
