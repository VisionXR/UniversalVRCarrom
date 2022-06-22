using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewAIHard : MonoBehaviour
{
    private GameObject[] Blacks;
    private GameObject[] Whites;
    public List<GameObject> StrikerPos;
    private GameObject Red;
    public GameObject Striker;
    public List<GameObject> Holes;
    public List<CoinInfo> info1,info2,info3,info4;
    public float CoinRadius = 0.03f, StrikerRadius = 0.04f;
    Vector3 dir;
    public SortedDictionary<float, Vector3> ReverseShotDictionary;
    LineRenderer lr;
    public float delayTime = 0.1f;
    public event Action<Vector3> StrikeStarted, StrikeEnded;
    public void StartAI()
    {
        ReverseShotDictionary = new SortedDictionary<float, Vector3>();
        info1 = new List<CoinInfo>();
        info2 = new List<CoinInfo>();
        info3 = new List<CoinInfo>();
        info4 = new List<CoinInfo>();
        Striker = GameObject.Find("P2Striker");
    }
    private IEnumerator StartSequence(string coinType)
    {
        
        yield return new WaitForSeconds(1);
        if (info1.Count > 0)
        {
            info1.Clear();
        }
        if (info2.Count > 0)
        {
            info2.Clear();
        }
        if (info3.Count > 0)
        {
            info3.Clear();
        }
        if (info4.Count > 0)
        {
            info4.Clear();
        }
        if (ReverseShotDictionary.Count > 0)
        {
            ReverseShotDictionary.Clear();
        }

        if (coinType == "White")
        {
            foreach (GameObject coin in Whites)
            {
                if (coin != null)
                {
                    foreach (GameObject hole in Holes)
                    {
                        foreach (GameObject pos in StrikerPos)
                        {
                            SubFill(coin, hole, pos);
                        }
                    }
                }
            }

        }
        else if (coinType == "Black")
        {
            foreach (GameObject coin in Blacks)
            {
                if (coin != null)
                {
                    foreach (GameObject hole in Holes)
                    {
                        foreach (GameObject pos in StrikerPos)
                        {
                            SubFill(coin, hole, pos);
                        }
                    }
                }
            }
        }
        else if (coinType == "Red")
        {
            foreach (GameObject hole in Holes)
            {
                foreach (GameObject pos in StrikerPos)
                {
                    SubFill(Red, hole, pos);
                }
            }
        }
        else
        {
            foreach (GameObject coin in Whites)
            {
                if (coin != null)
                {
                    foreach (GameObject hole in Holes)
                    {
                        foreach (GameObject pos in StrikerPos)
                        {
                            SubFill(coin, hole, pos);
                        }
                    }
                }
            }
            foreach (GameObject coin in Blacks)
            {
                if (coin != null)
                {
                    foreach (GameObject hole in Holes)
                    {
                        foreach (GameObject pos in StrikerPos)
                        {
                            SubFill(coin, hole, pos);
                        }
                    }
                }
            }
        }
        if (info1.Count > 1)
        {
            yield return StartCoroutine(SortBalls(info1));
            yield return StartCoroutine(SortBalls1(info1));
        } 
        else if(info2.Count > 1)
        {
            yield return StartCoroutine(SortBalls(info2));
            yield return StartCoroutine(SortBalls1(info2));
        }
        else if (info3.Count > 1)
        {
            yield return StartCoroutine(SortBalls(info3));
            yield return StartCoroutine(SortBalls2(info3));
        }
        else if (info4.Count > 1)
        {
            yield return StartCoroutine(SortBalls(info4));
            yield return StartCoroutine(SortBalls1(info4));
        }
        yield return StartCoroutine(HitCoin(0));
    }
    public void FillData(string coinType)
    {
        Blacks = GameObject.FindGameObjectsWithTag("Black");
        Whites = GameObject.FindGameObjectsWithTag("White");
        Red = GameObject.FindGameObjectWithTag("Red");
        StartCoroutine(StartSequence(coinType));
    }
    private void SubFill(GameObject coin, GameObject hole, GameObject StrikerPosition)
    {
        if (coin != null)
        {
            CoinInfo b = new CoinInfo();
            RaycastHit hitInfo;
            b.Coin = coin;
            b.Hole = hole;
            b.StrikerPos = StrikerPosition;
            Vector3 holedir = (hole.transform.position - coin.transform.position).normalized;
            Vector3 finalPos = coin.transform.position - holedir * (CoinRadius + StrikerRadius);
            b.FinalPos = finalPos;
            Vector3 Strikerdir = (finalPos - StrikerPosition.transform.position).normalized;
            b.angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(holedir, Strikerdir));
            b.distance = Vector3.Distance(coin.transform.position, hole.transform.position) + Vector3.Distance(coin.transform.position, StrikerPosition.transform.position);
            if (Physics.SphereCast(coin.transform.position, CoinRadius, holedir, out hitInfo))
            {
                GameObject tmpobj = hitInfo.collider.gameObject;
                if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
                {              
                    b.isBlockedH = true;            
                }
                else
                {
                    b.isBlockedH = false;
                }
            }
            if (Physics.SphereCast(StrikerPosition.transform.position, StrikerRadius, Strikerdir, out hitInfo, (finalPos - StrikerPosition.transform.position).magnitude - 0.01f))
            {
                if (b.angle < 60 || b.angle > 90)
                {
                    GameObject tmpobj = hitInfo.collider.gameObject;
                    if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
                    {
                        b.isBlockedC = true;

                    }
                    else
                    {
                        b.isBlockedC = false;
                    }
                }
            }

            if (b.angle <= 40)
            {
                info1.Add(b);
            }
            else if(b.angle > 40 && b.angle < 60)
            {
                info2.Add(b);
            }
            else if(b.angle >= 60 && b.angle <= 90)
            {
                if (b.Hole.name != "Hole1" && b.Hole.name != "Hole4")
                {
                    info3.Add(b);
                }
            }
            else
            {
                info4.Add(b);
            }
        }
    } 
    private IEnumerator HitCoin(float t)
    {
     
        yield return new WaitForSeconds(t);
        float force;
        if (info1.Count != 0)
        {
            foreach (CoinInfo b1 in info1)
            {
                if(b1.isBlockedC == false && b1.isBlockedH == false)
                {
                    force = b1.distance + 1f;
                    StartCoroutine(TryForWardShot(b1, force));
                    yield break;
                }
            }
        }
        if(info2.Count != 0)
        {
            foreach (CoinInfo b1 in info2)
            {
                if (b1.isBlockedC == false && b1.isBlockedH == false)
                {
                    force = b1.distance + 1f;
                    StartCoroutine(TryForWardShot(b1, force));
                    yield break;
                }
            }
        }
        if(info3.Count != 0)
        {
            foreach (CoinInfo b1 in info3)
            {
                if (b1.isBlockedC == false && b1.isBlockedH == false)
                {
                    force = b1.distance + 1f;
                    StartCoroutine(TryReverseShot(b1, 0));
                    yield break;
                }
            }
        }
        if(info4.Count != 0)
        {
            foreach (CoinInfo b1 in info4)
            {
                if (b1.isBlockedC == false && b1.isBlockedH == false)
                {
                    force = b1.distance + 1.2f;
                    StartCoroutine(TryReverseShot(b1, 0));
                    yield break;
                }
            }
        }
        if (info1.Count != 0)
        {
                CoinInfo b1 = info1[0];
                force = b1.distance + 1f;
                b1 = DoubeTouch(b1);             
                StartCoroutine(TryForWardShot(b1, force));
                yield break;            
         }
        if (info2.Count != 0)
        {
            CoinInfo b1 = info2[0];
            force = b1.distance + 1.1f;
            b1 = DoubeTouch(b1);
            StartCoroutine(TryForWardShot(b1, force));
            yield break;

        }
        if (info3.Count != 0)
        {
            CoinInfo b1 = info3[0];
            force = b1.distance + 1f;
            StartCoroutine(TryReverseShot(b1, 0));
            yield break;
        }
        if (info4.Count != 0)
        {
            CoinInfo b1 = info4[0];
            force = b1.distance + 1.2f;
            StartCoroutine(TryForWardShot(b1, force));
            yield break;
        }
    }

    private IEnumerator TryForWardShot(CoinInfo b,float force)
    {
        if (b.StrikerPos.name != "P2RightBound")
        {
            Striker.transform.position = Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, -b.StrikerPos.transform.right);
        }
        else
        {
            Striker.transform.position = Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, b.StrikerPos.transform.right);
        }
        dir = (b.FinalPos - Striker.transform.position).normalized;

        if (StrikeStarted != null)
        {
            StrikeStarted(b.FinalPos);
        }
        yield return new WaitForSeconds(2);
        if (StrikeEnded != null)
        {
            StrikeEnded(b.FinalPos);
        }
        Striker.GetComponent<StrikerHitting>().FireStriker(dir, force);
    }

    private CoinInfo DoubeTouch(CoinInfo b1)
    {
        RaycastHit hitInfo;
        if (b1.isBlockedH == true)
        {
            Vector3 holedir = (b1.Hole.transform.position - b1.Coin.transform.position).normalized;
            if (Physics.SphereCast(b1.Coin.transform.position, CoinRadius, holedir, out hitInfo))
            {
                GameObject tmpobj = hitInfo.collider.gameObject;
                if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
                {

                    if (b1.Coin.tag == tmpobj.tag || tmpobj.tag == "Red")
                    {
                        Vector3 newHoleDir = (b1.Hole.transform.position - tmpobj.transform.position).normalized;
                        Vector3 finalpos1 = tmpobj.transform.position - newHoleDir * (CoinRadius + StrikerRadius);
                        Vector3 newCoinDir = (finalpos1 - b1.Coin.transform.position).normalized;
                        Vector3 finalpos2 = b1.Coin.transform.position - newCoinDir * (CoinRadius + StrikerRadius);
                        b1.FinalPos = finalpos2;
                    }
                }
            }
        }
        if (b1.isBlockedC)
        {
            Vector3 Strikerdir = (b1.FinalPos - Striker.transform.position).normalized;
            if (Physics.SphereCast(Striker.transform.position, StrikerRadius, Strikerdir, out hitInfo, (b1.FinalPos - Striker.transform.position).magnitude - 0.01f))
            {
                GameObject tmpobj = hitInfo.collider.gameObject;
                if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
                {

                    Vector3 newCoinDir = (b1.FinalPos - tmpobj.transform.position).normalized;
                    Vector3 finalpos2 = tmpobj.transform.position - newCoinDir * (CoinRadius + StrikerRadius);
                    b1.FinalPos = finalpos2;
                }
            }
        }
        return b1;
    }

    private IEnumerator TryReverseShot(CoinInfo b, float force)
    {

        if (b.StrikerPos.name != "P2RightBound")
        {
            Striker.transform.position = Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, -b.StrikerPos.transform.right);
        }
        else
        {
            Striker.transform.position = Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, b.StrikerPos.transform.right);
        }
        yield return new WaitForSeconds(0);
        if (b.Hole.name == "Hole3")
        {
           
            StartCoroutine(CheckRayCastFromStrikerToEdge(b, -1));
        }
        else if(b.Hole.name == "Hole2")
        {
            
            StartCoroutine(CheckRayCastFromStrikerToEdge(b, 1));
        }
        else if (b.Hole.name == "Hole1")
        {
            Vector3 holedir = (Holes[0].transform.position - b.Coin.transform.position).normalized;
            Vector3 finalPos = b.Coin.transform.position - holedir * (CoinRadius + StrikerRadius);
            b.FinalPos = finalPos;
            StartCoroutine(CheckRayCastFromStrikerToEdge(b, 1));
        }
        else if (b.Hole.name == "Hole4")
        {
            Vector3 holedir = (Holes[3].transform.position - b.Coin.transform.position).normalized;
            Vector3 finalPos = b.Coin.transform.position - holedir * (CoinRadius + StrikerRadius);
            b.FinalPos = finalPos;
            StartCoroutine(CheckRayCastFromStrikerToEdge(b, -1));
        }
    }
    private IEnumerator CheckRayCastFromStrikerToEdge(CoinInfo b,int Num)
    {
      
        RaycastHit HitInfo,HitInfo1;   
        for(float i = 0; i <= 90; i = i+0.005f)
        {
            if(Physics.Raycast(Striker.transform.position,-Striker.transform.forward,out HitInfo))
            {
                if (HitInfo.collider.name == "LeftEdge")
                {                  
                    Vector3 EdgeHitPoint = HitInfo.point;                   
                    Vector3 EdgeNormal = HitInfo.normal;
                    Vector3 OriginalDir = (EdgeHitPoint - Striker.transform.position).normalized;
                    Vector3 ReflectedDir = Vector3.Reflect(OriginalDir, EdgeNormal);
                    if(Physics.Raycast(EdgeHitPoint,ReflectedDir,out HitInfo1))
                    {
                        if (HitInfo1.collider.name == b.Coin.name)
                        {
                            ReverseShotDictionary.Add(Vector3.Distance(b.FinalPos, HitInfo1.point), EdgeHitPoint);
                       
                        }
                    }
                }
            }
            Striker.transform.localEulerAngles = new Vector3(0, Num * i, 0);

        }
        bool DidIHit = false;
        foreach (KeyValuePair<float, Vector3> dic in ReverseShotDictionary)
        {
            dir = (dic.Value-Striker.transform.position).normalized;
            if (StrikeStarted != null)
            {
                StrikeStarted(dic.Value);
            }
            yield return new WaitForSeconds(1);
            if (StrikeEnded != null)
            {
                StrikeEnded(dic.Value);
            }
            Striker.GetComponent<StrikerHitting>().FireStriker(dir, 5);
            DidIHit = true;
            break;
        }
        if (!DidIHit)
        {
            dir = (b.FinalPos - Striker.transform.position).normalized;
            if (StrikeStarted != null)
            {
                StrikeStarted(b.FinalPos);
            }
            yield return new WaitForSeconds(1);
            if (StrikeEnded != null)
            {
                StrikeEnded(b.FinalPos);
            }
            Striker.GetComponent<StrikerHitting>().FireStriker(dir, 3);
        }
    }
    private IEnumerator SortBalls(List<CoinInfo> info)
    {
        for (int j = 0; j <= info.Count - 1; j++)
        {
            for (int i = 0; i <= info.Count - 2; i++)
            {

                CoinInfo b1 = info[i];
                CoinInfo b2 = info[i + 1];
                if (b1.isBlockedC && b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i, info);
                    }

                }
                else if (b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && b2.isBlockedH)
                    {
                        CopyData(i, info);
                    }
                    else if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i, info);
                    }
                    else if (!b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i, info);
                    }
                }

            }
        }
        yield return null;
    }
    private IEnumerator SortBalls1(List<CoinInfo> info)
    {
        for (int j = 0; j <= info.Count - 1; j++)
        {
            for (int i = 0; i <= info.Count - 2; i++)
            {
                CoinInfo b1 = info[i];
                CoinInfo b2 = info[i + 1];
                if (b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
            }
        }
        yield return null;
    }
    private IEnumerator SortBalls2(List<CoinInfo> info)
    {
        for (int j = 0; j <= info.Count - 1; j++)
        {
            for (int i = 0; i <= info.Count - 2; i++)
            {
                CoinInfo b1 = info[i];
                CoinInfo b2 = info[i + 1];
                if (b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle < b2.angle)
                    {
                        CopyData(i, info);
                    }
                }
            }
        }
        yield return null;
    }
    private void CopyData(int i, List<CoinInfo> info)
    {
        CoinInfo b = new CoinInfo();
        b = info[i];
        info[i] = info[i + 1];
        info[i + 1] = b;
    }
}

