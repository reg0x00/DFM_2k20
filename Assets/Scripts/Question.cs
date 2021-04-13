using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    // Start is called before the first frame update
    public int FolderNuber;
    public KeyCode InteractKey;
    private string FolderPath = "Assets/Resources/Equations";
    private GameObject Block;    
    private GameObject PauseUIGO;
    private Sprite HeadEqSprite;
    private List<Sprite> Anws = new List<Sprite>();    
    void Start()
    {
        FolderPath = System.IO.Path.Combine(FolderPath, FolderNuber.ToString());
        Block = gameObject.transform.Find("Block").gameObject;
        //Block = GameObject.Find("Block");        
        PauseUIGO = GameObject.Find("MenuCtl");
        FolderPath = System.IO.Directory.GetDirectories(FolderPath)[Random.Range(0, System.IO.Directory.GetDirectories(FolderPath).Length)];
        SetAnwsers();        
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
                PauseUIGO.GetComponent<PauseUI>().DisplayEq(HeadEqSprite,Anws,this);
            }
        }
    }
}
