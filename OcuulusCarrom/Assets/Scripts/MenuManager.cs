using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject StartPanel,HomePanel,InputPanel;
    public event Action StartClicked;
    public GraphicRaycaster gr;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InputManager.instance.MouseClicked += CheckRaycast;
        InputManager.instance.InputEnabled += OnInputEnabled;
        GameManager.instance.GameCompleted += OnGameCompleted;
        EnablePanel(StartPanel);
    }

    private void OnInputEnabled(bool obj)
    {
        if(obj)
        {
            EnablePanel(InputPanel);
        }
        else
        {
            DisablePanel(InputPanel);
        }
    }

    private void OnGameCompleted(string obj)
    {
        EnablePanel(HomePanel);
    }

    void CheckRaycast(Vector2 pos)
    {
     
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        foreach(RaycastResult r in results)
        {
           if( r.gameObject.name == "StartButton")
            {
                StartButtonClicked();
             
            }
        }
    }

    public void StartButtonClicked()
    {       
        if (StartClicked != null)
        {
            DisablePanel(StartPanel);
            StartClicked();
        }   
    }

    public void OnHomeButtonClicked()
    {
        DisablePanel(HomePanel);
        EnablePanel(StartPanel);
    }
    private void EnablePanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    private void DisablePanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
