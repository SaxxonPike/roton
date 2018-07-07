using Roton.Core;
using Roton.Extensions;

namespace Roton.Emulation.Commands
{
    public class DieCommand : ICommand
    {
        private readonly IElements _elements;

        public DieCommand(IElements elements)
        {
            _elements = elements;
        }
        
        public string Name => "DIE";
        
        public void Execute(IOopContext context)
        {
            context.Died = true;
            context.DeathTile.SetTo(_elements.EmptyId, 0);
        }
    }
}