using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl
{
    public class Message : IMessage
    {
        public Message()
        {
            Text = new[] {string.Empty};
        }

        public Message(IEnumerable<string> message)
        {
            Text = new List<string>(message).ToArray();
        }

        public Message(params string[] message)
        {
            Text = new List<string>(message).ToArray();
        }

        public string[] Text { get; }
    }
}