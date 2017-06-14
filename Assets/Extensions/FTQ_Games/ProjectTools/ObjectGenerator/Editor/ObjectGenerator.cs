using UnityEditor;
using UnityEngine;
namespace Homebrew
{
  public class ObjectGenerator
  {

    [MenuItem ("Tools/Generate/Object")]

    public static void Execute()
    {
      CreateObject ();
    }


    /*[MenuItem ("Tools/Generate Prefab")]

    public static void ExecutePrefab()
    {
      CreatePrefab ();
    }


    [MenuItem ("Assets/Create/GameModel",false,100)]
    private static void CreatePrefab()
    {
      GameObject prop = new GameObject ("Object");
      GameObject bindings = new GameObject ("bindings");
      GameObject model = new GameObject ("model");
      GameObject offset = new GameObject ("offset");
      GameObject rotator = new GameObject ("rotator");
      GameObject settings = new GameObject ("settings");
      GameObject data = new GameObject ("data");

      bindings.transform.parent=prop.transform;
      model.transform.parent=prop.transform;
      settings.transform.parent=prop.transform;

      offset.transform.parent=model.transform;
      rotator.transform.parent=offset.transform;
      data.transform.parent=settings.transform;
      int val = 0;
      string fileName = "NewObject_"+val;
      string fileLocation = "Assets/GAME_ASSETS/Prefabs/"+fileName+".prefab";
      var prev = AssetDatabase.LoadMainAssetAtPath (fileLocation);

      while (prev!=null)
      {
        val++;
        fileName="NewObject_"+val;
        fileLocation="Assets/GAME_ASSETS/Prefabs/"+fileName+".prefab";
        prev=AssetDatabase.LoadMainAssetAtPath (fileLocation);
        if (prev==null)
          break;
      }

      PrefabUtility.CreatePrefab (fileLocation,prop,ReplacePrefabOptions.ConnectToPrefab);

    } */




    private static void CreateObject()
    {

      GameObject prop = new GameObject ("Object");
      GameObject bindings = new GameObject ("bindings");
      GameObject model = new GameObject ("model");
      GameObject offset = new GameObject ("offset");
      GameObject rotator = new GameObject ("rotator");
      GameObject settings = new GameObject ("settings");
      GameObject data = new GameObject ("data");

      bindings.transform.parent=prop.transform;
      model.transform.parent=prop.transform;
      settings.transform.parent=prop.transform;
      rotator.transform.parent=model.transform;
      offset.transform.parent=rotator.transform;
      data.transform.parent=settings.transform;

    }


  }
}