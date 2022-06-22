using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public GameObject StartPanel,HomePanel,InputPanel,DifficultyPanel,currentPanel;
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
        currentPanel = StartPanel;
        ResetPanels();
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
            else if (r.gameObject.name == "EasyButton")
            {
                DifficultyButtonClicked(1);
            }
            else if (r.gameObject.name == "MediumButton")
            {
                DifficultyButtonClicked(2);
            }
            else if (r.gameObject.name == "HardButton")
            {
                DifficultyButtonClicked(3);
            }
        }
    }
    public void StartButtonClicked()
    {             
       EnablePanel(DifficultyPanel);       
    }
    public void DifficultyButtonClicked(int id)
    {      
       DisablePanel(DifficultyPanel);
       MyAIManager.instance.ActivateAI(id);
       StartCoroutine(WaitAndStart());
    }
    private IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(1);
        StartClicked();
    }
    public void OnHomeButtonClicked()
    {
        EnablePanel(StartPanel);
    }
    private void EnablePanel(GameObject panel)
    {
        DisablePanel(currentPanel);
        currentPanel = panel;
        panel.GetComponent<CanvasGroup>().alpha = 1;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    private void DisablePanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().alpha = 0;
        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void ResetPanels()
    {
        DisablePanel(StartPanel);
        DisablePanel(DifficultyPanel);
        DisablePanel(HomePanel);
    }
}
