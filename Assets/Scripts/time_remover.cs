using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time_remover : MonoBehaviour
{
    public float time_to_add;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        if (ctl != null)
        {
            ctl.Add_time(time_to_add);
            Destroy(gameObject);
        }
            
    }
}
