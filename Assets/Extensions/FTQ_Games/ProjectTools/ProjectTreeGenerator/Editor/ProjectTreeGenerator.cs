using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;


namespace ProjectTreeGenerator
{

  public class CreateProjectTree
  {
    [MenuItem("Tools/Project/First Time Setup/Generate Project Tree")]
    public static void Execute()
    {
      var assets = GenerateFolderStructure();
      CreateFolders(assets);
    }

    private static void CreateFolders(Folder rootFolder)
    {
      // Folder does not yet exist - Creating
      if (!AssetDatabase.IsValidFolder(rootFolder.DirPath))
      {
        Debug.Log("Creating: <b>" + rootFolder.DirPath + "</b>");
        AssetDatabase.CreateFolder(rootFolder.ParentPath, rootFolder.Name);
        File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".gitkeep");
      }

      // Folder does exist
      else
      {
        Debug.Log("Directory <b>" + rootFolder.DirPath + "</b> already exists");
        // Has no content - add .gitkeep file to avoid git weirdness
        if (Directory.GetFiles(Directory.GetCurrentDirectory() + Path.AltDirectorySeparatorChar + rootFolder.DirPath).Length < 1)
        {
          File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".gitkeep");
          Debug.Log("Creating '.gitkeep' file in: <b>" + rootFolder.DirPath + "</b>");
        }
      }

      foreach (var folder in rootFolder.SubFolders)
      {
        CreateFolders(folder);
      }
    }


    private static Folder GenerateFolderStructure()
    {
      Folder rootFolder = new Folder("Assets", "");

      /* StreamingAssets Folder (reserved Unity name)*/
      rootFolder.Add("StreamingAssets");


      /* Package Folders */

      rootFolder.Add("Extensions").Add("FTQ_Games");
      rootFolder.Add("Packages").Add("FTQ_Games");
      rootFolder.Add("Plugins").Add("FTQ_Games");

      /* TEMP stuff */
      rootFolder.Add("_TEMP");

      /* GAME stuff */
      var gameAssets = rootFolder.Add("_GAME");

      gameAssets.Add("Data");
    
      gameAssets.Add("Prefabs").Add("Resources");

      var SceneAssets = gameAssets.Add("Scenes");
      SceneAssets.Add(
        "Gameplay", 
        "GameUI", 
        "Globals"
      );

      var ScriptAssets = gameAssets.Add("Scripts");
      ScriptAssets.Add(
        "Attributes",
        "Editor",
        "Enums",
        "Events",
        "Gameplay",
        "GameUI",
        "ScriptableObjects",
        "Utility"
      );

      var staticAssets = gameAssets.Add("Static Assets");
      staticAssets.Add(
        "Animations",
        "Animators",
        "Audio",
        "Effects",
        "Fonts",
        "Materials",
        "Models",
        "Shaders",
        "Sprites",
        "Textures",
        "Videos"
      );
      var audioAsssets = staticAssets.Get("Audio");

      audioAsssets.Add(
        "Sounds", 
        "Music"
      );

      return rootFolder;
    }

  }

  class Folder
  {
    public string DirPath { get; private set; }
    public string ParentPath { get; private set; }
    public string Name;
    public List<Folder> SubFolders;

    public Folder Add(string name)
    {
      Folder subFolder = null;
      if (ParentPath.Length > 0)
        subFolder = new Folder(name, ParentPath + Path.DirectorySeparatorChar + Name);
      else
        subFolder = new Folder(name, Name);

      SubFolders.Add(subFolder);
      return subFolder;
    }

    public void Add(params string[] names)
    {
      foreach(string name in names)
      {
        Add(name);
      }
    }

    public Folder Get(string subname)
    {
      return SubFolders.Find(f => f.Name == subname);
    }

    public Folder(string name, string dirPath)
    {
      Name = name;
      ParentPath = dirPath;
      DirPath = ParentPath + Path.DirectorySeparatorChar + Name;
      SubFolders = new List<Folder>();
    }
  }

}