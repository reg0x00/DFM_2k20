using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MainUI : MonoBehaviour
{
    public GameObject MainCnv;
    public GameObject LvlCnv;
    public GameObject EnterNameCnv;
    public GameObject EnterNameNext;
    public GameObject CollisionNameCanv;
    public GameObject ResultsCanv;
    ScoreCnt ScoreTable;
    private KeyCode ExitKey;
    private string SelectedScene;
    private ResultsCanvasCtrl ResCnvCtl;
    public Text TextInputField;
    private InputField NameIF;
    private void Awake()
    {
        NameIF = GameObject.Find("NameInputField").GetComponent<InputField>();
        ScoreTable = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();
        ResCnvCtl = ResultsCanv.GetComponent<ResultsCanvasCtrl>();
        ResetToStartState();
        if (Input.GetJoystickNames().Length == 0)
        {
            ExitKey = KeyCode.Escape;
        }
        else
        {
            ExitKey = KeyCode.Joystick1Button6;
        }
    }
    private void Update()
    {
        if (LvlCnv.activeSelf && Input.GetKeyDown(ExitKey))
        {
            OnBackToMnuClick();
            return;
        }
        if (!MainCnv.activeSelf && !LvlCnv.activeSelf && EnterNameCnv.activeSelf)
        {
            if (Input.GetKeyDown(ExitKey))
            {
                BackToLvls();
                return;
            }
            if (EnterNameNext.activeSelf && Input.GetButtonDown("Submit"))
            {
                CheckName();
                return;
            }
        }
        if (EnterNameCnv.activeSelf)
        {
            GetInputFromEnterName();
            return;
        }
        if (CollisionNameCanv.activeSelf && Input.GetKeyDown(ExitKey))
        {
            BackInNameFromCollisionWarn();
            return;
        }
        if(ResCnvCtl.VisibleState && Input.GetKeyDown(ExitKey))
        {
            LvlCnv.SetActive(true);
            ResCnvCtl.SetVisibility(false);
            return;
        }
    }
    private void BackToLvls()
    {
        MainCnv.SetActive(false);
        LvlCnv.SetActive(true);
        EnterNameCnv.SetActive(false);
    }
    private void OnBackToMnuClick()
    {
        ResetToStartState();
    }
    public void OnPlayClick()
    {
        MainCnv.SetActive(false);
        LvlCnv.SetActive(true);
    }
    public void OnLoadLvl(string scene)
    {
        SelectedScene = scene;
        LvlCnv.SetActive(false);
        EnterNameCnv.SetActive(true);
        EnterNameNext.SetActive(false);
        NameIF.Select();
    }
    public void CheckName()
    {
        ScoreTable.SetActiveKeys(TextInputField.text, SelectedScene);
        if (!ScoreTable.HaveUname(TextInputField.text, SelectedScene))
        {
            ScoreTable.AddName(TextInputField.text, SelectedScene);            
            LoadLvl();
        }
        else
        {
            EnterNameCnv.SetActive(false);
            CollisionNameCanv.SetActive(true);
        }
    }
    public void LoadLvl()
    {
        SceneManager.LoadScene(SelectedScene, LoadSceneMode.Single);
    }
    public void BackInNameFromCollisionWarn()
    {
        CollisionNameCanv.SetActive(false);
        EnterNameCnv.SetActive(true);
    }
    public void OnOptionsClick()
    {

    }
    public void OnExitClick()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Debug.LogError("Exit error");
        Application.Quit();
    }
    private void ResetToStartState()
    {
        MainCnv.SetActive(true);
        LvlCnv.SetActive(false);
        EnterNameCnv.SetActive(false);
        EnterNameNext.SetActive(false);
        CollisionNameCanv.SetActive(false);
        ResCnvCtl.SetVisibility(false);
    }
    public void GetInputFromEnterName()
    {
        if(TextInputField.text.Length >= 3)
        {
            EnterNameNext.SetActive(true);
        }
        else
        {
            EnterNameNext.SetActive(false);
        }
    }
    public void DisplayScoresTable(ButtonTableEtapsTagHolder holder)
    {
        ResCnvCtl.FillResTableByEtaps(holder.Etpas,holder.SceneToDisplay);
        LvlCnv.SetActive(false);
        ResCnvCtl.SetVisibility(true);
    }
}
