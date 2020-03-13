using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour
{
    float last_exit_time = 0.0F;
    float anti_jitter_cooldown = 0.2F;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            if ((Time.fixedTime - last_exit_time) < anti_jitter_cooldown)
            {
                ctl.ladder_stun_setter = true;
            }
            ctl.ladder_set = true;
            ctl.y_speed_set = 0.0f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            ctl.ladder_stun_setter = false;
            ctl.ladder_set = false;
            last_exit_time = Time.fixedTime;
        }
    }
}
