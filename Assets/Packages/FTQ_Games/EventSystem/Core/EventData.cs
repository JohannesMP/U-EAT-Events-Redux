using UnityEngine;

namespace UEAT.EventSystem
{
  // The default class that all custom events must inherit from.
  public class EventData
  {

  }

  // Custom events
  public class IntegerEvent : EventData
  {
    public int Value;
    public IntegerEvent(int intValue = 0)
    {
      Value = intValue;

    }
    public static implicit operator int(IntegerEvent value)
    {
      return value.Value;
    }
    public static implicit operator IntegerEvent(int value)
    {
      return new IntegerEvent(value);
    }
  }

  public class FloatEvent : EventData
  {
    public float Value;
    public FloatEvent(float floatValue = 0.0f)
    {
      Value = floatValue;
    }
    public static implicit operator float(FloatEvent value)
    {
      return value.Value;
    }
    public static implicit operator FloatEvent(float value)
    {
      return new FloatEvent(value);
    }
  }

  public class DoubleEvent : EventData
  {
    public double Value;
    public DoubleEvent(double doubleValue = 0.0)
    {
      Value = doubleValue;
    }
    public static implicit operator double(DoubleEvent value)
    {
      return value.Value;
    }
    public static implicit operator DoubleEvent(double value)
    {
      return new DoubleEvent(value);
    }
  }

  public class StringEvent : EventData
  {
    public string Value;
    public StringEvent(string stringValue = "")
    {
      Value = stringValue;

    }
    public static implicit operator string(StringEvent value)
    {
      return value.Value;
    }
    public static implicit operator StringEvent(string value)
    {
      return new StringEvent(value);
    }
  }

  public class CollisionEvent2D : EventData
  {
    public Collision2D StoredCollision;
    public CollisionEvent2D(Collision2D collision = null)
    {
      StoredCollision = collision;
    }

    public static implicit operator Collision2D(CollisionEvent2D value)
    {
      return value.StoredCollision;
    }

    public static implicit operator CollisionEvent2D(Collision2D value)
    {
      return new CollisionEvent2D(value);
    }
  }

  public class CollisionEvent3D : EventData
  {
    public Collision StoredCollision;
    public CollisionEvent3D(Collision collision = null)
    {
      StoredCollision = collision;
    }

    public static implicit operator Collision(CollisionEvent3D value)
    {
      return value.StoredCollision;
    }

    public static implicit operator CollisionEvent3D(Collision value)
    {
      return new CollisionEvent3D(value);
    }
  }

} // namespace EventSystem
