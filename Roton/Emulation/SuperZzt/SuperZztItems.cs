using System.Collections.Generic;
using Roton.Emulation.Items;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztItems : Items.Items
    {
        public SuperZztItems(IEnumerable<IItem> items) : base(items, new string[]{})
        {
        }
    }
}