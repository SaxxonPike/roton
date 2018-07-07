using System.Collections.Generic;
using Roton.Emulation.Items;

namespace Roton.Emulation.SuperZZT
{
    public class ZztItems : Items
    {
        public ZztItems(IEnumerable<IItem> items) : base(items, new string[]{})
        {
        }
    }
}