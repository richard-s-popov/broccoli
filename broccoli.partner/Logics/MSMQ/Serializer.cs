using System.IO;
using System.Xml.Serialization;
using BroccoliTrade.Domain.Models;

namespace BroccoliTrade.Logics.MSMQ
{
    public class Serializer
    {
        public static string SerializeMessage(EmailMessage message)
        {

            var outStream = new StringWriter();
            var s = new XmlSerializer(typeof(EmailMessage));
            s.Serialize(outStream, message);
            return outStream.ToString();
        }
    }
}
