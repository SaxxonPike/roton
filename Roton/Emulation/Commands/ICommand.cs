using System.Collections.Generic;
using Roton.Core;

namespace Roton.Emulation.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(IOopContext context);
    }
}