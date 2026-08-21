namespace Roton.Emulation.Data;

public interface IExits
{
    ref HWord this[int index] { get; }
    ref HWord East { get; }
    ref HWord North { get; }
    ref HWord South { get; }
    ref HWord West { get; }
}