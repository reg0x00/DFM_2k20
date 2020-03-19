using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public int health_restore;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            if (!ctl.check_max_hp_status)
            {
                ctl.Add_Health(health_restore);
                Destroy(gameObject);
            }
        }
    }
}
