/*!**************************************************************************
\file   EventString.cs
\author Filiecs
\author JohannesMP
\brief

This class represents a single Event string to be used for sending events.
It is basically just a string with a fancy inspector.

\copyright © 2016-2017 CC
*****************************************************************************/

using UnityEngine;

namespace UEAT.EventSystem
{
  [System.Serializable]
  public partial class EventString
  {
#if UNITY_EDITOR
    // Whether or not to display a text input box or a dropdown menu.
    public bool AsString = false;
#endif
    public string Value;


    public EventString()
    {
      Value = Events.Common.Default;
    }

    public EventString(string eventString)
    {
      Value = eventString;
    }


    public string GetEventName()
    {
      return EventCategory.GetEventNameFromEventString(Value);
    }

    public string GetEventCategory()
    {
      return EventCategory.GetCategoryFromEventString(Value);
    }


    public static implicit operator string(EventString value)
    {
      return value.Value;
    }

    public static implicit operator EventString(string value)
    {
      return new EventString(value);
    }


  }

} // namespace EventSystem
