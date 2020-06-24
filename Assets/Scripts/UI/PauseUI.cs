using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject MenuCnv;
    public GameObject BkgCnv;
    // Start is called before the first frame update
    void Awake()
    {
        CangeMenuStatus(false);
    }
    private void FixedUpdate()
    {
        if (Input.GetKey("escape") && Time.timeScale != 0)
        {
            CangeMenuStatus(true);
        }
    }
    public void ReturnBtn()
    {
        CangeMenuStatus(false);
    }
    public void ToMainMenuBtn()
    {
        CangeMenuStatus(false);
        SceneManager.LoadScene("Main menu", LoadSceneMode.Single);
    }
    private void CangeMenuStatus(bool status)
    {
        Time.timeScale = Convert.ToInt32(!status);
        MenuCnv.SetActive(status);
        BkgCnv.SetActive(status);
    }
}
