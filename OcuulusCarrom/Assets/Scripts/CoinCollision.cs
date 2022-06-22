using System;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    public event Action<string, GameObject> CoinFellInHole;
    public event Action<GameObject> CoinFellOnGround;
    private bool isPassed;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HoleTrigger")
        {           
                if (!isPassed)
                {
                    CoinFellInHole(gameObject.tag, gameObject);
                    isPassed = true;
                }
        }
        if(collision.gameObject.tag == "Ground")
        {
            if(!isPassed)
            {
                CoinFellOnGround(gameObject);
            }
        }
    }
}
