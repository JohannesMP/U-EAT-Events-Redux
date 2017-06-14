using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Required to use EventStrings
using UEAT.EventSystem;

public class EventStringExamples : MonoBehaviour
{
  // Default initialization
  public EventString EventC;
  // Initialize with a stored event string
  public EventString EventA = Events.Default.Initialize;
  // Initialize with string that does not correspond to a stored event
  public EventString EventB = "SomethingUnique";
}

