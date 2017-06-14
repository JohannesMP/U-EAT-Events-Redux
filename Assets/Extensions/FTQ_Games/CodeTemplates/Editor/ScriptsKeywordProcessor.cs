// - New script keyword processor for Unity 3D by Sarper Soher       -
// - http://www.sarpersoher.com                                      -
// -------------------------------------------------------------------
// - This script changes the keywords in a newly created script with -
// - the values below                                                -
// - Special thanks to hpjohn - http://bit.ly/1N4dd1C      
// -------------------------------------------------------------------
// - Extended by Dimitry "Pixeye" Mitrofanov
// - http://www.hbrew.store/
// -------------------------------------------------------------------
// - Further expanded on by JohannesMP

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
{
  // When unity creates a non-user asset file (namely .meta files)
  public static void OnWillCreateAsset(string metaPath)
  {
    string assetPath = metaPath.Replace(".meta", "");

    // Get the absolute file path of the raw asset
    string filePath = GetAbsoluteFilePath(assetPath);
    // We only care about scripts
    if (!FileIsScript(filePath)) return;

    // Read the raw contents
    string fileContents = File.ReadAllText(filePath);

    // 1. Parse and Replace all Template keywords
    fileContents = ApplyKeywords(fileContents);
    // 2. Normalize line endings
    fileContents = NormalizeNewlines(fileContents);

    // Finally write back to the file
    File.WriteAllText(filePath, fileContents);

    // Refresh the unity asset database to trigger a compilation of our changes on the script
    // NOTE: Disabled for now because of: https://forum.unity3d.com/threads/474023/
    //AssetDatabase.Refresh();

    // Force refresh on single asset
    AssetDatabase.ImportAsset(assetPath);
  }


  // Helper: get absolute file path of a unity asset file.
  static string GetAbsoluteFilePath(string assetPath)
  {
    var index = Application.dataPath.LastIndexOf("Assets");
    return Application.dataPath.Substring(0, index) + assetPath;
  }


  // Helper: check if a given file is a unity script based on file extension
  static bool FileIsScript(string filePath)
  {
    var extension = System.IO.Path.GetExtension(filePath);
    return (extension == ".cs" || extension == ".js" || extension == ".boo");
  }


  static string ApplyKeywords(string fileContents)
  {
    // Get the template keywords
    string codeKeywordsPath = Path.GetFullPath(UEAT.CodeTemplates.GetSettingsPath() + "TemplateKeywords.txt");

    // Extract Keywords
    List<CodeKeyword> codeKeywords = new List<CodeKeyword>();

    if (File.Exists(codeKeywordsPath))
    {
      using (var t = new StreamReader(codeKeywordsPath))
      {
        // Track if we are reading a template key or a value
        int i = 0;
        string key = string.Empty;

        // Reading in TemplateKeywords config file
        while (!t.EndOfStream)
        {
          string raw = string.Empty;

          // Handle multi-line keyword replacement
          do
          {
            string line = t.ReadLine();

            // We have an escaped line, keep reading
            if (line.Length > 0 && line[line.Length - 1] == '\\')
            {
              raw += line.Substring(0, line.Length - 1) + System.Environment.NewLine;
            }
            // We have a normal line, carry on.
            else
            {
              raw += line;
              break;
            }
          } while (!t.EndOfStream);

          // Have key
          if (i % 2 == 0)
          {
            // Ensure key is valid
            bool isValid = true;

            if (raw.Length <= 0) isValid = false;        // Empty line
            if (raw.IndexOf("//") == 0) isValid = false; // Single-line comment

            if(isValid)
              key = raw;
            else
              --i;
          }
          // Have Value, store using previous key
          else
          {
            codeKeywords.Add(new CodeKeyword(key, raw));
          }

          ++i;
        }
      }
    }

    // Replace each keyword in the file
    for (int i = 0; i < codeKeywords.Count; i++)
    {
      fileContents = HandleSpecialKeywords(fileContents, codeKeywords[i]);
    }

    return fileContents;
  }


  // Replace Any special tags
  static string HandleSpecialKeywords(string fileContents, CodeKeyword k)
  {
    string contentRaw = string.Empty;

    contentRaw = k.value.Replace("[Time]", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
    contentRaw = contentRaw.Replace("[PlayerSettings.productName]", PlayerSettings.productName);
    contentRaw = contentRaw.Replace("[PlayerSettings.companyName]", PlayerSettings.companyName);
    // Add more tags here!

    fileContents = fileContents.Replace(k.key, contentRaw);
    return fileContents;
  }


  // Normalize newlines
  public static string NormalizeNewlines(string fileContents)
  {
    // Normalize to Unix line ending
    fileContents = fileContents.Replace("\r\n", "\n");
#if UNITY_EDITOR_WIN
    // Normalize to Windows line ending (so Unity shuts up)
    fileContents = fileContents.Replace("\n", "\r\n");
#endif

    return fileContents;
  }


  public struct CodeKeyword
  {
    public string key;
    public string value;
    public CodeKeyword(string key, string value)
    {
      this.key=key;
      this.value=value;
    }
  }


}
