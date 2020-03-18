using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdder : MonoBehaviour
{
    public float speed_boost_set_value;
    public float speed_boost_duration;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            ctl.speed = speed_boost_set_value;
            ctl.set_speed_boost_time = speed_boost_duration;
            Destroy(gameObject);
        }
    }
}
