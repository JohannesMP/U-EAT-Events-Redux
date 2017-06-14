using UnityEngine;

using UEAT;


namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Default
    public class Default
    {
      static Default() { InitAll(); }

      public static readonly string DefaultEvent;
      public static readonly string Create;
      public static readonly string Initialize;
      public static readonly string LogicUpdate;
      public static readonly string LateUpdate;
      public static readonly string Destroy;
    }
  }
}