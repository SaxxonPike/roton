using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Roton.Windows
{
    public partial class Keyboard : Component, IKeyboard
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        public Keyboard(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool Alt
        {
            get
            {
                return false;
            }
        }

        public void Clear()
        {
        }

        public bool Control
        {
            get
            {
                return false;
            }
        }

        public int GetKey()
        {
            return 0;
        }

        public bool Shift
        {
            get
            {
                return false;
            }
        }
    }
}
