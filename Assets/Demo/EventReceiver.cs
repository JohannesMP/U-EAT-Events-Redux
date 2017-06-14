using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class EventReceiver : MonoBehaviour
{
  public EventString MyEvent = Events.CustomEventCategory.CustomEventCategoryEvent;

  void Start()
  {
    gameObject.Connect(MyEvent, OnMyEvent);
  }

  void OnMyEvent()
  {
    Debug.Log("EventReceived");
  }

}

