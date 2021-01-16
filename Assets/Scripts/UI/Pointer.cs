using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pointer : MonoBehaviour
{
    string CurrentTarget;
    Vector2 Target;
    RectTransform PointerRectTrs;
    Image PointerImg;
    private Stack<string> Targets=new Stack<string>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject ColCtl = GameObject.Find("Collectibles");
        Collectibles_ctl Collectibles_ctl_Targets = ColCtl.GetComponent<Collectibles_ctl>();
        PointerRectTrs = transform.Find("Pointer").GetComponent<RectTransform>();
        PointerImg = transform.Find("Pointer").GetComponent<Image>();
        if (Collectibles_ctl_Targets.CollectOrder.Length != 0)
        {
            for(int i = Collectibles_ctl_Targets.CollectOrder.Length - 1; i >= 0; i--)
            {
                Targets.Push(Collectibles_ctl_Targets.CollectOrder[i]);
            }
            CurrentTarget = Targets.Pop();
            Target = GameObject.Find(CurrentTarget).transform.position;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find(CurrentTarget) == null) // active target complete
        {
            if(Targets.Count==0) // all targets complete
            {
                return;
            }
            CurrentTarget = Targets.Pop();
            Target = GameObject.Find(CurrentTarget).transform.position;
        }
        //bool InWindow= 
        Plane[] planes;
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, GameObject.Find(CurrentTarget).GetComponent<SpriteRenderer>().bounds))
        {
            PointerImg.enabled = false;
        }
        else
        {
            PointerImg.enabled = true;
        }
        Vector2 NowCameraPosition = Camera.main.transform.position;
        Vector2 NormDir = (Target - NowCameraPosition).normalized;
        double AngelToTarget = Mathf.Atan2(NormDir.y, NormDir.x) * 180.0 / Mathf.PI; // -180<x<180
        PointerRectTrs.localEulerAngles= new Vector3(0, 0, (float)AngelToTarget);
        PointerRectTrs.anchoredPosition = NormDir*300;
        
    }
}
