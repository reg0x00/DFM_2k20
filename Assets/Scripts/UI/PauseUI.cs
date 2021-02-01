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
    private ResultsCanvasCtrl ResCnvCtl;
    private PlayerController_v3 Playerctl;
    private const string FinalTxt = "Поздравляю, вы сдали экзамен!\n Ваш результат : {0}";
    ScoreCnt ScoreTableCnt;
    // Start is called before the first frame update
    void Awake()
    {
        ResCnvCtl = GameObject.Find("ResultsCanvas").GetComponent<ResultsCanvasCtrl>();        
        ScoreTableCnt = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();        
        ResetAllWindows();
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
        if (Input.GetKeyDown("escape") && ResCnvCtl.VisibleState)
        {
            SetNextEtap();
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
    private void BkgAndTimeStop(bool status)
    {
        Time.timeScale = Convert.ToInt32(!status);
        BkgCnv.SetActive(status);
    }
    private void ResetAllWindows()
    {
        Time.timeScale = 1;
        MenuCnv.SetActive(false);
        BkgCnv.SetActive(false);
        FinalCnv.SetActive(false);
        ResCnvCtl.SetVisibility(false);
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
    public void EtapPass(Collision2D collision,string Etap)
    {
        BkgAndTimeStop(true);
        Playerctl = collision.collider.GetComponent<PlayerController_v3>();
        ScoreTableCnt.UpdateValueIfGreatherViaInnerKeys(Playerctl.GetTimePlayed, Etap);
        ResCnvCtl.FillResTableByEtaps(GetEtapsID(), SceneManager.GetActiveScene().name);
        ResCnvCtl.SetVisibility(true);
    }
    public void SetNextEtap()
    {
        Playerctl.ResetTime();
        BkgAndTimeStop(false);
        ResCnvCtl.SetVisibility(false);
    }
    public void DisplayResTableAndAddEntr(string etap)
    {
        BkgCnv.SetActive(true);
        ResCnvCtl.FillResTableByEtaps(GetEtapsID(), SceneManager.GetActiveScene().name);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private List<string> GetEtapsID()
    {
        List<string> tmplst = new List<string>();
        GameObject ColCtl = GameObject.Find("Collectibles");
        Collectibles_ctl Collectibles_ctl_Targets = ColCtl.GetComponent<Collectibles_ctl>();
        foreach (string ID in Collectibles_ctl_Targets.EtapsID)
        {
            tmplst.Add(ID);
        }
        return tmplst;
    }
}
