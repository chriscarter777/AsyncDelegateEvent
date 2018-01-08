using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsyncAndEvents.Models
{
    public class CustomEventArgs : EventArgs
    {
        private string msg;
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }

        public CustomEventArgs(string s)
        {
            msg = s;
        }  //ctor
    }
}