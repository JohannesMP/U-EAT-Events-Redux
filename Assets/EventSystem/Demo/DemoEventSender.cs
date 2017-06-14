using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UEAT.EventSystem;

public class DemoEventSender : MonoBehaviour
{
  public EventString DemoEvent = Events.DEMO.CustomEvent;

  void Update()
  {
    gameObject.DispatchEvent(DemoEvent, null);
  }
}

