using System.Collections.Generic;
using UnityEngine;

public class PhantomsZone : MonoBehaviour
{
    public int phantom_cnt;
    List<GameObject> PhantopmsGO = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject PhGo = gameObject.transform.Find("Phantom").gameObject;
        float X_cler = GetComponent<EdgeCollider2D>().bounds.size.x - PhGo.GetComponent<CapsuleCollider2D>().size.x * 4;
        for (int i = 0; i < phantom_cnt; i++)
        {
            GameObject tmp = Instantiate(PhGo);
            tmp.transform.SetParent(gameObject.transform);
            tmp.transform.position = PhGo.transform.position;
            tmp.transform.position = new Vector3(Random.Range(0, X_cler) + PhGo.transform.position.x, PhGo.transform.position.y, PhGo.transform.position.z);
            PhantopmsGO.Add(tmp);
        }
        PhGo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
