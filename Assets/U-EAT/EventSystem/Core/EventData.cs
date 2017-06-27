/*!**************************************************************************
\file   EventData.cs
\author Filiecs
\author JohannesMP
\brief

This file defines the base EventData vlass used to send date in the event
system, as well as several common types that derive from it.

\copyright © 2016-2017 CC
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UEAT.EventSystem
{
  // The default class that all custom events must inherit from.
  public class EventData
  {}

  // EventData with one argument
  public class EventData<T1> : EventData
  {
    public T1 Value;
    public EventData(T1 value = default(T1))
    {
      Value = value;
    }

    public static implicit operator T1(EventData<T1> value)
    {
      return value.Value;
    }

    public static implicit operator EventData<T1>(T1 value)
    {
      return new EventData<T1>(value);
    }
  }

  // EventData with two arguments
  public class EventData<T1, T2> : EventData<T1>
  {
    public T2 Value2;
    public EventData(T1 value = default(T1), T2 value2 = default(T2)) : base(value)
    {
      Value2 = value2;
    }
  }

  // EventData with three arguments
  public class EventData<T1, T2, T3> : EventData<T1, T2>
  {
    public T3 Value3;
    public EventData(T1 value = default(T1), T2 value2 = default(T2), T3 value3 = default(T3)) : base(value, value2)
    {
      Value3 = value3;
    }
  }

  // EventData with four arguments
  public class EventData<T1, T2, T3, T4> : EventData<T1, T2, T3>
  {
    public T4 Value4;
    public EventData(T1 value = default(T1), T2 value2 = default(T2), T3 value3 = default(T3), T4 value4 = default(T4)) : base(value, value2, value3)
    {
      Value4 = value4;
    }
  }


  // Custom events

  // Integer
  public class IntegerEvent : EventData<int> {}
  // Float
  public class FloatEvent : EventData<float> { }
  // Double
  public class DoubleEvent : EventData<double> { }
  // Bool
  public class BoolEvent : EventData<bool> { }
  // Long
  public class LongEvent : EventData<long> { }

  // String
  public class StringEvent : EventData<string> { }
  // Vector2
  public class Vector2Event : EventData<Vector2> { }
  // Vector3
  public class Vector3Event : EventData<Vector3> { }
  // Vector4
  public class Vector4Event : EventData<Vector4> { }

  // Collision 2D
  public class CollisionEvent2D : EventData<Collision2D> { }
  // Collision 3D
  public class CollisionEvent3D : EventData<Collision> { }

  // GameObject
  public class GameObjectEvent : EventData<GameObject> { }
  // Scene
  public class SceneEvent : EventData<Scene> {}


} // namespace EventSystem
