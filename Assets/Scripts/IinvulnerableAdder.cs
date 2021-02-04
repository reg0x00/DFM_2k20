using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IinvulnerableAdder : MonoBehaviour
{
    public float Invulnerable_time;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            ctl.SetIinvulnerableTime = Time.fixedTime+Invulnerable_time;
            Destroy(gameObject);
        }
    }
}
