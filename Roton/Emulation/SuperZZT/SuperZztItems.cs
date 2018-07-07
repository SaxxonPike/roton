using System.Collections.Generic;
using Roton.Emulation.Items;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztItems : Items
    {
        public SuperZztItems(IEnumerable<IItem> items) : base(items, new string[]{})
        {
        }
    }
}