using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IExits
{
    int this[int index] { get; set; }
    int East { get; set; }
    int North { get; set; }
    int South { get; set; }
    int West { get; set; }
}