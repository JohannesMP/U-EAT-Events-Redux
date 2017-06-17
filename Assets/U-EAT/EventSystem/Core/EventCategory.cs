/*!**************************************************************************
\file   EventCategory.cs
\author Filiecs
\author JohannesMP
\brief 

Maintains Categories for events.

This class marks any class that derives from it (as well as nested classes)
as candidates for defining a category of events.


\details
  To be registered as valid events a string must be
    1. Defined as public static readonly
    2. In (or have in its class tree) a class that inherits from EventCategory
       (which partial class Events is)
    3. In a class that has a static consttructor that calls InitAll().

  Side effects of being a valid event string:
    1. If Empty/Null/Undefined, will be initialized with its variable name.
    2. Will have its string value prefixed by its category.

FAQ:
    Q:  What happens with uninitialized (or empty) event strings?
    A:  The InitAll() call will use reflection to iterate over all string fields, 
        and for each uninitialized string field will assign its name as the value 
        of the field, preceded by its category.

    Q:  Why are strings readonly instead of const?
    A:  readonly values are computed at runtime. See http://bit.ly/1WT8tmB
        In this instance, it means InitAll() can initialize their values.

    Q:  Why InitAll() in static ctr?
    A:  This ensures you can use events even in other static constructor. 
        Because the event strings are static, the static ctr is guaranteed to be
        called before the a potentially uninitialized static value can be accessed.

    Q:  What else does InitAll() do?
    A:  Other than setting all the strings, it also then stores a mapping of each 
        event 'category' (a string constructed based on the class hierarchy) to
        the event strings that are defined.
        - The point of this is for custom editor inspectors for selecting events.

\copyright © 2016-2017 CC
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace UEAT.EventSystem
{
  public class EventCategory
  {
    static EventCategory() { InitAll(); }
    
    // For use in static functions without access to GetType()
    static System.Type BaseType = typeof(EventCategory);

    // When true will not consider the base class of event definitions a category
    // Example: 'Events' will not show up as a category
    static readonly bool IgnoreBaseClassCategories = true;

    // What we use to separate the category from the eventName
    const char CategoryDivider = '.';

    // Map category/namespace to events definitions. Value is cached
    private static Dictionary<string, Dictionary<string, string>> _eventCategoryMap = null;
    public static Dictionary<string, Dictionary<string, string>> EventCategoryMap
    {
      get { InitAll(); return _eventCategoryMap; }
    }
    
    // Lazy initialized List of event categories
    private static string[] _eventCategories = null;
    public static string[] GetEventCategories()
    {
      if(_eventCategories == null)
      {
        List<string> categories = new List<string>();
        foreach(var pair in EventCategoryMap)
        {
          categories.Add(pair.Key);
        }
        _eventCategories = categories.ToArray();
      }

      return _eventCategories;
    }

    // Look up the events stored (key and value) in a given event category
    public static Dictionary<string, string> GetEventDictInCategory(string category)
    {
      if (!EventCategoryMap.ContainsKey(category))
        return new Dictionary<string, string>();

      return EventCategoryMap[category];
    }

    // Look up the events stored (just keys) in a given event category
    public static string[] GetEventNamesInCategory(string category)
    {
      List<string> toRet = new List<string>();
      foreach(var pair in GetEventDictInCategory(category))
      {
        toRet.Add(pair.Key);
      }
      return toRet.ToArray();
    }

    // Look up the events stored (just values) in a given event category
    public static string[] GetEventStringsInCategory(string category)
    {
      List<string> toRet = new List<string>();
      foreach (var pair in GetEventDictInCategory(category))
      {
        toRet.Add(pair.Value);
      }
      return toRet.ToArray();
    }


    // Look up the event string for an event in a given event category
    public static string GetEventNameInCategory(string category, string eventName)
    {
      if (EventCategoryMap.ContainsKey(category))
      {
        if(EventCategoryMap[category].ContainsKey(eventName))
        {
          return EventCategoryMap[category][eventName];
        }
      }
      return null;
    }

    // Try to find the category for a given event
    public static string GetCategoryFromEventString(string concatString)
    {
      // First assume the string is formated as <CATEGORY><CategoryDivider><EVENT>
      int categoryIndex = concatString.LastIndexOf(CategoryDivider);
      if(categoryIndex > 0)
      {
        var category = concatString.Substring(0, categoryIndex);

        if (EventCategoryMap.ContainsKey(category))
          return category;
      }

      // Otherwise check if the user just provided <CATEGORY> with no divider/event
      if (EventCategoryMap.ContainsKey(concatString))
        return concatString;

      return null;
    }

    // Given a string formatted as <CATEGORY><CategoryDivider><EVENT>, extract the 'event'
    public static string GetEventNameFromEventString(string concatString)
    {
      int categoryIndex = concatString.LastIndexOf(CategoryDivider) + 1;
      if (categoryIndex > 1 && categoryIndex < concatString.Length)
      {
        var eventName = concatString.Substring(categoryIndex);
        return eventName;
      }

      return concatString;
    }

    public static string ConstructEventString(string category, string eventName)
    {
      return category + CategoryDivider + eventName;
    }


    // Initialize all event Categories
    protected static void InitAll()
    {
      // Guarantee that we only init once
      if (_eventCategoryMap != null)
      {
        return;
      }
      _eventCategoryMap = new Dictionary<string, Dictionary<string, string>>();

      // All types that represent event categories
      HashSet<System.Type> types = new HashSet<System.Type>();

      Assembly assembly = Assembly.Load(new AssemblyName("Assembly-CSharp"));

      // Start by grabbing all the base classes
      System.Type[] toCheck = assembly
        .GetTypes()
        .Where(
          type =>
            type.IsClass &&
            type.IsSubclassOf(BaseType) &&
            (type.IsPublic || type.IsNestedPublic)
        ).ToArray();

      // Iteratively walk through all nested classes
      while(toCheck.Length > 0)
      {
        // For stiring any new types we find this run
        List<System.Type> newFound = new List<System.Type>();

        // Check all nested types
        foreach(System.Type type in toCheck)
        {
          // Store new nested types
          newFound.AddRange(type.GetNestedTypes(BindingFlags.Public));

          // Skip storing base types if necessary
          if (IgnoreBaseClassCategories && type.IsSubclassOf(BaseType))
            continue;

          // Store the already nested classes we just checked
          types.Add(type);
        }

        // The types we found are checked next iteration
        toCheck = newFound.ToArray();
      }

      // Init and register each eventdefinitions container
      foreach (var type in types)
      {
        InitEventCategory(type);
      }
    }

    // Sets all static string values equal to their own name
    // Then stores them in the EventCategoryMap
    private static void InitEventCategory(System.Type eventCategoryType)
    {
      // Compute the category string based on what types it is nested inside of
      string category = GetEventCategory(eventCategoryType);

      if (!EventCategoryMap.ContainsKey(category))
        EventCategoryMap[category] = new Dictionary<string, string>();
      else
        throw new UnityException("Duplicate EventCategory class named '" + category + "'");

      // Get all string fields
      var fields = eventCategoryType
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(
          f => 
            f.IsStatic && 
            f.IsPublic &&
            f.IsInitOnly &&
            f.FieldType == typeof(string)
        ).ToArray();

      // No members at this level, don't bother showing it
      if(fields.Length == 0)
      {
        EventCategoryMap.Remove(category);
        return;
      }

      // Init all string fields that need it
      foreach (var field in fields)
      {
        string value = (string)field.GetValue(null);

        if (string.IsNullOrEmpty(value))
          value = field.Name;

        // Prefix the field's stored value with the category to avoid ambiguity (ex Events.A.Init and Events.B.Init)
        value = ConstructEventString(category, value);

        field.SetValue(null, value);

        // Store Event strings
        if (!EventCategoryMap[category].ContainsKey(field.Name))
          EventCategoryMap[category][field.Name] = value;
        // Really this should never happen...
        else
          throw new UnityException("Duplicate event string in category '" + category + "' named '" + field.Name);
      }
    }

    // Given a type (container for event definitions) construct a category string
    private static string GetEventCategory(System.Type containerType)
    {
      string path = containerType.FullName;

      // Walk until we find the base container
      System.Type tempType = containerType;
      while (tempType.DeclaringType != null && tempType.BaseType != BaseType)
      {
        tempType = tempType.DeclaringType;
      }

      // Not base container - containerType was not nested correctly (another example of something that should not happen)
      if (tempType.BaseType != BaseType)
        throw new UnityException(containerType + " is not nested inside a " + BaseType + " class!");

      // The passed in container was a base class
      if(tempType == containerType)
      {
        if (IgnoreBaseClassCategories)
          path = "";
        else
          path = containerType.Name;
      }
      // The passed in container was nested in a base class
      else
      {
        if (IgnoreBaseClassCategories)
          path = path.Substring(tempType.FullName.Length + 1);
        else
          path = tempType.Name + path.Substring(tempType.FullName.Length);
      }

      return path.Replace('+', '.');
    }

  }

  // By setting up Events to inherit from EventCategory the event
  // system will know about its nested event strings
  public partial class Events : EventCategory
  {
    static Events() { InitAll(); }
  }

} // namespace FTQ.Events
