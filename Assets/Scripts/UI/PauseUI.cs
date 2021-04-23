using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public List<Sprite> kb_Keys_Sprt;
    public List<Sprite> Js_Keys_Sprt;
    private List<Sprite> Keys_Sprt;
    private PlayerController_v3 Playerctl;
    public List<KeyCode> kb_keycodes;
    public List<KeyCode> Js_key_codes;    
    private List<KeyCode> KeysCodes;
    private List<GameObject> ActivEntry = new List<GameObject>();
    private List<GameObject> ActiveButtonGO = new List<GameObject>();
    private bool defeat = false;
    private KeyCode ExitKey;
    private string KeyImgName = "Key Image";
    private List<GameObject> KeyImgDisplayed = new List<GameObject>();
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
        if (Input.GetJoystickNames().Length == 0)
        {
            KeysCodes = kb_keycodes;
            Keys_Sprt = kb_Keys_Sprt;
            ExitKey = KeyCode.Joystick1Button6;
            ExitKey = KeyCode.Escape;
        }
        else
        {
            KeysCodes = Js_key_codes;
            Keys_Sprt = Js_Keys_Sprt;
            ExitKey = KeyCode.Joystick1Button6;
        }
    }
    private void Update()
    {

        if (ActiveButtonGO.Count != 0)
        {
            for (int i = 0; i < ActiveButtonGO.Count; i++)
            {
                if (Input.GetKeyDown(KeysCodes[i]))
                {
                    Button tmp_bt = ActiveButtonGO[i].GetComponent<Button>();
                    ActiveButtonGO.Clear();
                    tmp_bt.onClick.Invoke();
                }
            }
        }
        if (defeat)
            return;
        if (Input.GetKeyDown(ExitKey) && Time.timeScale != 0)
        {
            CangeMenuStatus(true);
            return;
        }
        if (EquGo.activeSelf)
        {
            KeyEqInput();
        }
        if (Input.GetKeyDown(ExitKey) && MenuCnv.activeSelf)
        {
            if (AboutPanel.activeSelf)
            {
                ResetAboutWindow();
                return;
            }
            CangeMenuStatus(false);
            return;
        }
        //if (Input.GetKeyDown(ExitKey) && ResCnvCtl.VisibleState)
        //{
        //    SetNextEtap();
        //}
    }
    void BindKeysToBtn(GameObject target)
    {
        ActiveButtonGO.Clear();
        foreach(GameObject obj in KeyImgDisplayed)
        {
            Destroy(obj);
        }
        KeyImgDisplayed.Clear();
        if (!target)
            return;

        for (int i = 0; i < target.transform.childCount; i++)
        {
            Transform BtnChildTr = target.transform.GetChild(i);
            if (BtnChildTr.GetComponent<Button>())
            {
                ActiveButtonGO.Add(BtnChildTr.gameObject);
                GameObject new_go = new GameObject();
                new_go.transform.SetParent(BtnChildTr);
                new_go.AddComponent<RectTransform>();
                new_go.AddComponent<CanvasRenderer>();
                new_go.AddComponent<Image>();
                new_go.GetComponent<Image>().sprite = Keys_Sprt[KeyImgDisplayed.Count];
                RectTransform new_rt = new_go.GetComponent<RectTransform>();
                new_rt.sizeDelta = new Vector2(BtnChildTr.GetComponent<RectTransform>().rect.height, BtnChildTr.GetComponent<RectTransform>().rect.height);
                new_rt.position = new Vector3(BtnChildTr.GetComponent<RectTransform>().position.x - BtnChildTr.GetComponent<RectTransform>().rect.width / (float)1.5, BtnChildTr.GetComponent<RectTransform>().position.y, 0);
                new_rt.name = KeyImgName;
                KeyImgDisplayed.Add(new_go);
            }
        }
    }
    public void Defeat()
    {
        defeat = true;
        Time.timeScale = Convert.ToInt32(false);
        ResetAllWindows();
        DefeatPanel.SetActive(true);
        BindKeysToBtn(DefeatPanel);
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
        foreach (Transform cmp in MenuCnv.transform)
        {
            cmp.gameObject.SetActive(false);
        }
        MenuCnv.SetActive(true);
        AboutPanel.SetActive(true);
        BindKeysToBtn(AboutPanel);
    }
    public void DisplayAboutWindow()
    {
        AboutPanel.SetActive(true);
        BindKeysToBtn(AboutPanel);
    }
    public void ResetAboutWindow()
    {
        AboutPanel.SetActive(false);
        if (FinalCnv.activeSelf)
        {
            BindKeysToBtn(FinalCnv);
        }
        else
        {
            BindKeysToBtn(MenuCnv);
        }
    }
    private void CangeMenuStatus(bool status)
    {
        Time.timeScale = Convert.ToInt32(!status);
        MenuCnv.SetActive(status);
        if (status)
        {
            BindKeysToBtn(MenuCnv);
        }
        else
        {
            BindKeysToBtn(null);
        }
        BkgCnv.SetActive(status);
    }
    private void BkgAndTimeStop(bool status)
    {
        Time.timeScale = Convert.ToInt32(!status);
        BkgCnv.SetActive(status);
    }
    private void ClearEqField()
    {
        foreach (var i in ActivEntry)
        {
            Destroy(i);
        }
        ActivEntry.Clear();
        BkgAndTimeStop(false);
        EquGo.SetActive(false);
        Playerctl.stun_player_mov = false;
    }
    private void CheckAnwser(int number)
    {
        if (PanelMsg.GetComponent<Image>().IsActive())
        {
            return;
        }
        if (ActivEntry[number].transform.Find("EqImg").GetComponent<Image>().sprite.name == "a")
        {
            LastCalledQ.PassQuestion();
            //PanelMsg.transform.Find("Text").GetComponent<Text>().text = "Верно!";
            PanelMsg.GetComponent<Animator>().SetTrigger("OK");
            ClearEqField();
            return;
        }
        //PanelMsg.transform.Find("Text").GetComponent<Text>().text = "не верно";
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
            element.transform.Find("EqImg").GetComponent<Image>().sprite = Eq[i];
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
        BindKeysToBtn(FinalCnv);
        //string FinalEtap = GameObject.Find("Collectibles").GetComponent<Collectibles_ctl>().GetLastEtap;
        //ScoreTableCnt.UpdateValueIfGreatherViaInnerKeys(ctl.GetTimePlayed,FinalEtap);
    }
    public void EtapPass(Collision2D collision, string Etap)
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
