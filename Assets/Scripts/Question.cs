using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    // Start is called before the first frame update
    public int FolderNuber;
    public int possibleanws;
    public KeyCode InteractKey;
    private string FolderName = "Equations";
    private GameObject Block;
    private GameObject PauseUIGO;
    private Sprite HeadEqSprite;
    private List<Sprite> Anws = new List<Sprite>();
    void Start()
    {
        FolderName = FolderName + "/" + FolderNuber.ToString();
        //FolderName = System.IO.Path.Combine(FolderName, FolderNuber.ToString());
        Block = gameObject.transform.Find("Block").gameObject;
        //Block = GameObject.Find("Block");        
        PauseUIGO = GameObject.Find("MenuCtl");
        //FolderName = System.IO.Directory.GetDirectories(FolderName)[Random.Range(1, possibleanws + 1)];
        FolderName = FolderName + "/" + (Random.Range(1, possibleanws + 1)).ToString();
        SetAnwsers();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void SetAnwsers()
    {
        Sprite r_eq = null;
        foreach (var i in Resources.LoadAll<Sprite>(FolderName))
        {
            if (i.name.Equals("a"))
            {
                r_eq = i;
                continue;
            }
            //Debug.Log(System.IO.Path.GetFileNameWithoutExtension(i));
            if (i.name.Equals("e"))
            {
                HeadEqSprite = i;
                continue;
            }
            Anws.Add(i);
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
    public void PassQuestion()
    {
        //gameObject.transform.Find("Equ").GetComponent<Collider2D>().enabled = false;
        Block.GetComponent<SpriteRenderer>().enabled = false;
        Block.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "MainCharacter")
        {
            if (Input.GetKey(InteractKey))
            {
                PauseUIGO.GetComponent<PauseUI>().DisplayEq(HeadEqSprite, Anws, this);
            }
        }
    }
}
