namespace Roton.Emulation.Commands
{
    public interface ICommands
    {
        ICommand Get(string name);
    }
}