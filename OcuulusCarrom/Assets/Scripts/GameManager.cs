using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private int turnid = 1;
    public bool isFoul = false, CheckForRedFollow = false;
    public event Action<string> GameCompleted;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (MenuManager.instance != null)
        {
            MenuManager.instance.StartClicked += StartGame;
        }
        if (StrikerManager.instance != null)
        {
            StrikerManager.instance.StrikeStarted += OnStrikeStarted;
            StrikerManager.instance.StrikeFinished += OnStrikeFinished;
            StrikerManager.instance.StrikerFellIntoHole += OnStrikerFellIntoHole;
        }
    }
    private void OnStrikerFellIntoHole()
    {
        isFoul = true;
        AudioManager.instance.PlayFoulSound();
    }
    private void OnStrikeFinished()
    {
        List<string> CurrentCoins = CoinManager.instance.GetFellCoins();
        if (!isFoul)
        {
            int id = BlackAndWhiteLogic.ExecuteLogic(turnid, CurrentCoins);
            if(CoinManager.instance.TotalWhites <= 0 || CoinManager.instance.TotalBlacks <= 0)
            {
                if(CoinManager.instance.TotalRed == 0)
                {
                    if(turnid == 1 && CoinManager.instance.TotalWhites == 0)
                    {
                        GameCompleted(" You Won ");
                        // player1 win
                    }
                    else if (turnid == 1 && CoinManager.instance.TotalBlacks == 0)
                    {
                        GameCompleted(" You Lost ");
                        //player2 win
                    }
                    else if (turnid == 2 && CoinManager.instance.TotalBlacks == 0)
                    {
                        //player2 win
                        GameCompleted(" You Lost ");
                    }
                    else if (turnid == 2 && CoinManager.instance.TotalWhites == 0)
                    {
                        //player1 win
                        GameCompleted(" You Won ");
                    }
                }
                else
                {
                    if (turnid == 1)
                    {
                        // player2 win
                        GameCompleted(" You Lost ");
                    }
                    else
                    {
                        //player1 win
                        GameCompleted(" You Won ");
                    }
                }
                EndGame();


            }
            else
            {
                if (CheckForRed(CurrentCoins))
                {
                    
                    if (CheckForFollowCoin(turnid,CurrentCoins))
                    {
                        CheckForRedFollow = false;
                        AudioManager.instance.PlayRedCoveredSound();
                    }
                    else
                    {
                        CheckForRedFollow = true;
                       
                    }
                    TurnChanged(id);
                }
                else
                {
                  
                    if (CheckForRedFollow)
                    {
                        if (CheckForFollowCoin(turnid, CurrentCoins))
                        {
                            CheckForRedFollow = false;
                            Debug.Log(" follow up coin fell ");
                            AudioManager.instance.PlayRedCoveredSound();
                            TurnChanged(id);
                        }
                        else
                        {
                           
                            CoinManager.instance.PutFine("Red");
                            CheckForRedFollow = false;
                            TurnChanged(id);
                        }
                    
                    }
                    else
                    {
                        TurnChanged(id);
                    }
                }
            }
           
        }
        else
        {
            if (turnid == 1)
            {            
                CoinManager.instance.PutFine("White");
                TurnChanged(2);
            }
            else
            {             
                CoinManager.instance.PutFine("Black");   
                TurnChanged(1);
            }

            if(CheckForRed(CurrentCoins))
            {
                CoinManager.instance.PutFine("Red");
                CheckForRedFollow = false;
            }
        }

    }
    private bool CheckForRed(List<string> CurrentCoins)
    {
        bool answer = false;
        foreach(string c in CurrentCoins)
        {
            if(c == "Red")
            {
                answer = true;              
            }
        }
        return answer;
    }
    private bool CheckForFollowCoin(int id, List<string> CurrentCoins)
    {
        bool isFollowCoinFell = false;
        if(id == 1)
        {
            foreach(string c in CurrentCoins)
            {
                if(c == "White")
                {
                    isFollowCoinFell = true;
                }
            }
        }
        else
        {
            foreach (string c in CurrentCoins)
            {
                if (c == "Black")
                {
                    isFollowCoinFell = true;
                }
            }
        }
        return isFollowCoinFell;
    }
    private void OnStrikeStarted()
    {
        
    }
    private void StartGame()
    {
        CoinManager.instance.InstantiateAllCoin();
        turnid = 1;
        TurnChanged(turnid);
    }
    private void EndGame()
    {
        CoinManager.instance.DestroyAllCoins();
        InputManager.instance.DisableInput();
        MyAIManager.instance.ResetAI();
    }
    private void TurnChanged(int id)
    {
        turnid = id;
        isFoul = false;
        MyAIManager.instance.TurnChanged(turnid);
        CoinManager.instance.ClearList();
        if(turnid == 1)
        {
            InputManager.instance.EnableInput();
            StrikerManager.instance.PlaceStriker(turnid);
            StrikerManager.instance.ActivateStriker(turnid);
        }
        else
        {
            InputManager.instance.DisableInput();
            StrikerManager.instance.PlaceStriker(turnid);
            StrikerManager.instance.ActivateStriker(turnid);
            if(CoinManager.instance.TotalBlacks < 3 && CoinManager.instance.TotalRed != 0)
            {
                MyAIManager.instance.AIShouldPlay("Red");
            }
            else
            {
                MyAIManager.instance.AIShouldPlay("Black");
            }
           
        }
    }

}
