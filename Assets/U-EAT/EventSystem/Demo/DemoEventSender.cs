using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UEAT.EventSystem;

public class DemoEventSender : MonoBehaviour
{
  public EventString DemoEvent = DemoEvents.DEMO.CustomEvent;

  void Update()
  {
    gameObject.DispatchEvent(DemoEvent, null);
  }
}

