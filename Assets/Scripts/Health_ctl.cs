using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_ctl : MonoBehaviour
{
    public static Health_ctl instance { get; private set; }
    public Image[] healths=new Image[7];
    private void Awake()
    {
        instance = this;
        Activate_all_health();
    }
    public void UpdateHealth(int hp)
    {
        for(int i=0;i<healths.Length;i++)
        {
            if ((i + 1) > hp)
            {
                healths[i].enabled = true;
            }
            else
            {
                healths[i].enabled = false;
            }
        }
    }
    private void Activate_all_health()
    {
        foreach(Image img in healths)
        {
            img.enabled = false;
        }
    }
}
