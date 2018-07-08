using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Cheats;

namespace Roton.Emulation.Zzt
{
    public class ZztCheatList : CheatList
    {
        public ZztCheatList(ICollection<ICheat> cheats) : base(new Dictionary<string, ICheat>
        {
            {"AMMO", cheats.OfType<AmmoCheat>().Single()},
            {"DARK", cheats.OfType<DarkCheat>().Single()},
            {"GEMS", cheats.OfType<GemsCheat>().Single()},
            {"HEALTH", cheats.OfType<HealthCheat>().Single()},
            {"KEYS", cheats.OfType<KeysCheat>().Single()},
            {"TIME", cheats.OfType<TimeCheat>().Single()},
            {"TORCHES", cheats.OfType<TorchesCheat>().Single()},
            {"ZAP", cheats.OfType<ZapCheat>().Single()}
        })
        {
        }
    }
}