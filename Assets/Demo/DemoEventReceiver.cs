using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class DemoEventReceiver : MonoBehaviour
{
  public EventString DemoEvent = Events.DEMO.CustomEvent;

  void Start()
  {
    gameObject.Connect(DemoEvent, OnMyEvent);
  }

  void OnMyEvent()
  {
    Debug.Log("EventReceived");
  }

}

