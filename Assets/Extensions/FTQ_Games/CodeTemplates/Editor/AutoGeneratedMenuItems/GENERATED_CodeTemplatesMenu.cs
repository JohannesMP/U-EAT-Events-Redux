
/****************************************************************\
| THIS FILE WAS DYNAMICALLY GENERATED!  DO NOT EDIT IT MANUALLY! *
\****************************************************************/

using UnityEditor;
using UnityEngine;

namespace UEAT
{
  public class CodeTemplatesMenuItems
  {
    private const string MENU_ITEM_PATH = "Assets/Create/";
    private const int MENU_ITEM_PRIORITY = 70;
    
    [MenuItem (MENU_ITEM_PATH + "Editor Script/Component", false, MENU_ITEM_PRIORITY)]
    private static void CreateEditor_Component()
    {
      CodeTemplates.CreateFromTemplate(
        @"Editor_Component.cs",
        CodeTemplates.GetTemplatesPath() + @"\EditorScripts\Editor_Component.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "Editor Script/Menu Item", false, MENU_ITEM_PRIORITY)]
    private static void CreateEditor_MenuItem()
    {
      CodeTemplates.CreateFromTemplate(
        @"Editor_MenuItem.cs",
        CodeTemplates.GetTemplatesPath() + @"\EditorScripts\Editor_MenuItem.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "Editor Script/Static Class", false, MENU_ITEM_PRIORITY)]
    private static void CreateEditor_StaticClass()
    {
      CodeTemplates.CreateFromTemplate(
        @"Editor_StaticClass.cs",
        CodeTemplates.GetTemplatesPath() + @"\EditorScripts\Editor_StaticClass.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "General Script/Event Category", false, MENU_ITEM_PRIORITY)]
    private static void CreateCustomEventCategory()
    {
      CodeTemplates.CreateFromTemplate(
        @"CustomEventCategory.cs",
        CodeTemplates.GetTemplatesPath() + @"\Events\CustomEventCategory.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "General Script/Event Data", false, MENU_ITEM_PRIORITY)]
    private static void CreateCustomEventData()
    {
      CodeTemplates.CreateFromTemplate(
        @"CustomEventData.cs",
        CodeTemplates.GetTemplatesPath() + @"\Events\CustomEventData.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "General Script/Component", false, MENU_ITEM_PRIORITY)]
    private static void CreateGeneral_Component()
    {
      CodeTemplates.CreateFromTemplate(
        @"General_Component.cs",
        CodeTemplates.GetTemplatesPath() + @"\GeneralScripts\General_Component.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "General Script/ScriptableObject", false, MENU_ITEM_PRIORITY)]
    private static void CreateGeneral_ScriptableObject()
    {
      CodeTemplates.CreateFromTemplate(
        @"General_ScriptableObject.cs",
        CodeTemplates.GetTemplatesPath() + @"\GeneralScripts\General_ScriptableObject.txt"
      );
    }

    [MenuItem (MENU_ITEM_PATH + "General Script/Static Class", false, MENU_ITEM_PRIORITY)]
    private static void CreateGeneral_StaticClass()
    {
      CodeTemplates.CreateFromTemplate(
        @"General_StaticClass.cs",
        CodeTemplates.GetTemplatesPath() + @"\GeneralScripts\General_StaticClass.txt"
      );
    }

  }
}
/****************************************************************\
| THIS FILE WAS DYNAMICALLY GENERATED!  DO NOT EDIT IT MANUALLY! *
\****************************************************************/

