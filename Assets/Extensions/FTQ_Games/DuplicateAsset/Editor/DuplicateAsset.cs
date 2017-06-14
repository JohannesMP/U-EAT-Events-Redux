/*===============================================================
Company:    FTQGames
Product:    NodeEditor
Created:    06/06/2017 09:54
Purpose:    Easy Right-click duplication
================================================================*/
using UnityEngine;
using UnityEditor;

namespace UEAT
{

  public static class DuplicateAsset
  {
    const string MenuPath = "Assets/Duplicate Selected";
    const int MenuPriority = 18;

    // Taken from http://answers.unity3d.com/questions/168580/how-do-i-properly-duplicate-an-object-in-a-editor.html
    [MenuItem(MenuPath, false, MenuPriority)]
    public static void DuplicateSelected()
    {
      EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
    }

    // Ensure that we only try to duplicate when something is selected
    [MenuItem(MenuPath, true)]
    public static bool Validate()
    {
      return Selection.activeObject != null;
    }
  }

} // namespace FTQ
