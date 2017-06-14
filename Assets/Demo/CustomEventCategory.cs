/*===============================================================
Company:    FTQ Games
Product:    PROTOTYPE
Created:    12/06/2017 12:34
Purpose:    <TYPE PURPOSE HERE>
================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UEAT.EventSystem
{

  public partial class Events
  {
    // Events.CustomEventCategory
    public class CustomEventCategory
    {
      static CustomEventCategory() { InitAll(); }

      public static readonly string CustomEventCategoryEvent = "My Blah Event";
      // Add events Here
    }
  }

} // namespace FTQ.EventSystem
