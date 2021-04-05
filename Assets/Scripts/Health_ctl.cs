using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_ctl : MonoBehaviour
{
    public static Health_ctl instance { get; private set; }
    public List<GameObject> healths;
    private void Awake()
    {
        instance = this;
        Activate_all_health();        
    }
    public void UpdateHealth(int hp)
    {
        for(int i=0;i<healths.Count;i++)
        {
            healths[i].SetActive((i + 1) > hp);
        }
    }
    private void Activate_all_health()
    {
        foreach(var img in healths)
        {
            img.SetActive(false);
        }
    }
}
