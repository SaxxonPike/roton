using System.Collections.Generic;

namespace Roton
{
    public partial class Display
    {
        protected int Ammo => DisplayInfo.Ammo;

        protected int Gems => DisplayInfo.Gems;

        protected IList<bool> Keys => DisplayInfo.Keys;

        protected int TorchCycles => DisplayInfo.TorchCycles;

        protected int Torches => DisplayInfo.Torches;
    }
}