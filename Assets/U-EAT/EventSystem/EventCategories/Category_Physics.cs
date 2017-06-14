/*!**************************************************************************
\file   Category_Physics.cs
\author Filiecs
\author JohannesMP
\brief

A set of Physics Events.

  To be registered as valid events a string must be
    1. defined as public static readonly
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
    // Events.Physics
    public class Physics
    {
      static Physics() { InitAll(); }

      // Called when this collider/rigidbody has begun touching another rigidbody/collider.
      public static readonly string CollisionEnter;
      // Sent when an incoming collider makes contact with this object's collider (2D physics only).
      public static readonly string CollisionEnter2D;
      // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
      public static readonly string CollisionExit;
      // Sent when a collider on another object stops touching this object's collider (2D physics only).
      public static readonly string CollisionExit2D;
      // Called once per frame for every collider/rigidbody that is touching rigidbody/collider.
      public static readonly string CollisionStay;
      // Sent each frame where a collider on another object is touching this object's collider (2D physics only).
      public static readonly string CollisionStay2D;
    }
  }
}