using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Display
    {
        protected int Ammo
        {
            get { return DisplayInfo.Ammo; }
        }

        protected int Gems
        {
            get { return DisplayInfo.Gems; }
        }

        protected IList<bool> Keys
        {
            get { return DisplayInfo.Keys; }
        }

        protected int TorchCycles
        {
            get { return DisplayInfo.TorchCycles; }
        }

        protected int Torches
        {
            get { return DisplayInfo.Torches; }
        }
    }
}
