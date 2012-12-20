using System;
using System.Messaging;
using BroccoliTrade.Domain.Models;

namespace BroccoliTrade.Logics.MSMQ
{
    public class QueueProcessor
    {
        private static MessageQueue msgQ;
        private static object lockObject = new object();

        public static void StartProcessing()
        {
            string queuePath = @".\private$\BroccoliEmails";

            QueueService.InsureQueueExists(queuePath);

            msgQ = new MessageQueue(queuePath);
            msgQ.Formatter = new BinaryMessageFormatter();
            msgQ.MessageReadPropertyFilter.SetAll();
            msgQ.ReceiveCompleted += msgQ_ReceiveCompleted;
            msgQ.BeginReceive();
        }

        static void msgQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
           lock (lockObject)
           {
                // The message is plain text.
                var msg = (EmailMessage)e.Message.Body;
                new EmailService().SendMessage(msg, 
                    "richard.s.popov@gmail.com", 
                    "hbxfhl91", 
                    "smtp.gmail.com", 
                    587, 
                    true);

                Console.WriteLine("Message received: " + msg.Message);
           }

           // Listen for the next message.
           msgQ.BeginReceive();
        }

        public static void StopProcessing()
        {
            msgQ.Close();
        }
    }
}
