using System;
using UnityEngine;

public class StrikerManager : MonoBehaviour
{

    public GameObject Striker1, Striker2;
    public static StrikerManager instance;
    public event Action StrikeStarted, StrikeFinished,StrikerFellIntoHole;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InputManager.instance.UserSwiped += MoveStriker;
        InputManager.instance.UserRotated += RotateStriker;
        Striker1.GetComponent<StrikerHitting>().StrikeFinished += OnStrikeFinished;
        Striker2.GetComponent<StrikerHitting>().StrikeFinished += OnStrikeFinished;
        Striker1.GetComponent<StrikerHitting>().StrikeStarted += OnStrikeStarted;
        Striker1.GetComponent<StrikerCollision>().StrikerFellIntoHole += OnStrikerFellIntoHole;
        Striker2.GetComponent<StrikerCollision>().StrikerFellIntoHole += OnStrikerFellIntoHole;
        InputManager.instance.UserClicked += FireStriker;

    }

    private void MoveStriker(string arg1, float arg2)
    {
        Striker1.GetComponent<StrikerMovement>().MoveStriker(arg1, arg2);
    }

    private void RotateStriker(float obj)
    {
        Striker1.GetComponent<StrikerMovement>().RotateStriker(obj);
    }

    private void FireStriker(float obj)
    {
        Striker1.GetComponent<StrikerHitting>().FireStriker(obj);
    }

    private void OnStrikerFellIntoHole()
    {
        if(StrikerFellIntoHole != null)
        {
            StrikerFellIntoHole();
        }
    }
    private void OnStrikeStarted()
    {
        InputManager.instance.DisableInput();
    }

    public void PlaceStriker(int id)
    {
        if (id == 1)
        {
            Striker1.GetComponent<StrikerMovement>().PlaceStriker();
        }
        else
        {
            Striker2.GetComponent<AIStrikerMovement>().PlaceStriker();
        }
    }
    private void OnStrikeFinished()
    {
        if (StrikeStarted != null)
        {
            StrikeFinished();
        }
    }

    public void ActivateStriker(int id)
    {
        if(id == 1)
        {
            Striker1.SetActive(true);
            Striker2.SetActive(false);
        }
        else
        {
            Striker2.SetActive(true);
            Striker1.SetActive(false);
        }
    }


    public void StrikeFired(int id,float force)
    {
        if (id == 1)
        {
            Striker1.GetComponent<StrikerHitting>().FireStriker(force);
        }
        else
        {
            Striker2.GetComponent<StrikerHitting>().FireStriker(force);
        }
    }
}
