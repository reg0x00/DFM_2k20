using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointCtl : MonoBehaviour
{
    public string passedEtap;
    ScoreCnt ScoreTable;
    // Start is called before the first frame update
    void Start()
    {
        ScoreTable = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
