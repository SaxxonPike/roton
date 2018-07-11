using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Items
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class ItemList : IItemList
    {
        private readonly IDictionary<string, IItem> _items = new Dictionary<string, IItem>();

        public ItemList(IContextMetadataService contextMetadataService, IEnumerable<IItem> items)
        {
            foreach (var item in items)
            {
                foreach (var attribute in contextMetadataService.GetMetadata(item))
                    _items.Add(attribute.Name, item);
            }
        }
        
        public IItem Get(string name)
        {
            return _items.ContainsKey(name)
                ? _items[name]
                : null;
        }        
    }
}