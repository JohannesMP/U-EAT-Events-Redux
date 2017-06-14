using UnityEngine;

namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Input
    public class Input
    {
      static Input() { InitAll(); }
      //public static readonly string GenericInput;


      // Events.Input.Keyboard
      public class Keyboard
      {
        static Keyboard() { InitAll(); }
        public static readonly string KeyboardEvent;
      }

      // Events.Input.Mouse
      public class Mouse
      {
        static Mouse() { InitAll(); }
        public static readonly string MouseUp;
        public static readonly string MouseDown;
        public static readonly string MouseEnter;
        public static readonly string MouseExit;
        public static readonly string MouseDrag;
        public static readonly string MouseOver;
      }
    }
  }

} // FTQ.EventSystem