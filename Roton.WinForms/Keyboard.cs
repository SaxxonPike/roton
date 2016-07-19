using System.ComponentModel;
using Roton.Core;

namespace Roton.WinForms
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

        public bool Alt => false;

        public void Clear()
        {
        }

        public bool Control => false;

        public int GetKey()
        {
            return 0;
        }

        public bool Shift => false;
    }
}