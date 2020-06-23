using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public GameObject MainCnv;
    public GameObject LvlCnv;
    private void Start()
    {
        MainCnv.SetActive(true);
        LvlCnv.SetActive(false);

    }
    public void OnPlayClick()
    {
        MainCnv.SetActive(!MainCnv.activeSelf);
        LvlCnv.SetActive(!LvlCnv.activeSelf);
    }
    public void LoadLvl(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
    public void OnOptionsClick()
    {

    }
    public void OnExitClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
