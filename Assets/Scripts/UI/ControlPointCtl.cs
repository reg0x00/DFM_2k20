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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PauseCtl.EtapPass(collision, passedEtap);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
