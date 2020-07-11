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
    public Text InputField;
    ScoreCnt ScoreTable;
    private string SelectedScene;
    private void Awake()
    {
        ScoreTable = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();
        ResetToStartState();
    }
    private void Update()
    {
        if (LvlCnv.activeSelf && Input.GetKeyDown("escape"))
        {
            OnBackToMnuClick();
            return;
        }
        if (!MainCnv.activeSelf && !LvlCnv.activeSelf && EnterNameCnv.activeSelf && Input.GetKeyDown("escape"))
        {
            BackToLvls();
            return;
        }
        if (EnterNameCnv.activeSelf)
        {
            GetInputFromEnterName();
            return;
        }
        if(CollisionNameCanv.activeSelf && Input.GetKeyDown("escape"))
        {
            BackInNameFromCollisionWarn();
            return;
        }
        if(ResultsCanv.activeSelf && Input.GetKeyDown("escape"))
        {
            LvlCnv.SetActive(true);
            ResultsCanv.SetActive(false);
            ResultsCanv.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = "";
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
    }
    public void CheckName()
    {
        ScoreTable.SetActiveKeys(InputField.text, SelectedScene);
        if (!ScoreTable.HaveUname(InputField.text, SelectedScene))
        {
            ScoreTable.AddName(InputField.text, SelectedScene);            
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    private void ResetToStartState()
    {
        MainCnv.SetActive(true);
        LvlCnv.SetActive(false);
        EnterNameCnv.SetActive(false);
        EnterNameNext.SetActive(false);
        CollisionNameCanv.SetActive(false);
        ResultsCanv.SetActive(false);
    }
    public void GetInputFromEnterName()
    {
        if(InputField.text.Length == 3)
        {
            EnterNameNext.SetActive(true);
        }
        else
        {
            EnterNameNext.SetActive(false);
        }
    }
    public void DisplayScoresTable(string scene)
    {      
        ResultsCanv.GetComponentInChildren<Image>().GetComponentInChildren<Text>().text = ScoreTable.GetSortedResultsByScene(scene);
        LvlCnv.SetActive(false);
        ResultsCanv.SetActive(true);
    }
}
