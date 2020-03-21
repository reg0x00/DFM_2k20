using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC2 : MonoBehaviour
{
    public float dlg_stay;
    public float print_up_delay;
    public Canvas canv0;
    public Canvas canv1;
    public Canvas canv2;
    public Canvas canv3;
    Animator a0;
    Animator a1;
    Animator a2;
    Animator a3;
    Animator animator;
    bool pass = false;
    float TTD_timer;
    float print_box_timer;
    Animator[] case1;
    Animator[] case2;
    Animator[] out_boxes;
    private void Awake()
    {
        a0 = canv0.GetComponent<Animator>();
        a1 = canv1.GetComponent<Animator>();
        a2 = canv2.GetComponent<Animator>();
        a3 = canv3.GetComponent<Animator>();
        case1 = new Animator[] { a0, a1 };
        case2 = new Animator[] { a0, a2, a3 };        
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (TTD_timer > 0)
        {
            if (GameObject.FindGameObjectsWithTag(Collectibles_ctl.instance.ID_tag).Length == 0)
            {
                out_boxes =case2;
                pass = true;                                
            }
            else
            {
                out_boxes =case1;
            }
            TTD_timer -= Time.fixedDeltaTime;
            Print_box();
        }
        else if(out_boxes != null)
        {
            Reset_all_boxes();
            if (pass)
            {
                animator.SetBool("Pass", true);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            if (!pass)
            {
                TTD_timer = dlg_stay;
            }
        }
    }
    private void Reset_all_boxes()
    {
        foreach(Animator box in out_boxes)
        {
            box.SetBool("Activate", false);            
        }        
    }
    private void Print_box()
    {
        if (out_boxes[out_boxes.Length - 1].GetBool("Activate"))
        {
            return;
        }
        if (print_box_timer <= 0)
        {
            foreach(Animator ani in out_boxes)
            {
                if (!ani.GetBool("Activate"))
                {
                    ani.SetBool("Activate", true);                    
                    break;
                }
                ani.SetTrigger("Up");
            }
            print_box_timer = print_up_delay;
        }
        else
        {
            print_box_timer -=Time.fixedDeltaTime;
        }
    }
}
