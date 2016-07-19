using Roton.Core;

namespace Roton.Emulation.ZZT
{
    internal sealed partial class Core
    {
        public override void Interact_Ammo(IXyPair location, int index, IXyPair vector)
        {
            Ammo += 5;
            base.Interact_Ammo(location, index, vector);
        }

        public override void Interact_Gem(IXyPair location, int index, IXyPair vector)
        {
            Health += 1;
            base.Interact_Gem(location, index, vector);
        }
    }
}