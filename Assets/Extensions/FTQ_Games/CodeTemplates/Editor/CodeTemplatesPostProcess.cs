using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace UEAT
{
  public class CodeTemplatesPostProcess : AssetPostprocessor
  {
    // Autogenerate
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
      if (CodeTemplates.AutoGenerateMenuItems)
        RegenerateCodeTemplateMenu();
    }

    [MenuItem("Tools/Project/Rebuild Template Menu Items")]
    public static void Execute()
    {
      RegenerateCodeTemplateMenu();
    }

    static void RegenerateCodeTemplateMenu()
    {
      string codeTemplatesMenuRawPath = Path.GetFullPath(CodeTemplates.GetMenuItemsGenPath() + @"CodeTemplatesMenu.txt");
      string codeTemplatesMenuItemRawPath = Path.GetFullPath(CodeTemplates.GetMenuItemsGenPath() + @"CodeTemplatesMenuItem.txt");
      string codeTemplatesGeneratedMenuPath = Path.GetFullPath(CodeTemplates.GetMenuItemsGenPath() + @"GENERATED_CodeTemplatesMenu.cs");

      string warningComment = @"
/****************************************************************\
| THIS FILE WAS DYNAMICALLY GENERATED!  DO NOT EDIT IT MANUALLY! *
\****************************************************************/

";

      string templatedContent = string.Empty;

      string templateNode = string.Empty;

      if (File.Exists(codeTemplatesGeneratedMenuPath))
      {
        File.Delete(codeTemplatesGeneratedMenuPath);
      }

      if (File.Exists(codeTemplatesMenuRawPath))
      {
        using (var t = new StreamReader(codeTemplatesMenuRawPath))
        {
          templatedContent += t.ReadToEnd();
        }
      }

      if (File.Exists(codeTemplatesMenuItemRawPath))
      {
        using (var t = new StreamReader(codeTemplatesMenuItemRawPath))
        {
          templateNode = t.ReadToEnd();
        }
      }
      AssetNode[] nodes = GetAtPath(CodeTemplates.GetTemplatesPath());

      string completeCode = "";
      for (int i = 0; i < nodes.Length; i++)
      {
        //var t = templateNode.Replace("##ITEM##", nodes[i].MenuItemName).Replace("##TEMPLATE##", nodes[i].TemplateName);
        var t = templateNode
          .Replace("##ITEM##", nodes[i].MenuItemName)
          .Replace("##TEMPLATE##", nodes[i].TemplateName)
          .Replace("##TEMPLATE_PATH##", nodes[i].TemplatePath);
        completeCode += t;
      }

      string final = templatedContent.Replace("##CODE##", completeCode);
      final = warningComment + final + warningComment;

      UTF8Encoding encoding = new UTF8Encoding(true, false);
      using (var fileStream = new FileStream(codeTemplatesGeneratedMenuPath, FileMode.Create))
      {
        fileStream.Write(encoding.GetBytes(final), 0, final.Length);
      }

      string generatedMenuAssetPath = "Assets" + codeTemplatesGeneratedMenuPath.Substring(Application.dataPath.Length);
      AssetDatabase.ImportAsset(generatedMenuAssetPath);
    }


    public static string[] GetFilesRecursive(string path)
    {
      List<string> files = new List<string>();

      // 1. Store our own files
      files.AddRange(System.Array.FindAll(Directory.GetFiles(path), f => f.Contains(".meta") == false));

      // 2. Store files in our subdirectory
      foreach(string dir in Directory.GetDirectories(path))
        files.AddRange(GetFilesRecursive(dir));

      return files.ToArray();
    }

    public static AssetNode[] GetAtPath(string path)
    {
      string[] fileEntries = GetFilesRecursive(path);
      AssetNode[] nodes = new AssetNode[fileEntries.Length];

      string templatePath = new FileInfo(CodeTemplates.GetTemplatesPath()).Directory.FullName;

      for (int i = 0; i < fileEntries.Length; i++)
      {
        FileInfo info = new FileInfo(fileEntries[i]);

        string fullPath = info.Directory.FullName;

        string relPath = fullPath.Substring(templatePath.Length) + Path.DirectorySeparatorChar + info.Name;

        string name = info.Name.Substring(0, info.Name.IndexOf('.'));

        using (var t = new StreamReader(info.FullName))
        {
          nodes[i] = new AssetNode(t.ReadLine(), name, relPath);
        }
      }
      return nodes;
    }
  }


  public struct AssetNode
  {
    public string MenuItemName;
    public string TemplateName;
    public string TemplatePath;
    public AssetNode(string MenuItemName, string TemplateName, string TemplatePath)
    {
      this.MenuItemName = MenuItemName;
      this.TemplateName = TemplateName;
      this.TemplatePath = TemplatePath;
    }
  }

}