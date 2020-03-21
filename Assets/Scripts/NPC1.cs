using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour
{
    public Text NPC1_txt_out;
    public float time_to_print_dlg;
    public float print_delay;
    float print_delay_timer;
    const string d1 = "- Где ваша \n карточка?\n- ... ";
    const string d2 = "- Где ваша \n карточка?\n- вот!\n- проходите ";
    string out_str="";
    float TTD_timer = 0;
    bool pass = false;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        print_delay_timer = print_delay;
    }
    private void FixedUpdate()
    {
        if (TTD_timer > 0)
        {
            if (GameObject.FindGameObjectsWithTag(Collectibles_ctl.instance.ID_tag).Length == 0)
            {
                //NPC1_txt_out.text = d2;
                print_text(d2);                
                pass = true;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                //NPC1_txt_out.text = d1;
                print_text(d1);
            }
            TTD_timer -= Time.fixedDeltaTime;
        }
        else
        {
            //NPC1_txt_out.text = "";
            print_text("");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            if (!pass)
            {
                TTD_timer = time_to_print_dlg;
            }
        }
    }
    private void print_text(string text)
    {
        if (text.Length != (1+ out_str.Length))
        {
            if (print_delay_timer <= 0)
            {
                if (text.Equals(""))
                {
                    out_str = "";
                }
                else
                {
                    out_str = text.Remove(out_str.Length + 1);
                }
                NPC1_txt_out.text = out_str;
                print_delay_timer = print_delay;
            }
            else
            {
                print_delay_timer -= Time.fixedDeltaTime;
            }
        }
        else
        {
            if (pass)
            {
                animator.SetBool("Pass", true);
            }
        }
    }
}
