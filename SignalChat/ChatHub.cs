using System;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;
using Microsoft.AspNet.SignalR;
namespace SignalChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, object PublicKey,int type)
        {
            // Call the broadcastMessage method to update clients.
            if (type == 1)
            {
                Clients.All.broadcastMessage(name, message, PublicKey);
                
            }
            else
            {
                Clients.All.getkey(name, PublicKey);
            }
        }
        public void SendKey(string name, object PublicKey)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.getkey(name, PublicKey);

        }
    }
}