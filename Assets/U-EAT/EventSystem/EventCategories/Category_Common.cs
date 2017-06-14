/*!**************************************************************************
\file   Category_Commons.cs
\author Filiecs
\author JohannesMP
\brief

A set of Common Events.

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

using UEAT;


namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Common
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