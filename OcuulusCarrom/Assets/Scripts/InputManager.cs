using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public event Action<string,float> UserSwiped;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessOculusInputs();
        ProcessKeyboardInputs();
    }

    private void ProcessOculusInputs()
    {
        Vector2 dir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        if (dir.x != 0)
        {
            if (dir.x > 0)
            {
                UserSwiped("RIGHT", dir.x * 0.03f);
            }
            else
            {
                UserSwiped("LEFT", -dir.x * 0.03f);
            }
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
    }



}
