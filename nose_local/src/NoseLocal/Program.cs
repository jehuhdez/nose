using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using IronCow;

namespace Nose.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            DropboxFacade dbox = new DropboxFacade();
            ProcessLogger ctrl = new ProcessLogger(dbox);
            ctrl.start();
            Thread.Sleep(120000);
            ctrl.stop("");

            //RTMFacade rtm = new RTMFacade();
            //Console.WriteLine("authenticated: " + rtm.Authenticated);
            //Console.Read();
            //Console.Write("list to display: ");
            //string userList = Console.ReadLine().Trim();

            //TaskList list = rtm.getTaskList(userList);
            //var activeTasks = from task in list.Tasks
            //                  where task.IsIncomplete
            //                  orderby task.Created
            //                  select task;

            //foreach (Task t in activeTasks)
            //{
            //    Console.WriteLine("({0}) {1}:\t{2}", t.Id, t.Created, t.Name);
            //}

            
            Console.Read();
        }
    }
}