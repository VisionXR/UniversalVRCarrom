using UnityEngine;

public class MyAIManager : MonoBehaviour
{

    public static MyAIManager instance;
    int DifficultyLevel = 3; 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ActivateAI(DifficultyLevel);
        
    }
    public void TurnChanged(int id)
    {
        GetComponent<AIBotControlling>().OnTurnChanged(id);
    }
    public void AIShouldPlay(string CoinName)
    {
       if(DifficultyLevel == 1)
        {
            GetComponent<NewAIEasy>().FillData(CoinName);
        }
        else if(DifficultyLevel == 2)
        {
            GetComponent<NewAIMedium>().FillData(CoinName);
        }
        else if(DifficultyLevel == 3)
        {
            GetComponent<NewAIHard>().FillData(CoinName);
        }
    }
    public void ActivateAI(int id)
    {
 
        DifficultyLevel = id;
        if(id == 1)
        {
            GetComponent<NewAIEasy>().StartAI();          
        }
        else if(id == 2)
        {
            GetComponent<NewAIMedium>().StartAI();
        }
        else if(id == 3)
        {
            GetComponent<NewAIHard>().StartAI();
        }
        GetComponent<AIBotControlling>().CreateBot(id);
    }   
}
