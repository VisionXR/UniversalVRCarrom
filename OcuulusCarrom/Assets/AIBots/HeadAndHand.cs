using System.Collections;
using UnityEngine;

public class HeadAndHand : MonoBehaviour
{
    public GameObject Head, Hand,HandPosStrike,HandPosRest,AIBot;
    private Animator HandAnimator;
    public GameObject Striker;
    public Vector3  FirePosition;
    public Vector3 HeadInitPos,HandInitPos;
    public float Offset=0.08f;
    void Start()
    {
        HandAnimator = Hand.GetComponent<Animator>();
        Striker = GameObject.Find("P2Striker");
        HandPosStrike = GameObject.Find("HandPosStrike");
        HandPosRest = GameObject.Find("HandPosRest");
        HeadInitPos = -Head.transform.forward;
        HandInitPos = Hand.transform.forward;
    }
    public void ShowFingerCloseAnimation(Vector3 pos)
    {
      
        Hand.transform.position = Vector3.Lerp(Hand.transform.position,Striker.transform.position-(pos-Striker.transform.position).normalized * Offset,1);
        Hand.transform.forward = Vector3.Lerp(Hand.transform.forward,   (pos - Striker.transform.position).normalized, 1);
        HandAnimator.SetBool("StrikeFinished", false);
        HandAnimator.SetBool("CloseFinger", true);
        Head.transform.forward = Vector3.Lerp(Head.transform.forward,-1*(pos - Head.transform.position).normalized,1);
    }
    private IEnumerator WaitAndResetHandAndHead()
    {
        yield return new WaitForSeconds(1.5f);
        Head.transform.forward = Vector3.Lerp(Head.transform.forward,-HeadInitPos,1);
        Hand.transform.forward = Vector3.Lerp(Hand.transform.forward,HandInitPos,1);
    }
    public void ShowFingerStrikeAnimation(Vector3 pos)
    {
   
        HandAnimator.SetBool("CloseFinger", true);
        HandAnimator.SetBool("FingerStrike", true);
        StartCoroutine(AfterStrikeFinished());
    }
    private IEnumerator AfterStrikeFinished()
    {
        yield return new WaitForSeconds(0.1f);
        HandAnimator.SetBool("StrikeFinished", true);
        HandAnimator.SetBool("CloseFinger", false);
        HandAnimator.SetBool("FingerStrike", false);
        StartCoroutine(WaitAndResetHandAndHead());
    }
    public void SetParent()
    {
        Hand.transform.parent = Striker.transform;
        Hand.transform.localPosition = Vector3.Lerp(Hand.transform.localPosition, HandPosStrike.transform.localPosition,1);
        Hand.transform.localRotation = Quaternion.Lerp(Hand.transform.localRotation, HandPosStrike.transform.localRotation,1);
    }
    public void RemoveParent()
    {
        Hand.transform.parent = AIBot.transform;
        StartCoroutine(Wait(2));
    }
    private IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        Hand.transform.localPosition = Vector3.Lerp(Hand.transform.localPosition,HandPosRest.transform.localPosition,1);
        Hand.transform.localRotation = Quaternion.Lerp(Hand.transform.localRotation,HandPosRest.transform.localRotation,1);
    }
    
   
}
