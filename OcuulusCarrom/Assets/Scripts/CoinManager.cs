using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject AllCoins;
    public GameObject WhitCoin;
    public GameObject BlackCoin;
    public GameObject RedCoin;
    public Transform AllCoinsPos;
    private GameObject TmpCoins;
    public static CoinManager instance;
    public List<GameObject> GameCoins;
    public List<Transform> P1FinePos;
    private float CoinRadius = 0.03f;
    private List<string> CoinsFellinThisTurn;
    public int TotalWhites = 0,TotalBlacks = 0,TotalRed = 0;

    private void Awake()
    {
        instance = this;
        GameCoins = new List<GameObject>();
        CoinsFellinThisTurn = new List<string>();
    }
    public void ClearList()
    {
        CoinsFellinThisTurn.Clear();
    }
    public List<string> GetFellCoins()
    {
        return CoinsFellinThisTurn;
    }
    private void SetCoins()
    {
        TotalWhites = 9;
        TotalRed = 1;
        TotalBlacks = 9;
    }
    private void ResetVariables()
    {
        TotalWhites = 0;
        TotalRed = 0;
        TotalBlacks = 0;
    }
    public void AddCoin(GameObject coin)
    {
        GameCoins.Add(coin);
        coin.GetComponent<CoinCollision>().CoinFellInHole += OnCoinFellIntoHole;
        coin.GetComponent<CoinCollision>().CoinFellOnGround += OnCoinFellOnGround;
    }
    public void RemoveCoin(GameObject coin)
    {
        coin.GetComponent<CoinCollision>().CoinFellInHole -= OnCoinFellIntoHole;
        coin.GetComponent<CoinCollision>().CoinFellOnGround -= OnCoinFellOnGround;
        GameCoins.Remove(coin);
    }
    public void InstantiateAllCoin()
    {
        TmpCoins = Instantiate(AllCoins, AllCoinsPos.position, AllCoinsPos.rotation);
        SetCoins();
    }
    public void DestroyAllCoins()
    {
        Destroy(TmpCoins);
        ResetVariables();
    }
    public void DestroyCoin(GameObject coin)
    {
        RemoveCoin(coin);
        Destroy(coin);
    }
    public void PutWhiteCoinInBoard(Vector3 position)
    {
        GameObject TmpWhite = Instantiate(WhitCoin, position, Quaternion.identity);
        if (TmpCoins != null)
        {
            TmpWhite.transform.parent = TmpCoins.transform;
            TotalWhites++;
        }
    }
    public void PutBlackCoinInBoard(Vector3 position)
    {
        GameObject TmpBlack = Instantiate(BlackCoin, position, Quaternion.identity);
        if (TmpCoins != null)
        {
            TmpBlack.transform.parent = TmpCoins.transform;
            TotalBlacks++;
        }
    }
    public void PutRedCoinInBoard(Vector3 position)
    {
        GameObject TmpRed = Instantiate(RedCoin, position, Quaternion.identity);
        if (TmpCoins != null)
        {
            TmpRed.transform.parent = TmpCoins.transform;
            TotalRed++;
        }
    }
    private void OnCoinFellIntoHole(string tag,GameObject coin)
    {
        CoinsFellinThisTurn.Add(coin.tag);
        if(tag == "White")
        {
            TotalWhites--;
        }
        if(tag == "Black")
        {
            TotalBlacks--;
       
        }
        if(tag == "Red")
        {
            TotalRed--;
        }
        DestroyCoin(coin);
    }
    private void OnCoinFellOnGround(GameObject coin)
    {
        coin.transform.position = FindCoinPosition();
    }
    public void PutFine(string CoinName)
    {

        if(CoinName == "Red")
        {
            if (TotalRed < 1)
            {
                PutRedCoinInBoard(FindCoinPosition());
            }
        }
        else if (CoinName == "White")
        {
            if (TotalWhites < 9)
            {
                PutWhiteCoinInBoard(FindCoinPosition());
            }
        }
        else if (CoinName == "Black")
        {
            if (TotalBlacks < 9)
            {
                PutBlackCoinInBoard(FindCoinPosition());
            }
        }
    }
    public Vector3 FindCoinPosition()
    {
        bool isThisCorrectPosition;
        Vector3 correctPosition = new Vector3(0,0.85f,0); 
        foreach (Transform fine in P1FinePos)
        {
                isThisCorrectPosition = true;
                correctPosition = fine.transform.position;
                Collider[] cols = Physics.OverlapSphere(fine.transform.position, CoinRadius);
                foreach (Collider c in cols)
                {
                    if (c.gameObject.tag == "White" || c.gameObject.tag == "Red" || c.gameObject.tag == "Black")
                    {
                        isThisCorrectPosition = false;
                    }

                }
                if (isThisCorrectPosition)
                {
                    break;
                }
        }      
        return correctPosition;
    }
}
