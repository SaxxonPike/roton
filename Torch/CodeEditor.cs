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
            Actor = actor;
        }

        public Actor Actor
        {
            get { return _actor; }
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
            cancelButton.Click += (sender, e) => { Close(); };
            okButton.Click += (sender, e) => { SaveAndClose(); };
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