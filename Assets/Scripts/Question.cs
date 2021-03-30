using UnityEngine;

public class Question : MonoBehaviour
{
    // Start is called before the first frame update
    public int FolderNuber;
    private string FolderPath = "Assets/Resources/Equations";
    private GameObject init_eq;
    private Sprite HeadEq;
    private Sprite[] Anws;
    private int right_anws;
    void Start()
    {
        FolderPath = System.IO.Path.Combine(FolderPath, FolderNuber.ToString());
        init_eq = GameObject.Find("Equation");
        FolderPath = System.IO.Directory.GetDirectories(FolderPath)[Random.Range(0, System.IO.Directory.GetDirectories(FolderPath).Length)];
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetAnwsers()
    {
        foreach (var i in System.IO.Directory.GetFiles(FolderPath))
        {
            if (!i.Contains(".meta"))
                Debug.Log(System.IO.Path.GetFileNameWithoutExtension(i));
        }
    }
}
