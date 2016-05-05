namespace ChatServer
{
    public enum ClientType : int
    {
        CLIENT,
        OPERATOR,
        ADMIN
    }

    public class Client
    {
        /// <summary>
        /// ConnectionID of the user
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP address of the user
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Client type
        /// </summary>
        public ClientType Type { get; set; }

        /// <summary>
        /// Quick work around to keep track in which support room this client is
        /// </summary>
        public string SupportChatRoomID { get; set; }
    }
}
