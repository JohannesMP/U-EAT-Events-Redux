using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class DemoEventStringExamples : MonoBehaviour
{
  // Default initialization
  public EventString EventA;
  // Initialize with a stored event string
  public EventString EventB = Events.Input.Mouse.MouseDown;
  // Initialize with string that does not correspond to a stored event
  public EventString EventC = "SomethingUnique";
}

