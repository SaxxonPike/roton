using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Cheats;

namespace Roton.Emulation.SuperZzt
{
    public class SuperZztCheatList : CheatList
    {
        public SuperZztCheatList(ICollection<ICheat> cheats) : base(new Dictionary<string, ICheat>
        {
            {"AMMO", cheats.OfType<AmmoCheat>().Single()},
            {"DARK", cheats.OfType<DarkCheat>().Single()},
            {"GEMS", cheats.OfType<GemsCheat>().Single()},
            {"HEALTH", cheats.OfType<HealthCheat>().Single()},
            {"KEYS", cheats.OfType<KeysCheat>().Single()},
            {"TIME", cheats.OfType<TimeCheat>().Single()},
            {"ZAP", cheats.OfType<ZapCheat>().Single()},
            {"Z", cheats.OfType<ZCheat>().Single()},
            {"NOZ", cheats.OfType<NoZCheat>().Single()}
        })
        {
        }
    }
}