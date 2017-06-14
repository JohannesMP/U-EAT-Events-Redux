
# U-EAT-Events-Redux

![Example of Event Categories](http://i.imgur.com/eVipIrl.gif)

A redux of the U-EAT event system: http://u-eat.org/events.html

## Goals of Redux

1. Allow Event strings to be stored in categories, while still providing an intuitive editor UI for selecting them.
2. Allow users to define event strings in their own files, so they can keep their codebase separated from the U-EAT codebase, while still having them show up in the custom property inspector.
3. No longer require event strings to be initialized manually (ex: `static readonly string LogicUpdate = "LogicUpdate"`.
4. Allow Functions without an `EventData` argument to be Connected/Disconnected to an event string.

All of these goals have been completed. See the [Todo](#todo) section at the bottom of this readme for some potential future changes.

<br>

## General Change Overview

Here is an overview of the changes made:


### 1. Namespace and renaming the EventSystem class
 
Everything to do with the event system is now in a new `UEAT.EventSystem` namespace.

Users need to add `using UEAT.EventSystem` if they want to use it in their code.

Therefore the `EventSystem` class was renamed to `EventDispatch` to avoid name ambiguity, and `EventSystem/EventSystem.cs` is now [`EventSystem/Core/EventDispatch.cs`](Assets/U-EAT/EventSystem/Core/EventDispatch.cs)


### 2. Events property and static string changes

`Events.cs` originally served three purposes:
1. It defined several `static readonly String` fields corresponding to events in the event system.
2. It defined the `Events` class used to select an event, along with a custom PropertyDrawer.
3. It defined various `EventData` types for convenience.

These three parts have been broken up:
1. The `static readonly String` events are still inside the `Events` class, but it is now partial to allow being split into multiple files. The original default event strings are now located in the files in [`EventSystem/EventCategories/`](Assets/U-EAT/EventSystem/EventCategories).
   - The strings are now initialized dynamically if they are left uninitialized, null or empty.
   - The strings are also now divided into sub-classes which are treated as categories corresponding to their name, and to allow the fields to be initialized correctly, each class containing the event strings must have a static constructor that calls `InitAll()` which is inherited from the parent `Events` class.
   - To allow `Events.A.Init` and `Events.B.Init` to be disambiguated, they are initialized by prefixing their category.
2. `Events` (the non-static class for selecting event strings) is now `EventString`, and its `EventPropertyDrawer` is now `EventStringPropertyDrawer`, located in [`EventSystem/Core/EventString.cs`](Assets/U-EAT/EventSystem/Core/EventString.cs) and [`EventSystem/Core/Editor/EventStringPropertyDrawer.cs`](Assets/U-EAT/EventSystem/Core/Editor/EventStringPropertyDrawer.cs) respectively.
   - The propertydrawer also now has two dropdowns, one first for the category, and then for the event itself.
3. The `EventData` types moved to [`EventSystem/Core/EventData.cs`](Assets/U-EAT/EventSystem/Core/EventData.cs)

For more information see the [How event strings now work](#how-event-strings-now-work) section below.


### 3. Functions without arguments

The `EventDispatch` (formerly `EventSystem`) and the `EventHandler` class both now support Connecting functions without arguments, as well as functions that accept an `EventData` instance.

This is accomplished by wrapping any `Action` without arguments in a wrapper `Action<EventData>`. When the event is dispatched, the `Action<EvendData>` wrapper simply calls the `Action` that it wraps.

To ensure that Connecting an action multiple times will result in the same wrapper, `EventHandler` now also stores a `CallbackWrapperList` Dictionary.

Similarly, to ensure that `EventDisconnect(string eventName, object thisPointer)` still works, `EventHandler` also stores the inverse mapping `WrapperCallbackList`. This is necessary since the events are removed by looking at the `Target` property of each stored `Action<EventData`, but the wrapper does have that same Target. We use `WrapperCallbackList` to efficiently locate the original `Action` and then compare against its `Target` instead.

<br />

## How event strings now work

It's easiest to explain how event strings work by providing an example and then breaking it down.

### Adding an Example event
Let's add an event string `SomeEvent` in the category `Category`. The following could be placed in a new script:

```c#
namespace UEAT.EventSystem {                   // 1. Namespace
  public partial class Events {                // 2. Inherits from 'EventsCategory' to provide static InitAll()
    public class Category {                    // 3. The container for the static readonly strings
      static Category() { InitAll(); }         // 4. Ensures that all static readonly strings are init
      
      public static readonly string SomeEvent; // 5. Guaranteed to be initialized with string "Category.SomeEvent"
      public static readonly string Other = "Custom" // Guaranteed to be init with string "Category.Custom"
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

### What is happening

The event strings are guaranteed to be initialized to their own name if they are left empty or null, and all event strings are prefixed with their Category - the string of classes they are inside of, which by default does not count 'Events' (this can be toggled in [`EventCategory._ignoreBaseClassCategories`](Assets/U-EAT/EventSystem/Core/EventCategory.cs#L64)).

It is easy to iterate over fields with C# Reflection and Linq and initialize them, but what if someone accesses an event string in a static constructor, who's initialization order might be before that of where we place the initialization logic?

To get around this we leverage the guarantee that C# provides for statics: Before any static field/property/function/etc returns its value, the static constructor will always be called first. 

<br/>

Therefore in the example above, even if `Events.Category.SomeEvent` was accessed in a static constructor, C# guarantees that Category's static constructor is run before the value in `SomeEvent` is returned.

We leverage this by having the `InitAll()` call in every class that contains static readonly event strings.

The first time any static property is accessed in a class with `InitAll()` in its static constructor, that is the moment the logic in [`EventSystem/Core/EventCategory.cs`](Assets/U-EAT/EventSystem/Core/EventCategory.cs) is run to initialize them.

<br/>

To make it easy for users to add their own events, the `Events` class is now `partial` and inherits from EventCategory which provides all contained classes access to `InitAll()`, and means we can easily locate their static readonly strings.


## Todo

While the Category/Event dropdowns already update correctly when typing in the string field in the Inspector of an `EventString`, it might be nice to show the user an autocomplete dropdown that shows them potential Category/EventNames they might want to write.

However this is very low priority since in that case the user can just use the dropdowns anyways.
