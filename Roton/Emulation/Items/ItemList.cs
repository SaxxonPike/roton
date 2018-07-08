using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Items
{
    public abstract class ItemList : IItemList
    {
        private readonly IDictionary<string, IItem> _items;

        protected ItemList(IDictionary<string, IItem> items)
        {
            _items = items;
        }
        
        public IItem Get(string name)
        {
            return _items.ContainsKey(name)
                ? _items[name]
                : null;
        }        
    }
}