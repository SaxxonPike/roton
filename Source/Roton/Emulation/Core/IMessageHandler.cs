using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMessageHandler
{
    ScrollResult ExecuteMessage(ref OopContext context);
    string[] GetMessageLines();
}