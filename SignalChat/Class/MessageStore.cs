namespace SignalChat.Class
{
    public class MessageStore
    {
        public string FromUsername { get; set; }
        public object UserPublicKey { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
    }
}