using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent GameEventName;
    public UnityEvent Response;
    public void OnEventRaised()
    {
        Response.Invoke();
    }
    private void OnEnable()
    {
        GameEventName.Register(this);
    }
    private void OnDisable()
    {
        GameEventName.UnRegister(this);
    }

}

