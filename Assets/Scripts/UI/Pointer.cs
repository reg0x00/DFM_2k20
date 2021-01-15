using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public string[] OrderedTargets = new string[1];
    string CurrentTarget;
    Vector2 Target;
    RectTransform PointerRectTrs;
    // Start is called before the first frame update
    void Start()
    {
        PointerRectTrs = transform.Find("Pointer").GetComponent<RectTransform>();
        if (OrderedTargets.Length != 0)
        {
            CurrentTarget = OrderedTargets[0];
            Target = GameObject.Find(CurrentTarget).transform.position;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find(CurrentTarget) == null) // active target complete
        {
            if(CurrentTarget== OrderedTargets[OrderedTargets.Length - 1]) // all targets complete
            {
                return;
            }
            CurrentTarget = OrderedTargets[0];
            Target = GameObject.Find(CurrentTarget).transform.position;
        }
        //bool InWindow= 
        Vector2 NowCameraPosition = Camera.main.transform.position;
        Vector2 NormDir = (Target - NowCameraPosition).normalized;
        double AngelToTarget = Mathf.Atan2(NormDir.y, NormDir.x) * 180.0 / Mathf.PI; // -180<x<180
        PointerRectTrs.localEulerAngles= new Vector3(0, 0, (float)AngelToTarget);
        PointerRectTrs.anchoredPosition = NormDir*300;
        
    }
}
