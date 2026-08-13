namespace Roton.Emulation.Commands;

public interface ICommandList
{
    ICommand Get(string name);
}