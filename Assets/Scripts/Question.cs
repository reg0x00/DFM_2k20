using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    // Start is called before the first frame update
    public int FolderNuber;
    public List<Sprite> Keys_Sprt;
    public List<KeyCode> KeysCodes;
    private string FolderPath = "Assets/Resources/Equations";
    private GameObject init_eq;
    private GameObject head_eq;
    private GameObject Block;
    private GameObject PanelMsg;
    private GameObject MainHer;
    private Sprite HeadEqSprite;
    private List<Sprite> Anws = new List<Sprite>();
    private List<GameObject> ActivEntry = new List<GameObject>();
    private GameObject EntryPaernt;
    void Start()
    {
        FolderPath = System.IO.Path.Combine(FolderPath, FolderNuber.ToString());
        init_eq = gameObject.transform.Find("Equ").Find("Eq Viewport").Find("Content").Find("Equation").gameObject;
        //init_eq = GameObject.Find("Equation");
        head_eq = gameObject.transform.Find("Equ").Find("HeadEquation").gameObject;
        //head_eq = GameObject.Find("HeadEquation");
        EntryPaernt = gameObject.transform.Find("Equ").Find("Eq Viewport").Find("Content").gameObject;
        //EntryPaernt = GameObject.Find("Content");
        Block = gameObject.transform.Find("Block").gameObject;
        //Block = GameObject.Find("Block");
        PanelMsg = GameObject.Find("PanelMessage");
        MainHer = GameObject.Find("MainCharacter");
        init_eq.SetActive(false);
        FolderPath = System.IO.Directory.GetDirectories(FolderPath)[Random.Range(0, System.IO.Directory.GetDirectories(FolderPath).Length)];
        SetAnwsers();
        head_eq.GetComponent<SpriteRenderer>().sprite = HeadEqSprite;
        for (int i = 0; i < 4; i++)
        {
            GameObject element = Instantiate(init_eq);
            ActivEntry.Add(element);
            element.GetComponent<SpriteRenderer>().sprite = Anws[i];
            element.transform.SetParent(EntryPaernt.transform);
            element.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().sprite = Keys_Sprt[i];
            element.SetActive(true);
        }
    }
    private void CheckAnwser(int number)
    {
        if (PanelMsg.transform.Find("Text").GetComponent<Text>().IsActive())
        {
            return;
        }
        if (Anws[number].name == "a")
        {
            gameObject.transform.Find("Equ").GetComponent<Collider2D>().enabled=false;
            Block.GetComponent<SpriteRenderer>().enabled = false;
            Block.GetComponent<Collider2D>().enabled = false;
            PanelMsg.transform.Find("Text").GetComponent<Text>().text = "Верно!";
            PanelMsg.GetComponent<Animator>().SetTrigger("OK");
            return;
        }
        PanelMsg.transform.Find("Text").GetComponent<Text>().text = "не верно";
        PanelMsg.GetComponent<Animator>().SetTrigger("Not_ok");
        MainHer.GetComponent<PlayerController_v3>().Add_Health(-1);        
        if(!(MainHer.GetComponent<PlayerController_v3>().GetHealth > 0))
        {
            GameObject.Find("MenuCtl").GetComponent<PauseUI>().Defeat();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void SetAnwsers()
    {
        Sprite r_eq = null;
        foreach (var i in System.IO.Directory.GetFiles(FolderPath))
        {
            if (!i.Contains(".meta"))
            {
                if (System.IO.Path.GetFileNameWithoutExtension(i).Equals("a"))
                {
                    r_eq = Resources.Load<Sprite>(System.IO.Path.ChangeExtension(i, null).Replace("Assets/Resources/", ""));
                    continue;
                }
                //Debug.Log(System.IO.Path.GetFileNameWithoutExtension(i));
                if (System.IO.Path.GetFileNameWithoutExtension(i).Equals("e"))
                {
                    HeadEqSprite = Resources.Load<Sprite>(System.IO.Path.ChangeExtension(i, null).Replace("Assets/Resources/", ""));
                    continue;
                }
                Anws.Add(Resources.Load<Sprite>(System.IO.Path.ChangeExtension(i, null).Replace("Assets/Resources/", "")));
            }
        }
        int n = Anws.Count;
        while (n > 1)
        {
            n--;
            int change_id = Random.Range(0, n + 1);
            var tmp = Anws[change_id];
            Anws[change_id] = Anws[n];
            Anws[n] = tmp;
        }
        Anws.Insert(0, r_eq);
        while (Anws.Count > 4)
        {
            Anws.RemoveAt(Random.Range(1, Anws.Count));
        }
        n = Anws.Count;
        while (n > 1)
        {
            n--;
            int change_id = Random.Range(0, n + 1);
            var tmp = Anws[change_id];
            Anws[change_id] = Anws[n];
            Anws[n] = tmp;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "MainCharacter")
        {
            for (int i = 0; i < 4; i++)
            {
                if (Input.GetKey(KeysCodes[i]))
                {
                    CheckAnwser(i);
                }
            }
        }
    }
}
