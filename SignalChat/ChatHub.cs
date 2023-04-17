using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using SignalChat.Class;
using System.Data.SqlClient;

namespace SignalChat
{
    public class ChatHub : Hub
    {
        static SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[tbl_Request]" +
           "([request_date]" +
           ",[fromusername]" +
           ",[message]" +
           ",[tousername]" +
           ",[public_key])" +
           "VALUES" +
           "(getdate()" +
           ",@fromusername" +
           ",@message" +
           ",@tousername" +
           ",@public_key)", new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString));

        static List<StoreMessage> UserStoreMessage = new List<StoreMessage>();

        public void Send(string FromUserName, string Message, string ToUserName, object SenderPublicKey)
        {
            try
            {
                UserDetail ReceiverUser = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == ToUserName.ToLower()).FirstOrDefault();
                if (ReceiverUser != null && ReceiverUser.Status == true)
                {
                    var NewMessage = new StoreMessage { FromUsername = FromUserName, Message = Message, ToUsername = ToUserName, UserPublicKey = SenderPublicKey, MessageDateTime = DateTime.Now, Status = true };
                    UserStoreMessage.Add(NewMessage);
                    if (cmd.Parameters.Count == 0)
                    {
                        cmd.Parameters.Add("@fromusername", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@message", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@tousername", System.Data.SqlDbType.NVarChar);
                        cmd.Parameters.Add("@public_key", System.Data.SqlDbType.NVarChar);
                    }
                    cmd.Parameters["@fromusername"].Value = FromUserName;
                    cmd.Parameters["@message"].Value = Message;
                    cmd.Parameters["@tousername"].Value = ToUserName;
                    cmd.Parameters["@public_key"].Value = SenderPublicKey.ToString();
                    if (cmd.Connection.State != System.Data.ConnectionState.Open) { cmd.Connection.Open(); }
                    cmd.ExecuteNonQuery();
                    var TempMessages = UserStoreMessage.Where(x => x.ToUsername.ToLower() == ToUserName.ToLower() && x.Status==false).ToList();
                    TempMessages.Add(NewMessage);
                    Clients.Client(ReceiverUser.ConnectionId).receivemsg(TempMessages);
                }
                else
                {
                    UserStoreMessage.Add(new StoreMessage { FromUsername = FromUserName, Message = Message, ToUsername = ToUserName, UserPublicKey = SenderPublicKey, MessageDateTime = DateTime.Now, Status = false });
                }
            }
            catch { }
        }
        public void SendKey(string name, string receiverusername, object PublicKey)
        {
            if (name.Trim() == "" || PublicKey == null)
                return;
            UserDetail user = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == name.ToLower()).FirstOrDefault();
            if (user != null)
            {
                Clients.Client(user.ConnectionId).sendrequestkey(PublicKey, receiverusername);
            }
        }

        public void setuser(string username)
        {
            try
            {
                UserDetail User = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
                if (User != null)
                {
                    var TempMessages = UserStoreMessage.Where(x => x.ToUsername.ToLower() == username.ToLower() && x.Status == false).ToList();
                    User.Status = true;
                    User.ConnectionId = Context.ConnectionId;
                    UserStoreMessage.Where(x => x.ToUsername.ToLower() == username.ToLower() && x.Status == false).ToList().ForEach(x => x.Status = true);
                    if (User.UserCategoryID != 5)//Sync Admins
                    {
                        Clients.Client(User.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.SelectedAdminToChat.ToLower() == User.UserName.ToLower()).ToArray(), TempMessages.ToArray());
                    }
                    else//Sync Clients and admins
                    {
                        Clients.Client(User.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == User.SelectedAdminToChat.ToLower()).ToArray(), TempMessages.ToArray());
                        UserDetail AdminUser = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == User.SelectedAdminToChat.ToLower()).FirstOrDefault();
                        Clients.Client(AdminUser.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == username.ToLower()).ToArray(), TempMessages.ToArray());
                    }
                }
            }
            catch { }
        }
        public void AskUserPublicKey(string name, string myusername)
        {
            // Call the broadcastMessage method to update clients.
            try
            {
                if (UserDetail.ConnectedUsers.Count() > 1)
                {
                    UserDetail RequestedUserInfo = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == name.ToLower()).FirstOrDefault();
                    //Clients.Client(Context.ConnectionId).getuserkey(RequestedUserInfo.UserPublicKey, name, ConnectedUsers.Select(x => x.UserName).ToArray());
                    //UserDetail SenderInfo = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                    Clients.Client(RequestedUserInfo.ConnectionId).getuserkey(myusername, name, UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() != myusername.ToLower() && x.UserCategoryID == 5).ToArray());
                }
            }
            catch { }
        }
        public void ClearUsersData()
        {
            UserDetail.ConnectedUsers.Clear();
            Clients.All.refreshpage();
        }

        public void SignOut(string username)
        {
            UserDetail user = UserDetail.ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user != null)
            {
                user.Status = false;
            }
        }

        public void SendArchiveMessageToSender(string receiverusername)
        {
            var msg = UserStoreMessage.Where(x => x.ToUsername.ToLower() == receiverusername.ToLower() && x.Status == false).ToList();
            if (msg != null)
            {
                Clients.Client(Context.ConnectionId).receivearchivemsg(msg);

            }
        }
        public void SendArryMsg(object arrymsg, string ToUser, string FromUser)
        {
            if (arrymsg != null)
            {
                UserDetail user = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == ToUser.ToLower()).FirstOrDefault();
                if (user != null)
                {
                    Clients.Client(user.ConnectionId).receivearchivemsg(arrymsg, FromUser);
                    UserStoreMessage.Where(x => x.ToUsername.ToLower() == ToUser.ToLower() && x.Status == false).ToList().ForEach(x => x.Status = true);
                }
            }
        }
    }
}