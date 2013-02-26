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
            var messages = msgQ.GetAllMessages();
            msgQ.Purge();

            foreach (var message in messages)
            {
                var msg = (EmailMessage) message.Body;
                msgQ_Send(msg);
            }

            msgQ.Close();
        }

        static void msgQ_Send(EmailMessage message)
        {
            lock (lockObject)
            {
                // The message is plain text.
                new EmailService().SendMessage(message,
                    "support@broccoli-trade.ru",
                    "gqTS4Zne",
                    "smtp.yandex.ru",
                    587,
                    true);
            }
        }

        public static void StopProcessing()
        {
            msgQ.Close();
        }
    }
}
