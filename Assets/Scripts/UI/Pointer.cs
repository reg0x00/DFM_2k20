using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pointer : MonoBehaviour
{
    string CurrentTarget;
    string[] Tags;
    private const float ADistmin = 3.0f;
    private const float ADistmax = 6.0f;
    int TaggedObjCount;
    Vector2 Target;
    RectTransform PointerRectTrs;
    Bounds TargetBounds;
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
            Tags = Collectibles_ctl_Targets.CollectTagsToTrack;
            TaggedObjCount= CountTaggedObj();
            for (int i = Collectibles_ctl_Targets.CollectOrder.Length - 1; i >= 0; i--)
            {
                Targets.Push(Collectibles_ctl_Targets.CollectOrder[i]);
            }
            CurrentTarget = Targets.Pop();
            Target = GameObject.Find(CurrentTarget).transform.position;
            TargetBounds = GameObject.Find(CurrentTarget).GetComponent<SpriteRenderer>().bounds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TaggedObjCount!= CountTaggedObj()) // active target complete
        {
            if(Targets.Count==0) // all targets complete
            {
                return;
            }
            CurrentTarget = Targets.Pop();
            Target = GameObject.Find(CurrentTarget).transform.position;
            TargetBounds = GameObject.Find(CurrentTarget).GetComponent<SpriteRenderer>().bounds;
            TaggedObjCount = CountTaggedObj();
        }
        //bool InWindow= 
        Plane[] planes;
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Vector2 NowCameraPosition = Camera.main.transform.position;
        Vector2 NormDir = (Target - NowCameraPosition).normalized;
        double AngelToTarget = Mathf.Atan2(NormDir.y, NormDir.x) * 180.0 / Mathf.PI; // -180<x<180
        PointerRectTrs.localEulerAngles= new Vector3(0, 0, (float)AngelToTarget);
        PointerRectTrs.anchoredPosition = NormDir*300;
        var TmpImColor = PointerImg.color;
        //if (GeometryUtility.TestPlanesAABB(planes, TargetBounds))
        //{            
        //    TmpImColor.a = 0;            
        //}
        //else
        //{
        //    TmpImColor.a = 1;
        //}
        var dist = (Target - NowCameraPosition).magnitude;
        if (dist < ADistmax)
        {
            if(dist < ADistmin)
            {
                TmpImColor.a = 0;
            }
            else
            {
                TmpImColor.a = (dist - ADistmin) / (ADistmax - ADistmin);
            }
        }
        else
        {
            TmpImColor.a = 1;
        }
        PointerImg.color = TmpImColor;
    }
    private int CountTaggedObj()
    {
        int cnt = 0;
        foreach(string i in Tags)
        {
            cnt += GameObject.FindGameObjectsWithTag(i).Length;
        }
        return cnt;
    }
}
