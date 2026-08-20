using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public sealed class Message : IMessage
{
    public Message()
    {
        Text = [string.Empty];
    }

    public Message(IEnumerable<string> message)
    {
        Text = [.. message];
    }

    public Message(params string[] message)
    {
        Text = [.. message];
    }

    public IReadOnlyList<string> Text { get; }
}