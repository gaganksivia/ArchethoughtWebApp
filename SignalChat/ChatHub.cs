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
            UserDetail ReceiverUser = UserDetail.ConnectedUsers.Where(x => x.UserName == ToUserName).FirstOrDefault();
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
                var TempMessages = UserStoreMessage.Where(x => x.ToUsername == ToUserName && x.Status == false).ToList();
                TempMessages.Add(NewMessage);
                Clients.Client(ReceiverUser.ConnectionId).receivemsg(TempMessages);
            }
            else
            {
                UserStoreMessage.Add(new StoreMessage { FromUsername = FromUserName, Message = Message, ToUsername = ToUserName, UserPublicKey = SenderPublicKey, MessageDateTime = DateTime.Now, Status = false });
            }
        }
        public void SendKey(string name, string receiverusername, object PublicKey)
        {
            if (name.Trim() == "" || PublicKey == null)
                return;
            UserDetail user = UserDetail.ConnectedUsers.Where(x => x.UserName == name).FirstOrDefault();
            if (user != null)
            {
                Clients.Client(user.ConnectionId).sendrequestkey(PublicKey, receiverusername);
            }
        }

        public void setuser(string username)
        {
            UserDetail User = UserDetail.ConnectedUsers.Where(x => x.UserName == username).FirstOrDefault();
            if (User != null)
            {
                var TempMessages = UserStoreMessage.Where(x => x.ToUsername == username && x.Status == false).ToList();
                User.Status = true;
                User.ConnectionId = Context.ConnectionId;
                UserStoreMessage.Where(x => x.ToUsername == username && x.Status == false).ToList().ForEach(x => x.Status = true);
                if (User.UserCategoryID != 5)//Sync Admins
                {
                    Clients.Client(User.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.SelectedAdminToChat == User.UserName).ToArray(), TempMessages.ToArray());
                }
                else//Sync Clients and admins
                {
                    Clients.Client(User.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.UserName == User.SelectedAdminToChat).ToArray(), TempMessages.ToArray());
                    UserDetail AdminUser = UserDetail.ConnectedUsers.Where(x => x.UserName == User.SelectedAdminToChat).FirstOrDefault();
                    Clients.Client(AdminUser.ConnectionId).newuseradd(UserDetail.ConnectedUsers.Where(x => x.UserName == username).ToArray(), TempMessages.ToArray());
                }
            }
        }
        public void AskUserPublicKey(string name, string myusername)
        {
            // Call the broadcastMessage method to update clients.

            if (UserDetail.ConnectedUsers.Count() > 1)
            {
                UserDetail RequestedUserInfo = UserDetail.ConnectedUsers.Where(x => x.UserName == name).FirstOrDefault();
                //Clients.Client(Context.ConnectionId).getuserkey(RequestedUserInfo.UserPublicKey, name, ConnectedUsers.Select(x => x.UserName).ToArray());
                //UserDetail SenderInfo = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                Clients.Client(RequestedUserInfo.ConnectionId).getuserkey(myusername, name, UserDetail.ConnectedUsers.Where(x => x.UserName != myusername && x.UserCategoryID == 5).ToArray());
            }
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
            var msg = UserStoreMessage.Where(x => x.ToUsername == receiverusername && x.Status == false).ToList();
            if (msg != null)
            {
                Clients.Client(Context.ConnectionId).receivearchivemsg(msg);
                //UserDetail user = ConnectedUsers.Where(x => x.UserName == msg[0].FromUsername).FirstOrDefault();
                //if (user != null)
                //{
                //    Clients.Client(user.ConnectionId).askforresend(msg.ToArray());
                //}
            }
        }
        public void SendArryMsg(object arrymsg, string ToUser, string FromUser)
        {
            if (arrymsg != null)
            {
                UserDetail user = UserDetail.ConnectedUsers.Where(x => x.UserName == ToUser).FirstOrDefault();
                if (user != null)
                {
                    Clients.Client(user.ConnectionId).receivearchivemsg(arrymsg, FromUser);
                    UserStoreMessage.Where(x => x.ToUsername == ToUser && x.Status == false).ToList().ForEach(x => x.Status = true);
                }
            }
        }
    }
}