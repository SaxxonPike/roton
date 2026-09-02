using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMessageHandler
{
    IScrollState? ExecuteMessage(ref OopContext context);
    string[] GetMessageLines();
}