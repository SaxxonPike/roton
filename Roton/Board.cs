using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Roton
{
    abstract public partial class Board
    {
        virtual public Location Camera { get; set; }
        virtual public bool Dark { get; set; }
        virtual public Location Enter { get; set; }
        virtual public int ExitEast { get; set; }
        virtual public int ExitNorth { get; set; }
        virtual public int ExitSouth { get; set; }
        virtual public int ExitWest { get; set; }
        virtual public string Name { get; set; }
        virtual public bool RestartOnZap { get; set; }
        virtual public int Shots { get; set; }
        virtual public int TimeLimit { get; set; }
    }
}
