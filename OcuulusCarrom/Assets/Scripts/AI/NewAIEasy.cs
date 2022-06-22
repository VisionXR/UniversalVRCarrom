using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewAIEasy : MonoBehaviour
{
    private GameObject[] Blacks;
    private GameObject[] Whites;
    public List<GameObject> StrikerPos;
    private GameObject Red;
    public GameObject Striker;
    public List<GameObject> Holes;
    private List<CoinInfo> info;
    public float CoinRadius = 0.03f, StrikerRadius = 0.04f;
    Vector3 dir;
    public float CutOffAngle = 30;
    public event Action<Vector3> StrikeStarted, StrikeEnded;
    public void StartAI()
    {
        info = new List<CoinInfo>();
        Striker = GameObject.Find("P2Striker");
    }
    private IEnumerator StartSequence(string coinType)
    {
        yield return new WaitForSeconds(1);
        if (info.Count > 0)
        {
            info.Clear();
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
            foreach (GameObject hole in Holes)
            {
                foreach (GameObject pos in StrikerPos)
                {
                    SubFill(Red, hole, pos);
                }
            }
        }
        if (info.Count > 1)
        {
            yield return StartCoroutine(SortBalls());
            yield return StartCoroutine(SortBalls1());
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
            info.Add(b);
        }
    }
    private IEnumerator SortBalls()
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
                        CopyData(i);
                    }

                }
                else if (b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (!b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }

            }
        }
        yield return null;
    }
    public void Display(CoinInfo c)
    {

    }
    private IEnumerator SortBalls1()
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
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
            }
        }
        yield return null;
    }
    private void CopyData(int i)
    {
        CoinInfo b = new CoinInfo();
        b = info[i];
        info[i] = info[i + 1];
        info[i + 1] = b;
    }
    private IEnumerator HitCoin(float t)
    {
        yield return new WaitForSeconds(t);
        float force;
        CoinInfo b = info[0];
        Display(b);
        force = b.distance + 1f;
        if (b.StrikerPos.name != "P2RightBound")
        {
           Striker.transform.position =  Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, -info[0].StrikerPos.transform.right);
        }
        else
        {
            Striker.transform.position = Striker.GetComponent<AIStrikerMovement>().FindStrikerNextPosition(b.StrikerPos.transform.position, info[0].StrikerPos.transform.right);
        }
        if (b.angle < CutOffAngle)
        {
            dir = (b.FinalPos - Striker.transform.position).normalized;
        }
        else
        {
            dir = (b.Coin.transform.position - Striker.transform.position).normalized;
        }
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
}

