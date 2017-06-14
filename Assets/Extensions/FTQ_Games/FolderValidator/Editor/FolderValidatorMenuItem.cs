/*===============================================================
Company:    FTQ Games
Product:    Wingspan
Created:    04/06/2017 03:25
Purpose:    Sanitize the directories to make them git-friendly by adding .gitkeep files when necessary
================================================================*/
using UnityEngine;
using System.IO;

namespace UEAT
{
  using UnityEditor; // WARNING: This script must be in an 'Editor' folder
  public static class FolderValidatorMenuItem
  {
    // The file that is inserted into empty files
    public const string KeepFileName = ".gitkeep";

    private static int _keepFilesAdded;
    private static int _keepFilesRemoved;
    private static int _keepFilesTotal;


    [MenuItem("Tools/Project/Validate Folders")]
    public static void Execute()
    {
      ValidateDirectory(Application.dataPath);
    }



    static void ValidateDirectory(string dirPath)
    {
      _keepFilesAdded = 0;
      _keepFilesRemoved = 0;
      _keepFilesTotal = 0;

      RecurseValidateDirectory(Application.dataPath);

      if (_keepFilesAdded == 0 && _keepFilesRemoved == 0)
      {
        string msg = "All Folders are valid.";
        
        if (_keepFilesTotal > 0)
        {
          bool one = _keepFilesTotal == 1;
          msg += " <b>" + _keepFilesTotal + " " + (one ?"directory</b> is" : "directories</b> are") + " empty and so " + (one ? "" : "each ") + "contains a " + KeepFileName + ".";
        }

        Debug.Log(msg);
      }
    }


    static void RecurseValidateDirectory(string dirPath)
    {
      string assetPath = AbsoluteToAssetPath(dirPath);

      int fileCount = Directory.GetFiles(dirPath).Length;


      // Directory is empty - Add a keep file
      if (fileCount == 0)
      {
        Debug.Log("Directory <b>" + assetPath + "</b> is empty. Adding " + KeepFileName + ".");

        AddKeepfileToDirectory(dirPath);
        ++_keepFilesAdded;
        ++_keepFilesTotal;
        return;
      }


      string keepPath = dirPath + Path.DirectorySeparatorChar + KeepFileName;

      if(File.Exists(keepPath))
      {
        // Only a single Keepfile - Keep it
        if(fileCount == 1)
        {
          ++_keepFilesTotal;
        }
        // Keepfile exists and dir has other content - Delete keep file
        else
        {
          Debug.Log("Directory <b>" + assetPath + "</b> is non-empty. Removing its " + KeepFileName + ".");

          File.Delete(keepPath);
          ++_keepFilesRemoved;
        }
      }


      // Recurse through subdirectories
      foreach(string subDirPath in Directory.GetDirectories(dirPath))
        RecurseValidateDirectory(subDirPath);
    }


    // Given a an absolute path, get the Unity asset path. (Assumes path is actually in the Unity project)
    static string AbsoluteToAssetPath(string path)
    {
      return "Assets" + path.Substring(Application.dataPath.Length);
    }


    // Add a 'keep' file to an empty directory
    static void AddKeepfileToDirectory(string dirPath)
    {
      if (!Directory.Exists(dirPath))
        throw new DirectoryNotFoundException("Directory " + dirPath + " Was expected but not found!");

      // Create empty 'keep' file
      File.Create(dirPath + Path.DirectorySeparatorChar + KeepFileName);
    }
  }

} // namespace FTQ
