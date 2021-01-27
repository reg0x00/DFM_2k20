using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject MenuCnv;
    public GameObject BkgCnv;
    public GameObject FinalCnv;    
    private const string FinalTxt = "Поздравляю, вы сдали экзамен!\n Ваш результат : {0}";
    ScoreCnt ScoreTable;
    // Start is called before the first frame update
    void Awake()
    {
        ResetAllWindows();
        ScoreTable = GameObject.Find("TableCtl").GetComponent<ScoreCnt>();



        GameObject ResTableCnv = GameObject.Find("Entry");
        GameObject.Instantiate(ResTableCnv).transform.SetParent(GameObject.Find("Content").transform);
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
    }

    public void FinalViaCollider(Collider2D collision)
    {
        Time.timeScale = 0;
        PlayerController_v3 ctl = collision.GetComponent<PlayerController_v3>();
        FinalCnv.GetComponentInChildren<Text>().text = String.Format(FinalTxt,ctl.GetTimePlayed.ToString("F2"));
        FinalCnv.SetActive(true);
        BkgCnv.SetActive(true);
        string FinalEtap = GameObject.Find("Collectibles").GetComponent<Collectibles_ctl>().GetLastEtap;
        ScoreTable.UpdateValueIfGreatherViaInnerKeys(ctl.GetTimePlayed,FinalEtap);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
