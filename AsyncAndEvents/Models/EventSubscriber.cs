using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AsyncAndEvents.Models
{
    public class EventSubscriber
    {
        private string id;
        public EventSubscriber(string name, EventPublisher pub, bool handlesDivBy2, bool handlesHalfway)
        {
            id = name;
            pub.GeneralEventHandler += SubscriberGeneralEventHandler;
            if (handlesDivBy2)
            {
                pub.DivBy2EventHandler += SubscriberDivBy2EventHandler;
            }
            if (handlesHalfway)
            {
                pub.HalfwayEventHandler += SubscriberHalfwayEventHandler;
            }
        }  //ctor

        void SubscriberGeneralEventHandler(object sender, CustomEventArgs e)
        {
            Debug.WriteLine(id + " received a General message: " + e.Message);
        } 

        void SubscriberDivBy2EventHandler(object sender, CustomEventArgs e)
        {
            Debug.WriteLine(id + " received a Divisible by 2 message: " + e.Message);
        }

        void SubscriberHalfwayEventHandler(object sender, CustomEventArgs e)
        {
            Debug.WriteLine(id + " received a Halfway message: " + e.Message);
        }

    }  //EventSubscriber
}  //namespace