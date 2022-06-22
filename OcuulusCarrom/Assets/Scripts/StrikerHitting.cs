using System.Collections;
using UnityEngine;
using System;

public class StrikerHitting : MonoBehaviour
{
    public event Action StrikeStarted, StrikeFinished;
    Rigidbody StrikerRb;
    void Start()
    {
        StrikerRb = GetComponent<Rigidbody>();
      
    }
    public void FireStriker(float force)
    {
            GetComponent<StrikerLine>().ClearLines();
       
            if (StrikeStarted != null)
            {
                StrikeStarted();
            }
            StrikerRb.AddForce(transform.forward * force, ForceMode.VelocityChange);
            StartCoroutine(ResetPosition());  
    }
    public void FireStriker(Vector3 dir,float force)
    {
       
        if (StrikeStarted != null)
        {
            StrikeStarted();
        }
        StrikerRb.AddForce(dir * force, ForceMode.VelocityChange);
        StartCoroutine(ResetPosition());
    }
    private IEnumerator ResetPosition()
    {
        yield return new WaitUntil(() => StrikerRb.velocity.magnitude < 0.002f);
        yield return new WaitForSeconds(5.5f);
        StrikerRb.velocity = Vector3.zero;
        StrikerRb.angularVelocity = Vector3.zero;
        if (StrikeFinished != null)
        {
            StrikeFinished();
          
        }
    }

    


}
