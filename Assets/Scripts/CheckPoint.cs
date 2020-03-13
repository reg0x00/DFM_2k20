using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator animator;
    public int priority;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController_v3>() != null)
        {
            animator.SetBool("Active", true);
        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
