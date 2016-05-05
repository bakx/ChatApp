using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace ChatServer
{
    [HubName("support")]
    public class SupportHub : Hub
    {
        static string JOINED_ROOM = "has entered the room with id {0}";
        static string CHAT_MESSAGE = "{{\"name\" : \"{0}\", \"message\" : \"{1}\", \"date\" : \"{2}\"}}";
        static string SYSTEM_MESSAGE_UNKNOWN_USER = "UNKNOWN_USER";

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


        /// <summary>
        /// Sets the name of the chatroom client
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            Client client = GetClient(Context.ConnectionId);

            if (client != null)
                client.Name = name;
        }

        /// <summary>
        /// Client joins a room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task JoinRoom(string room, bool isAgent = false)
        {
            string roomID = (isAgent) ? room : room + "_" + Guid.NewGuid().ToString();

            await Groups.Add(Context.ConnectionId, roomID);

            // Get the client reference
            Client client = GetClient(Context.ConnectionId);

            // Keep track of the room ID
            client.SupportChatRoomID = roomID;

            // Notify all clients in the room
            SendMessageToRoom(string.Format(JOINED_ROOM, roomID));
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

        public void SendMessageToRoom(string message)
        {
            // Get the client, based on connection ID
            Client client = GetClient(Context.ConnectionId);

            // Send the message to the room
            Clients.Group(client.SupportChatRoomID).addChatMessage(GetChatMessage(message));
        }

        public string GetChatMessage(string message)
        {
            // Get the client, based on connection ID
            Client client = GetClient(Context.ConnectionId);

            if (client == null || client.Name == null)
                return SYSTEM_MESSAGE_UNKNOWN_USER;
            else
            {
                message = System.Web.HttpUtility.HtmlEncode(message);
                message = message.Replace("\n", "\\n");

                // Prepare the message
                return string.Format(CHAT_MESSAGE, client.Name, System.Web.HttpUtility.HtmlEncode(message), DateTime.Now.ToLongTimeString());
            }
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