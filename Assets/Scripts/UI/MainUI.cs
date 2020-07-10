using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public GameObject MainCnv;
    public GameObject LvlCnv;
    public GameObject EnterNameCnv;
    public GameObject EnterNameNext;
    public GameObject CollisionNameCanv;
    public Text InputField;
    ScoreCnt ScoreTable;
    private string SelectedScene;
    private string Uname;
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
        }
        if (!MainCnv.activeSelf && !LvlCnv.activeSelf && EnterNameCnv.activeSelf && Input.GetKeyDown("escape"))
        {
            BackToLvls();
        }
        if (EnterNameCnv.activeSelf)
        {
            GetInputFromEnterName();
        }
        if(CollisionNameCanv.activeSelf && Input.GetKeyDown("escape"))
        {
            BackInNameFromCollisionWarn();
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
        Uname = InputField.text;
        if (!ScoreTable.HaveUname(Uname, SelectedScene))
        {
            ScoreTable.AddName(Uname, SelectedScene);            
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
}
