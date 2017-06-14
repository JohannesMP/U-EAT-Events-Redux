# U-EAT-Events-Redux
A redux of events from U-EAT by Filiecs: http://u-eat.org/

## Goals of Redux

1. Allow Event strings to be stored in categories, while still providing an intuitive editor UI for selecting them.
2. Allow users to define event strings in their own files, so they can keep their codebase separated from the U-EAT codebase, while still having them show up in the custom property inspector.
3. No longer require event strings to be initialized manually (ex: `static readonly string LogicUpdate = "LogicUpdate"`.
4. Allow Functions without an `EventData` argument to be Connected/Disconnected to an event string.

All of these goals have been completed.


## General Change Overview

- Wrapped everything in new `UEAT.EventSystem` namespace.

- `EventSystem` class renamed to `EventDispatch` to avoid name ambiguity.
  - `EventSystem/EventSystem.cs` is now `EventSystem/Core/EventDispatch.cs`

- `Events.cs` broken up into logical parts:
  1. `EventData` definitions and types moved to `EventSystem/Core/EventData.cs`
  2. `Events` static event strings:
     - Can be stored accessed and defined through nested categories (classes)
     - If empty or null, are dynamically initialized with a string corresponding to each field's name
     - With defaults now located in `EventSystem/EventCategories/` (more detail below)
  3. `Events` non-static serializable class renamed to `EventString`, now found in `Core/EventString.cs`
     - Associated `EventPropertyDrawer` renamed `EventStringPropertyDrawer`, now found in `EventSystem/Core/Editor/EventStringPropertyDrawer.cs`
     - `EventStringPropertyDrawer` updated to allow user to select events by category in the Inspector.
  
- `EventHandler` and `EventDispatch` (formerly `EventSystem`) now support Connecting/Disconnecting Actions that take no arguments.

---------

## How event strings now work

It's easiest to explain how event strings work by providing an example and then breaking it down.

### Adding an Example event
Let's add an event string `SomeEvent` in the category `Category`. The following could be placed in a new script:

```c#
namespace UEAT.EventSystem {                   // 1. Namespace
  public partial class Events {                // 2. Inherits from 'EventsCategory' to provide static InitAll()
    public class Category {                    // 3. The container for the static readonly strings
      static Category() { InitAll(); }         // 4. Ensures that all static readonly strings are init
      
      public static readonly string SomeEvent; // 5. Guaranteed to be initialized with string "SomeEvent"
    }
  }
}
```

And a user would use it as follows:

```c#
using UEAT.EventSystem;
public class EventDemo : MonoBehaviour {
  void Start() {
    gameObject.Connect( Events.Category.SomeEvent, OnSomeEvent );
  }
  void OnSomeEvent() { Debug.Log("Some Event Happened"); }
}
```

### How Static Event strings work

The event strings are guaranteed to be initialized to their own name if they are left empty or null. 

It is easy to iterate over fields with C# Reflection and Linq and initialize them, but what if someone accesses an event string in a static constructor, who's initialization order might be before that of where we place the initialization logic?

To get around this we levagare the guarantee that C# provides: For any static field/property/function/etc, a static constructor will always be called if they are accessed. Therefore in the example above, even if `Events.Category.SomeEvent` was accessed in a static constructor, C# guarantees that Category's static constructor is run before the value in `SomeEvent` is returned.

We leverage this by having the `InitAll()` call in every class that contains static readonly event strings.

The first time any static property is accessed in a class with `InitAll()` in its static constructor, that is the moment the logic in `EventSystem/Core/EventCategory.cs` is run to initialize them.


To make it easy for users to add their own events, the `Events` class is now `partial` and inherits from EventCategory which provides all contained classes access to `InitAll()`, and means we can easily locate their static readonly strings.

```



