using System.Collections.Generic;

namespace SignalChat.Class
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public object UserpublicKey { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public int UserCategoryID { get; set; }
        public int RequestTypeID { get; set; }
        public int UserID { get; set; }
        public string SelectedAdminToChat { get; set; }
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
    }
}