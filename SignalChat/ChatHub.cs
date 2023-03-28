using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using DotNetOpenAuth.Messaging.Bindings;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalChat.Class;

namespace SignalChat
{
    public class ChatHub : Hub
    {

        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<StoreMessage> UserStoreMessage = new List<StoreMessage>();

        //public override Task OnConnected()
        //{
        //    UserDetail user = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
        //    if (user != null)
        //    {
        //        user.Status = true;
        //    }
        //    return base.OnConnected();
        //}


        public void Send(string FromUserName, string Message, string ToUserName, object SenderPublicKey)
        {
            UserDetail ReceiverUser = ConnectedUsers.Where(x => x.UserName == ToUserName).FirstOrDefault();
            if (ReceiverUser!= null && ReceiverUser.Status == true)
            {
                var NewMessage = new StoreMessage { FromUsername = FromUserName, Message = Message, ToUsername = ToUserName, UserPublicKey = SenderPublicKey, MessageDateTime = DateTime.Now, Status = true };
                UserStoreMessage.Add(NewMessage);
                var TempMessages = UserStoreMessage.Where(x => x.ToUsername == ToUserName && x.Status == false).ToList();
                TempMessages.Add(NewMessage);
                Clients.Client(ReceiverUser.ConnectionId).receivemsg(TempMessages);
            }
            else
            {
                UserStoreMessage.Add(new StoreMessage { FromUsername = FromUserName, Message = Message, ToUsername = ToUserName, UserPublicKey = SenderPublicKey, MessageDateTime = DateTime.Now, Status = false });
            }
        }
        public void SendKey(string name, object PublicKey)
        {
            if (name.Trim() == "" || PublicKey == null)
                return;
            UserDetail user = ConnectedUsers.Where(x => x.UserName == name).FirstOrDefault();
            if (user != null)
            {
                Clients.Client(user.ConnectionId).sendrequestkey(PublicKey);
            }
        }

        public void setuser(string username)
        {
            UserDetail User = ConnectedUsers.Where(x => x.UserName == username).FirstOrDefault();
            var TempMessages = UserStoreMessage.Where(x => x.ToUsername == username && x.Status == false).ToList(); ;
            if (User != null)
            {
                ConnectedUsers.Where(x => x.UserName == username).ToList().ForEach(x => x.Status = true);
                ConnectedUsers.Where(x => x.UserName == username).ToList().ForEach(x => x.ConnectionId = Context.ConnectionId);
                UserStoreMessage.Where(x => x.ToUsername == username && x.Status == false).ToList().ForEach(x => x.Status = true);
                Clients.Client(User.ConnectionId).newuseradd(ConnectedUsers.Select(x => x.UserName).ToArray(), TempMessages.ToArray());
            }
            else
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = Context.ConnectionId, UserName = username, Status = true });
                Clients.All.newuseradd(ConnectedUsers.Select(x => x.UserName).ToArray(), "");
            }
            
        }
        public void AskUserPublicKey(string name, string myusername)
        {
            // Call the broadcastMessage method to update clients.

            if (ConnectedUsers.Count() > 1)
            {
                UserDetail RequestedUserInfo = ConnectedUsers.Where(x => x.UserName == name).FirstOrDefault();
                //Clients.Client(Context.ConnectionId).getuserkey(RequestedUserInfo.UserPublicKey, name, ConnectedUsers.Select(x => x.UserName).ToArray());
                //UserDetail SenderInfo = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                Clients.Client(RequestedUserInfo.ConnectionId).getuserkey(myusername, ConnectedUsers.Select(x => x.UserName).ToArray());
            }
        }
        public void ClearUsersData()
        {
            ConnectedUsers.Clear();
            Clients.All.refreshpage();
        }

        public void SignOut(string username)
        {
            UserDetail user = ConnectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
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
                UserDetail user = ConnectedUsers.Where(x => x.UserName == ToUser).FirstOrDefault();
                if (user != null)
                {
                    Clients.Client(user.ConnectionId).receivearchivemsg(arrymsg, FromUser);
                    UserStoreMessage.Where(x => x.ToUsername == ToUser && x.Status == false).ToList().ForEach(x => x.Status = true);
                }
            }
        }
    }
}