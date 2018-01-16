using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AsyncAndEvents.Models;

namespace AsyncAndEvents.Models
{
    public class EventPublisher
    {
        //option 1, the traditional way
        public delegate void CustomEventHandler(object sender, CustomEventArgs args);
        public event CustomEventHandler GeneralEventHandler;
        public event CustomEventHandler DivBy2EventHandler;
        public event CustomEventHandler HalfwayEventHandler;

        //option 2, using generic-version event, does not need the delegate declaration 
        //public event EventHandler<CustomEventArgs> GeneralEventHandler;
        //public event EventHandler<CustomEventArgs> DivBy2EventHandler;
        //public event EventHandler<CustomEventArgs> HalfwayEventHandler;

        //Option 3 uses System.EventHandler, if no CustomEventArgs are required 
        //public event EventHandler GeneralEventHandler;
        public void Count()
        {
            for (int i = 0; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    RaiseGeneralEvent(new CustomEventArgs("i is " + i));
                    RaiseDivBy2Event(new CustomEventArgs("i is divisible by 2"));
                }
                if (i == 5)
                {
                    RaiseGeneralEvent(new CustomEventArgs("i is " + i));
                    RaiseHalfwayEvent(new CustomEventArgs("i is halfway to 10"));
                }
            }
        }  //Count

        // Wrap event invocations inside a protected virtual method to allow derived classes to override the invocation behavior
        protected virtual void RaiseGeneralEvent(CustomEventArgs e)
        {
            // Use a copy of the event, to avoid a race condition if the last subscriber unsubscribes between null check and raise.
            CustomEventHandler handler = GeneralEventHandler;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Here you can add a bit to the message before publishing it
                e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                // Raise the event!
                handler(this, e);
            }
        }  //RaiseCustomEvent

        protected virtual void RaiseDivBy2Event(CustomEventArgs e)
        {
            CustomEventHandler handler = DivBy2EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }  //RaiseDivBy2Event

        //this event handler uses a simplification suggested by Visual Studio...
        protected virtual void RaiseHalfwayEvent(CustomEventArgs e)
        {
            HalfwayEventHandler?.Invoke(this, e);
        }  //RaiseHalfwayEvent


    }  //EventPublisher
}  //namespace