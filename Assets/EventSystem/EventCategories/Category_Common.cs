using UnityEngine;

using UEAT;


namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Default
    public class Common
    {
      static Common() { InitAll(); }

      public static readonly string Default;
      public static readonly string Create;
      public static readonly string Initialize;
      public static readonly string LogicUpdate;
      public static readonly string LateUpdate;
      public static readonly string Destroy;
    }
  }
}