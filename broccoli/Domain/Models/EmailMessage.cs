using System;

namespace BroccoliTrade.Domain.Models
{
    [Serializable]
    public class EmailMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string DisplayNameFrom { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; } 
    }
}
