using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour
{
    public Text NPC1_txt_out;
    public float time_to_print_dlg;
    string d1 = "- Где ваша \n карточка?\n- ...";
    string d2 = "- Где ваша \n карточка?\n- вот!\n- проходите";
    float TTD_timer = 0;
    bool pass = false;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (TTD_timer > 0)
        {
            if (GameObject.FindGameObjectsWithTag(Collectibles_ctl.instance.ID_tag).Length == 0)
            {
                NPC1_txt_out.text = d2;
                animator.SetBool("Pass",true);
                pass = true;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                NPC1_txt_out.text = d1;
            }
            TTD_timer -= Time.fixedDeltaTime;
        }
        else
        {
            NPC1_txt_out.text = "";
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
}
