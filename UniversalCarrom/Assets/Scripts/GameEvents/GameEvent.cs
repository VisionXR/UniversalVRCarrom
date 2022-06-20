using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]

public class GameEvent : ScriptableObject
{
    private List<GameEventListener> Listeners = new List<GameEventListener>();
    public void Register(GameEventListener listner)
    {
        if (!Listeners.Contains(listner))
        {
            Listeners.Add(listner);
        }
    }
    public void UnRegister(GameEventListener listner)
    {
        if (Listeners.Contains(listner))
        {
            Listeners.Remove(listner);
        }
    }
    public void RaiseEvent()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].OnEventRaised();
        }
    }
}

