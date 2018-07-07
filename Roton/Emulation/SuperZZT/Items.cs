using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Items;

namespace Roton.Emulation.SuperZZT
{
    public abstract class Items : IItems
    {
        private readonly IDictionary<string, IItem> _items;

        protected Items(IEnumerable<IItem> items, string[] enabledNames)
        {
            _items = items
                .Where(c => enabledNames.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c);
        }
        
        public IItem Get(string name)
        {
            return _items.ContainsKey(name)
                ? _items[name]
                : null;
        }        
    }
}