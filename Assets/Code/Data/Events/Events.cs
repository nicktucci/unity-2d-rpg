using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Events : MonoBehaviour
{
    private Dictionary<string, List<Action<GameEvent>>> dictionary = new Dictionary<string, List<Action<GameEvent>>>();
    public Action<MovementState> MoveStateChanged;

    private void Subscribe(string eventName, Action<GameEvent> listener)
    {
        if (dictionary.ContainsKey(eventName))
        {
            dictionary[eventName].Add(listener);

        }
        else
        {
            dictionary.Add(eventName, new List<Action<GameEvent>>());
            dictionary[eventName].Add(listener);

        }
    }

    private void Unsubscribe(string eventName, Action<GameEvent> listener)
    {
        if (dictionary.ContainsKey(eventName) && dictionary[eventName].Contains(listener))
        {
            dictionary[eventName].Remove(listener);
        } 
    }

    private void Emit(string eventName, GameEvent eventData)
    {
        if (dictionary.ContainsKey(eventName))
        {
            foreach (var action in dictionary[eventName])
            {
                action.Invoke(eventData);
            }
        }
    }
    public void Subscribe(Enum eventEnum, Action<GameEvent> listener)
    {
        string eventName = eventEnum.ToString();
        Subscribe(eventName, listener);
    }
    public void Unsubscribe(Enum eventEnum, Action<GameEvent> listener)
    {
        string eventName = eventEnum.ToString();
        Unsubscribe(eventName, listener);
    }

    public void Emit(MovementState state)
    {
        MoveStateChanged?.Invoke(state);
    }
    public void Emit(Enum eventName, GameEvent eventData)
    {
        Emit(eventName.ToString(), eventData);
    }

    public void Emit(Enum eventName)
    {
        Emit(eventName.ToString(), null);
        
    }

}
