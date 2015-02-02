using Roton;
using System;
using System.Windows.Forms;

namespace Torch
{
    public partial class CodeEditor : UserControl
    {
        private Actor _actor;

        public event EventHandler Closed;

        public CodeEditor()
        {
            InitializeComponent();
            Initialize();
        }

        public CodeEditor(Actor actor)
        {
            InitializeComponent();
            Initialize();
            this.Actor = actor;
        }

        public Actor Actor
        {
            get
            {
                return _actor;
            }
            set
            {
                if (value != null)
                {
                    _actor = value;
                    codeTextBox.Text = _actor.Code ?? "";
                }
            }
        }

        void Close()
        {
            if (Closed != null)
            {
                Closed(this, EventArgs.Empty);
            }
        }

        void Initialize()
        {
            this.cancelButton.Click += (object sender, EventArgs e) => { Close(); };
            this.okButton.Click += (object sender, EventArgs e) => { SaveAndClose(); };
        }

        void SaveAndClose()
        {
            if (_actor != null)
            {
                _actor.Code = codeTextBox.Text;
            }
            Close();
        }
    }
}
