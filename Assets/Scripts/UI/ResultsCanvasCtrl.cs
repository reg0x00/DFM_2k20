﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsCanvasCtrl : MonoBehaviour
{
    private GameObject init_entry;
    private ScoreCnt ScoreTableCnt;
    public string EtapHeadTrack = "Eatp";
    private bool is_visible = true;
    public bool VisibleState { get { return is_visible; } }
    private List<string> EtapKeys;
    private GameObject ContentGO;
    private List<GameObject> ActivatedEntrys = new List<GameObject>();
    private void Awake()
    {
        init_entry = GameObject.Find("Entry");
        init_entry.SetActive(false);
        ContentGO = GameObject.Find("Content");
        ScoreTableCnt = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();
    }
    public void SetVisibility(bool state)
    {
        is_visible = state;
        gameObject.GetComponent<Canvas>().enabled = state;
    }
    private void ResrtTableEntrys()
    {
        foreach (var ent in ActivatedEntrys)
        {
            Destroy(ent);
        }
    }
    public void FillResTableByEtaps(List<string> InEtaps, string Scene)
    {
        ResrtTableEntrys();
        EtapKeys = new List<string>(InEtaps);
        init_Head(InEtaps);
        List<string> users = ScoreTableCnt.GetSortedUsers(Scene, EtapKeys);
        Dictionary<string, Dictionary<string, string>> data = ScoreTableCnt.GetResultsByScene(Scene, EtapKeys);
        foreach (var us in users)
        {
            AddEntry(us, data[us], EtapKeys);
        }
    }
    private void init_Head(List<string> Etaps)
    {
        GameObject HeadPnl = GameObject.Find("HeadPanel");
        Queue<string> EtapsName = new Queue<string>(Etaps);
        for (int i = 0; i < HeadPnl.transform.childCount; i++)
        {
            if (HeadPnl.transform.GetChild(i).name.Contains(EtapHeadTrack))
                HeadPnl.transform.GetChild(i).GetComponentInChildren<Text>().text = EtapsName.Dequeue();
        }
        if (EtapsName.Count != 0)
        {
            Debug.LogError("Not enough EtapHeads");
        }
    }
    private void AddEntry(string user, Dictionary<string, string> ent, List<string> Fields)
    {
        // Example of adding entry
        //GameObject el =Instantiate(init_entry);
        //el.transform.SetParent(GameObject.Find("Content").transform);
        //el.SetActive(true);
        Queue<string> EntryFields = new Queue<string>();
        EntryFields.Enqueue(user);
        foreach (var field in Fields)
        {
            EntryFields.Enqueue(ent[field]);
        }
        EntryFields.Enqueue(ent[ScoreTableCnt.GetSumKey]);
        GameObject element = Instantiate(init_entry);
        ActivatedEntrys.Add(element);
        element.transform.SetParent(ContentGO.transform);
        for (int i = 0; (i < element.transform.childCount) && (EntryFields.Count != 0); i++)
        {
            element.transform.GetChild(i).GetComponentInChildren<Text>().text = EntryFields.Dequeue();
        }
        element.SetActive(true);
    }
}
