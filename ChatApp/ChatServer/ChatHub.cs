using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatServer
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        static string JOINED_ROOM = "{0} has entered the room";
        static string CHAT_MESSAGE = "<p><b>{0}</b>: {1}</p>";
        static string SYSTEM_MESSAGE_JOIN_ROOM = "JOIN_ROOM|{0}";

        public override Task OnConnected()
        {
            Client client = new Client();
            client.ConnectionID = Context.ConnectionId;
            client.Ip = Context.Request.GetHttpContext().Request.ServerVariables["REMOTE_ADDR"];

            Config.Instance.clients.Add(client);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Config.Instance.clients.Exists(c => c.ConnectionID == Context.ConnectionId))
                Config.Instance.clients.Remove(Config.Instance.clients.Find(c => c.ConnectionID == Context.ConnectionId));

            return base.OnDisconnected(stopCalled);
        }

        public void Authenticate(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (Config.Instance.Authenticate(username, password))
                {

                }
                else
                {
                    // Get request
                    Client client = Config.Instance.clients.Find(c => c.ConnectionID == Context.ConnectionId);

                    if (client != null)
                        SystemMessage(client, "Unknown User Name or Bad Password");
                }
            }
        }

        /// <summary>
        /// Client joins a room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task JoinRoom(string room)
        {
            string roomID = room + "_" + Guid.NewGuid().ToString();

            await Groups.Add(Context.ConnectionId, roomID);

            // Get the client reference
            Client client = GetClient(Context.ConnectionId);

            // Return feedback to the client
            SystemMessage(client, string.Format(SYSTEM_MESSAGE_JOIN_ROOM, roomID));

            // Notify all clients in the room
            SendMessageToRoom(roomID, string.Format(JOINED_ROOM, client.Name));
        }

        /// <summary>
        /// Client leaves a room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Task LeaveRoom(string room)
        {
            return Groups.Remove(Context.ConnectionId, room);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>

        public void SendMessageToRoom(string room, string message)
        {
            // Get the client, based on connection ID
            Client client = GetClient(Context.ConnectionId);

            // Prepare the message
            string chatMessage = string.Format(CHAT_MESSAGE, client.Name, message);

            // Send the message to the room
            Clients.Group(room).addChatMessage(System.Web.HttpUtility.HtmlEncode(chatMessage));
        }

        public void SendMessage(string name, string message)
        {
            Clients.All.SendMessage(name, System.Web.HttpUtility.HtmlEncode(message));
        }

        /// <summary>
        /// Handles system messages
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void SystemMessage(Client client, string message)
        {
            Clients.Client(client.ConnectionID).SystemMessage(System.Web.HttpUtility.HtmlEncode(message));
        }

        // Helper functions
        public Client GetClient(string connectionID)
        {
            return Config.Instance.clients.Find(c => c.ConnectionID == connectionID);
        }

        // Debug functions
        public int GetClients()
        {
            return Config.Instance.clients.Count;
        }
    }
}