/*!**************************************************************************
\file   EventString.cs
\author Filiecs
\author JohannesMP
\brief

This class represents a single Event string to be used for sending events.

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
    public string StoredString;

    private void SetEventName(string eventName)
    {
      StoredString = eventName;
    }

    public EventString()
    {
      SetEventName(Events.Common.Default);
    }

    public EventString(string eventName)
    {
      SetEventName(eventName);
    }

    // This class is essentially a string with a fancy inspector.
    public static implicit operator string(EventString value)
    {
      return value.StoredString;
    }

    public static implicit operator EventString(string value)
    {
      return new EventString(value);
    }


  }

} // namespace EventSystem
