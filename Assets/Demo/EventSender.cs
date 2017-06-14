using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class EventSender : MonoBehaviour
{
  public EventString MyEvent = "MyEvent";

  void Update()
  {
    gameObject.DispatchEvent(MyEvent, null);
  }
}

