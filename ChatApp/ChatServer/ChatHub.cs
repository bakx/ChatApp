﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace ChatServer
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        static string JOINED_ROOM = "{0} has entered the room";
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
            Client client = Config.Instance.GetClient(Context.ConnectionId);

            if (client != null)
            {
                client.Name = name;
                SendWelcomeMessage(name);
            }
        }

        public void SendWelcomeMessage(string name)
        {
            Clients.All.SendMessage(
            string.Format(CHAT_MESSAGE,
                "SYSTEM",
                string.Format(JOINED_ROOM, name),
                 DateTime.Now.ToLongTimeString()
                 ));
        }

        /// <summary>
        /// General chat room
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            // Send the message 
            Clients.All.SendMessage(GetChatMessage(message));
        }

        public string GetChatMessage(string message)
        {
            // Get the client, based on connection ID
            Client client = Config.Instance.GetClient(Context.ConnectionId);

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
    }
}