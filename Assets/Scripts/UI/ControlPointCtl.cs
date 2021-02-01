using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointCtl : MonoBehaviour
{
    public string passedEtap;
    private PauseUI PauseCtl; 
    // Start is called before the first frame update
    void Start()
    {
        PauseCtl = GameObject.Find("MenuCtl").GetComponent<PauseUI>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameObject.Find("Collectibles").GetComponent<Collectibles_ctl>().IsEtapComplete(passedEtap))
        {
            PauseCtl.EtapPass(collision, passedEtap);
            gameObject.SetActive(false);
        }
    }
}
