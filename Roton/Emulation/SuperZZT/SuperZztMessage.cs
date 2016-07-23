using Roton.Core;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztMessage : IMessage
    {
        public SuperZztMessage()
        {
            Text = new[] {string.Empty};
        }

        public SuperZztMessage(string message)
        {
            Text = new[] {message};
        }

        public SuperZztMessage(string message, string message2)
        {
            Text = new[] {message, message2};
        }

        public string[] Text { get; }
    }
}