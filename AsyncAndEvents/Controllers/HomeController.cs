using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using AsyncAndEvents.Models;
using System.Diagnostics;

namespace AsyncAndEvents.Controllers
{
    public class HomeController : Controller
    {
        private HomeModel model;

        public ActionResult Index()
        {
            model = new HomeModel();
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult FaceOfAsync()
        {
            HomeModel model = new HomeModel { StringInput = "tacocat"};
            return View("FaceOfAsync", model);
        }

        [HttpPost]
        public async Task<ActionResult> FaceOfAsync(HomeModel model)
        {
            Task<string> thisWillBeStringData4 = CallsSubmethodAsync(model.StringInput);
            model.StringData = CallsSubmethod(model.StringInput);
            model.StringData2 = CallsSubmethodAsyncResult(model.StringInput);
            model.StringData3 = CallsSubmethodAsync(model.StringInput).Result;
            model.StringData4 = await thisWillBeStringData4;
            return View("FaceOfAsync", model);
        }

        #region AsyncMethods
        public async Task<ActionResult> ReturnWithNumbers()
        {
            Debug.WriteLine("The controller method is starting.");
            model = new HomeModel();
            model.BooleanData = false;
            model.StringData = "";

            Debug.WriteLine("Start the counting routine, which takes a while");
            Task<string> result = CountWhileWaitingAsync();
            Debug.WriteLine("The controller method called the counter, and is awaiting the result.");
            Debug.WriteLine("Controller method: The counter itself is waiting, and has returned control to me.  Let's do a little work.");
            model.BooleanData = true;

            Debug.WriteLine("Nothing more to do.  Now I will just await the result.");
            model.StringData = await result;

            Debug.WriteLine("The controller method has all its info.");
            return View("Index", model);
        }

        private async Task<string> CountWhileWaitingAsync()
        {
            Debug.WriteLine("I am the counter, and am starting.");
            StringBuilder sb = new StringBuilder();
            Task<int> getNumberTask = Return999SlowlyAsync();
            Debug.WriteLine("I am the counter, and have started the long-running subroutine.  Meanwhile, let's do some counting");
            for (int i = 1; i < 4; i++)
            {
                sb.Append(i + ", ");
            }
            Debug.WriteLine("The counter has finished counting, and has nothing more to do except wait for the long-running subroutine to finish.");
            int finalNumber = await getNumberTask;
            sb.Append(finalNumber);
            Debug.WriteLine("I am the counter, and have all my info.");
            return sb.ToString();
        }

        private int Return999Slowly()
        {
            System.Threading.Thread.Sleep(3000);
            return 999;
        }

        private async Task<int> Return999SlowlyAsync()
        {
            await Task.Delay(3000);
            return 999;
        }
        #endregion

        #region FaceOfAsyncMethods
        public string CallsSubmethod(string submission)
        {
            return Submethod(submission);
        }

        public string CallsSubmethodAsyncResult(string submission)
        {
            return SubmethodAsync(submission).Result;
        }

        public async Task<string> CallsSubmethodAsync(string submission)
        {
            Task<string> submissionresponsetask = SubmethodAsync(submission);
            //The task is set in motion.  Meanwhile, you could do things here.
            string submissionresponse = await submissionresponsetask;
            return submissionresponse;
        }

        public async Task<string> SubmethodAsync(string submission)
        {
            char[] submissionarray = submission.ToCharArray();
            Array.Reverse(submissionarray);
            return new string(submissionarray);
        }

        public string Submethod(string submission)
        {
            char[] submissionarray = submission.ToCharArray();
            Array.Reverse(submissionarray);
            return new string(submissionarray);
        }


        #endregion

        #region Delegates
        private delegate void DelegateType1(int i);
        private delegate void DelegateType2();

        public ActionResult LetDelegatesSpeak()
        {
            model = new HomeModel();
            model.DelegateMessage = "";
            //define three different delegates, as handlers of three conditions, to which actions can be assigned
            //they share the same signature, so they can each be passed to the Handler method as an argument
            //alternatively, you could just call the delegate directly(as below)
            DelegateType1 handlerForDivBy2;
            DelegateType1 handlerForDivBy3;
            DelegateType1 handlerFor10;


            //assign four methods to assign to the three handlers
            handlerForDivBy2 = Action1;
            handlerForDivBy3 = Action3;
            handlerForDivBy2 += Action2;
            handlerForDivBy3 += Action2;
            handlerFor10 = Action4;

            //If you are not going to share use of HandleThis, and are going to call the handler directly,
            //you can have different signatures, like this
            DelegateType2 otherHandlerFor10;
            otherHandlerFor10 = Action5;

            for (int i = 0; i < 11; i++)
            {
                model.DelegateMessage += (i + "\n");
                if (i % 2 == 0)
                {
                    model.DelegateMessage += "handler passed to HandleThis as as argument:";
                    HandleThis(model, handlerForDivBy2, i);
                    model.DelegateMessage += "handler called directly:";
                    handlerForDivBy2(i);
                }
                if (i % 3 == 0)
                {
                    HandleThis(model, handlerForDivBy3, i);

                }
                if (i == 10)
                {
                    HandleThis(model, handlerFor10, i);
                    otherHandlerFor10();
                }
            }

            //now things have changed, and we don't want to use one of the methods
            handlerForDivBy2 -= Action2;
            handlerForDivBy3 -= Action2;
            model.DelegateMessage += ("\n\nRepeat without Action 2... \n");


            for (int i = 0; i < 11; i++)
            {
                model.DelegateMessage += (i + "\n");
                if (i % 2 == 0)
                {
                    HandleThis(model, handlerForDivBy2, i);
                }
                if (i % 3 == 0)
                {
                    HandleThis(model, handlerForDivBy3, i);

                }
                if (i == 10)
                {
                    HandleThis(model, handlerFor10, i);
                }
            }
            return View("Index", model);
        }

        private void Action1(int i)
        {
            model.DelegateMessage += i + " is divisible by 2.\n";
        }

        private void Action2(int i)
        {
            model.DelegateMessage += "I am also notified when divisible by 2, 3.\n";
        }

        public void Action3(int i)
        {
            model.DelegateMessage += i + " is divisible by 3.\n";
        }

        private void Action4(int i)
        {
            model.DelegateMessage += i + " is 10.\n";
        }

        private void Action5()
        {
            model.DelegateMessage += "The value is 10.\n";
        }

        private void HandleThis(HomeModel model, DelegateType1 d, int i)
        {
            d(i);
        }
        #endregion

        #region Events

        public ActionResult TriggerEvents()
        {
            model = new HomeModel();
            model.EventMessage = "";
            EventPublisher pub = new EventPublisher();
            EventSubscriber sub1 = new EventSubscriber("sub1", pub, true, false);
            EventSubscriber sub2 = new EventSubscriber("sub2", pub, false, true);

            pub.Count();

            return View("Index", model);
        }
        #endregion

    }  //class
}  //namespace