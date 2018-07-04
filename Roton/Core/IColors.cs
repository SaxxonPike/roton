using System.Collections.Generic;

namespace Roton.Core
{
    public interface IColors : IEnumerable<string>
    {
        string this[int index] { get; }
    }
}