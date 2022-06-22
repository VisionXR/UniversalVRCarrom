using System;
using UnityEngine;

public class StrikerCollision : MonoBehaviour
{

    public event Action StrikerFellIntoHole;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "HoleTrigger")
        {
            if(StrikerFellIntoHole != null)
            {
                StrikerFellIntoHole();
            }
        }
    }
}
