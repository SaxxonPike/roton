using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace Roton
{
    public partial class Vector
    {
        virtual public int X { get; set; }
        virtual public int Y { get; set; }

        static public Vector East
        {
            get
            {
                return new Vector(1, 0);
            }
        }

        static public Vector North
        {
            get
            {
                return new Vector(0, -1);
            }
        }

        static public Vector South
        {
            get
            {
                return new Vector(0, 1);
            }
        }

        static public Vector West
        {
            get
            {
                return new Vector(-1, 0);
            }
        }
    }
}
