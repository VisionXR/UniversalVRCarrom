

using System;
using UnityEngine;

public class AIBotControlling : MonoBehaviour
{
   
    public GameObject AIBotEasy,AIBotMedium,AIBotHard;
    private GameObject CurrentBot;
    public NewAIEasy AIEasyScript;
    public NewAIMedium AIMediumScript;
    public NewAIHard AIHardScript;

    // Start is called before the first frame update
    void Start()
    {
     
        AIEasyScript.StrikeStarted += OnStrikedStarted;
        AIEasyScript.StrikeEnded += OnStrikeEnded;
        AIMediumScript.StrikeStarted += OnStrikedStarted;
        AIMediumScript.StrikeEnded += OnStrikeEnded;
        AIHardScript.StrikeStarted += OnStrikedStarted;
        AIHardScript.StrikeEnded += OnStrikeEnded;
    }

    private void OnStrikeEnded(Vector3 pos)
    {
       
        CurrentBot.GetComponent<HeadAndHand>().ShowFingerStrikeAnimation(pos);
        CurrentBot.GetComponent<HeadAndHand>().RemoveParent();
    }

    private void OnStrikedStarted(Vector3 pos)
    {
       
        CurrentBot.GetComponent<HeadAndHand>().ShowFingerCloseAnimation(pos);
    }
    public void OnTurnChanged(int id)
    {
        if (id == 2)
        {
            CurrentBot.GetComponent<HeadAndHand>().SetParent();
        }
        else
        {
            CurrentBot.GetComponent<HeadAndHand>().RemoveParent();
        }
    }

    public void CreateBot(int i)
    {
        if(CurrentBot != null)
        {
            Destroy(CurrentBot);
        }
        if (i == 1)
        {
            CurrentBot = Instantiate(AIBotEasy, AIBotEasy.transform.position, AIBotEasy.transform.rotation);
        }
        if (i == 2)
        {
            CurrentBot = Instantiate(AIBotMedium, AIBotEasy.transform.position, AIBotEasy.transform.rotation);
        }
        if (i == 3)
        {
            CurrentBot = Instantiate(AIBotHard, AIBotEasy.transform.position, AIBotEasy.transform.rotation);
        }

    }
    public void DestroyBot()
    {
        if (CurrentBot != null)
        {
            Destroy(CurrentBot);
        }
    }
}
