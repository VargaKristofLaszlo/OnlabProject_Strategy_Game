using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Client.Helpers
{
    public class MessageStorage
    {
        public List<string> Messages { get; private set; }

        public MessageStorage()
        {
            Messages = new List<string>();
        }

        public void AddMessage(string message) => Messages.Add(message);
    }
}
