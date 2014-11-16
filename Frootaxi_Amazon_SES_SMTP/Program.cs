using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Frootaxi_Amazon_SES_SMTP
{
    class Program
    {
        static bool IsRunning = false;
        static Thread thread;
        static Thread thread2;

        static void Main(string[] args)
        {
            IsRunning = true;
            thread = new Thread(Process);
            thread.Start();

            thread2 = new Thread(Process2);
            thread2.Start();

            Console.ReadLine();
            IsRunning = false;
        }

        static void Process()
        {
            while (IsRunning)
            {
                SendEmail se = new SendEmail();
                se.Send();
            }
        }

        static void Process2()
        {
            while (IsRunning)
            {
                Thread.Sleep(259200000);

                SendEmail se = new SendEmail();
                se.ClearSentEmails();
            }
        }
    }
}