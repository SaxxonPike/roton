using System.Collections.Generic;
using Roton.Emulation.Items;

namespace Roton.Emulation.Zzt
{
    public class ZztItems : Items.Items
    {
        public ZztItems(IEnumerable<IItem> items) : base(items, new string[]{})
        {
        }
    }
}