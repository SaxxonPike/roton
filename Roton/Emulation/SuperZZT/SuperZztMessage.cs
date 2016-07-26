using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztMessage : IMessage
    {
        public SuperZztMessage()
        {
            Text = new[] {string.Empty};
        }

        public SuperZztMessage(string message)
        {
            Text = new[] {string.Empty, message};
        }

        public SuperZztMessage(string message, string message2)
        {
            Text = new[] {message, message2};
        }

        public string[] Text { get; }
    }
}