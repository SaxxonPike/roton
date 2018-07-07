using System.Collections.Generic;
using Roton.Emulation.Cheats;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztCheats : Cheats
    {
        public SuperZztCheats(IEnumerable<ICheat> cheats) : base(cheats, new[]
        {
            "AMMO",
            "GEMS",
            "HEALTH",
            "KEYS",
            "NOZ",
            "TIME",
            "Z",
            "ZAP"
        })
        {
        }
    }
}