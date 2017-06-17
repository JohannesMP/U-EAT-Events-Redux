/*!**************************************************************************
\file   EventDispatch.cs
\author Filiecs
\author JohannesMP
\brief

This file contains the static EventDispatch class. The methods in this class
make interfacing with the EventHandler components easier.

\copyright © 2016-2017 CC
*****************************************************************************/
using System;
using UnityEngine;

namespace UEAT.EventSystem
{

  public static class EventDispatch
  {
    // An empty EventData object to be used when no data needs to be passed.
    public static EventData DefaultEvent = new EventData();

    // If true, event sending/connecting is case insensitive
    public static readonly bool IgnoreEventCase = false;

    // Any eventNames passed into the event system 
    public static string ValidateEventName(string eventName)
    {
      return IgnoreEventCase ? eventName.ToLower() : eventName;
    }
    

    public static void EventConnect(GameObject target, string eventName, Action<EventData> func)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        targetHandler = target.AddComponent<EventHandler>();
      }
      targetHandler.EventConnect(eventName, func);
    }

    public static void EventConnect(GameObject target, string eventName, Action func)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        targetHandler = target.AddComponent<EventHandler>();
      }
      targetHandler.EventConnect(eventName, func);
    }

    public static void EventDisconnect(GameObject target, String eventName, object thisPointer = null)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        return;
      }
      targetHandler.EventDisconnect(eventName, thisPointer);
    }

    public static void EventDisconnect(GameObject target, string eventName, Action<EventData> func)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        return;
      }
      targetHandler.EventDisconnect(eventName, func);
    }

    public static void EventDisconnect(GameObject target, string eventName, Action func)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        return;
      }
      targetHandler.EventDisconnect(eventName, func);
    }

    public static void EventSend(GameObject target, string eventName, EventData eventData = null)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        return;
      }
      targetHandler.EventSend(eventName, eventData);
    }

    public static void DisconnectObject(GameObject target)
    {
      var targetHandler = target.GetComponent<EventHandler>();
      if (!targetHandler)
      {
        return;
      }
      targetHandler.DisconnectAll();
    }

    // Extension Methods to the GameObject class.

    // Send event data
    public static void DispatchEvent(this GameObject target, string eventName, EventData eventData = null)
    {
      EventSend(target, eventName, eventData);
    }

    // Connect Callback with EventData arg
    public static void Connect(this GameObject target, string eventName, Action<EventData> func)
    {
      EventConnect(target, eventName, func);
    }
    // Connect Callback with no arg
    public static void Connect(this GameObject target, string eventName, Action func)
    {
      EventConnect(target, eventName, func);
    }

    // Disconnect Callback with EventData arg
    public static void Disconnect(this GameObject target, string eventName, Action<EventData> func)
    {
      EventDisconnect(target, eventName, func);
    }
    // Disconnect Callback with no arg
    public static void Disconnect(this GameObject target, string eventName, Action func)
    {
      EventDisconnect(target, eventName, func);
    }
    // Disconnect all Callbacks for the given event on the given object
    public static void Disconnect(this GameObject target, string eventName, object funcThisPointer)
    {
      EventDisconnect(target, eventName, funcThisPointer);
    }
    // Disconnect all Callbacks on the given object
    public static void DisconnectAll(this GameObject target)
    {
      DisconnectObject(target);
    }
  }

}

