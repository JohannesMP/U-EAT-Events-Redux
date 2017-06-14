using UnityEngine;

namespace UEAT.EventSystem
{
  [System.Serializable]
  public partial class EventString
  {
#if UNITY_EDITOR
    // Whether or not to display a text input box or a dropdown menu.
    public bool AsString = false;
    // The category this event was located under
    public string Category;
#endif
    public string EventName;

    private void SetEventName(string eventName)
    {
      EventName = eventName;
#if UNITY_EDITOR
      // Try to find the category
      Category = EventCategory.GetCategoryForEventString(EventName);
      if (string.IsNullOrEmpty(Category))
        AsString = true;
#endif
    }

    public EventString()
    {
      SetEventName(Events.Default.DefaultEvent);
    }

    public EventString(string eventName)
    {
      SetEventName(eventName);
    }

    // This class is essentially a string with a fancy inspector.
    public static implicit operator string(EventString value)
    {
      return value.EventName;
    }

    public static implicit operator EventString(string value)
    {
      return new EventString(value);
    }
  }

} // namespace EventSystem
