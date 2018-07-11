using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class CommandList : ICommandList
    {
        private readonly IDictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public CommandList(IContextMetadataService contextMetadataService, IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(command))
                    _commands.Add(attribute.Name, command);
            }
        }
        
        public ICommand Get(string name)
        {
            return _commands.ContainsKey(name)
                ? _commands[name]
                : null;
        }        
    }
}