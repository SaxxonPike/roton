using System.Collections.Generic;

namespace Roton.Core
{
    public class Message : IMessage
    {
        public Message()
        {
            Text = new[] {string.Empty};
        }

        public Message(params string[] message)
        {
            Text = new List<string>(message).ToArray();
        }

        public string[] Text { get; }
    }
}