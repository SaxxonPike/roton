using System.Collections.Generic;
using Roton.Emulation.Cheats;

namespace Roton.Emulation.SuperZZT
{
    public class ZztCheats : Cheats
    {
        public ZztCheats(IEnumerable<ICheat> cheats) : base(cheats, new[]
        {
            "AMMO",
            "DARK",
            "GEMS",
            "HEALTH",
            "KEYS",
            "TIME",
            "TORCHES",
            "ZAP"
        })
        {
        }
    }
}