using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerLine : MonoBehaviour
{
    public LineRenderer StrikerToCoinlr, CoinToHolelr;
    private RaycastHit HitInfo;
    public GameObject DisplayStriker;
    public float StrikerRadius = 0.04f, CoinRadius = 0.03f;
    void Start()
    {
        StrikerToCoinlr.startWidth = 0.02f;
        StrikerToCoinlr.endWidth = 0.02f;
        CoinToHolelr.startWidth = 0.02f;
        CoinToHolelr.endWidth = 0.02f;

    }
    public void ClearLines()
    {
        StrikerToCoinlr.positionCount = 0;
        CoinToHolelr.positionCount = 0;
        DisplayStriker.transform.position = new Vector3(100, 0, 0);
    }
    public void DrawLine()
    {
        if (Physics.SphereCast(transform.position, StrikerRadius, transform.forward, out HitInfo))
        {
     
            StrikerToCoinlr.positionCount = 2;
            StrikerToCoinlr.SetPosition(0, transform.position);
            StrikerToCoinlr.SetPosition(1, HitInfo.point + (HitInfo.normal) * (StrikerRadius));
            if (DisplayStriker != null)
            {
                DisplayStriker.transform.position = HitInfo.point + (HitInfo.normal) * (StrikerRadius);              
                CoinToHolelr.positionCount = 2;
                Vector3 tmpPoint = HitInfo.point - (HitInfo.normal) * (2*StrikerRadius);
                if (HitInfo.collider.tag != "Edge")
                {
                    CoinToHolelr.SetPosition(0, tmpPoint);
                    CoinToHolelr.SetPosition(1, tmpPoint - (HitInfo.normal) * (2 * CoinRadius));
                }
                else
                {
                    Vector3 ReflectedDir = Vector3.Reflect((-transform.position + HitInfo.point).normalized, HitInfo.normal);
                    CoinToHolelr.SetPosition(0, DisplayStriker.transform.position);
                    CoinToHolelr.SetPosition(1, DisplayStriker.transform.position+ReflectedDir*3*StrikerRadius);
                }
             
            }
        }
    }
}
