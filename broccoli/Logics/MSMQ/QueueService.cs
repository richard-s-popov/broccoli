using System.Messaging;
using BroccoliTrade.Domain.Models;

namespace BroccoliTrade.Logics.MSMQ
{
    public class QueueService
    {
        public void QueueMessage(EmailMessage message)
        {
            var msg = new Message();
            msg.Body = message;
            msg.Recoverable = true;
            msg.Formatter = new BinaryMessageFormatter();

            const string queuePath = @".\private$\BroccoliEmails";
            
            MessageQueue msgQ;

            InsureQueueExists(queuePath);
            
            msgQ = new MessageQueue(queuePath);
            msgQ.Formatter = new BinaryMessageFormatter();

            msgQ.Send(msg);
        }

        public static void InsureQueueExists(string queuePath)
        {
            //if this queue doesn't exist we will create it
            if (!MessageQueue.Exists(queuePath))
                MessageQueue.Create(queuePath);
        }
    }
}
