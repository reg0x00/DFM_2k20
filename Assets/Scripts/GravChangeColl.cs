using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravChangeColl : MonoBehaviour
{
    private GameObject PlayerGO;
    public float gravity;
    private float orig_gravity;
    // Start is called before the first frame update
    void Start()
    {
        PlayerGO = GameObject.Find("MainCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        orig_gravity = PlayerGO.GetComponent<PlayerController_v3>().gravity;
        PlayerGO.GetComponent<PlayerController_v3>().gravity = gravity;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerGO.GetComponent<PlayerController_v3>().gravity = orig_gravity;
    }
}
