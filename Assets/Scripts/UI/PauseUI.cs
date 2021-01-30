using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseUI : MonoBehaviour
{
    public GameObject MenuCnv;
    public GameObject BkgCnv;
    public GameObject FinalCnv;
    public GameObject ResCnv;
    public string EtapHeadTrack;
    private const string FinalTxt = "Поздравляю, вы сдали экзамен!\n Ваш результат : {0}";
    private GameObject init_entry;
    private List<string> EntryKeys;
    ScoreCnt ScoreTableCnt;
    // Start is called before the first frame update
    void Awake()
    {
        ResCnv = GameObject.Find("ResultsCanvas");
        init_entry = GameObject.Find("Entry");
        init_entry.SetActive(false);
        ResetAllWindows();
        ScoreTableCnt = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();


        //FillResTable();
    }
    private void Update()
    {        
        if (Input.GetKeyDown("escape") && Time.timeScale != 0)
        {
            CangeMenuStatus(true);
            return;
        }
        if (Input.GetKeyDown("escape") && MenuCnv.activeSelf)
        {
            CangeMenuStatus(false);
            return;
        }
    }
    public void ReturnBtn()
    {
        CangeMenuStatus(false);
    }
    public void ToMainMenuBtn()
    {
        ResetAllWindows();
        SceneManager.LoadScene("Main menu", LoadSceneMode.Single);
    }
    private void CangeMenuStatus(bool status)
    {
        Time.timeScale = Convert.ToInt32(!status);
        MenuCnv.SetActive(status);
        BkgCnv.SetActive(status);
    }
    private void ResetAllWindows()
    {
        Time.timeScale = 1;
        MenuCnv.SetActive(false);
        BkgCnv.SetActive(false);
        FinalCnv.SetActive(false);
        ResCnv.SetActive(false);
    }

    public void FinalViaCollider(Collider2D collision)
    {
        Time.timeScale = 0;
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        FinalCnv.GetComponentInChildren<Text>().text = String.Format(FinalTxt,ctl.GetTimePlayed.ToString("F2"));
        FinalCnv.SetActive(true);
        BkgCnv.SetActive(true);
        string FinalEtap = GameObject.Find("Collectibles").GetComponent<Collectibles_ctl>().GetLastEtap;
        ScoreTableCnt.UpdateValueIfGreatherViaInnerKeys(ctl.GetTimePlayed,FinalEtap);
    }
    public void DisplayResTable(bool status)
    {
        BkgCnv.SetActive(status);
        if (status)
        {
            FillResTable();
        }
    }
    private void ResrtResTable()
    {



    }
    private void FillResTable()
    {
        //GameObject el =Instantiate(init_entry);
        //el.transform.SetParent(GameObject.Find("Content").transform);
        //el.SetActive(true);
        EntryKeys = new List<string>(init_Head());
        EntryKeys.Add(ScoreTableCnt.GetSumKey);
        string Scene = SceneManager.GetActiveScene().name;
        List<string> users=ScoreTableCnt.GetSortedUsers(Scene);
        Dictionary<string, Dictionary<string, string>> data = ScoreTableCnt.GetResultsByScene(Scene);
        foreach (var us in users)
        {
            AddEntry(us, data[us], EntryKeys);
        }
    }
    private List<string> init_Head()
    {
        GameObject HeadPnl = GameObject.Find("HeadPanel");
        Queue<string> EtapsName = new Queue<string>(ScoreTableCnt.GetEtaps);        
        for (int i = 0; i < HeadPnl.transform.childCount; i++)
        {
            if (HeadPnl.transform.GetChild(i).name.Contains(EtapHeadTrack))
                HeadPnl.transform.GetChild(i).GetComponentInChildren<Text>().text = EtapsName.Dequeue();
        }
        if(EtapsName.Count != 0)
        {
            Debug.LogError("Not enough EtapHeads");
        }
        return ScoreTableCnt.GetEtaps;
    }
    private void AddEntry(string user, Dictionary<string, string> ent, List<string> Fields)
    {

        Debug.Log(user);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
