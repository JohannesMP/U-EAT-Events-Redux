/*===============================================================
Company:    FTQ Games
Product:    Wingspan
Created:    07/06/2017 16:46
Purpose:    <TYPE PURPOSE HERE>
================================================================*/
using UnityEngine;
using UnityEditor; // WARNING: This script must be in an 'Editor' folder

namespace UEAT
{

  public static class FirstTimeConfig
  {
    [MenuItem("Tools/Project/First Time Setup/Set Project Settings")]
    public static void Execute()
    {
      /* PLAYER SETTINGS*/

      PlayerSettings.companyName = "FTQ Games";
      LogChange("Player Settings->Company Name", PlayerSettings.companyName);

      PlayerSettings.productName = "PROTOTYPE";
      LogChange("Player Settings->Product Name", PlayerSettings.productName);

      // construct identifier with company name and product name and apply to relevant build targets
      var identifier = "com." + PlayerSettings.companyName.Replace(" ", "").ToLower() + "." + PlayerSettings.productName.ToLower();
      PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, identifier);
      PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, identifier);
      PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, identifier);
      LogChange("Player Settings->Bundle Identifier/Package Identifier/etc.", identifier);


      /* EDITOR SETTINGS */

      EditorSettings.externalVersionControl = "Visible Meta Files";
      LogChange("Editor Settings->Version Control Mode", EditorSettings.externalVersionControl);

      EditorSettings.serializationMode = SerializationMode.ForceText;
      LogChange("Editor Settings->Asset Serialization Mode", EditorSettings.serializationMode.ToString());
    }

    private static void LogChange(string menuLocation, string newValue)
    {
      Debug.LogFormat("Set <b>{0}</b> to <b>{1}</b>", menuLocation, newValue);
    }
  }

} // namespace FTQ
