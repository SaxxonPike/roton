using Roton.Core;

namespace Roton.Emulation.ZZT
{
    public class ZztMessage : IMessage
    {
        public ZztMessage()
        {
            Text = new[] {string.Empty};
        }

        public ZztMessage(string message)
        {
            Text = new[] {message};
        }

        public string[] Text { get; }
    }
}