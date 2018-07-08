using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Items;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztItemList : Items.ItemList
    {
        public SuperZztItemList(ICollection<IItem> items) : base(new Dictionary<string, IItem>
        {
            {"AMMO", items.OfType<AmmoItem>().Single()},
            {"GEMS", items.OfType<GemsItem>().Single()},
            {"HEALTH", items.OfType<HealthItem>().Single()},
            {"SCORE", items.OfType<ScoreItem>().Single()},
            {"TIME", items.OfType<TimeItem>().Single()},
            {"Z", items.OfType<ZItem>().Single()},
        })
        {
        }
    }
}