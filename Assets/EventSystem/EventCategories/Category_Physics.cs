using UnityEngine;

namespace UEAT.EventSystem
{
  public partial class Events
  {
    // Events.Physics
    public class Physics
    {
      static Physics() { InitAll(); }

      // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
      public static readonly string OnCollisionEnter;
      // Sent when an incoming collider makes contact with this object's collider (2D physics only).
      public static readonly string OnCollisionEnter2D;
      // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
      public static readonly string OnCollisionExit;
      // Sent when a collider on another object stops touching this object's collider (2D physics only).
      public static readonly string OnCollisionExit2D;
      // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
      public static readonly string OnCollisionStay;
      // Sent each frame where a collider on another object is touching this object's collider (2D physics only).
      public static readonly string OnCollisionStay2D;
    }
  }
}