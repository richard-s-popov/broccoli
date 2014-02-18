using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BroccoliTrade.Logics.MSMQ;

namespace WebsiteEmailProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueProcessor.StartProcessing();

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                // Press q to exit.
                Thread.Sleep(0);
            }
        }
    }
}
