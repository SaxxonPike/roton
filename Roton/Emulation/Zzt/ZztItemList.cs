using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Items;

namespace Roton.Emulation.Zzt
{
    public class ZztItemList : Items.ItemList
    {
        public ZztItemList(ICollection<IItem> items) : base(new Dictionary<string, IItem>
        {
            {"AMMO", items.OfType<AmmoItem>().Single()},
            {"GEMS", items.OfType<GemsItem>().Single()},
            {"HEALTH", items.OfType<HealthItem>().Single()},
            {"SCORE", items.OfType<ScoreItem>().Single()},
            {"TIME", items.OfType<TimeItem>().Single()},
            {"TORCHES", items.OfType<TorchesItem>().Single()},
        })
        {
        }
    }
}