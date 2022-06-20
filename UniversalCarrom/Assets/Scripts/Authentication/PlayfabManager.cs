using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class PlayfabManager : ScriptableObject
{
    public RegisterScript registerScript;
    public string UserName, Password, Email;
  
    public void Register()
    {
        registerScript.RegisterToPlayfab(UserName, Email, Password);
    }
    

}
