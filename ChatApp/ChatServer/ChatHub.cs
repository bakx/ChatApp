using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatServer
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            Clients.All.SendMessage(name, message);
        }
    }
}