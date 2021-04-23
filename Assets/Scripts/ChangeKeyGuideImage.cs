using UnityEngine;
using UnityEngine.UI;

public class ChangeKeyGuideImage : MonoBehaviour
{
    public Sprite keyImg;
    public Sprite JsImge;
    // Start is called before the first frame update
    void Start()
    {
        if (Input.GetJoystickNames().Length == 0)
        {
            GetComponent<Image>().sprite = keyImg;
        }
        else
        {
            GetComponent<Image>().sprite = JsImge;
        }
    }
}
