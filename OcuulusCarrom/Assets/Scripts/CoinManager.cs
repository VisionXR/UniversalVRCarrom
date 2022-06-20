using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject AllCoins;
    public GameObject WhitCoin;
    public GameObject BlackCoin;
    public GameObject RedCoin;
    public void InstantiateCoin()
    {
        GameObject TmpCoins = Instantiate(AllCoins, Vector3.zero, Quaternion.identity);
    }
    public void DestroyCoin(GameObject coin)
    {
        Destroy(coin);
    }
    public void PutWhiteCoinInBoard(Vector3 position)
    {
        GameObject TmpWhite = Instantiate(WhitCoin, position, Quaternion.identity);
        TmpWhite.transform.parent = AllCoins.transform;
    }
    public void PutBlackCoinInBoard(Vector3 position)
    {
        GameObject TmpBlack = Instantiate(BlackCoin, position, Quaternion.identity);
        TmpBlack.transform.parent = AllCoins.transform;
    }
}
