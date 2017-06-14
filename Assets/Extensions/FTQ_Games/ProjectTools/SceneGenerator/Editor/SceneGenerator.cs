using UnityEngine;
using UnityEditor;
namespace Homebrew
{
  public class SceneGenerator
  {

    [MenuItem ("Tools/Generate/Scene")]
    public static void Execute()
    {
      CreateScene ();
    }
    
    private static void CreateScene()
    {
      GameObject prop = new GameObject ("GameBinder");
      prop=new GameObject ("Cameras");
      MonoBehaviour.FindObjectOfType<Camera> ().transform.parent=prop.transform;

      new GameObject ("UI");
      new GameObject ("Lights");
      new GameObject ("World");
      new GameObject ("_Dynamic");
    }
  }
}