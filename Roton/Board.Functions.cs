using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Roton
{
    abstract public partial class Board
    {
        public Board()
        {
            Initialize();

            if (this.Camera == null)
            {
                this.Camera = new Location();
            }

            if (this.Enter == null)
            {
                this.Enter = new Location();
            }
        }

        virtual protected void Initialize()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
