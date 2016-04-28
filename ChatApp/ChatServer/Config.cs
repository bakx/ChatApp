using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public sealed class Config
    {
        private static volatile Config instance = new Config();
        private static object syncRoot = new Object();

        private Config() { }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Config();
                    }
                }

                return instance;
            }
        }

        public List<Client> clients = new List<Client>();

        public static void LoadConfig()
        {
        }

        public bool Authenticate(string username, string password)
        {
            return true;
        }
    }
}