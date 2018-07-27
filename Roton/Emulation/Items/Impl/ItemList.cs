using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Items.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class ItemList : IItemList
    {
        private readonly Lazy<IDictionary<string, IItem>> _items;

        public ItemList(Lazy<IContextMetadataService> contextMetadataService, Lazy<IEnumerable<IItem>> items)
        {
            _items = new Lazy<IDictionary<string, IItem>>(() =>
            {
                var result = new Dictionary<string, IItem>();
                foreach (var item in items.Value)
                {
                    foreach (var attribute in contextMetadataService.Value.GetMetadata(item))
                        result.Add(attribute.Name, item);
                }

                return result;
            });
        }

        public IItem Get(string name)
        {
            return _items.Value.ContainsKey(name)
                ? _items.Value[name]
                : null;
        }
    }
}