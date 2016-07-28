using System.Collections.Generic;

namespace Roton.Core
{
    public interface IFlagList : IEnumerable<string>
    {
        string this[int index] { get; set; }
        void Add(string item);
        void Clear();
        bool Contains(string item);
        bool Remove(string item);
    }
}