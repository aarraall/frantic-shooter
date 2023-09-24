using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    public enum EventSignature
    {
        //Add your event callouts here
        OnGameStart, 
        OnGameEnd, 
        OnGamePause, 
        OnGameResume,
        OnLevelStart,
        OnPlayerDefeated,
        OnPlayerWin,
    }
    private static readonly Dictionary<EventSignature, Action<object>> eventDictionary
        = new();


    public static void StartListening(EventSignature eventName, Action<object> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventSignature eventName, Action<object> listener)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(EventSignature eventName, object eventData = null)
    {
        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
        {
            thisEvent.Invoke(eventData);
        }
    }
}