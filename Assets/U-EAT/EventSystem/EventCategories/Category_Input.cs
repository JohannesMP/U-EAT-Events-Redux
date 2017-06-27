/*!**************************************************************************
\file   Category_Inputs.cs
\author Filiecs
\author JohannesMP
\brief

A set of Input Events.

  To be registered as valid events a string must be
    1. Defined as public static readonly
    2. In (or have in its class tree) a class that inherits from EventCategory
       (which partial class Events is)
    3. In a class that has a static consttructor that calls InitAll().

  Side effects of being a valid event string:
    1. If Empty/Null/Undefined, will be initialized with its variable name.
    2. Will have its string value prefixed by its category.


\copyright © 2016-2017 CC
*****************************************************************************/

using UnityEngine;

namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Input
    public class Input
    {
      static Input() { InitAll(); }

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

} // UEAT.EventSystem
