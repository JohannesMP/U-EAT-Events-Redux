using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class EventReceiver : MonoBehaviour
{
  public EventString MyEvent = "MyEvent";

  void Start()
  {
    gameObject.Connect(MyEvent, OnMyEvent);
    gameObject.Disconnect(MyEvent, this);
  }

  void OnEmptyEvent()
  {

  }

  void OnMyEvent(EventData evt)
  {
    Debug.Log("MyEventReceived");
  }
}

