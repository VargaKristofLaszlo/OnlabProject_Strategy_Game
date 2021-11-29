using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class MessageStorage
    {
        public List<(string senderName, string text)> Messages { get; private set; }

        public MessageStorage()
        {
            Messages = new List<(string senderName, string text)>();
        }

        public void AddMessage(string senderUserName, string message) =>
            Messages.Add(new(senderUserName, message));
    }
}
