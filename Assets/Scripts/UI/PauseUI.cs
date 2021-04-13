using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseUI : MonoBehaviour
{
    public GameObject MenuCnv;
    public GameObject BkgCnv;
    //public GameObject FinalCnv;
    //private ResultsCanvasCtrl ResCnvCtl;
    private GameObject AboutPanel;
    private GameObject DefeatPanel;
    private GameObject init_eq;
    private GameObject head_eq;
    private GameObject EntryPaernt;
    private GameObject PanelMsg;
    private GameObject EquGo;
    private GameObject FinalCnv;
    private Question LastCalledQ;
    public List<Sprite> Keys_Sprt;
    private PlayerController_v3 Playerctl;
    public List<KeyCode> KeysCodes;
    private List<GameObject> ActivEntry = new List<GameObject>();
    private bool defeat = false;
    private const string FinalTxt = "Поздравляю, вы сдали экзамен!\n Ваш результат : {0}";
    //ScoreCnt ScoreTableCnt;
    // Start is called before the first frame update
    void Awake()
    {
        //ResCnvCtl = GameObject.Find("ResultsCanvas").GetComponent<ResultsCanvasCtrl>();        
        //ScoreTableCnt = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();
        AboutPanel = GameObject.Find("About panel");
        DefeatPanel = GameObject.Find("DefeatPanel");
        //init_eq = gameObject.transform.Find("Equ").Find("Eq Viewport").Find("Content").Find("Equation").gameObject;
        init_eq = GameObject.Find("Equation");
        //head_eq = gameObject.transform.Find("Equ").Find("HeadEquation").gameObject;
        head_eq = GameObject.Find("HeadEquation");
        //EntryPaernt = gameObject.transform.Find("Equ").Find("Eq Viewport").Find("Content").gameObject;
        EntryPaernt = GameObject.Find("Content");
        EquGo = GameObject.Find("Equ");
        PanelMsg = GameObject.Find("PanelMessage");
        Playerctl = GameObject.Find("MainCharacter").GetComponent<PlayerController_v3>();
        FinalCnv = GameObject.Find("Final Cnv");
        ResetAllWindows();
    }
    private void Update()
    {
        if (defeat)
            return;
        if (Input.GetKeyDown("escape") && Time.timeScale != 0)
        {
            CangeMenuStatus(true);
            return;
        }
        if (EquGo.activeSelf)
        {
            KeyEqInput();
        }
        if (Input.GetKeyDown("escape") && MenuCnv.activeSelf)
        {
            if (AboutPanel.activeSelf)
            {
                ResetAboutWindow();
                return;
            }
            CangeMenuStatus(false);
            return;
        }
        //if (Input.GetKeyDown("escape") && ResCnvCtl.VisibleState)
        //{
        //    SetNextEtap();
        //}
    }
    public void Defeat()
    {
        defeat = true;
        Time.timeScale = Convert.ToInt32(false);
        DefeatPanel.SetActive(true);
        BkgCnv.SetActive(true);
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
    public void Zerolling()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void DisplayAboutWindowOnly()
    {
        foreach(Transform cmp in MenuCnv.transform)
        {
            cmp.gameObject.SetActive(false);
        }
        MenuCnv.SetActive(true);
        AboutPanel.SetActive(true);
    }
    public void DisplayAboutWindow()
    {
        AboutPanel.SetActive(true);
    }
    public void ResetAboutWindow()
    {
        AboutPanel.SetActive(false);
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
    private void ClearEqField()
    {
        foreach(var i in ActivEntry)
        {
            Destroy(i);
        }
        BkgAndTimeStop(false);
        EquGo.SetActive(false);
        Playerctl.stun_player_mov = false;
    }
    private void CheckAnwser(int number)
    {
        if (PanelMsg.transform.Find("Text").GetComponent<Text>().IsActive())
        {
            return;
        }
        if (ActivEntry[number].GetComponent<Image>().sprite.name == "a")
        {
            LastCalledQ.PassQuestion();
            PanelMsg.transform.Find("Text").GetComponent<Text>().text = "Верно!";
            PanelMsg.GetComponent<Animator>().SetTrigger("OK");
            ClearEqField();
            return;
        }
        PanelMsg.transform.Find("Text").GetComponent<Text>().text = "не верно";
        PanelMsg.GetComponent<Animator>().SetTrigger("Not_ok");
        Playerctl.Add_Health(-1);
        if (!(Playerctl.GetHealth > 0))
        {
            Defeat();
        }
    }
    private void ResetAllWindows()
    {
        Time.timeScale = 1;
        MenuCnv.SetActive(false);
        BkgCnv.SetActive(false);
        AboutPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        init_eq.SetActive(false);
        EquGo.SetActive(false);
        FinalCnv.SetActive(false);
        //FinalCnv.SetActive(false);
        //ResCnvCtl.SetVisibility(false);
    }
    public void DisplayEq(Sprite Head, List<Sprite> Eq, Question CalledQuestion)
    {
        if (EquGo.activeSelf)
            return;
        Playerctl.stun_player_mov = true;
        BkgCnv.SetActive(true);
        EquGo.SetActive(true);
        LastCalledQ = CalledQuestion;       
        head_eq.GetComponent<Image>().sprite = Head;
        for (int i = 0; i < 4; i++)
        {
            GameObject element = Instantiate(init_eq);
            ActivEntry.Add(element);
            element.GetComponent<Image>().sprite = Eq[i];
            element.transform.SetParent(EntryPaernt.transform);
            element.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Keys_Sprt[i];
            element.SetActive(true);
        }        
    }
    public void KeyEqInput()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(KeysCodes[i]))
            {                
                CheckAnwser(i);
            }
        }
    }

    public void FinalViaCollider(Collider2D collision)
    {
        Time.timeScale = 0;
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        //FinalCnv.GetComponentInChildren<Text>().text = String.Format(FinalTxt,ctl.GetTimePlayed.ToString("F2"));
        //FinalCnv.SetActive(true);
        BkgCnv.SetActive(true);
        FinalCnv.SetActive(true);
        //string FinalEtap = GameObject.Find("Collectibles").GetComponent<Collectibles_ctl>().GetLastEtap;
        //ScoreTableCnt.UpdateValueIfGreatherViaInnerKeys(ctl.GetTimePlayed,FinalEtap);
    }
    public void EtapPass(Collision2D collision,string Etap)
    {
        BkgAndTimeStop(true);
        //Playerctl = collision.collider.GetComponent<PlayerController_v3>();
        //ScoreTableCnt.UpdateValueIfGreatherViaInnerKeys(Playerctl.GetTimePlayed, Etap);
        //ResCnvCtl.FillResTableByEtaps(GetEtapsID(), SceneManager.GetActiveScene().name);
        //ResCnvCtl.SetVisibility(true);
    }
    public void SetNextEtap()
    {
        Playerctl.ResetTime();
        BkgAndTimeStop(false);
        //ResCnvCtl.SetVisibility(false);
    }
    public void DisplayResTableAndAddEntr(string etap)
    {
        BkgCnv.SetActive(true);
        //ResCnvCtl.FillResTableByEtaps(GetEtapsID(), SceneManager.GetActiveScene().name);
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
