using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    public float JumpForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Time.fixedTime % 2 == 0)
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce));
    }
}
