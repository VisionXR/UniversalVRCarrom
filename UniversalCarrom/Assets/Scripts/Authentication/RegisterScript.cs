
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

[CreateAssetMenu()]
public class RegisterScript : ScriptableObject
{
    public string RegiserFailedReason;
    public bool isRegistered;
    public void RegisterToPlayfab(string Username, string Email, string Password)
    {
 
        var request = new RegisterPlayFabUserRequest
        {
            Username = Username,
            Email = Email,
            Password = Password,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailed);
    }

    private void OnRegisterFailed(PlayFabError obj)
    {
        RegiserFailedReason = obj.ToString();
        isRegistered = false;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        if (isRegistered == false)
        {
            isRegistered = true;
        }
    }
}
