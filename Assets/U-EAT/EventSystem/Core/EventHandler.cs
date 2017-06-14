/*!**************************************************************************
\file   EventHandler.cs
\par    Email: Support@U-EAT.org
\par    Developed: Summer 2016
\brief

This file contains the EventHandler class. This class manages what events
this object is listening to and what functions should be called when it
recieves an event.

\copyright © 2016-2017 CC
*****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System;


namespace UEAT.EventSystem
{
  public class EventHandler : MonoBehaviour
  {
    // Is this component visible in the inspector on every object that is listening to events?
    const bool IsVisibleInInspector = false;
    // This Dictionary stores all the different lists of functions this object is going to call when it recieves an event.
    Dictionary<string, List<Action<EventData>>> EventList = new Dictionary<string, List<Action<EventData>>>();
    
    // This Dictionary stores the wrappers for all functions without arguments
    Dictionary<Action, Action<EventData>> CallbackWrapperList = new Dictionary<Action, Action<EventData>>();
    // For fast Bi-directional lookup when removing callbacks by target
    Dictionary<Action<EventData>, Action> WrapperCallbackList = new Dictionary<Action<EventData>, Action>();

    void Awake()
    {
      if(!IsVisibleInInspector)
      {
        hideFlags = HideFlags.HideInInspector;
      }
    }

    // If the EventList does not have that event as a key, add it, then push the function to the list of functions to be called.
    public void EventConnect(string eventName, Action<EventData> func)
    {
      eventName = EventDispatch.ValidateEventName(eventName);

      if (!EventList.ContainsKey(eventName))
      {
        EventList.Add(eventName, new List<Action<EventData>>());
      }
      EventList[eventName].Add(func);
    }

    // If the EventList does not have that event as a key, add it, then push a wrapper of the function to the list of functions to be called.
    public void EventConnect(string eventName, Action func)
    {
      // If no wrapper exists, create it
      if (!CallbackWrapperList.ContainsKey(func))
      {
        Action<EventData> wrapper = (EventData ignored) => func();
        CallbackWrapperList[func] = wrapper;
        WrapperCallbackList[wrapper] = func;
      }

      EventConnect(eventName, CallbackWrapperList[func]);
    }


    // Removes ALL the functions with the given this pointer from the function list.
    public void EventDisconnect(string eventName, object thisPointer = null)
    {
      eventName = EventDispatch.ValidateEventName(eventName);

      if (!EventList.ContainsKey(eventName))
      {
        return;
      }
      var functionList = EventList[eventName];
      for (int i = 0; i < functionList.Count; ++i)
      {
        // Regular Action
        if (functionList[i].Target.Equals(thisPointer))
        {
          functionList.RemoveAt(i);
          --i;
        }
        // Action Wrapper, target is stored on original action
        else if(WrapperCallbackList.ContainsKey(functionList[i]))
        {
          object target = WrapperCallbackList[functionList[i]].Target;
          if(target.Equals(thisPointer))
          {
            functionList.RemoveAt(i);
            --i;
          }
        }
      }
      // If there are no more functions to be called for that event, remove it from the list.
      if (functionList.Count == 0)
      {
        EventList.Remove(eventName);
      }
    }

    /* Removes the first equivalent function from the function list.
      If a function is connected to be called twice, it must be disconnected twice. */
    public void EventDisconnect(string eventName, Action<EventData> func)
    {
      eventName = EventDispatch.ValidateEventName(eventName);

      if (!EventList.ContainsKey(eventName))
      {
        return;
      }
      var functionList = EventList[eventName];
      for (int i = 0; i < functionList.Count; ++i)
      {
        if (functionList[i] == func)
        {
          functionList.RemoveAt(i);
          break;
        }
      }
      // If there are no more functions to be called for that event, remove it from the list.
      if (functionList.Count == 0)
      {
        EventList.Remove(eventName);
      }
    }


    public void EventDisonnect(string eventName, Action func)
    {
      if (!CallbackWrapperList.ContainsKey(func))
        return;

      EventDisconnect(eventName, CallbackWrapperList[func]);
    }

    // Calls all the functions associated with the given event, and passes them the given EventData.
    public void EventSend(string eventName, EventData eventData = null)
    {
      eventName = EventDispatch.ValidateEventName(eventName);

      if (!EventList.ContainsKey(eventName))
      {
        return;
      }
      if (eventData == null)
      {
        eventData = EventDispatch.DefaultEvent;
      }
      var functionList = EventList[eventName];
      for (var i = 0; i < functionList.Count; ++i)
      {
        var func = functionList[i];

        if (func.Method.IsStatic || !func.Target.Equals(null))
        {
          func(eventData);
        }
        else
        {
          // Remove any invalid functions.
          functionList.RemoveAt(i);
          --i;
        }
      }
    }
    // Clears the Dictionary of all events.
    public void DisconnectAll()
    {
      EventList.Clear();
    }   
  }
}