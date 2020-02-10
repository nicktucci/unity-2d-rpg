using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Events : MonoBehaviour
{
    private Dictionary<string, List<Action<object>>> dictionary = new Dictionary<string, List<Action<object>>>();
    public Action<MovementState> MoveStateChanged;

    private void Subscribe(string eventName, Action<object> listener)
    {
        if (dictionary.ContainsKey(eventName))
        {
            dictionary[eventName].Add(listener);

        }
        else
        {
            dictionary.Add(eventName, new List<Action<object>>());
            dictionary[eventName].Add(listener);

        }
    }

    private void Unsubscribe(string eventName, Action<object> listener)
    {
        if (dictionary.ContainsKey(eventName) && dictionary[eventName].Contains(listener))
        {
            dictionary[eventName].Remove(listener);
        } 
    }

    private void Emit(string eventName, object data)
    {
        if (dictionary.ContainsKey(eventName))
        {
            foreach (var action in dictionary[eventName])
            {
                action.Invoke(data);
            }
        }
    }
    public void Subscribe(Enum eventEnum, Action<object> listener)
    {
        string eventName = eventEnum.ToString();
        Subscribe(eventName, listener);
    }
    public void Unsubscribe(Enum eventEnum, Action<object> listener)
    {
        string eventName = eventEnum.ToString();
        Unsubscribe(eventName, listener);
    }

    public void Emit(MovementState state)
    {
        MoveStateChanged?.Invoke(state);
    }
    public void Emit(Enum eventName, object data)
    {
        Emit(eventName.ToString(), data);
    }

    public void Emit(Enum eventName)
    {
        Emit(eventName.ToString(), null);
    }

}
