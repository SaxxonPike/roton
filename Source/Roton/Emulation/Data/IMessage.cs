using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IMessage
{
    IReadOnlyList<string> Text { get; }
}