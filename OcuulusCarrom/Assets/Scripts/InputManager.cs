using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public event Action<string,float> UserSwiped;
    public event Action<Vector2> MouseClicked;
    public event Action<bool> InputEnabled;
    public event Action<float> UserRotated,UserClicked;
    public bool isInputEnabled = false,CanIStrike = false;
    private bool isClock = false;
    public float LL = 1f, UL = 5;
    public Slider s;
    private float force = 3;
    private Coroutine ForceRoutine;
    private void Awake()
    {
        instance = this;
    }

    public void GiveVibrationFeedBack()
    {
        OVRInput.SetControllerVibration(1, 0.2f);
    }
    public void EnableInput()
    {
        isInputEnabled = true;
        InputEnabled(isInputEnabled);
    }
    public void DisableInput()
    {
        isInputEnabled = false;
        InputEnabled(isInputEnabled);
    }
    void Update()
    {
        if (isInputEnabled)
        {
            ProcessOculusInputs();
            ProcessKeyboardInputs();
        }
        ProcessMouseInputs();
    }
    private void ProcessMouseInputs()
    {
        if (Input.GetMouseButton(0))
        {
            if (MouseClicked != null)
            {
                MouseClicked(Input.mousePosition);
            }
        }
        if (Input.GetMouseButton(1))
        {
            //MyAIManager.instance.AIShouldPlay("Black");
        }
    }
    private void ProcessOculusInputs()
    {
        Vector2 Movedir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (Movedir.x != 0)
        {
            if (Movedir.x > 0)
            {
                UserSwiped("RIGHT", Movedir.x * 0.02f);
            }
            else if(Movedir.x < 0)
            {
                UserSwiped("LEFT", -Movedir.x * 0.02f);
            }
        }
        Vector2 Rotdir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (Rotdir.x != 0  )
        {            
            UserRotated(Rotdir.x * 0.75f);                  
        }
        float value = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        if(value > 0.9f && !CanIStrike)
        {            
            CanIStrike = true;
            ForceRoutine = StartCoroutine(ForceSHM());
        }
        if(value < 0.1f && CanIStrike)
        {      
            UserClicked(force);
            GiveVibrationFeedBack();
            StopCoroutine(ForceRoutine);
            CanIStrike = false;
        }
        
    }
    private void ProcessKeyboardInputs()
    {
        float x = Input.GetAxis("Horizontal");
        if (x != 0)
        {
            if (x > 0)
            {
                UserSwiped("RIGHT", x * 0.03f);
            }
            else
            {
                UserSwiped("LEFT", -x * 0.03f);
            }
        }
        float y = Input.GetAxis("Vertical");
        if (y != 0)
        {        
            UserRotated(y * 0.5f);          
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            UserClicked(4);
        }
    }

    public void OnForceChanged()
    {
        force = s.value;
    }
    private IEnumerator ForceSHM()
    {
        while (true)
        {       
            if (force < UL && isClock)
            {
                force += 0.15f;
                s.value = force;
            }
            else if (force > LL && !isClock)
            {
                force -= 0.15f;
                s.value = force;
            }
            else if (force >= UL)
            {
                isClock = false;
            }
            else if (force <= LL)
            {
                isClock = true;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
