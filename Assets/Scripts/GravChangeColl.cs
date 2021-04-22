using UnityEngine;

public class GravChangeColl : MonoBehaviour
{
    private GameObject PlayerGO;
    public float gravity;
    public float Y_atten;
    public float jump_height;
    private float orig_gravity;
    private float orig_Y_atten;
    private float orig_jump_height;
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
        if (collision.GetComponent<PlayerController_v3>())
        {
            orig_gravity = PlayerGO.GetComponent<PlayerController_v3>().gravity;
            orig_jump_height = PlayerGO.GetComponent<PlayerController_v3>().jump_height;
            orig_Y_atten = PlayerGO.GetComponent<PlayerController_v3>().y_speed_attenuation_by_time;
            PlayerGO.GetComponent<PlayerController_v3>().gravity = gravity;
            PlayerGO.GetComponent<PlayerController_v3>().y_speed_attenuation_by_time = Y_atten;
            PlayerGO.GetComponent<PlayerController_v3>().jump_height = jump_height;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController_v3>())
        {
            PlayerGO.GetComponent<PlayerController_v3>().gravity = orig_gravity;
            PlayerGO.GetComponent<PlayerController_v3>().y_speed_attenuation_by_time = orig_Y_atten;
            PlayerGO.GetComponent<PlayerController_v3>().jump_height = orig_jump_height;
        }
    }
}
