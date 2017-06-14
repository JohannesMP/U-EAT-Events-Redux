using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Events don't need to be in UEAT namespace, but must have
// Base class that inherits from UEAT.EventSystem.EventCategory
public partial class DemoEvents : UEAT.EventSystem.EventCategory
{
  // DemoEvents.DEMO
  public class DEMO
  {
    static DEMO() { InitAll(); }

    public static readonly string CustomEvent;
    // Add events Here
  }
}