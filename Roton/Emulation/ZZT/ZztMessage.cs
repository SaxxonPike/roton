using Roton.Core;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztMessage : IMessage
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