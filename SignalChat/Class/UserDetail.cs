namespace SignalChat.Class
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public object UserPublicKey { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public int UserCategoryID { get; set; }
        public string UserCategory { get; set; }
        public int RequestTypeID { get; set; }
    }
}