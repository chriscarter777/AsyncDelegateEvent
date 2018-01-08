using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsyncAndEvents.Models
{
    public class HomeModel
    {
        //for async
        public string StringData { get; set; }
        public bool BooleanData { get; set; }

        //for delegates
        public string DelegateMessage { get; set; }

        //for events
        public string EventMessage { get; set; }
    }
}