using System.Collections.Generic;

namespace Roton.Core
{
    public interface IColorList : IEnumerable<string>
    {
        string this[int index] { get; }
    }
}