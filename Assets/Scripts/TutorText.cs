using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorText : MonoBehaviour
{
    public Animator animator;
    //private void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            animator.SetBool("Active", true);
        }
            
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            animator.SetBool("Active", false);
        }

    }
}
