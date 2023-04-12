using System;

namespace SignalChat.Class
{
    public class StoreMessage
    {
        public string FromUsername { get; set; }
        public string ToUsername { get; set; }
        public string Message { get; set; }
        public object UserPublicKey { get; set; }
        public DateTime MessageDateTime { get; set; }
        public bool Status { get; set; }
    }
}