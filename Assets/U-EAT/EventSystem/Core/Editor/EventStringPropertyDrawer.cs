/*!**************************************************************************
\file   EventStringPropertyDrawer.cs
\author Filiecs
\author JohannesMP
\brief

The custom editor drawer for EventString objects.
When created as a public variable displays a UI for manually entering an
event string, or choosing from a Category/Event dropdown.

\copyright © 2016-2017 CC
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// A custom property drawer for the 'EventString' class.
// It can toggle between a string input field and a dropdown menu of all the events by category
namespace UEAT.EventSystem
{
  // Custom values for how the property scales when the inspector is resized.
  // These values very closely match with Unity's.
  public static class InspectorValues
  {
    // The width of an average text label.
    public static readonly float LabelWidth = 120;
    // The minimum possible width before clipping will occur.
    public static readonly float MinRectWidth = 340;
    // How fast the property scales with the width of the window.
    public static readonly float WidthScaler = 2.21f;
    // The general amount of padding from the inner edge of the inspector. (Towards the center of the screen)
    public static readonly float EdgePadding = 14;
    // The height of a single inspector row
    public static readonly float RowHeight = 16;
  }

  [CustomPropertyDrawer(typeof(EventString))]
  public class EventStringPropertyDrawer : PropertyDrawer
  {
    // Will hide the Category/Event dropdowns when in string mode
    static bool CompactWhenString = false;
    // The width of the toggle button.
    const float ToggleWidth = 60;
    // The margins around the property box
    const float PropertyBoxMargins = 3;
    // What fraction of the width Is used by the category field (vs what is used by the event field)
    const float CategoryWidthPercent = 1.0f / 3.0f;

    // How many lines the GUI for this field is tall.
    const float RowCount = 3;
    // Cached event categories
    static readonly string[] EventCategories;
    // For right-aligned labels
    static readonly GUIStyle LabelStyle;

    static EventString LastEventString = new EventString();

    static EventStringPropertyDrawer()
    {
      LabelStyle = new GUIStyle(GUI.skin.label);
      LabelStyle.alignment = TextAnchor.UpperRight;
      EventCategories = EventCategory.GetEventCategories();
      System.Array.Sort(EventCategories);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var eventNameRef = property.FindPropertyRelative("Value");
      var asStringRef = property.FindPropertyRelative("AsString");

      LastEventString.AsString = asStringRef.boolValue;
      LastEventString.Value = eventNameRef.stringValue;
      LastEventString = Draw(position, LastEventString, label);

      eventNameRef.stringValue = LastEventString.Value;
      asStringRef.boolValue = LastEventString.AsString;
      property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      bool asString = property.FindPropertyRelative("AsString").boolValue;

      // Compact string view, only a single row and margins
      if (CompactWhenString && asString)
        return InspectorValues.RowHeight + PropertyBoxMargins * 2;
      // Top, bottom and divider between text field and dropdowns
      else
        return InspectorValues.RowHeight * RowCount + PropertyBoxMargins*3; 
    }

    static public EventString Draw(Rect position, object eventStringObj, GUIContent content)
    {
      EventString eventString = eventStringObj as EventString;

      // Where the Left and Right UI area separate
      var labelDivider = position.x + InspectorValues.LabelWidth;
      // Scale dynamically when wide enuogh
      labelDivider += Mathf.Max(0, (position.width - InspectorValues.MinRectWidth) / InspectorValues.WidthScaler);


      // The Left Label Area
      var labelRect = position;
      labelRect.xMax = labelDivider;
      EditorGUI.SelectableLabel(labelRect, content.text);

      // The right UI Area
      var propBoxRect = position;
      propBoxRect.xMin = labelDivider;
      var boxStyle = new GUIStyle(EditorStyles.helpBox);
      // Use that to draw a box
      GUI.BeginGroup(propBoxRect, boxStyle);
      GUI.EndGroup();


      // Add margin for the box
      var propRect = propBoxRect;
      propRect.xMin += PropertyBoxMargins;
      propRect.xMax -= PropertyBoxMargins;
      propRect.yMin += PropertyBoxMargins;
      propRect.yMax += PropertyBoxMargins;
      propRect.height = InspectorValues.RowHeight;

      // The left Properties
      var leftPropRect = propRect;
      leftPropRect.width = ToggleWidth;

      // The right Properties
      var rightPropRect = propRect;
      rightPropRect.xMin = leftPropRect.xMax;


      // First Property Row
      {
        var ToggleLabel = eventString.AsString ? "String" : "Type";
        eventString.AsString = EditorGUI.ToggleLeft(leftPropRect, ToggleLabel, eventString.AsString, LabelStyle);

        EditorGUI.BeginDisabledGroup(!eventString.AsString);
        eventString.Value = EditorGUI.TextField(rightPropRect, eventString.Value);
        EditorGUI.EndDisabledGroup();

        // Early exit if configured to not show dropdowns when in string mode
        if (CompactWhenString && eventString.AsString)
          return eventString;
      }


      // Second and Third Property row are disabled when string mode is active
      EditorGUI.BeginDisabledGroup(eventString.AsString);
      {
        var category = eventString.GetEventCategory();

        // Second Property Row
        {
          leftPropRect.y += InspectorValues.RowHeight + PropertyBoxMargins;
          rightPropRect.y += InspectorValues.RowHeight + PropertyBoxMargins;

          EditorGUI.LabelField(leftPropRect, "Category", LabelStyle);

          // Check if the currently stored category is in the list of categories (since we need the index for the dropdown array)
          int categoryIndex = IndexOf(EventCategories, category);

          // When not found add default and select it
          if (categoryIndex < 0)
          {
            categoryIndex = EditorGUI.Popup(rightPropRect, 0, ConcatArrays(new string[] { "-" }, EventCategories)) - 1;
          }
          else
          {
            categoryIndex = EditorGUI.Popup(rightPropRect, categoryIndex, EventCategories);
          }

          // User selected a valid category
          if (categoryIndex >= 0)
          {
            // And a category that is different than before
            if(!EventCategories[categoryIndex].Equals(category))
            {
              category = EventCategories[categoryIndex];
              eventString.Value = EventCategory.ConstructEventString(category, "");
            }
          }
        }

        // Third Property Row
        {
          leftPropRect.y += InspectorValues.RowHeight;
          rightPropRect.y += InspectorValues.RowHeight;

          EditorGUI.BeginDisabledGroup(category == null);
          EditorGUI.LabelField(leftPropRect, "Event", LabelStyle);

          // No valid category selected yet
          if(category == null)
          {
            EditorGUI.Popup(rightPropRect, 0, new string[] { "-" });
          }
          // We have a valid category
          else
          {
            // Get the events for this category, prefixing an empty element to represent no choice
            var eventNames = EventCategory.GetEventNamesInCategory(category);

            // Check if the currently stored eventString is in the list of eventNames
            int eventIndex = IndexOf(EventCategory.GetEventStringsInCategory(category), eventString.Value);

            // When not found add default and adjust index
            if (eventIndex < 0)
              eventIndex = EditorGUI.Popup(rightPropRect, 0, ConcatArrays(new string[] { "-" }, eventNames)) - 1;
            else
              eventIndex = EditorGUI.Popup(rightPropRect, eventIndex, eventNames);

            // User selected a valid eventName so store it
            if (eventIndex >= 0)
            {
              eventString.Value = EventCategory.GetEventNameInCategory(category, eventNames[eventIndex]);
            }
          }
          EditorGUI.EndDisabledGroup();
        }

      }
      EditorGUI.EndDisabledGroup();

      return eventString;
    }

    // Join two arrays
    static string[] ConcatArrays(string[] array1, string[] array2)
    {
      var ret = new string[array1.Length + array2.Length];
      array1.CopyTo(ret, 0);
      array2.CopyTo(ret, array1.Length);
      return ret;
    }

    // Find the index of a given element in an array. -1 if not exist
    static int IndexOf(string[] eventNames, string name)
    {
      for (int i = 0; i < eventNames.Length; ++i)
      {
        if (eventNames[i] == name)
        {
          return i;
        }
      }
      return -1;
    }

  }
}
