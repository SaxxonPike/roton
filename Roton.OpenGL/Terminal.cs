using Roton.Windows;
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace Roton.OpenGL {
    public partial class Terminal : UserControl, ITerminal
    {
        private KeysBuffer _keys;
        private bool _shiftHoldX;
        private bool _shiftHoldY;

        public Terminal()
        {
            _keys = new KeysBuffer();

            InitializeComponent();

            // Initialize the GLControl late so that the WinForm designer
            // doesn't crash.
            var glControl = new GLControl {Dock = DockStyle.Fill};
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool Alt {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        public void AttachKeyHandler(Form form) {
            form.KeyDown += (object sender, KeyEventArgs e) => { OnKey(e); };
            form.KeyUp += (object sender, KeyEventArgs e) => { OnKey(e); };
        }

        public void ClearKeys() {
            _keys.Clear();
        }

        public IKeyboard Keyboard {
            get { return _keys as IKeyboard; }
        }

        void OnKey(KeyEventArgs e) {
            if(!e.Shift) {
                Shift = false;
                _shiftHoldX = false;
                _shiftHoldY = false;
            } else {
                Shift = true;
            }
            Alt = e.Alt;
            Control = e.Control;
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e) {
            base.OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }

        public bool Shift {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public bool Control {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            throw new NotImplementedException();
        }

        public void SetScale(int xScale, int yScale)
        {
            throw new NotImplementedException();
        }

        public void SetSize(int width, int height, bool wide)
        {
            throw new NotImplementedException();
        }

        public void Write(int x, int y, string value, int color)
        {
            throw new NotImplementedException();
        }
    }
}
